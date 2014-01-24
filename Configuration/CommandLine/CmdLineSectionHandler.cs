// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using Autofac.CommandLine.Args;
using Autofac.Configuration;

namespace Autofac.CommandLine
{
    public class CmdLineSectionHandler : SectionHandler
    {
        private readonly CmdLineModulesCollection modules = new CmdLineModulesCollection();

        public CmdLineSectionHandler()
        {
            base["modules"] = modules;
        }

        public void Add(LoadArg l)
        {
            modules.Add(l);
        }
    }
}