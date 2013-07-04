// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;

namespace Autofac.CommandLine
{
    // TODO: rename to something more generic, such as CommandLineAttribute or something
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)] public class AliasAttribute : Attribute
    {
        public readonly string Alias;

        public AliasAttribute(string alias)
        {
            Alias = alias;
        }

        public override string ToString()
        {
            return Alias ?? "<none>";
        }
    }
}