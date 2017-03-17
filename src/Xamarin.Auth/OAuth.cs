using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    /// <summary>
    /// Type of method used to fetch the username of an account
    /// after it has been successfully authenticated.
    /// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal delegate Task<string> GetUsernameAsyncFunc (IDictionary<string, string> accountProperties);
#else
    public delegate Task<string> GetUsernameAsyncFunc(IDictionary<string, string> accountProperties);
#endif
}