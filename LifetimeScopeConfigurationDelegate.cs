// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

namespace Autofac
{
    public delegate void LifetimeScopeConfigurationDelegate(ILifetimeScope parentScope, ContainerBuilder childBuilder);
}