using System;
using System.Drawing;

#if ! __UNIFIED__
using MonoTouch.UIKit;
#else
using UIKit;
#endif

#if ! AZURE_MOBILE_SERVICES
namespace Xamarin.Controls
#else
namespace Xamarin.Controls._MobileServices
#endif
{
    internal class ProgressLabel : UIView
    {
        UIActivityIndicatorView activity;

        public ProgressLabel(string text)
            : base(new RectangleF(0, 0, 200, 44))
        {
            BackgroundColor = UIColor.Clear;

            activity = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White)
            {
                Frame = new RectangleF(0, 11.5f, 21, 21),
                HidesWhenStopped = false,
                Hidden = false,
            };
            AddSubview(activity);

            var label = new UILabel()
            {
                Text = text,
                TextColor = UIColor.White,
                Font = UIFont.BoldSystemFontOfSize(20),
                BackgroundColor = UIColor.Clear,
#if !__UNIFIED__
				Frame = new RectangleF (25, 0, Frame.Width - 25, 44),
#else
                Frame = new CoreGraphics.CGRect(25, 0, Frame.Width - 25, 44),
#endif
            };
            AddSubview(label);

            var f = Frame;
#if !__UNIFIED__
			f.Width = label.Frame.X + label.StringSize (label.Text, label.Font).Width;
#else
            f.Width = label.Frame.X + UIStringDrawing.StringSize(label.Text, label.Font).Width;
#endif
            Frame = f;
        }

        public void StartAnimating()
        {
            activity.StartAnimating();
        }

        public void StopAnimating()
        {
            activity.StopAnimating();
        }
    }
}

