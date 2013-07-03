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
        private readonly IEnumerable<PropArg> props;

        // default to enabled
        private bool enabled = true;

        public static readonly string[] DefaultSetFlags = new[] { "set:", "s:" };
        private List<SetArg> sets = new List<SetArg>();

        public CommandLineAwareModule()
            :this(DefaultSetFlags)
        { }

        public CommandLineAwareModule(IEnumerable<string> setFlags)
        {
            var args = Environment.GetCommandLineArgs().AsCleanedArgs();

            foreach (var arg in args)
            {
                var setFlag = setFlags.FirstOrDefault(f => arg.StartsWith(f, StringComparison.InvariantCultureIgnoreCase));
                if (setFlag != null)
                {
                    string chomped = arg.Substring(setFlag.Length);
                    sets.Add(new SetArg(chomped));
                }
            }

            props = GetType().GetProperties().Select(p => new PropArg(this, p));

            var aliasAtt = GetType().GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof (AliasAttribute)) as AliasAttribute;
            if (aliasAtt != null)
            {
                Alias = aliasAtt.Alias;
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

        public string Alias { get; private set; }

        protected override sealed void Load(ContainerBuilder builder)
        {
            IEnumerable<SetArg> matches = from s in sets
                                          where s.TargetName == Alias || s.TargetName == GetType().Name
                                          select s;

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