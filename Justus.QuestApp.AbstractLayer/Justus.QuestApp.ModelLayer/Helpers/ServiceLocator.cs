using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Justus.QuestApp.ModelLayer.Helpers
{
    /// <summary>
    /// Sevice locator type.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type,Lazy<object>> Services = new Dictionary<Type, Lazy<object>>();

        /// <summary>
        /// Register service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="initializer"></param>
        public static bool Register<T>(Func<T> initializer)
        {
            if(!Services.ContainsKey(typeof(T)))
            {
                Services[typeof(T)] = new Lazy<object>(() => initializer());
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resolves by generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return (T)ResolveAssignable(typeof(T));
        }

        /// <summary>
        /// Returns enumaration of T implementations.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ResolveAll<T>()
        {
            return ResolveAllAssignable(typeof(T)).OfType<T>();
        }

        /// <summary>
        /// Releases all services.
        /// </summary>
        public static void ReleaseAll()
        {
            Services.Clear();
        }

        /// <summary>
        /// Resolve service by looking whether given type is assignable from any type from inner collection.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object ResolveAssignable(Type type)
        {
            foreach (var pair in Services)
            {
                TypeInfo currentInfo = pair.Key.GetTypeInfo();
                TypeInfo paramInfo = type.GetTypeInfo();

                if(paramInfo.IsAssignableFrom(currentInfo))
                {
                    return pair.Value.Value;
                }
            }
            throw new InvalidOperationException($"Service {type} not found!");
        }

        /// <summary>
        /// Return enumeration of assignable to type types.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<object> ResolveAllAssignable(Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            return from serv in Services
                where typeInfo.IsAssignableFrom(serv.Key.GetTypeInfo())
                select serv.Value.Value;
        }
    }
}
