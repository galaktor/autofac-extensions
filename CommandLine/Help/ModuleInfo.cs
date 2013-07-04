// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.Collections.Generic;
using Autofac.CommandLine.Args;

namespace Autofac.CommandLine.Help
{
    public class ModuleInfo
    {
        public ModuleInfo(Type type, AliasAttribute modAlias, IEnumerable<Prop> props)
        {
            Type = type;
            ModuleAlias = modAlias;
            Props = props;
        }

        public Type Type { get; private set; }
        public AliasAttribute ModuleAlias { get; private set; }
        public IEnumerable<Prop> Props { get; private set; }
    }
}