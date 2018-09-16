using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using AuthenticateUIType =
            UIKit.UIViewController
            //System.Object
            ;

namespace Xamarin.Auth
{
	/// <summary>
	/// An authenticator that presents a form to the user.
	/// </summary>
	public abstract partial class FormAuthenticator : Authenticator
	{
		protected override AuthenticateUIType GetPlatformUI ()
		{
			return new UIKit.UINavigationController (new FormAuthenticatorController (this));
		}
	}
}

