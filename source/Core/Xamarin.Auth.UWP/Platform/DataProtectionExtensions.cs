using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace Xamarin.Auth
{
    internal static class DataProtectionExtensions
    {
        public static Task<IBuffer> ProtectAsync(this IBuffer buffer, string protectionDescriptor = "LOCAL=user")
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (protectionDescriptor == null)
                throw new ArgumentNullException(nameof(protectionDescriptor));

            var provider = new DataProtectionProvider(protectionDescriptor);
            return provider.ProtectAsync(buffer).AsTask();
        }

        public static Task<IBuffer> UnprotectAsync(this IBuffer buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            var provider = new DataProtectionProvider();
            return provider.UnprotectAsync(buffer).AsTask();
        }
    }
}
