using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    // Pull Request - manually added/fixed
    //		Added IsAuthenticated check #88
    //		https://github.com/xamarin/Xamarin.Auth/pull/88
    public partial class AccountResult
	{
		public string Name { get; set; }
		public string AccountType { get; set; }
		public string Token { get; set; }
	}
}
