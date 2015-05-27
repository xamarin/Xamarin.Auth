using System.Collections.Generic;

namespace Xamarin.Auth
{
	public interface ILoginEvents {
		void OnSuccessfulLogin(Dictionary<string, string> userinfos);
	}
}