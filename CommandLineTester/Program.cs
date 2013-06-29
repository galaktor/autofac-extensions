using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Configuration;
using Autofac.Configuration.Core;
using Autofac.Core;
using Module = Autofac.Module;

namespace CommandLineTester
{
    [Alias("m")]
    public class CommandLineSettingsReader:CommandLineAwareModule
    {
        private List<Type> _modules = new List<Type>();
        private string _modulesText = "";

        [Alias("load")]
        public string Modules
        {
            get { return _modulesText; }
            set
            {
                //var assembly = Assembly.Load(value);
                // TODO: sanity check type/assembly string?
                var modType = Type.GetType(value);

                _modules.Add(modType);
                _modulesText = _modules.Aggregate("", (s, type) => type.FullName + ";");
            }
        }

        protected override void _Load(ContainerBuilder builder)
        {
            foreach (var m in _modules)
            {
                // TODO: each module must provide default ctor; later cmd line args could be used?
                var instance = (IModule) Activator.CreateInstance(m);
                builder.RegisterModule(instance);
            }
        }
    }

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

    public class ModuleWithoutDefaultCtor: Module
    {
        private readonly string blah;

        public ModuleWithoutDefaultCtor(string blah)
        {
            this.blah = blah;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            //cb.RegisterModule<MyModule>();
            cb.RegisterModule<CommandLineSettingsReader>();
            var c = cb.Build();

            using (var l = c.BeginLifetimeScope())
            {
                //var sf = l.Resolve<ScopeFactory<Foo>>();
                //sf.Get(new {blah = "test"});
            }
        }
    }
}
