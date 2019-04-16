//
//  Copyright 2012-2016, Xamarin Inc.
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
using System.Text;

#if !AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    /// <summary>
    /// A persistent storage for <see cref="Account"/>s. This storage is encrypted.
    /// Accounts are stored using a service ID and the username of the account
    /// as a primary key.
    /// </summary>
    #if XAMARIN_AUTH_INTERNAL
    internal abstract partial class AccountStore
    #else
    public abstract partial class AccountStore
    #endif
    {
        /// <summary>
        /// Create an account store.
        /// </summary>
        [Obsolete
            (
            @"Use Xamarin.Essentials SecureStorage instead: https://aka.ms/xamarin-auth-accountstore-migration-guide
             Insecure version with hardcoded password. Please use AccountStore.Create(Context, string)"
            )
        ]
        public static AccountStore Create(global::Android.Content.Context context)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Using:");
            sb.AppendLine("    AccountStore.Create(Contex);");
            sb.AppendLine("or");
            sb.AppendLine("    AccountStore.Create();");
            sb.AppendLine("This version is insecure, because of default password.");
            sb.AppendLine("Please use version with supplied password for AccountStore:");
            sb.AppendLine("    AccountStore.Create(Contex, string);");
            sb.AppendLine("or");
            sb.AppendLine("    AccountStore.Create(string);");
            Console.WriteLine(sb.ToString());

            return new AndroidAccountStore(context);
        }

        /// <summary>
        /// AccountStore Create method overload
        /// 
        /// Password string is needed on Android and it is not used on other platforms.
        /// This way hardcoded passwords in Android code  avoided
        /// </summary>
        /// <returns>Created AccountStore</returns>
        /// <param name="password">Password used for the Store (Android, ignored on other platforms</param>
        [Obsolete("Use Xamarin.Essentials SecureStorage instead: https://aka.ms/xamarin-auth-accountstore-migration-guide")]
        public static AccountStore Create(global::Android.Content.Context context, string password)
        {
            return new AndroidAccountStore(context, password);
        }

        /// <summary>
        /// Create the specified context.
        /// </summary>
        /// <param name="context">Context.</param>
        [Obsolete
            (
            @"Use Xamarin.Essentials SecureStorage instead: https://aka.ms/xamarin-auth-accountstore-migration-guide
             Insecure version with hardcoded password. Please use AccountStore.Create(Context, string)"
            )
        ]
        public static AccountStore Create()
        {
            return Create(global::Android.App.Application.Context);
        }

        /// <summary>
        /// AccountStore Create method overload
        /// 
        /// Password string is needed on Android and it is not used on other platforms.
        /// This way hardcoded passwords in Android code  avoided
        /// </summary>
        /// <returns>Created AccountStore</returns>
        /// <param name="password">Password used for the Store (Android, ignored on other platforms</param>
        [Obsolete("Use Xamarin.Essentials SecureStorage instead: https://aka.ms/xamarin-auth-accountstore-migration-guide")]
        public static AccountStore Create(string password)
        {
            return Create(global::Android.App.Application.Context, password);
        }
    }
}

