// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using Autofac;

namespace CommandLineTester
{
    public class MyModule : Module
    {
        public string Foo { get; set; }

        public long Bar { get; set; }

        public string FooBarBaz { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Foo>();
            builder.RegisterAndActivate<ScopeFactory<Foo>>();

            Console.WriteLine("MyModule.Load()");
        }
    }
}