using System;
using System.Linq;
using System.Reflection;
using Autofac.Core;

namespace Autofac
{
    public class PropArg
    {
        private readonly IModule target;
        private readonly PropertyInfo p;
        public string FullName { get; set; }
        public Type Type { get; set; }
        public string Alias { get; set; }

        public PropArg(IModule target, PropertyInfo p)
        {
            this.target = target;
            this.p = p;
            FullName = p.Name;
            Type = p.PropertyType;
            var aliasAtt = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof (AliasAttribute)) as AliasAttribute;
            if (aliasAtt != null)
            {
                Alias = aliasAtt.Alias;
            }

        }

        public void Set(string value)
        {
            if (String.IsNullOrWhiteSpace(value) || Type == typeof (bool))
            {
                // treat presence of boolean flag as implicit "True" 
                value = "true";
            }

            var val = Convert.ChangeType(value, Type);
            p.SetValue(target, val,new object[0]);
        }
    }
}