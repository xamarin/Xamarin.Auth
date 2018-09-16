
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
