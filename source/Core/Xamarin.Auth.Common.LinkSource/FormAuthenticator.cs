//
//  Copyright 2012-2016, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

//--------------------------------------------------------------------
//	Original defines
//		usings are in FormAuthenticator.<Platform>.cs
//
//#if PLATFORM_IOS
//using AuthenticateUIType = MonoTouch.UIKit.UIViewController;
//#elif PLATFORM_ANDROID
//using AuthenticateUIType = Android.Content.Intent;
//using UIContext = Android.Content.Context;
//#elif PLATFORM_WINPHONE
//using AuthenticateUIType = System.Uri;
//#else
//using AuthenticateUIType = System.Object;
//#endif
//--------------------------------------------------------------------

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
	/// <summary>
	/// An authenticator that presents a form to the user.
	/// </summary>
    #if XAMARIN_AUTH_INTERNAL
	internal abstract partial class FormAuthenticator : Authenticator
    #else
	public abstract partial class FormAuthenticator : Authenticator
    #endif
	{
		/// <summary>
		/// The fields that need to be filled in by the user in order to authenticate.
		/// </summary>
		/// <value>
		/// The fields.
		/// </value>
		public IList<FormAuthenticatorField> Fields { get; set; }

		/// <summary>
		/// A link to a website or other resource that allows the user to create a new account.
		/// </summary>
		/// <value>
		/// The create account link.
		/// </value>
		public Uri CreateAccountLink { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.FormAuthenticator"/> class
		/// with the given link to create accounts.
		/// </summary>
		/// <param name='createAccountLink'>
		/// A link to a website or other resource that allows the user to create a new account.
		/// </param>
		public FormAuthenticator (Uri createAccountLink = null)
		{
			Fields = new List<FormAuthenticatorField> ();
			CreateAccountLink = createAccountLink;
		}

		/// <summary>
		/// Gets the value of a field using its key.
		/// </summary>
		/// <returns>
		/// The field value.
		/// </returns>
		/// <param name='key'>
		/// The key of the field.
		/// </param>
		public string GetFieldValue (string key)
		{
			var f = Fields.FirstOrDefault (x => x.Key == key);
			return (f != null) ? f.Value : null;
		}

		/// <summary>
		/// Method called to authenticate the user using the values in the <see cref="Fields"/>.
		/// </summary>
		/// <returns>
		/// A task to retrieve the <see cref="Account"/> for the authenticated user.
		/// </returns>
		/// <param name='cancellationToken'>
		/// Cancellation token used to cancel the authentication.
		/// </param>
		public abstract Task<Account> SignInAsync (CancellationToken cancellationToken);

        #if __IOS__
		/// <summary>
		/// Gets the UI to present this form.
		/// </summary>
		/// <returns>
		/// The UI that needs to be presented.
		/// </returns>
		//protected override AuthenticateUIType GetPlatformUI ()
		//{
		//	return new MonoTouch.UIKit.UINavigationController (new FormAuthenticatorController (this));
		//}
        #elif __ANDROID__
		/// <summary>
		/// Gets the UI to present this form.
		/// </summary>
		/// <returns>
		/// The UI that needs to be presented.
		/// </returns>
		//protected override AuthenticateUIType GetPlatformUI (UIContext context)
		//{
		//	var i = new global::Android.Content.Intent (context, typeof (FormAuthenticatorActivity));
		//	var state = new FormAuthenticatorActivity.State {
		//		Authenticator = this,
		//	};
		//	i.PutExtra ("StateKey", FormAuthenticatorActivity.StateRepo.Add (state));
		//	return i;
		//}
        #else
		/// <summary>
		/// Gets the UI to present this form.
		/// </summary>
		/// <returns>
		/// The UI that needs to be presented.
		/// </returns>
		//protected override AuthenticateUIType GetPlatformUI ()
		//{
		//	throw new NotSupportedException ("FormAuthenticator not supported on this platform.");
		//}
    #endif
	}

	/// <summary>
	/// Account credential form field.
	/// </summary>
    #if XAMARIN_AUTH_INTERNAL
	internal class FormAuthenticatorField
    #else
	public class FormAuthenticatorField
    #endif
	{
		/// <summary>
		/// A key used to identify this field.
		/// </summary>
		/// <value>
		/// The key.
		/// </value>
		public string Key { get; set; }

		/// <summary>
		/// The title of this field when presented in a UI.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		public string Title { get; set; }

		/// <summary>
		/// Placeholder text shown when there is no input value for this field.
		/// </summary>
		/// <value>
		/// The placeholder.
		/// </value>
		public string Placeholder { get; set; }

		/// <summary>
		/// The value of this field.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public string Value { get; set; }

		/// <summary>
		/// The type of this field.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public FormAuthenticatorFieldType FieldType { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.FormAuthenticatorField"/> class.
		/// </summary>
		/// <param name='key'>
		/// A key used to identify the field.
		/// </param>
		/// <param name='title'>
		/// The title of the field when presented in a UI.
		/// </param>
		/// <param name='fieldType'>
		/// The type of the field.
		/// </param>
		/// <param name='placeholder'>
		/// Placeholder text shown when there is no input value for the field.
		/// </param>
		/// <param name='defaultValue'>
		/// The value of the field.
		/// </param>
		public FormAuthenticatorField (string key, string title, FormAuthenticatorFieldType fieldType, string placeholder = "", string defaultValue = "")
		{
			if (string.IsNullOrWhiteSpace (key)) {
				throw new ArgumentException ("key must not be blank", "key");
			}
			Key = key;

			if (string.IsNullOrWhiteSpace (title)) {
				throw new ArgumentException ("title must not be blank", "title");
			}
			Title = title;

			Placeholder = placeholder ?? "";

			Value = defaultValue ?? "";

			FieldType = fieldType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.FormAuthenticatorField"/> class.
		/// </summary>
		public FormAuthenticatorField ()
		{
		}
	}

	/// <summary>
	/// The display type of a credential field.
	/// </summary>
    #if XAMARIN_AUTH_INTERNAL
	internal enum FormAuthenticatorFieldType
    #else
	public enum FormAuthenticatorFieldType
    #endif
	{
		/// <summary>
		/// The field is plain text.
		/// </summary>
		PlainText,

		/// <summary>
		/// The field is an email address.
		/// </summary>
		Email,

		/// <summary>
		/// The field is protected from onlookers.
		/// </summary>
		Password,
	}
}

