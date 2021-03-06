// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Core;

namespace Autofac
{
    /// <summary>
    ///     Resolves an instance into a child lifetime scope, with optional ctor arguments and using a context object that can provided custom services to the child lifetime scope.
    /// </summary>
    /// <remarks>To undersand autofac lifetime scopes: http://nblumhardt.com/2011/01/an-autofac-lifetime-primer </remarks>
    /// <typeparam name="TResult">The service which will be resolved - along with its dependencies - within its own scope.</typeparam>
    public class ScopeFactory<TResult>
    {
        private readonly ILifetimeScope parent;

        public event LifetimeScopeConfigurationDelegate OnLifetimeScopeConfiguring = (p,b) => { };

        public ScopeFactory(ILifetimeScope parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Resolve an instance of TResult within it's own child lifetimescope.
        /// </summary>
        /// <param name="context">Scanned with reflection, the properties of this object will become services in the child lifetime scope.</param>
        /// <param name="args">Ctor arguments to be provided to the resolved service.</param>
        /// <returns>The service registered as TResult in the autofac scope.</returns>
        public TResult Get(object context, object[] args)
        {
            var scope = parent.BeginLifetimeScope(typeof (TResult), builder =>
                {
                    RegisterContextProperties(context, builder);
                    OnLifetimeScopeConfiguring(parent, builder);
                });
            return scope.Resolve<TResult>(ToParams(args));
        }

        /// <summary>
        /// Registers all properties on the context object as individual services in the builder.
        /// </summary>
        /// <param name="context">An object that will be scanned for it's properties using reflection.</param>
        /// <param name="builder">The autofac builder with which the services will be registered.</param>
        private void RegisterContextProperties(object context, ContainerBuilder builder)
        {
            var type = context.GetType();
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                var service = prop.GetValue(context, null);
                var serviceType = prop.PropertyType;
                builder.Register(c => service).As(serviceType);
            }
        }

        /// <summary>
        /// Resolve an instance of TResult within it's own child lifetimescope.
        /// </summary>
        /// <param name="context">Scanned with reflection, the properties of this object will become services in the child lifetime scope.</param>
        /// <param name="args">Ctor arguments to be provided to the resolved service.</param>
        /// <param name="output">The service registered as TResult in the autofac scope.</param>
        /// <returns>False if exceptions were thrown, True otherwise.</returns>
        public bool TryGet(object context, object[] args, out TResult output)
        {
            try
            {
                output = Get(context, args);
                return true;
            }
            catch (Exception)
            {
                output = default(TResult);
                return false;
            }
        }

        /// <summary>
        /// Converts an array of arguments into an enumerable of autofac Parameters so they can be provided as args to a Resolve call.
        /// </summary>
        /// <param name="args">Ctor parameters for the target object. Will be matched by order.</param>
        /// <returns>Array of Parameters</returns>
        private IEnumerable<Parameter> ToParams(object[] args)
        {
            if (args == null || args.Length == 0)
                return Enumerable.Empty<Parameter>();

            var result = new List<Parameter>(args.Length);
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                result.Add(new PositionalParameter(i, arg));
            }

            return result;
        }

        /// <summary>
        /// Resolve an instance of TResult within it's own child lifetimescope.
        /// </summary>
        /// <param name="context">Scanned with reflection, the properties of this object will become services in the child lifetime scope.</param>
        /// <returns>The service registered as TResult in the autofac scope.</returns>
        public TResult Get(object context)
        {
            return parent.BeginLifetimeScope(typeof (TResult), builder =>
                {
                    RegisterContextProperties(context, builder);
                    OnLifetimeScopeConfiguring(parent, builder);
                }).Resolve<TResult>();
        }

        /// <summary>
        /// Resolve an instance of TResult within it's own child lifetimescope.
        /// </summary>
        /// <param name="context">Scanned with reflection, the properties of this object will become services in the child lifetime scope.</param>
        /// <param name="output">The service registered as TResult in the autofac scope.</param>
        /// <returns>False if exceptions were thrown, True otherwise.</returns>
        public bool TryGet(object context, out TResult output)
        {
            try
            {
                output = Get(context);
                return true;
            }
            catch (Exception)
            {
                output = default(TResult);
                return false;
            }
        }
    }
}