using System;

namespace Autofac
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true
        )]
    public class AliasAttribute : Attribute
    {
        public readonly string Alias;

        public AliasAttribute(string alias)
        {
            Alias = alias;
        }
    }
}