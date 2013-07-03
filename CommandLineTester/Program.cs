// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using Autofac;
using Autofac.CommandLine;

namespace CommandLineTester
{
    public class Foo
    {
        private readonly string blah;

        public Foo(string blah)
        {
            this.blah = blah;
        }
    }

    public class MyModule : Module
    {
        public string Foo { get; set; }

        public long Bar { get; set; }

        public string FooBarBaz { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Foo>();
            builder.RegisterAndActivate<ScopeFactory<Foo>>();

            Console.WriteLine("MyModule.Load()");
        }
    }

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

    internal class Program
    {
        private static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule(new CommandLineSettingsReader());
            IContainer c = cb.Build();

            using (ILifetimeScope l = c.BeginLifetimeScope())
            {
                var sf = l.Resolve<ScopeFactory<Foo>>();
                sf.Get(new {blah = "test"});
            }
        }
    }
}