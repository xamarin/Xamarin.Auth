using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace Xamarin.Auth
{
    [Activity(Label = "Web Authenticator")]
    public class FormAuthenticatorActivity : Activity
    {
        private static readonly ActivityStateRepository<State> stateRepo = new ActivityStateRepository<State>();

        private readonly Dictionary<FormAuthenticatorField, EditText> fieldEditors = new Dictionary<FormAuthenticatorField, EditText>();

        private Button signIn;
        private ProgressBar progress;
        private State state;

        public static Intent CreateIntent(Context context, FormAuthenticator authenticator)
        {
            var state = new State
            {
                Authenticator = authenticator,
            };

            var i = new Intent(context, typeof(FormAuthenticatorActivity));
            i.PutExtra("StateKey", stateRepo.Add(state));
            return i;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Load the state either from a configuration change or from the intent.
            state = LastNonConfigurationInstance as State;
            if (state == null && Intent.HasExtra("StateKey"))
            {
                var stateKey = Intent.GetStringExtra("StateKey");
                state = stateRepo.Remove(stateKey);
            }
            if (state == null)
            {
                Finish();
                return;
            }

            // watch for completion
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
                    this.ShowError("Authentication Error", e.Exception);
                else
                    this.ShowError("Authentication Error", e.Message);
            };

            Title = state.Authenticator.Title;

            BuildUI();

            // Restore the UI state or start over
            if (savedInstanceState != null)
            {
                // restore
            }
            else
            {
                // fresh
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (state.Authenticator.AllowCancel && state.Authenticator.IsAuthenticated())
                state.Authenticator.OnCancelled();
        }

        public override void OnBackPressed()
        {
            if (state.Authenticator.AllowCancel)
                base.OnBackPressed();
        }

        private void BuildUI()
        {
            var wrapContent = LinearLayout.LayoutParams.WrapContent;
            var matchParent = LinearLayout.LayoutParams.MatchParent;

            // Fields
            var fields = new TableLayout(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(wrapContent, wrapContent)
                {
                    TopMargin = 12,
                    LeftMargin = 12,
                    RightMargin = 12,
                },
            };
            fields.SetColumnStretchable(1, true);
            foreach (var f in state.Authenticator.Fields)
            {
                var row = new TableRow(this);

                var label = new TextView(this)
                {
                    Text = f.Title,
                    LayoutParameters = new TableRow.LayoutParams(wrapContent, wrapContent)
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

                if (f.FieldType == FormAuthenticatorFieldType.Password)
                {
                    editor.InputType = InputTypes.TextVariationPassword;
                    editor.TransformationMethod = new PasswordTransformationMethod();
                }

                row.AddView(editor);
                fieldEditors[f] = editor;

                fields.AddView(row);
            }

            // Sign In Button
            var signInLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = new LinearLayout.LayoutParams(matchParent, wrapContent)
                {
                    TopMargin = 24,
                    LeftMargin = 12,
                    RightMargin = 12,
                },
            };
            progress = new ProgressBar(this)
            {
                Visibility = state.IsSigningIn ? ViewStates.Visible : ViewStates.Gone,
                Indeterminate = true,
            };
            signInLayout.AddView(progress);
            signIn = new Button(this)
            {
                Text = "Sign In",
                LayoutParameters = new LinearLayout.LayoutParams(matchParent, wrapContent),
            };
            signIn.Click += HandleSignIn;
            signInLayout.AddView(signIn);

            // Container Views
            var content = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical,
            };
            content.AddView(fields);
            content.AddView(signInLayout);

            // Create Account Button
            if (state.Authenticator.CreateAccountLink != null)
            {
                var createAccount = new Button(this)
                {
                    Text = "Create Account",
                    LayoutParameters = new LinearLayout.LayoutParams(matchParent, wrapContent)
                    {
                        TopMargin = 12,
                        LeftMargin = 12,
                        RightMargin = 12,
                    },
                };
                createAccount.Click += HandleCreateAccount;
                content.AddView(createAccount);
            }

            var scroller = new ScrollView(this);
            scroller.AddView(content);

            SetContentView(scroller);
        }

        private async void HandleSignIn(object sender, EventArgs e)
        {
            if (state.IsSigningIn)
                return;

            state.IsSigningIn = true;

            // Read the values and disable them
            foreach (var kv in fieldEditors)
            {
                var field = kv.Key;
                var editor = kv.Value;

                field.Value = editor.Text;
                editor.Enabled = false;
            }
            signIn.Enabled = false;
            progress.Visibility = ViewStates.Visible;

            // Try to log in
            try
            {
                var account = await state.Authenticator.SignInAsync();
                state.Authenticator.OnSucceeded(account);
            }
            catch (Exception ex)
            {
                state.Authenticator.OnError(ex);
            }
            finally
            {
                state.IsSigningIn = false;
                foreach (var fe in fieldEditors)
                {
                    var editor = fe.Value;
                    editor.Enabled = true;
                }
                signIn.Enabled = true;
                progress.Visibility = ViewStates.Gone;
            }
        }

        private void HandleCreateAccount(object sender, EventArgs e)
        {
            var uri = global::Android.Net.Uri.Parse(state.Authenticator.CreateAccountLink.AbsoluteUri);
            var intent = new Intent(Intent.ActionView, uri);
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

        private class State : Java.Lang.Object
        {
            public FormAuthenticator Authenticator;
            public bool IsSigningIn = false;
        }
    }
}
