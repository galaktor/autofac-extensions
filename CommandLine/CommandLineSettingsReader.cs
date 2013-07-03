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
    // -MyModule,MyAssembly:foo=bar
    // -MyModule:foo=bar
    // -m:f=bar


    // TODO: -l:MyModule,MyAssembly:foo=bar,baz=42
    // add ModuleElement with type MyModule,MyAssembly
    // to that add properties with name="foo" and value="bar"

    public class CommandLineSettingsReader : ConfigurationModule
    {
        public static readonly string[] DefaultLoadFlags = new[] {"load:", "l:"};
        private List<SetArg> _props = new List<SetArg>();

        public CommandLineSettingsReader()
            : this(DefaultLoadFlags)
        { }

        public CommandLineSettingsReader(IEnumerable<string> loadFlags)
        {
            var handler = new CmdLineSectionHandler();

            var args = Environment.GetCommandLineArgs().AsCleanedArgs();

            // TODO: scan for types with Alias attribute and find ones that match the provided name
            // TODO: configurable probing paths, maybe just for assemblies not found?

            foreach (var arg in args)
            {
                string loadFlag = loadFlags.FirstOrDefault(f => arg.StartsWith(f, StringComparison.InvariantCultureIgnoreCase));
                if (loadFlag != null)
                {
                    string chomped = arg.Substring(loadFlag.Length);
                    handler.Add(new LoadArg(chomped));
                }
            }

            SectionHandler = handler;


            // TODO: property config
            // TODO: property AutoAlias via CamelCase initials (no need for AliasAttribute)
        }
    }
}