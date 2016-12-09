using System;

namespace Xamarin.Auth
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false, Inherited = true)]
    public class PlatformAccountStoreAttribute : Attribute
    {
        public Type AccountStoreFactoryType { get; }

        public PlatformAccountStoreAttribute(Type accountStoreFactoryType)
        {
            if (accountStoreFactoryType == null) throw new ArgumentNullException(nameof(accountStoreFactoryType));
            AccountStoreFactoryType = accountStoreFactoryType;
        }
    }
}
