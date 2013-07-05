// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using Autofac;
using Autofac.CommandLine;

namespace CommandLineTester
{
    [Alias("cmd")]
    public class BlahModule: CommandLineAwareModule
    {
        [Alias("f")]
        public string Foo { get; set; }

        protected override void Load_(ContainerBuilder builder)
        {
            Console.WriteLine("BlahModule.Load()");
        }
    }

    [Alias("cmd")]
    public class CmdModule : CommandLineAwareModule
    {
        [Alias("c")]
        public ConsoleColor Color { get; set; }

        [Alias("nr")]
        public ulong Number { get; set; }

        protected override void Load_(ContainerBuilder builder)
        {
            Console.WriteLine("CmdModule.Load()");
        }
    }
}