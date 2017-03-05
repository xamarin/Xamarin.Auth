using System;

namespace Xamarin.Auth
{
    [global::Android.App.Activity()]
    [
        global::Android.App.IntentFilter
        (
            new[] { global::Android.Content.Intent.ActionView },
            DataScheme = "xamarin.auth",
            Categories = new[]
                            {
                                global::Android.Content.Intent.CategoryDefault,
                                global::Android.Content.Intent.CategoryBrowsable
                            }
        )
    ]
    [global::Android.App.IntentFilter
        (
            new[] { global::Android.Content.Intent.ActionView },
            DataScheme = "xamarin.auth",
            Categories = new[]
                            {
                                global::Android.Content.Intent.CategoryDefault,
                                global::Android.Content.Intent.CategoryBrowsable
                            }
        )
    ]
    [global::Android.App.IntentFilter
        (
            new[] { global::Android.Content.Intent.ActionView },
            DataScheme = "xamarinauth",
            Categories = new[]
                            {
                                global::Android.Content.Intent.CategoryDefault,
                                global::Android.Content.Intent.CategoryBrowsable
                            }
        )
    ]
    [global::Android.App.IntentFilter
        (
            new[] { global::Android.Content.Intent.ActionView },
            DataScheme = "localhost",
            Categories = new[]
                            {
                                global::Android.Content.Intent.CategoryDefault,
                                global::Android.Content.Intent.CategoryBrowsable
                            }
        )
    ]
    public class CustomUrlSchemeCallbackActivity : global::Android.App.Activity
    {
        protected override void OnCreate(global::Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);

            return;
        }
    }
}
