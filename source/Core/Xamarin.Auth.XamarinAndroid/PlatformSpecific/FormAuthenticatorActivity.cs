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
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Views;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Util;
using Xamarin.Utilities.Android;
using Android.Text;

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    [Activity(Label = "Web Authenticator")]
    #if XAMARIN_AUTH_INTERNAL
	internal class FormAuthenticatorActivity : Activity
    #else
    public class FormAuthenticatorActivity : Activity
    #endif
    {
        Button signIn;
        ProgressBar progress;

        readonly Dictionary<FormAuthenticatorField, EditText> fieldEditors =
            new Dictionary<FormAuthenticatorField, EditText>();

        internal class State : Java.Lang.Object
        {
            public FormAuthenticator Authenticator;
            public CancellationTokenSource CancelSource;
            public bool IsSigningIn = false;
        }
        internal static readonly ActivityStateRepository<State> StateRepo = new ActivityStateRepository<State>();

        State state;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //
            // Load the state either from a configuration change or from the intent.
            //
            state = LastNonConfigurationInstance as State;
            if (state == null && Intent.HasExtra("StateKey"))
            {
                var stateKey = Intent.GetStringExtra("StateKey");
                state = StateRepo.Remove(stateKey);
            }
            if (state == null)
            {
                Finish();
                return;
            }

            Title = state.Authenticator.Title;

            //
            // Watch for completion
            //
            state.Authenticator.Completed += (s, e) =>
            {
                SetResult(e.IsAuthenticated ? Result.Ok : Result.Canceled);
                Finish();
            };
            state.Authenticator.Error += (s, e) =>
            {

                if (!state.Authenticator.ShowErrors)
                    return;

                if (e.Exception != null)
                {
                    this.ShowError("Authentication Error", e.Exception);
                }
                else
                {
                    this.ShowError("Authentication Error", e.Message);
                }
            };

            //
            // Build the UI
            //
            BuildUI();

            //
            // Restore the UI state or start over
            //
            if (savedInstanceState != null)
            {
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		Added IsAuthenticated check #88
        ///		https://github.com/xamarin/Xamarin.Auth/pull/88
        protected override void OnResume()
        {
            base.OnResume();
            if (state.Authenticator.AllowCancel && state.Authenticator.IsAuthenticated())
            {
                state.Authenticator.OnCancelled();
            }
        }
        ///-------------------------------------------------------------------------------------------------

        public override void OnBackPressed()
        {
            if (state.Authenticator.AllowCancel)
                base.OnBackPressed();
        }

        void BuildUI()
        {
            var hmargin = 12;

            var content = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical,
            };
            var scroller = new ScrollView(this)
            {
            };
            scroller.AddView(content);
            SetContentView(scroller);

            //
            // Fields
            //
            var fields = new TableLayout(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams
                                                        (
                                                            LinearLayout.LayoutParams.WrapContent, 
                                                            LinearLayout.LayoutParams.WrapContent
                                                        )
                {
                    TopMargin = 12,
                    LeftMargin = hmargin,
                    RightMargin = hmargin,
                },
            };
            fields.SetColumnStretchable(1, true);
            foreach (var f in state.Authenticator.Fields)
            {
                var row = new TableRow(this);

                var label = new TextView(this)
                {
                    Text = f.Title,
                    LayoutParameters = new TableRow.LayoutParams
                                                        (
                                                            LinearLayout.LayoutParams.WrapContent, 
                                                            LinearLayout.LayoutParams.WrapContent
                                                        )
                    {
                        RightMargin = 6,
                    },
                };
                label.SetTextSize(ComplexUnitType.Sp, 20);
                row.AddView(label);

                var editor = new EditText(this)
                {
                    Hint = f.Placeholder,
                };

                #region
                ///-------------------------------------------------------------------------------------------------
                /// Pull Request - manually added/fixed
                ///		Fix : Android password field now hides the user input in FormAuthenticatorActivity #63
                ///		https://github.com/xamarin/Xamarin.Auth/pull/63
                if (f.FieldType == FormAuthenticatorFieldType.Password)
                {
                    editor.InputType = InputTypes.TextVariationPassword;
                    editor.TransformationMethod = new global::Android.Text.Method.PasswordTransformationMethod();
                }
                ///-------------------------------------------------------------------------------------------------
                #endregion
                row.AddView(editor);
                fieldEditors[f] = editor;

                fields.AddView(row);
            }
            content.AddView(fields);

            //
            // Buttons
            //
            var signInLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = new LinearLayout.LayoutParams
                                                        (
                                                            //mc++ LinearLayout.LayoutParams.FillParent,
                                                            LinearLayout.LayoutParams.MatchParent,
                                                            LinearLayout.LayoutParams.WrapContent
                                                        )
                {
                    TopMargin = 24,
                    LeftMargin = hmargin,
                    RightMargin = hmargin,
                },
            };
            content.AddView(signInLayout);

            progress = new ProgressBar(this)
            {
                Visibility = state.IsSigningIn ? ViewStates.Visible : ViewStates.Gone,
                Indeterminate = true,
            };
            signInLayout.AddView(progress);

            signIn = new Button(this)
            {
                Text = "Sign In",
                LayoutParameters = new LinearLayout.LayoutParams
                                                   (
                                                       //mc++ LinearLayout.LayoutParams.FillParent,
                                                       LinearLayout.LayoutParams.MatchParent,
                                                       LinearLayout.LayoutParams.WrapContent
                                                  )
                {
                },
            };
            signIn.Click += HandleSignIn;
            signInLayout.AddView(signIn);

            if (state.Authenticator.CreateAccountLink != null)
            {
                var createAccount = new Button(this)
                {
                    Text = "Create Account",
                    LayoutParameters = new LinearLayout.LayoutParams
                                                            (
                                                                //LinearLayout.LayoutParams.FillParent, 
                                                                LinearLayout.LayoutParams.MatchParent,
                                                                LinearLayout.LayoutParams.WrapContent
                                                            )
                    {
                        TopMargin = 12,
                        LeftMargin = hmargin,
                        RightMargin = hmargin,
                    },
                };
                createAccount.Click += HandleCreateAccount;
                content.AddView(createAccount);
            }
        }

        void HandleSignIn(object sender, EventArgs e)
        {
            if (state.IsSigningIn) return;

            state.IsSigningIn = true;

            //
            // Read the values and disable them
            //
            foreach (var kv in fieldEditors)
            {
                kv.Key.Value = kv.Value.Text;
                kv.Value.Enabled = false;
            }
            signIn.Enabled = false;
            progress.Visibility = ViewStates.Visible;

            //
            // Try to log in
            //
            state.CancelSource = new CancellationTokenSource();
            state.Authenticator.SignInAsync(state.CancelSource.Token).ContinueWith(task =>
            {

                state.IsSigningIn = false;
                foreach (var fe in fieldEditors)
                {
                    fe.Value.Enabled = true;
                }
                signIn.Enabled = true;
                progress.Visibility = ViewStates.Gone;
                state.CancelSource = null;

                if (task.IsFaulted)
                {
                    state.Authenticator.OnError(task.Exception);
                }
                else
                {
                    state.Authenticator.OnSucceeded(task.Result);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void HandleCreateAccount(object sender, EventArgs e)
        {
            var intent = new Intent
                                (
                                    Intent.ActionView, 
                                    global::Android.Net.Uri.Parse(state.Authenticator.CreateAccountLink.AbsoluteUri)
                                );
            StartActivity(intent);
        }

        public override Java.Lang.Object OnRetainNonConfigurationInstance()
        {
            return state;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }
    }
}

