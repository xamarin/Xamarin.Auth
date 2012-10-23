//
//  Copyright 2012, Xamarin Inc.
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

#if PLATFORM_IOS
using AuthenticateUIType = MonoTouch.UIKit.UIViewController;
#elif PLATFORM_ANDROID
using AuthenticateUIType = Android.Content.Intent;
using UIContext = Android.Content.Context;
#else
using AuthenticateUIType = System.Object;
#endif

namespace Xamarin.Auth
{
	/// <summary>
	/// An authenticator that presents a form to the user.
	/// </summary>
	public abstract class FormAuthenticator : Authenticator
	{
		public IList<FormAuthenticatorField> Fields { get; private set; }

		public Uri CreateAccountLink { get; set; }

		public FormAuthenticator (Uri createAccountLink)
		{
			Fields = new List<FormAuthenticatorField> ();
			CreateAccountLink = createAccountLink;
		}

		public string GetFieldValue (string key) {
			var f = Fields.FirstOrDefault (x => x.Key == key);
			return (f != null) ? f.Value : null;
		}

		public abstract Task<Account> SignInAsync (CancellationToken cancellationToken);

#if PLATFORM_IOS
		protected override AuthenticateUIType GetPlatformUI ()
		{
			return new MonoTouch.UIKit.UINavigationController (new FormAuthenticatorController (this));
		}
#elif PLATFORM_ANDROID
		protected override AuthenticateUIType GetPlatformUI (UIContext context)
		{
			var i = new global::Android.Content.Intent (context, typeof (FormAuthenticatorActivity));
			var state = new FormAuthenticatorActivity.State {
				Authenticator = this,
			};
			i.PutExtra ("StateKey", FormAuthenticatorActivity.StateRepo.Add (state));
			return i;
		}
#else
		protected override AuthenticateUIType GetPlatformUI ()
		{
			throw new NotSupportedException ("FormAuthenticator not supported on this platform.");
		}
#endif
	}

	/// <summary>
	/// Account credential form field.
	/// </summary>
	public class FormAuthenticatorField
	{
		public string Key { get; set; }
		public string Title { get; set; }
		public string Placeholder { get; set; }
		public string Value { get; set; }
		public FormAuthenticatorFieldType FieldType { get; set; }

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
	}

	/// <summary>
	/// The display type of a credential field.
	/// </summary>
	public enum FormAuthenticatorFieldType
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

