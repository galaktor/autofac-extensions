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