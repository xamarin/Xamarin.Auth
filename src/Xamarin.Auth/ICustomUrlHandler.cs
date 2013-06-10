//
//  Copyright 2013, Stampsy Ltd.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
	/// <summary>
	/// Represents a manager that can open a URL in system browser and then wait for user to return to the app.
	/// </summary>
#if XAMARIN_AUTH_INTERNAL
	internal interface ICustomUrlHandler
#else
	public interface ICustomUrlHandler
#endif
	{
		/// <summary>
		/// Opens <paramref name="url"/> in system browser and waits for user to return to the app.
		/// 
		/// If the user returns to the app by opening custom URL that starts from <paramref name="callbackScheme"/> from browser,
		/// this custom URL will be the result of the task.
		/// 
		/// If custom URL doesn't match <paramref name="callbackScheme"/>, or if the app went foreground
		/// without receiving a custom URL, the task will be cancelled.
		/// </summary>
		Task<Uri> OpenUrl (Uri url, string callbackScheme);
	}
}

