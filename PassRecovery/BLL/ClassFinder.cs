using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassRecovery
{
    /// <summary>
    /// Reflection helper class
    /// </summary>
    public sealed class ClassFinder
    {
        /// <summary>
        /// Gets all the concrete implementations and subclasses of a type.
        /// </summary>
        /// <param name="type">Ancestor class or interface</param>
        /// <returns>List of matching types</returns>
        public List<Type> GetSubClasses(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && t != type && !t.IsAbstract)
                .ToList();
        }

        /// <summary>
        /// Creates a new instance of a type with the default constructor.
        /// </summary>
        /// <param name="type">Type of instance</param>
        /// <returns>New instance</returns>
        public object CreateInstance(Type type)
        {
            return CreateInstance(type, new Type[0], new object[0]);
        }

        /// <summary>
        /// Creates a new instance of a type with the selected constructor.
        /// </summary>
        /// <param name="type">Type of instance</param>
        /// <param name="parameterTypes">Types of the parameters in constructor</param>
        /// <param name="parameters">Values to invoke the constructor with</param>
        /// <returns>New instance</returns>
        public object CreateInstance(Type type, Type[] parameterTypes, object[] parameters)
        {
            var constructor = type.GetConstructor(parameterTypes);
            return constructor.Invoke(parameters);
        }
    }
}
