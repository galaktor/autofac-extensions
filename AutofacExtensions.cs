// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.Reflection;
using Autofac.Builder;

namespace Autofac
{
    // TODO: move these into extra DLL within Autofac package! consider extra package if this grows into more...
    public static class AutofacExtensions
    {
        /// <summary>
        ///     Forces resolve of a single instance of T. Useful for services that are not dependend on by any other component
        ///     but need to be instantiated in the system.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="b">The container builder used to register the service.</param>
        public static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>
            RegisterAndActivate<T>(this ContainerBuilder b)
        {
            b.RegisterType<StartableBootstrap<T>>()
             .As<IStartable>()
             .SingleInstance();

            return b.RegisterType<T>().As<T>();
        }

        /// <summary>
        ///     Forces resolve of a single instance of T. Useful for services that are not dependend on by any other component
        ///     but need to be instantiated in the system.
        /// </summary>
        /// <param name="b">The container builder used to register the service.</param>
        /// <param name="c">A delegate that uses the context to create/resolve the component.</param>
        /// <returns></returns>
        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterAndActivate<T>(
            this ContainerBuilder b, Func<IComponentContext, T> c)
        {
            b.RegisterType<StartableBootstrap<T>>()
             .As<IStartable>()
             .SingleInstance();

            return b.Register(c).As<T>();
        }

        public static IRegistrationBuilder<ScopeFactory<T>, ConcreteReflectionActivatorData, SingleRegistrationStyle>
            RegisterScopeFactory<T>(this ContainerBuilder b, LifetimeScopeConfigurationDelegate c = null)
        {
            return b.RegisterType<ScopeFactory<T>>()
                    .OnActivated(args =>
                        {
                            if (c != null)
                            {
                                args.Instance.OnLifetimeScopeConfiguring += c;
                            }
                        });
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithParameter
            <TParam, TLimit, TReflectionActivatorData, TStyle>(
            this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration,
            Func<ParameterInfo, IComponentContext, TParam> valueProvider)
            where TReflectionActivatorData : ReflectionActivatorData
        {
            return registration.WithParameter((info, context) => info.ParameterType == typeof (TParam),
                                              (info, context) => valueProvider(info, context));
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithParameter
            <TParam, TLimit, TReflectionActivatorData, TStyle>(
            this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, TParam param)
            where TReflectionActivatorData : ReflectionActivatorData
        {
            return registration.WithParameter((info, context) => info.ParameterType == typeof (TParam),
                                              (info, context) => param);
        }
    }
}