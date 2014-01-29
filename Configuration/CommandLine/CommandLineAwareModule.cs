// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.CommandLine.Args;
using Autofac.CommandLine.Help;

namespace Autofac.CommandLine
{
    // working
    // -MyModule:foo=bar
    // -m:f=bar

    public class CommandLineAwareModule : Module
    {
        public static readonly string[] DefaultSetFlags = new[] {"set:", "s:"};
        private readonly IEnumerable<PropArg> props;
        private readonly List<SetArg> sets = new List<SetArg>();
        private bool enabled = true;

        protected CommandLineAwareModule()
            :this(new ParamList(Environment.GetCommandLineArgs()), DefaultSetFlags)
        {
            
        }

        protected CommandLineAwareModule(ParamList args)
            : this(args, DefaultSetFlags)
        {
        }

        protected CommandLineAwareModule(ParamList argsRaw, IEnumerable<string> setFlags)
        {
            var args = argsRaw.Args.AsCleanedArgs();

            foreach (var arg in args)
            {
                var setFlag = setFlags.FirstOrDefault(f => arg.StartsWith(f, StringComparison.InvariantCultureIgnoreCase));
                if (setFlag != null)
                {
                    string chomped = arg.Substring(setFlag.Length);
                    sets.Add(new SetArg(chomped));
                }
            }

            props = this.GetProps();

            var aliasAtt = this.GetModuleAlias();
            if (aliasAtt != null)
            {
                Alias = aliasAtt;
            }
        }

        [Alias("on")]
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        [Alias("off")]
        public bool Disabled
        {
            get { return enabled; }
            set { enabled = !value; }
        }

        public AliasAttribute Alias { get; private set; }

        protected override sealed void Load(ContainerBuilder builder)
        {
            // TODO: register ModuleInfo so that it can be picked up later
            builder.RegisterInstance(new ModuleInfo(GetType(), Alias, props));

            IEnumerable<SetArg> matches = from s in sets
                                          where (Alias != null ? s.TargetName == Alias.Alias : false) || s.TargetName == GetType().Name
                                          select s;

            foreach (var m in matches)
            {
                foreach (var a in m.Args)
                {
                    IEnumerable<PropArg> toSet = from p in props
                                              where (p.Alias != null ? p.Alias.Alias == a.Key : false) || p.FullName == a.Key
                                              select p;
                    foreach (var p in toSet)
                    {
                        p.Set(a.Value);
                    }
                }
            }

            if (Enabled)
            {
                Load_(builder);
            }
        }

        protected virtual void Load_(ContainerBuilder builder)
        {
        }
    }
}
