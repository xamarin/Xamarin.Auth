using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace LoginAccounts
{
    public abstract class OAuthProviderBase
    {
        public virtual string ProviderName { get; }
        public virtual string ProviderVariant { get; }
        public virtual string Description { get; }
        public virtual Dictionary<string, string> AccountProperties { get; }

        public virtual Task<string> RetriveUsernameAsync(Account account)
        {
            return Task.FromResult(string.Empty);
        }
    }

    public abstract class OAuthProvider : OAuthProviderBase
    {
        public virtual string ClientId { get; }
        public virtual Uri AuthorizationUri { get; }
        public virtual Uri RedirectUri { get; }
        public virtual Uri AccessTokenUri { get; }

        public static IEnumerable<OAuthProvider> GetProviders()
        {
            var providerType = typeof(OAuthProvider);
            foreach (var type in providerType.Assembly.ExportedTypes)
            {
                if (!type.IsAbstract && providerType.IsAssignableFrom(type))
                    yield return (OAuthProvider)Activator.CreateInstance(type);
            }
        }

        public static Dictionary<string, OAuthProvider[]> GetGroupedProviders()
        {
            return GetProviders()
                .GroupBy(p => p.ProviderName, p => p)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.OrderBy(p => p.ProviderVariant).ToArray());
        }
    }

    public abstract class OAuth1Provider : OAuthProvider
    {
        public virtual string ClientSecret { get; }
        public virtual Uri RequestTokenUri { get; }
    }

    public abstract class OAuth2Provider : OAuthProvider
    {
        public virtual string ClientSecret { get; }
        public virtual string Scope { get; }
    }
}
