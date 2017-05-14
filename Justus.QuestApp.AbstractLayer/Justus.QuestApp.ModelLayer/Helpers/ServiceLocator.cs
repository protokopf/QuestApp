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
        private static readonly Dictionary<Type, Func<object>> ServicesFactories = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// Register service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="initializer"></param>
        /// <param name="useLikeFactory"></param>
        public static bool Register<T>(Func<T> initializer, bool useLikeFactory = false) where T : class
        {
            Type currentType = typeof(T);
            if (Services.ContainsKey(currentType) || ServicesFactories.ContainsKey(currentType))
            {
                return false;
            }

            if (!useLikeFactory)
            {
                Services[currentType] = new Lazy<object>(initializer);
                return true;
            }
            else
            {
                ServicesFactories[currentType] = initializer;
                return true;
            }
        }

        /// <summary>
        /// Resolves by generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>(bool preserved = true) where T : class
        {
            return (T)ResolveAssignable(typeof(T));
        }

        /// <summary>
        /// Returns enumaration of T implementations.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ResolveAll<T>() where T : class
        {
            return ResolveAllAssignable(typeof(T)).OfType<T>();
        }

        /// <summary>
        /// Releases all services.
        /// </summary>
        public static void ReleaseAll()
        {
            Services.Clear();
            ServicesFactories.Clear();
        }

        /// <summary>
        /// Resolve service by looking whether given type is assignable from any type from inner collection.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object ResolveAssignable(Type type)
        {
            TypeInfo paramInfo = type.GetTypeInfo();

            foreach (var pair in Services)
            {
                TypeInfo currentInfo = pair.Key.GetTypeInfo();
                
                if(paramInfo.IsAssignableFrom(currentInfo))
                {
                    return pair.Value.Value;
                }
            }

            foreach (var pair in ServicesFactories)
            {
                TypeInfo currentInfo = pair.Key.GetTypeInfo();

                if (paramInfo.IsAssignableFrom(currentInfo))
                {
                    return pair.Value.Invoke();
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

            return Services.
                Where(pair => typeInfo.IsAssignableFrom(pair.Key.GetTypeInfo())).
                Select(pair => pair.Value.Value).
                Union(ServicesFactories.
                            Where(factPair => typeInfo.IsAssignableFrom(factPair.Key.GetTypeInfo())).
                            Select(factPair => factPair.Value()));
        }
    }
}
