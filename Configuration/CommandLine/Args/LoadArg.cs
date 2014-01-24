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
            //string[] parts = arg.SplitClean(':');

            // ALWAYS PARSE ASSEMBLY/MODULE
            var sep = arg.IndexOf(':');
            string modType = sep >= 0 ? arg.Substring(0, sep) : arg;
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
            arg = sep >= 0 ? arg.Substring(sep + 1) : string.Empty;
            if (!String.IsNullOrWhiteSpace(arg))
            {
                string modProps = arg;
                string[] propParts = modProps.SplitClean(',');

                foreach (var part in propParts)
                {
                    var assignmentArg = new AssignmentArg(part);
                    assignmentArg.Key = assignmentArg.Key.ToPropertyFullName(FullyQualifiedTypeName);
                    _properties.Add(assignmentArg);
                }
            }
        }

        public IEnumerable<AssignmentArg> Properties
        {
            get { return _properties; }
        }

        public override string ToString()
        {
            return FullyQualifiedTypeName;
        }

        private string FullyQualifiedTypeName
        {
            get
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
}
