using System;
using AuthenticateUIType = System.Type;

namespace Xamarin.Auth
{
    partial class FormAuthenticator : Authenticator
    {
        protected override AuthenticateUIType GetPlatformUI()
        {
            throw new NotImplementedException();
        }
    }
}
