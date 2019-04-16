//
//  Copyright 2013, Xamarin Inc.
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
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace Xamarin.Auth
{
    internal static class DataProtectionExtensions
    {
        /// <summary>
        /// These providers do not require the enterprise authentication capability on either platform:
        /// "LOCAL=user"
        /// "LOCAL=machine"
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="protectionDescriptor"></param>
        /// <returns></returns>
        public static async Task<IBuffer> ProtectAsync(this IBuffer buffer, string protectionDescriptor = "LOCAL=user")
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (protectionDescriptor == null)
                throw new ArgumentNullException("protectionDescriptor");

            var provider = new DataProtectionProvider(protectionDescriptor);
            return await provider.ProtectAsync(buffer).AsTask().ConfigureAwait(false);
        }

        public static async Task<IBuffer> UnprotectAsync(this IBuffer buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            var provider = new DataProtectionProvider();
            return await provider.UnprotectAsync(buffer).AsTask().ConfigureAwait(false);
        }
    }
}
