// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.CommandLine
{
    public static class CommandLineExtensions
    {
        public static IEnumerable<string> AsCleanedArgs(this string[] args)
        {
            return args.Skip(1).Where(a => a.StartsWith("-")).Select(a => a.SkipWhile(c => c == '-').Aggregate("", (agg, e) => agg += e)).Where(a => !String.IsNullOrWhiteSpace(a));
        }
    }
}