﻿using System;
using Autofac;

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

    [Alias("mym")]
    public class MyModule : Module
    {
        public bool On { get; set; }
        public string Foo { get; set; }

        [Alias("b")]
        public long Bar { get; set; }

        public string FooBarBaz { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Foo>();
            builder.RegisterAndActivate<ScopeFactory<Foo>>();

            Console.WriteLine("MyModule.Load()");
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