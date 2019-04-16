using System;

namespace Xamarin.Auth.OAuthProviders
{
    public class ClassActivation
    {
        static System.Reflection.Assembly assembly = null;

        public static object CreateInstance(string class_name)
        {
            assembly = System.Reflection.Assembly.GetExecutingAssembly();

        	var type = assembly.GetTypes().First(t => t.Name == className);

        	return Activator.CreateInstance(type);           
        }

        private static System.Collections.Generic.IEnumerable<Type> GetDerivedTypesFor(Type baseType)
        {
        	assembly = System.Reflection.Assembly.GetExecutingAssembly();

        	return assembly.GetTypes()
        		.Where(baseType.IsAssignableFrom)
        		.Where(t => baseType != t);
        }
    }
}
