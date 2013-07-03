// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using Autofac.CommandLine.Args;
using Autofac.Configuration.Elements;

namespace Autofac.CommandLine
{
    public class CmdLinePropertyElementCollection : PropertyElementCollection
    {
        public void Add(AssignmentArg a)
        {
            var e = new CmdLinePropertyElement(a);
            BaseAdd(e);
        }
    }
}