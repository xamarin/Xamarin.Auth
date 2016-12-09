using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    public interface IPlatformEngine
    {
        IAccountStore Create(char[] password = null);
        Task InvokeOnMainThread(Action action);
        IDisposable Disable100();
    }

    internal class Platform
    {
        private static readonly object Lock = new object();
        private static IPlatformEngine _engine = null;

        internal static IPlatformEngine Engine
        {
            get
            {
                if (_engine == null)
                {
                    lock (Lock)
                    {
                        if (_engine == null)
                        {
                            var assemblies = GetAppDomainAssemblies();
                            var attribute = GetAssemblyAttribute<PlatformAccountStoreAttribute>(assemblies).FirstOrDefault();
                            if (attribute == null)
                                throw new InvalidOperationException(
                                    "Could not find platform specific AccountStore implementation. Make sure there is one and it is decorated with the [PlatformAccountStoreAttribute]!");

                            _engine = Activator.CreateInstance(attribute.AccountStoreFactoryType) as IPlatformEngine;
                            if (_engine == null)
                                throw new InvalidOperationException(
                                    "The type decorated by the [PlatformAccountStoreAttribute] must implement the interface Xamarin.Auth.IAccountStoreFactory!");
                        }
                    }
                }

                return _engine;
            }
        }

        private static IEnumerable<T> GetAssemblyAttribute<T>(Assembly[] assemblies)
        {
            var platformSetupAttributeTypes =
                assemblies.SelectMany(a => a.CustomAttributes.Where(ca => ca.AttributeType == typeof(T)))
                    .ToList();

            foreach (var pt in platformSetupAttributeTypes)
            {
                var ctor = pt.AttributeType.GetTypeInfo().DeclaredConstructors.First();
                var parameters = pt.ConstructorArguments?.Select(carg => carg.Value).ToArray();
                yield return (T)ctor.Invoke(parameters);
            }
        }

        private static Assembly[] GetAppDomainAssemblies()
        {
            var ass = typeof(string).GetTypeInfo().Assembly;
            var ty = ass.GetType("System.AppDomain");
            var gm = ty.GetRuntimeProperty("CurrentDomain").GetMethod;
            var currentdomain = gm.Invoke(null, new object[] { });
            var getassemblies = currentdomain.GetType().GetRuntimeMethod("GetAssemblies", new Type[] { });
            var assemblies = getassemblies.Invoke(currentdomain, new object[] { }) as Assembly[];
            return assemblies;
        }
    }
}
