﻿// Copyright Matthias Koch 2017.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;

namespace Nuke.Core.OutputSinks
{
    public class BitriseOutputSink : ConsoleOutputSink
    {
        public new static IOutputSink Instance { get; } = BuildServers.Bitrise.Instance != null ? new BitriseOutputSink() : null;

        public BitriseOutputSink ()
        {
            SetFont("ansi-shadow");
        }
    }
}