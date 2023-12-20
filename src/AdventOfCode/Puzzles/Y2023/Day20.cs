// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/20</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 20, "Pulse Propagation", RequiresData = true)]
public sealed class Day20 : Puzzle
{
    private const string Broadcaster = "broadcaster";
    private const string Button = "button";

    private enum Pulse
    {
        Low = 1,
        High,
    }

    /// <summary>
    /// Gets the product of the count of the number of
    /// high and low pulses sent ater 1,000 presses of the button.
    /// </summary>
    public int PulsesProduct { get; private set; }

    /// <summary>
    /// Presses the button connecting the modules the specified number of times.
    /// </summary>
    /// <param name="configuration">The configuration of the modules.</param>
    /// <param name="presses">The number of times to press the button.</param>
    /// <returns>
    /// The product of the count of the number of high and low pulses sent.
    /// </returns>
    public static int Run(IList<string> configuration, int presses)
    {
        var button = new ButtonModule();

        var modules = new Dictionary<string, Module>()
        {
            [Button] = button,
        };

        var connections = new Dictionary<string, IList<string>>();

        foreach (string config in configuration)
        {
            string[] parts = config.Split(" -> ");
            char type = parts[0][0];
            string name = parts[0][1..];
            string[] outputs = parts[1].Split(", ");

            Module module = type switch
            {
                'b' => new BroadcasterModule(),
                '&' => new ConjunctionModule(name),
                '%' => new FlipFlopModule(name),
                _ => throw new PuzzleException($"Unknown module type '{type}'."),
            };

            modules[module.Name] = module;
            connections[module.Name] = outputs;
        }

        button.Outputs.Add(modules[Broadcaster]);

        int highPulses = 0;
        int lowPulses = 0;

        foreach (var module in modules.Values.ToArray())
        {
            if (!connections.TryGetValue(module.Name, out var outputs))
            {
                continue;
            }

            module.Received += OnPulse;

            foreach (string name in outputs)
            {
                if (!modules.TryGetValue(name, out var next))
                {
                    next = modules[name] = new OutputModule(name);
                    next.Received += OnPulse;
                }

                module.Outputs.Add(next);
            }
        }

        for (int i = 0; i < presses; i++)
        {
            button.Press();
        }

        return highPulses * lowPulses;

        void OnPulse(object? sender, Pulse p)
        {
            if (p is Pulse.High)
            {
                highPulses++;
            }
            else
            {
                lowPulses++;
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var configuration = await ReadResourceAsLinesAsync(cancellationToken);

        PulsesProduct = Run(configuration, presses: 1_000);

        if (Verbose)
        {
            Logger.WriteLine("The total number of low pulses sent multiplied by the total number of high pulses sent is {0}.", PulsesProduct);
        }

        return PuzzleResult.Create(PulsesProduct);
    }

    [DebuggerDisplay("{Name,nq} ({Type,nq})")]
    private abstract class Module(string name)
    {
        public event EventHandler<Pulse>? Received;

        public List<Module> Outputs { get; } = [];

        public string Name { get; } = name;

        public abstract string Type { get; }

        public virtual void Receive(Module sender, Pulse pulse)
        {
            OnReceived(pulse);

            foreach (var output in Outputs)
            {
                output.Receive(this, pulse);
            }
        }

        protected void OnReceived(Pulse pulse)
            => Received?.Invoke(this, pulse);
    }

    private sealed class FlipFlopModule(string name) : Module(name)
    {
        public bool On { get; private set; }

        public override string Type { get; } = "Flip-flop";

        public override void Receive(Module sender, Pulse pulse)
        {
            OnReceived(pulse);

            if (pulse is Pulse.Low)
            {
                pulse = On ? Pulse.Low : Pulse.High;
                On = !On;

                foreach (var output in Outputs)
                {
                    output.Receive(this, pulse);
                }
            }
        }
    }

    private sealed class ConjunctionModule(string name) : Module(name)
    {
        private readonly Dictionary<Module, Pulse> _inputs = [];

        public override string Type { get; } = "Conjunction";

        public override void Receive(Module sender, Pulse pulse)
        {
            OnReceived(pulse);

            _inputs[sender] = pulse;

            pulse = _inputs.Values.All((p) => p is Pulse.High) ? Pulse.Low : Pulse.High;

            foreach (var output in Outputs)
            {
                output.Receive(this, pulse);
            }
        }
    }

    private sealed class BroadcasterModule() : Module(Broadcaster)
    {
        public override string Type { get; } = "Broadcast";
    }

    private sealed class ButtonModule() : Module(Button)
    {
        public override string Type { get; } = "Button";

        public void Press() => Receive(this, Pulse.Low);
    }

    private sealed class OutputModule(string name) : Module(name)
    {
        public override string Type { get; } = "Output";
    }
}
