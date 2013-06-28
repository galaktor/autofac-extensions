//using System.Collections.Generic;
//using Autofac.Core;

//namespace Autofac
//{
//    public class ScopeFactory<T>
//    {
//        private readonly ILifetimeScope parent;

//        public ScopeFactory(ILifetimeScope parent)
//        {
//            this.parent = parent;
//        }

//        public T Get(object context)
//        {
//            return parent.BeginLifetimeScope(typeof(T), builder => builder.Register(c => context)).Resolve<T>();
//        }

//        private void RegisterContextProperties(object context, ContainerBuilder builder)
//        {
//            // TODO
//        }

//        private IEnumerable<Parameter> ObjectsToParams(object[] objects)
//        {
//            // TODO
//        }
//    }
//}
