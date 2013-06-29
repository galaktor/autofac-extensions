using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Configuration;

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

    public class MyModule: CommandLineAwareModule
    {
        public string Foo { get; set; }
        public long Bar { get; set; }

        protected override void _Load(ContainerBuilder builder)
        {
            builder.RegisterType<Foo>();
            builder.RegisterAndActivate<ScopeFactory<Foo>>();

            Console.WriteLine("MyModule.Load()");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<MyModule>();
            var c = cb.Build();

            using (var l = c.BeginLifetimeScope())
            {
                var sf = l.Resolve<ScopeFactory<Foo>>();
                sf.Get(new {blah = "test"});
            }
        }
    }
}
