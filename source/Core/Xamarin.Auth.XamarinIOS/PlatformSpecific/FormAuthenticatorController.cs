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
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
# if ! __UNIFIED__
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#else
using UIKit;
using Foundation;
#endif

#if ! AZURE_MOBILE_SERVICES
using Xamarin.Controls;
using Xamarin.Utilities.iOS;
#else
using Xamarin.Controls._MobileServices;
using Xamarin.Utilities._MobileServices.iOS;
#endif

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Auth
#else
namespace Xamarin.Auth._MobileServices
#endif
{
    internal class FormAuthenticatorController : UITableViewController
    {
        FormAuthenticator authenticator;

        ProgressLabel progress;

        CancellationTokenSource cancelSource;

        public FormAuthenticatorController(FormAuthenticator authenticator)
            : base(UITableViewStyle.Grouped)
        {
            this.authenticator = authenticator;

            Title = authenticator.Title;

            TableView.DataSource = new FormDataSource(this);
            TableView.Delegate = new FormDelegate(this);

            if (authenticator.AllowCancel)
            {
                NavigationItem.LeftBarButtonItem = new UIBarButtonItem
                                                        (
                                                            UIBarButtonSystemItem.Cancel,
                                                            delegate
                                                            {
                                                                StopProgress();
                                                                authenticator.OnCancelled();
                                                            }
                                                        );
            }
        }

        void HandleSubmit()
        {
            if (progress == null)
            {
                progress = new ProgressLabel
                            (
                                NSBundle.MainBundle.LocalizedString
                                                    (
                                                        "Verifying", 
                                                        "Verifying status message when adding accounts"
                                                    )
                            );
                NavigationItem.TitleView = progress;
                progress.StartAnimating();
            }

            cancelSource = new CancellationTokenSource();

            authenticator.SignInAsync(cancelSource.Token).ContinueWith(task =>
            {
                StopProgress();

                if (task.IsFaulted)
                {

                    if (!authenticator.ShowErrors)
                        return;

                    this.ShowError("Error Signing In", task.Exception);
                }
                else
                {
                    authenticator.OnSucceeded(task.Result);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

            return;
        }

        void StopProgress()
        {
            if (progress != null)
            {
                progress.StopAnimating();
                NavigationItem.TitleView = null;
                progress = null;
            }

            return;
        }

#region
        ///-------------------------------------------------------------------------------------------------
        /// Pull Request - manually added/fixed
        ///		Added IsAuthenticated check #88
        ///		https://github.com/xamarin/Xamarin.Auth/pull/88
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (authenticator.AllowCancel && authenticator.IsAuthenticated())
            {
                authenticator.OnCancelled();
            }

            return;
        }
        ///-------------------------------------------------------------------------------------------------
#endregion

        class FormDelegate : UITableViewDelegate
        {
            FormAuthenticatorController controller;

            public FormDelegate(FormAuthenticatorController controller)
            {
                this.controller = controller;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                tableView.ResignFirstResponder();

                if (indexPath.Section == 1)
                {
                    tableView.DeselectRow(indexPath, true);
                    ((FormDataSource)tableView.DataSource).ResignFirstResponder();
                    controller.HandleSubmit();
                }
                else if (indexPath.Section == 2)
                {
                    tableView.DeselectRow(indexPath, true);
                    UIApplication.SharedApplication.OpenUrl(
                        new NSUrl(controller.authenticator.CreateAccountLink.AbsoluteUri));

                }
            }
        }

        class FieldCell : UITableViewCell
        {
            public static readonly UIFont LabelFont = UIFont.BoldSystemFontOfSize(16);
            public static readonly UIFont FieldFont = UIFont.SystemFontOfSize(16);

            static readonly UIColor FieldColor = UIColor.FromRGB(56, 84, 135);

            public UITextField TextField { get; private set; }

#if !__UNIFIED__
			public FieldCell (FormAuthenticatorField field, float fieldXPosition, Action handleReturn)
#else
            public FieldCell(FormAuthenticatorField field, nfloat fieldXPosition, Action handleReturn)
#endif
                : base(UITableViewCellStyle.Default, "Field")
            {
                SelectionStyle = UITableViewCellSelectionStyle.None;

                TextLabel.Text = field.Title;

                var hang = 3;
                var h = FieldFont.PointSize + hang;

                var cellSize = Frame.Size;

#if !__UNIFIED__
				TextField = new UITextField 
                                (
                                    new RectangleF 
                                            (
					                            fieldXPosition, (cellSize.Height - h)/2, 
					                            cellSize.Width - fieldXPosition - 12, h)
                                            ) 
#else
                TextField = new UITextField
                                (
                                    new CoreGraphics.CGRect
                                            (
                                                fieldXPosition, (cellSize.Height - h) / 2,
                                                cellSize.Width - fieldXPosition - 12, h
                                            )
                                )
#endif
                {
                    Font = FieldFont,
                    Placeholder = field.Placeholder,
                    Text = field.Value,
                    TextColor = FieldColor,
                    AutoresizingMask = UIViewAutoresizing.FlexibleWidth,

                    SecureTextEntry = (field.FieldType == FormAuthenticatorFieldType.Password),

                    KeyboardType = (field.FieldType == FormAuthenticatorFieldType.Email) ?
                        UIKeyboardType.EmailAddress :
                        UIKeyboardType.Default,

                    AutocorrectionType = (field.FieldType == FormAuthenticatorFieldType.PlainText) ?
                        UITextAutocorrectionType.Yes :
                        UITextAutocorrectionType.No,

                    AutocapitalizationType = UITextAutocapitalizationType.None,

                    ShouldReturn = delegate
                    {
                        handleReturn();
                        return false;
                    },
                };

                TextField.EditingDidEnd += delegate
                {
                    field.Value = TextField.Text;
                };

                ContentView.AddSubview(TextField);
            }
        }

        class FormDataSource : UITableViewDataSource
        {
            FormAuthenticatorController controller;

            public FormDataSource(FormAuthenticatorController controller)
            {
                this.controller = controller;
            }

#if !__UNIFIED__
			public override int NumberOfSections (UITableView tableView)
#else
            public override nint NumberOfSections(UITableView tableView)
#endif
            {
                return 2 + (controller.authenticator.CreateAccountLink != null ? 1 : 0);
            }

#if !__UNIFIED__
			public override int RowsInSection (UITableView tableView, int section)
#else
            public override nint RowsInSection(UITableView tableView, nint section)
#endif
            {
                if (section == 0)
                {
                    return controller.authenticator.Fields.Count;
                }
                else
                {
                    return 1;
                }
            }

            FieldCell[] fieldCells = null;

            public void SelectNext()
            {
                for (var i = 0; i < controller.authenticator.Fields.Count; i++)
                {
                    if (fieldCells[i].TextField.IsFirstResponder)
                    {
                        if (i + 1 < fieldCells.Length)
                        {
                            fieldCells[i + 1].TextField.BecomeFirstResponder();
                            return;
                        }
                        else
                        {
                            fieldCells[i].TextField.ResignFirstResponder();
                            controller.HandleSubmit();
                            return;
                        }
                    }
                }
            }

            public void ResignFirstResponder()
            {
                foreach (var cell in fieldCells)
                {
                    cell.TextField.ResignFirstResponder();
                }
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                if (indexPath.Section == 0)
                {
                    if (fieldCells == null)
                    {
                        var fieldXPosition = controller
                            .authenticator
                            .Fields
#if !__UNIFIED__
							.Select (f => tableView.StringSize (f.Title, FieldCell.LabelFont).Width)
#else
                            .Select(f => UIKit.UIStringDrawing.StringSize(f.Title, FieldCell.LabelFont).Width)
#endif
                            .Max();
                        fieldXPosition += 36;

                        fieldCells = controller
                            .authenticator
                            .Fields
#if !__UNIFIED__
							.Select (f => new FieldCell (f, fieldXPosition, SelectNext))
#else
                            .Select(f => new FieldCell(f, fieldXPosition, SelectNext))
#endif
                            .ToArray();
                    }

                    return fieldCells[indexPath.Row];
                }
                else if (indexPath.Section == 1)
                {
                    var cell = tableView.DequeueReusableCell("SignIn");
                    if (cell == null)
                    {
                        cell = new UITableViewCell(UITableViewCellStyle.Default, "SignIn");
                        cell.TextLabel.TextAlignment = UITextAlignment.Center;
                    }

                    cell.TextLabel.Text = NSBundle.MainBundle.LocalizedString("Sign In", "Sign In button title");

                    return cell;
                }
                else
                {
                    var cell = tableView.DequeueReusableCell("CreateAccount");
                    if (cell == null)
                    {
                        cell = new UITableViewCell(UITableViewCellStyle.Default, "CreateAccount");
                        cell.TextLabel.TextAlignment = UITextAlignment.Center;
                    }

                    cell.TextLabel.Text = NSBundle.MainBundle.LocalizedString("Create Account", "Create Account button title");

                    return cell;
                }
            }
        }

    }
}

