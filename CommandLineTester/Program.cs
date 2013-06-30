using System;
using Autofac;
using Module = Autofac.Module;

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

    public class MyModule: Module
    {
        public bool On { get; set; }
        public string Foo { get; set; }
        public long Bar { get; set; }

        protected override void Load(ContainerBuilder builder)
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
            cb.RegisterModule(new CommandLineSettingsReader());
            var c = cb.Build();

            using (var l = c.BeginLifetimeScope())
            {
                //var sf = l.Resolve<ScopeFactory<Foo>>();
                //sf.Get(new {blah = "test"});
            }
        }
    }
}
