using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    // Pull Request - manually added/fixed
    //		Added IsAuthenticated check #88
    //		https://github.com/xamarin/Xamarin.Auth/pull/88
    #if XAMARIN_AUTH_INTERNAL
    internal partial class AccountResult
    #else
    public partial class AccountResult
    #endif
    {
        public string Name { get; set; }
        public string AccountType { get; set; }
        public string Token { get; set; }
    }
}
