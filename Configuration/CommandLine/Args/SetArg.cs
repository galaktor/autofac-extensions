// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System.Collections.Generic;

namespace Autofac.CommandLine.Args
{
    public class SetArg
    {
        public List<AssignmentArg> Args = new List<AssignmentArg>();
        public string TargetName;

        public SetArg(string rawblob)
        {
            // TODO: checks

            //var parts = rawblob.Split(':').Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a));
            var sep = rawblob.IndexOf(':');
            TargetName = sep >= 0 ? rawblob.Substring(0, sep) : rawblob;

            if(sep >= 0)
            {
                rawblob = rawblob.Substring(sep + 1);
                var rest = rawblob.Split(',');
                foreach (var arg in rest)
                {
                    Args.Add(new AssignmentArg(arg));
                }    
            }
        }
    }
}