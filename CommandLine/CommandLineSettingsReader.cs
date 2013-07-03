// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.CommandLine.Args;
using Autofac.Configuration.Core;

namespace Autofac.CommandLine
{
    // -l:MyModule,MyAssembly
    // -l:MyModule,MyAssembly:foo=bar,baz=42


    // TODO: -l:MyModule,MyAssembly:foo=bar,baz=42
    // add ModuleElement with type MyModule,MyAssembly
    // to that add properties with name="foo" and value="bar"

    public class CommandLineSettingsReader : ConfigurationModule
    {
        public static readonly string[] DefaultLoadFlags = new[] {"load:", "l:"};
        public static readonly string[] DefaultSetFlags = new[] {string.Empty};
        private List<SetArg> _props = new List<SetArg>();

        public CommandLineSettingsReader()
            : this(DefaultLoadFlags, DefaultSetFlags)
        {
        }

        public CommandLineSettingsReader(IEnumerable<string> loadFlags, IEnumerable<string> setFlags)
        {
            var handler = new CmdLineSectionHandler();

            IEnumerable<string> args = Environment.GetCommandLineArgs()
                                                  .Skip(1)
                                                  .Where(a => a.StartsWith("-"))
                                                  .Select(
                                                      a =>
                                                      a.SkipWhile(c => c == '-').Aggregate("", (agg, e) => agg += e))
                                                  .Where(a => !String.IsNullOrWhiteSpace(a));

            // TODO: scan for types with Alias attribute and find ones that match the provided name
            // TODO: configurable probing paths, maybe just for assemblies not found?

            foreach (var arg in args)
            {
                string loadFlag =
                    loadFlags.FirstOrDefault(f => arg.StartsWith(f, StringComparison.InvariantCultureIgnoreCase));
                //var setFlag = setFlags.FirstOrDefault(f => arg.StartsWith(f, StringComparison.InvariantCultureIgnoreCase));
                if (loadFlag != null)
                {
                    string chomped = arg.Substring(loadFlag.Length);
                    handler.Add(new LoadArg(chomped));
                }
                //else if (setFlag != null)
                //{
                //    // set
                //    var chomped = arg.Substring(setFlag.Length);
                //    _props.Add(new SetArg(chomped));
                //}
            }

            SectionHandler = handler;


            // TODO: property config
            // TODO: property AutoAlias via CamelCase initials (no need for AliasAttribute)
        }

        //protected override void Load(ContainerBuilder builder)
        //{
        //    foreach (var m in _modules)
        //    {
        //        // TODO: each module must provide default ctor; later cmd line args could be used?
        //        var instance = (IModule)Activator.CreateInstance(m);

        //        // config props
        //        Type m1 = m;
        //        var alias = m1.GetCustomAttributes(true).FirstOrDefault(att => att.GetType() == typeof (AliasAttribute)) as AliasAttribute;
        //        var matches = from a in _props
        //                      where a.TargetName == m1.Name || (alias != null ? alias.Alias == a.TargetName : false)
        //                      select a;

        //        var publicProps = instance.GetType().GetProperties().Select(p => new PropArg(instance, p));
        //        foreach (var match in matches)
        //        {
        //            foreach (var a in match.Args)
        //            {
        //                var a1 = a;
        //                var toSet = from p in publicProps
        //                            where p.FullName == a1.Key || p.Alias == a1.Key
        //                            select p;
        //                foreach (var p in toSet)
        //                {
        //                    p.Set(a.Value);
        //                }
        //            }

        //        }

        //        builder.RegisterModule(instance);
        //    }
        //}
    }
}