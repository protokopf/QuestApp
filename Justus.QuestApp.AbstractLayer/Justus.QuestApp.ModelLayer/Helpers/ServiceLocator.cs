using System;
using System.Collections.Generic;

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
            return (T) Resolve(typeof(T));
        }

        /// <summary>
        /// Releases all services.
        /// </summary>
        public static void ReleaseAll()
        {
            Services.Clear();
        }

        /// <summary>
        /// Resolve object by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object Resolve(Type type)
        {
            Lazy<object> service = null;
            if (Services.TryGetValue(type, out service))
            {
                return service.Value;
            }
            throw new InvalidOperationException("Service not found!");
        }
    }
}
