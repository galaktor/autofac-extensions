// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using Autofac;
using Autofac.CommandLine;

namespace CommandLineTester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule(new CommandLineSettingsReader());
            IContainer c = cb.Build();

            using (ILifetimeScope l = c.BeginLifetimeScope())
            {
                //var sf = l.Resolve<ScopeFactory<Foo>>();
                //sf.Get(new {blah = "test"});
            }
        }
    }
}