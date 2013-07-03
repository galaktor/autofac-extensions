// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.Collections.Generic;

namespace Autofac.CommandLine.Args
{
    public class LoadArg
    {
        public string AssemblyName;
        public string ModuleTypeName;
        public List<AssignmentArg> _properties = new List<AssignmentArg>();

        public LoadArg(string arg)
        {
            string[] parts = arg.SplitClean(':');

            // ALWAYS PARSE ASSEMBLY/MODULE
            string modType = parts[0];
            string[] modTypeParts = modType.SplitClean(',');
            switch (modTypeParts.Length)
            {
                case 2:
                    // Type and AssemblyName given
                    ModuleTypeName = modTypeParts[0];
                    AssemblyName = modTypeParts[1];
                    break;
                case 1:
                    // only TypeName given
                    ModuleTypeName = modTypeParts[0];
                    break;
                default:
                    throw new ArgumentException("Assembly/Type names in wrong format: " + modType);
            }

            // IF PROVIDED, PARSE PROPERTIES
            if (parts.Length > 1)
            {
                string modProps = parts[1];
                string[] propParts = modProps.SplitClean(',');

                foreach (var part in propParts)
                {
                    _properties.Add(new AssignmentArg(part));
                }
            }
        }

        public IEnumerable<AssignmentArg> Properties
        {
            get
            {
                return _properties;
            }
        }

        public override string ToString()
        {
            string result = ModuleTypeName;
            if (!String.IsNullOrWhiteSpace(AssemblyName))
            {
                result += ", " + AssemblyName;
            }

            return result;
        }
    }
}