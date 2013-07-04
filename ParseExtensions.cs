// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.CommandLine;
using Autofac.CommandLine.Args;
using Autofac.Core;

namespace Autofac
{
    public static class ParseExtensions
    {
        public static string[] SplitClean(this string input, params char[] separators)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                return new string[0];
            }

            if (separators == null || separators.Length == 0)
            {
                separators = new[] {' '}; // default to space
            }

            return input.Split(separators).Select(s => s.Trim()).Where(s => !String.IsNullOrWhiteSpace(s)).ToArray();
        }

        public static AliasAttribute GetModuleAlias(this IModule m)
        {
            return m.GetType().GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof (AliasAttribute)) as AliasAttribute;
        }

        public static IEnumerable<Prop> GetProps(this IModule m)
        {
            return m.GetType().GetProperties().Select(p => new Prop(m, p));
        }
    }
}