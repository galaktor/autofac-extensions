// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

namespace Autofac.CommandLine.Args
{
    public class AssignmentArg
    {
        public string Key;
        public string Value;

        public AssignmentArg(string raw)
        {
            string[] parts = raw.SplitClean('=');
            Key = parts[0];
            if(parts.Length > 1)
            {
                Value = parts[1];   
            }
        }
    }
}