// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System.Collections.Generic;
using System.Linq;

namespace Autofac.CommandLine.Args
{
    public class SetArg
    {
        public List<AssignmentArg> Args = new List<AssignmentArg>();
        public string TargetName;

        public SetArg(string rawblob)
        {
            // TODO: checks

            string[] parts =
                rawblob.Split(':', ',').Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
            TargetName = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                string arg = parts[i];
                Args.Add(new AssignmentArg(arg));
            }
        }
    }
}