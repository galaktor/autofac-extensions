using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac
{
    public class CmdModule: Module
    {
        private static readonly IEnumerable<ArgBlob> args;
        private IEnumerable<Prop> props;

        [Alias("on")]
        public bool Enabled { get; set; }

        public string Alias { get; private set; }

        static CmdModule()
        {
            args = Environment.GetCommandLineArgs()
                              .Skip(1)
                              .Where(a => a.StartsWith("-"))
                              .Select(a => a.SkipWhile(c => c == '-').Aggregate<char,string>("", (agg, e) => agg+=e))
                              .Where(a => !String.IsNullOrWhiteSpace(a))
                              .Select(a => new ArgBlob(a));
        }

        protected CmdModule()
        {
            this.props = this.GetType().GetProperties().Select(p => new Prop(this,p));

            var aliasAtt = this.GetType().GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(AliasAttribute)) as AliasAttribute;
            if (aliasAtt != null)
            {
                Alias = aliasAtt.Alias;
            }
        }

        protected sealed override void Load(ContainerBuilder builder)        
        {
            var matches = from a in args
                          where a.Target == this.Alias || a.Target == this.GetType().Name
                          select a;

            foreach (var m in matches)
            {
                foreach (var a in m.Args)
                {
                    var toSet = from p in props
                                where p.FullName == a.Key || p.Alias == a.Key
                                select p;
                    foreach (var p in toSet)
                    {
                        p.Set(a.Value);
                    }
                }
                
            }
            

            this._Load(builder);
        }

        protected virtual void _Load(ContainerBuilder builder)
        { }
    }
}