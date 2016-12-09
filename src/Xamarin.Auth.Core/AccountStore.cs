//
//  Copyright 2012, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Xamarin.Auth
{
    /// <summary>
	/// A persistent storage for <see cref="Account"/>s. This storage is encrypted.
	/// Accounts are stored using a service ID and the username of the account
	/// as a primary key.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal abstract class AccountStore
#else
	public abstract class AccountStore
#endif
        : IAccountStore
    {
#if PLATFORM_IOS
		/// <summary>
		/// Create an account store.
		/// </summary>
		public static AccountStore Create ()
		{
			return new KeyChainAccountStore ();
		}
#elif PLATFORM_ANDROID
		/// <summary>
		/// Create an account store.
		/// </summary>
		public static AccountStore Create (Android.Content.Context context, char[] password = null)
		{
			return new AndroidAccountStore (context, password);
		}
#else
	    /// <summary>
	    /// Create an account store.
	    /// </summary>
	    /// <returns>A new <see cref="AccountStore"/> instance.</returns>
	    public static IAccountStore Create(char[] password = null)
		{
		    return Factory.Create(password);
		}
#endif
	    private static readonly object Lock = new object();
	    private static IAccountStoreFactory _factory = null;

	    private static IAccountStoreFactory Factory
	    {
	        get
	        {
	            if (_factory == null)
	            {
	                lock (Lock)
	                {
	                    if (_factory == null)
	                    {
	                        var assemblies = GetAppDomainAssemblies();
	                        var attribute = GetAssemblyAttribute<PlatformAccountStoreAttribute>(assemblies).FirstOrDefault();
	                        if (attribute == null)
	                            throw new InvalidOperationException(
	                                "Could not find platform specific AccountStore implementation. Make sure there is one and it is decorated with the [PlatformAccountStoreAttribute]!");

	                        _factory = Activator.CreateInstance(attribute.AccountStoreFactoryType) as IAccountStoreFactory;
                            if (_factory == null)
                                throw new InvalidOperationException(
                                    "The type decorated by the [PlatformAccountStoreAttribute] must implement the interface Xamarin.Auth.IAccountStoreFactory!");
                        }
                    }
	            }

	            return _factory;
	        }
	    }
        
	    /// <summary>
		/// Finds the accounts for a given service.
		/// </summary>
		/// <returns>
		/// The accounts for the service.
		/// </returns>
		/// <param name='serviceId'>
		/// Service identifier.
		/// </param>
		public abstract IEnumerable<Account> FindAccountsForService (string serviceId);

		/// <summary>
		/// Save the specified account by combining its username and the serviceId
		/// to form a primary key.
		/// </summary>
		/// <param name='account'>
		/// Account to store.
		/// </param>
		/// <param name='serviceId'>
		/// Service identifier.
		/// </param>
		public abstract void Save (Account account, string serviceId);

		/// <summary>
		/// Deletes the account for a given serviceId.
		/// </summary>
		/// <param name='account'>
		/// Account to delete.
		/// </param>
		/// <param name='serviceId'>
		/// Service identifier.
		/// </param>
		public abstract void Delete (Account account, string serviceId);

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

