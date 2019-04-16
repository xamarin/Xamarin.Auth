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
using System.Collections.Generic;

#if ! AZURE_MOBILE_SERVICES
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
    internal partial class AccountStoreException : AuthException
    #else
    public partial class AccountStoreException : AuthException
    #endif
    {
        public string Operation
        {
            get;
            set;
        } = "N/A";

        public AccountStoreException(string operation)
            : base(operation)
        {
            this.Operation = operation;
        }

        public AccountStoreException(string operation, System.Exception exc)
            : base(operation, exc)
        {
        }
    }
}

