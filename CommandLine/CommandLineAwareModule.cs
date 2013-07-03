// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.CommandLine.Args;

namespace Autofac.CommandLine
{
    public class CommandLineAwareModule : Module
    {
        private static readonly IEnumerable<SetArg> args;
        private readonly IEnumerable<PropArg> props;

        // default to enabled
        private bool enabled = true;

        static CommandLineAwareModule()
        {
            args = Environment.GetCommandLineArgs()
                              .Skip(1)
                              .Where(a => a.StartsWith("-"))
                              .Select(a => a.SkipWhile(c => c == '-')
                                            .Aggregate("", (agg, e) => agg += e))
                              .Where(a => !String.IsNullOrWhiteSpace(a))
                              .Select(a => new SetArg(a));
        }

        protected CommandLineAwareModule()
        {
            props = GetType()
                .GetProperties()
                .Select(p => new PropArg(this, p));

            var aliasAtt = GetType()
                               .GetCustomAttributes(true)
                               .FirstOrDefault(a => a.GetType() == typeof (AliasAttribute)) as AliasAttribute;
            if (aliasAtt != null)
            {
                Alias = aliasAtt.Alias;
            }
        }

        [Alias("on")]
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        [Alias("off")]
        public bool Disabld
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = !value;
            }
        }

        public string Alias { get; private set; }

        protected override sealed void Load(ContainerBuilder builder)
        {
            IEnumerable<SetArg> matches = from a in args
                                          where a.TargetName == Alias || a.TargetName == GetType()
                                                                                             .Name
                                          select a;

            foreach (var m in matches)
            {
                foreach (var a in m.Args)
                {
                    IEnumerable<PropArg> toSet = from p in props
                                                 where p.FullName == a.Key || p.Alias == a.Key
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