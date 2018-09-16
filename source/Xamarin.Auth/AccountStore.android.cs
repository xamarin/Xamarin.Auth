using System;
using System.Text;

namespace Xamarin.Auth
{
    /// <summary>
    /// A persistent storage for <see cref="Account"/>s. This storage is encrypted.
    /// Accounts are stored using a service ID and the username of the account
    /// as a primary key.
    /// </summary>
    public abstract partial class AccountStore
    {
        /// <summary>
        /// Create an account store.
        /// </summary>
        [Obsolete("Insecure version with hardcoded password. Please use AccountStore.Create(Context, string)")]
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

        public static AccountStore Create(global::Android.Content.Context context, string password)
        {
            return new AndroidAccountStore(context, password);
        }

        /// <summary>
        /// Create the specified context.
        /// </summary>
        /// <param name="context">Context.</param>
        [Obsolete("Insecure version with hardcoded password. Please use AccountStore.Create(Context, string)")]
        public static AccountStore Create()
        {
            return Create(global::Android.App.Application.Context);
        }

        /// <summary>
        /// Create the specified context.
        /// </summary>
        /// <param name="password">Password.</param>
        public static AccountStore Create(string password)
        {
            return Create(global::Android.App.Application.Context, password);
        }
    }
}

