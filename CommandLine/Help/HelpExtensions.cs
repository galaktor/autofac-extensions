// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autofac.CommandLine.Help
{
    public static class HelpExtensions
    {
        public static void PrintHelpAndExit(this ILifetimeScope scope)
        {
            var infos = scope.Resolve<IEnumerable<ModuleInfo>>().Distinct();


            var sb = new StringBuilder();

            sb.AppendLine("*********************************");
            sb.AppendLine("*** Autofac Command-Line Help ***");
            sb.AppendLine("*********************************");
            sb.AppendLine();

            // TODO: example call(s) and descriptions provided through ModuleInfo and Attribute(s)

            sb.AppendLine();

            var margin = 2;
            var col1Width = infos.Max(info => info.Props.Max(prop => prop.FullName.Length)) + margin;
            var col2Width = infos.Max(info => info.ModuleAlias == null ? 6 : info.ModuleAlias.ToString().Length) + margin;
            var format = String.Format("{{0,-{0}}}{{1,-{1}}}{{2}}", col1Width, col2Width);

            foreach (var info in infos)
            {
                sb.AppendFormat("{0} (Alias: {1})", info.Type.FullName, (info.ModuleAlias != null ? info.ModuleAlias.ToString() : "<none>")).AppendLine();
                sb.AppendFormat(format, "---------", "-----", "-----------").AppendLine();
                sb.AppendFormat(format, "Full Name", "Alias", "Description").AppendLine();
                sb.AppendFormat(format, "---------", "-----", "-----------").AppendLine();
                foreach (var prop in info.Props)
                {
                    sb.AppendFormat(format, prop.FullName, prop.Alias == null ? "<none>" : prop.Alias.ToString(), "TODO DESCRIPTION").AppendLine();
                }

                sb.AppendLine();
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }
    }
}