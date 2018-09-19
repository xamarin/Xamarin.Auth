using CoreGraphics;
using Foundation;
using System;
using System.Linq;
using UIKit;

namespace Xamarin.Auth
{
    internal class FormAuthenticatorController : UITableViewController
    {
        private FormAuthenticator authenticator;
        private ProgressLabel progress;

        public FormAuthenticatorController(FormAuthenticator authenticator)
            : base(UITableViewStyle.Grouped)
        {
            this.authenticator = authenticator;

            Title = authenticator.Title;

            TableView.DataSource = new FormDataSource(this);
            TableView.Delegate = new FormDelegate(this);

            if (authenticator.AllowCancel)
            {
                NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, delegate
                {
                    StopProgress();
                    authenticator.OnCancelled();
                });
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (authenticator.AllowCancel && authenticator.IsAuthenticated())
                authenticator.OnCancelled();
        }

        private async void HandleSubmit()
        {
            if (progress == null)
            {
                progress = new ProgressLabel(NSBundle.MainBundle.GetLocalizedString("Verifying"));
                NavigationItem.TitleView = progress;
                progress.StartAnimating();
            }

            try
            {
                var result = await authenticator.SignInAsync();
                StopProgress();
                authenticator.OnSucceeded(result);
            }
            catch (Exception ex)
            {
                StopProgress();
                if (authenticator.ShowErrors)
                    this.ShowError("Error Signing In", ex);
            }
        }

        private void StopProgress()
        {
            NavigationItem.TitleView = null;
            progress?.StopAnimating();
            progress = null;
        }

        private class FormDelegate : UITableViewDelegate
        {
            private FormAuthenticatorController controller;

            public FormDelegate(FormAuthenticatorController controller)
            {
                this.controller = controller;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                tableView.ResignFirstResponder();
                tableView.DeselectRow(indexPath, true);

                if (indexPath.Section == 1)
                {
                    ((FormDataSource)tableView.DataSource).ResignFirstResponder();
                    controller.HandleSubmit();
                }
                else if (indexPath.Section == 2)
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(controller.authenticator.CreateAccountLink.AbsoluteUri));
                }
            }
        }

        private class FieldCell : UITableViewCell
        {
            public static readonly UIFont LabelFont = UIFont.BoldSystemFontOfSize(16);
            public static readonly UIFont FieldFont = UIFont.SystemFontOfSize(16);

            private static readonly UIColor FieldColor = UIColor.FromRGB(56, 84, 135);

            public UITextField TextField { get; private set; }

            public FieldCell(FormAuthenticatorField field, nfloat fieldXPosition, Action handleReturn)
                : base(UITableViewCellStyle.Default, "Field")
            {
                SelectionStyle = UITableViewCellSelectionStyle.None;
                TextLabel.Text = field.Title;

                var hang = 3;
                var h = FieldFont.PointSize + hang;
                var cellSize = Frame.Size;
                var frame = new CGRect(fieldXPosition, (cellSize.Height - h) / 2, cellSize.Width - fieldXPosition - 12, h);

                TextField = new UITextField(frame)
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

        private class FormDataSource : UITableViewDataSource
        {
            private FormAuthenticatorController controller;
            private FieldCell[] fieldCells = null;

            public FormDataSource(FormAuthenticatorController controller)
            {
                this.controller = controller;
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return 2 + (controller.authenticator.CreateAccountLink != null ? 1 : 0);
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                if (section == 0)
                    return controller.authenticator.Fields.Count;
                else
                    return 1;
            }

            public void SelectNext()
            {
                for (var i = 0; i < controller.authenticator.Fields.Count; i++)
                {
                    if (fieldCells[i].TextField.IsFirstResponder)
                    {
                        if (i + 1 < fieldCells.Length)
                        {
                            fieldCells[i + 1].TextField.BecomeFirstResponder();
                        }
                        else
                        {
                            fieldCells[i].TextField.ResignFirstResponder();
                            controller.HandleSubmit();
                        }
                        break;
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
                        var fieldXPosition = controller.authenticator.Fields
                            .Select(f => UIStringDrawing.StringSize(f.Title, FieldCell.LabelFont).Width)
                            .Max();
                        fieldXPosition += 36;

                        fieldCells = controller.authenticator.Fields
                            .Select(f => new FieldCell(f, fieldXPosition, SelectNext))
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

                    cell.TextLabel.Text = NSBundle.MainBundle.GetLocalizedString("Sign In");

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

                    cell.TextLabel.Text = NSBundle.MainBundle.GetLocalizedString("Create Account");

                    return cell;
                }
            }
        }
    }
}
