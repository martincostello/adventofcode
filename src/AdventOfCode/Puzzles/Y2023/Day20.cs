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
    private const string Activator = "rx";
    private const string Broadcaster = "broadcaster";
    private const string Button = "button";

    private enum Pulse
    {
        Low = 0,
        High,
    }

    /// <summary>
    /// Gets the product of the count of the number of
    /// high and low pulses sent ater 1,000 presses of the button.
    /// </summary>
    public int PulsesProduct { get; private set; }

    /// <summary>
    /// Gets the fewest number of button presses required
    /// to activate the machine, if possible.
    /// </summary>
    public long ActivationCycles { get; private set; }

    /// <summary>
    /// Presses the button connecting the modules the specified number of times.
    /// </summary>
    /// <param name="configuration">The configuration of the modules.</param>
    /// <param name="presses">The number of times to press the button.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The product of the count of the number of high and low pulses sent and
    /// he fewest number of button presses required to activate the machine
    /// or -1 if the module used to activate the machine does not exist.
    /// </returns>
    public static (int PulsesProduct, long RxMinimum) Run(
        IList<string> configuration,
        int presses,
        CancellationToken cancellationToken)
    {
        int highPulses = 0;
        int lowPulses = 0;

        var button = new ButtonModule();
        button.PulseReceived += OnPulse;

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

            module.PulseReceived += OnPulse;

            modules[module.Name] = module;
            connections[module.Name] = outputs;
        }

        button.Outputs.Add(modules[Broadcaster]);

        foreach (var module in modules.Values.ToArray())
        {
            if (!connections.TryGetValue(module.Name, out var outputs))
            {
                continue;
            }

            foreach (string name in outputs)
            {
                if (!modules.TryGetValue(name, out var next))
                {
                    connections[name] = Array.Empty<string>();
                    next = modules[name] = new OutputModule(name);
                    next.PulseReceived += OnPulse;
                }

                module.Outputs.Add(next);
            }
        }

        foreach (var module in modules.Values)
        {
            if (module is ConjunctionModule conjunction)
            {
                foreach (var other in modules.Values.Where((p) => p.Outputs.Any((r) => r.Name == module.Name)))
                {
                    conjunction.Inputs[other] = Pulse.Low;
                }
            }
        }

        for (int i = 0; i < presses; i++)
        {
            button.Press();
        }

        int product = highPulses * lowPulses;
        long cycles = -1;

        if (modules.TryGetValue(Activator, out var activator))
        {
            var inputs = modules.Values
                .Where((p) => p.Outputs.Any((r) => r.Name == activator.Name))
                .ToHashSet();

            if (inputs.Count == 1)
            {
                var activationInput = inputs.First();

                foreach (var module in modules.Values)
                {
                    module.PulseReceived -= OnPulse;
                }

                if (activationInput is ConjunctionModule conjunction)
                {
                    inputs = new(conjunction.Inputs.Keys);

                    var activationMinima = new Dictionary<Module, long>(inputs.Count);

                    foreach (var input in inputs)
                    {
                        presses = 1;
                        bool found = false;

                        foreach (var module in modules.Values)
                        {
                            module.Reset();
                            module.PulseReceived += WaitForLowPulse;
                        }

                        for (; !found && !cancellationToken.IsCancellationRequested; presses++)
                        {
                            button.Press();
                        }

                        cancellationToken.ThrowIfCancellationRequested();

                        void WaitForLowPulse(object? sender, PulseReceivedEventArgs args)
                        {
                            if (args.Pulse is Pulse.High &&
                                args.Receiver == conjunction &&
                                args.Sender == input &&
                                !activationMinima.ContainsKey(input))
                            {
                                activationMinima[input] = presses;
                                found = true;
                            }
                        }
                    }

                    cycles = activationMinima.Values.Aggregate(Maths.LowestCommonMultiple);
                }
                else if (activationInput is FlipFlopModule flipFlop)
                {
                    presses = 1;
                    bool found = false;

                    foreach (var module in modules.Values)
                    {
                        module.Reset();
                        module.PulseReceived += WaitForLowPulseWhenOn;
                    }

                    for (; !found && !cancellationToken.IsCancellationRequested; presses++)
                    {
                        button.Press();
                    }

                    cancellationToken.ThrowIfCancellationRequested();

                    void WaitForLowPulseWhenOn(object? sender, PulseReceivedEventArgs args)
                    {
                        if (args.Pulse is Pulse.Low &&
                            args.Receiver == flipFlop &&
                            flipFlop.On)
                        {
                            cycles = presses;
                            found = true;
                        }
                    }
                }
            }
            else if (inputs.Count > 1)
            {
                throw new NotImplementedException();
            }
        }

        return (product, cycles);

        void OnPulse(object? sender, PulseReceivedEventArgs args)
        {
            if (args.Pulse is Pulse.High)
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

        (PulsesProduct, ActivationCycles) = Run(configuration, presses: 1_000, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The total number of low pulses sent multiplied by the total number of high pulses sent is {0}.", PulsesProduct);
            Logger.WriteLine("The fewest number of button presses required to deliver a single low pulse to the module named rx is {0}.", ActivationCycles);
        }

        return PuzzleResult.Create(PulsesProduct, ActivationCycles);
    }

    private sealed class PulseReceivedEventArgs(Pulse pulse, Module sender, Module receiver) : EventArgs
    {
        public Pulse Pulse { get; } = pulse;

        public Module Sender { get; } = sender;

        public Module Receiver { get; } = receiver;
    }

    [DebuggerDisplay("{Name,nq} ({Type,nq})")]
    private abstract class Module(string name)
    {
        public event EventHandler<PulseReceivedEventArgs>? PulseReceived;

        public List<Module> Outputs { get; } = [];

        public string Name { get; } = name;

        public abstract string Type { get; }

        protected Queue<Pulse> Values { get; } = new();

        public virtual void Receive(Module sender, Pulse value)
        {
            OnPulseReceived(sender, value);
            Values.Enqueue(value);
        }

        public virtual void Send()
        {
            while (Values.TryDequeue(out var value))
            {
                foreach (var output in Outputs)
                {
                    output.Receive(this, value);
                }

                foreach (var output in Outputs)
                {
                    output.Send();
                }
            }
        }

        public virtual void Reset() => Values.Clear();

        protected virtual void OnPulseReceived(Module sender, Pulse value)
            => PulseReceived?.Invoke(this, new(value, sender, this));
    }

    private sealed class FlipFlopModule(string name) : Module(name)
    {
        public bool On { get; private set; }

        public override string Type { get; } = "Flip-flop";

        public override void Receive(Module sender, Pulse pulse)
        {
            OnPulseReceived(sender, pulse);

            if (pulse == Pulse.Low)
            {
                On = !On;
                Values.Enqueue(On ? Pulse.High : Pulse.Low);
            }
        }

        public override void Reset()
        {
            base.Reset();
            On = false;
        }
    }

    private sealed class ConjunctionModule(string name) : Module(name)
    {
        public Dictionary<Module, Pulse> Inputs { get; } = [];

        public override string Type { get; } = "Conjunction";

        public override void Receive(Module sender, Pulse value)
        {
            OnPulseReceived(sender, value);

            Inputs[sender] = value;

            Values.Enqueue(Inputs.Values.All((p) => p is Pulse.High) ? Pulse.Low : Pulse.High);
        }

        public override void Reset()
        {
            base.Reset();

            foreach (var input in Inputs.Keys)
            {
                Inputs[input] = Pulse.Low;
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

        public void Press()
        {
            Values.Enqueue(Pulse.Low);
            Send();
        }
    }

    private sealed class OutputModule(string name) : Module(name)
    {
        public override string Type { get; } = "Output";
    }
}
