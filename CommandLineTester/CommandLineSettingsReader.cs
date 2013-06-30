using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace CommandLineTester
{
    public class CommandLineSettingsReader:Module
    {
        private List<Type> _modules = new List<Type>();
        private IEnumerable<LoadArg> _loads;
        private IEnumerable<SetArg> _props;
        
        public static readonly string[] DefaultLoadFlags = new[] { "load", "l" };
        public static readonly string[] DefaultSetFlags = new[] { "set", "s" };

        public CommandLineSettingsReader()
            : this(DefaultLoadFlags, DefaultSetFlags)
        { }

        public CommandLineSettingsReader(IEnumerable<string> loadFlags, IEnumerable<string> setFlags)
        {
            var args = Environment.GetCommandLineArgs()
                                  .Skip(1)
                                  .Where(a => a.StartsWith("-"))
                                  .Select(a => a.SkipWhile(c => c == '-').Aggregate("", (agg, e) => agg += e))
                                  .Where(a => !String.IsNullOrWhiteSpace(a));

            loadFlags = loadFlags.Select(f => f + ":");
            setFlags = setFlags.Select(f => f + ":");

            // TODO: scan for types with Alias attribute and find ones that match the provided name
            // TODO: configurable probing paths, maybe just for assemblies not found?
            _loads = args.Where(a => loadFlags.Any(a.StartsWith))
                         .Select(a => new LoadArg(a.Substring(a.IndexOf(':') + 1)));
            foreach (var l in _loads)
            {
                var t = Type.GetType(l.ToString());
                _modules.Add(t);
            }

            _props = args.Where(a => setFlags.Any(a.StartsWith))
                         .Select(a => new SetArg(a.Substring(a.IndexOf(':') + 1)));
            // TODO: property config
            // TODO: property AutoAlias via CamelCase initials (no need for AliasAttribute)
        }

        protected override void Load(ContainerBuilder builder)
        {
            foreach (var m in _modules)
            {
                // TODO: each module must provide default ctor; later cmd line args could be used?
                var instance = (IModule)Activator.CreateInstance(m);

                // config props
                Type m1 = m;
                var matches = from a in _props
                              where a.ModuleTypeName == m1.Name // TODO: ALIAS   || a.ModuleTypeName == this.Alias
                              select a;

                var publicProps = instance.GetType().GetProperties().Select(p => new PropArg(instance, p));
                foreach (var match in matches)
                {
                    foreach (var a in match.Args)
                    {
                        var a1 = a;
                        var toSet = from p in publicProps
                                    where p.FullName == a1.Key // TODO: ALIAS    || p.Alias == a1.Key
                                    select p;
                        foreach (var p in toSet)
                        {
                            p.Set(a.Value);
                        }
                    }

                }

                builder.RegisterModule(instance);
            }
        }
    }
}