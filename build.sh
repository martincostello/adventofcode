#!/bin/sh
SOLUTIONDIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
xbuild /verbosity:minimal $SOLUTIONDIR/src/AdventOfCode2015.msbuild "$@"
