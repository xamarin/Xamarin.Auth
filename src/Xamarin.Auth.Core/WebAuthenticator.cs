//
//  Copyright 2012-2013, Xamarin Inc.
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
using System.Threading.Tasks;
using System.Threading;

namespace Xamarin.Auth
{
	/// <summary>
	/// An authenticator that displays a web page.
	/// </summary>
	public abstract class WebAuthenticator : Authenticator
	{
		/// <summary>
		/// Gets or sets whether to automatically clear cookies before logging in.
		/// </summary>
		/// <seealso cref="ClearCookies"/>
		public bool ClearCookiesBeforeLogin
		{
			get { return this.clearCookies; }
			set { this.clearCookies = value; }
		}
        
        /// <summary>
        /// Gets or sets a value indicating whether [enable DOM storage].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable DOM storage]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableDomStorage
        {
            get { return _enableDomStorage; }
            set { _enableDomStorage = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable java script].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable java script]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableJavaScript
        {
            get { return _enableJavaScript; }
            set { _enableJavaScript = value; }
        }


        /// <summary>
        /// Gets or sets whether to display authentication errors in the UI. Set to false if you want to handle the errors yourself.
        /// </summary>
        public bool ShowUIErrors 
		{
			get { return this.showUIErrors; }
			set { this.showUIErrors = value; }
		}

		/// <summary>
		/// Method that returns the initial URL to be displayed in the web browser.
		/// </summary>
		/// <returns>
		/// A task that will return the initial URL.
		/// </returns>
		public abstract Task<Uri> GetInitialUrlAsync ();

		/// <summary>
		/// Event handler called when a new page is being loaded in the web browser.
		/// </summary>
		/// <param name='url'>
		/// The URL of the page.
		/// </param>
		public virtual void OnPageLoading (Uri url)
		{
		}

		/// <summary>
		/// Event handler called when a new page has been loaded in the web browser.
		/// Implementations should call <see cref="Authenticator.OnSucceeded(Xamarin.Auth.Account)"/> if this page
		/// signifies a successful authentication.
		/// </summary>
		/// <param name='url'>
		/// The URL of the page.
		/// </param>
		public abstract void OnPageLoaded (Uri url);

		/// <summary>
		/// Occurs when the visual, user-interactive, browsing has completed but there
		/// is more authentication work to do.
		/// </summary>
		public event EventHandler BrowsingCompleted;

		private bool clearCookies = true;
		private bool showUIErrors = true;
        private bool _enableDomStorage = false;
        private bool _enableJavaScript = true;

        /// <summary>
        /// Raises the browsing completed event.
        /// </summary>
        protected virtual void OnBrowsingCompleted ()
		{
			var ev = BrowsingCompleted;
			if (ev != null) {
				ev (this, EventArgs.Empty);
			}
		}
	}
}

