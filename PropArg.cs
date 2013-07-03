using System;
using System.Linq;
using System.Reflection;
using Autofac.Core;
using Autofac.Configuration.Util;

namespace Autofac
{
    public class PropArg
    {
        private readonly PropertyInfo p;
        private readonly IModule target;

        public PropArg(IModule target, PropertyInfo p)
        {
            this.target = target;
            this.p = p;
            FullName = p.Name;
            Type = p.PropertyType;
            var aliasAtt =
                p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof (AliasAttribute)) as
                AliasAttribute;
            if (aliasAtt != null)
            {
                Alias = aliasAtt.Alias;
            }
        }

        public string FullName { get; set; }
        public Type Type { get; set; }
        public string Alias { get; set; }

        public void Set(string value)
        {
            if (String.IsNullOrWhiteSpace(value) || Type == typeof (bool))
            {
                // treat presence of boolean flag as implicit "True" 
                value = "true";
            }
            
            //object val = Convert.ChangeType(value, Type);
            var val = value.ConvertTo(this.Type);
            p.SetValue(target, val, new object[0]);
        }
    }
}