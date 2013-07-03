// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using Autofac.CommandLine.Args;
using Autofac.Configuration.Elements;

namespace Autofac.CommandLine
{
    public class CmdLineModuleElement : ModuleElement
    {
        private readonly CmdLinePropertyElementCollection _properties = new CmdLinePropertyElementCollection();

        public CmdLineModuleElement(LoadArg l)
        {
            base["type"] = l.ToString();
            base["properties"] = _properties;
        }

        public void Add(AssignmentArg a)
        {
            _properties.Add(a);
        }
    }
}