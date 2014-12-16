using System;
using System.Drawing;

#if __UNIFIED__
using UIKit;
using CoreGraphics;
#else
using MonoTouch.UIKit;
using CGRect = global::System.Drawing.RectangleF;
#endif

namespace Xamarin.Controls
{
	internal class ProgressLabel : UIView
	{
		UIActivityIndicatorView activity;
		
		public ProgressLabel (string text)
			: base (new RectangleF (0, 0, 200, 44))
		{
			BackgroundColor = UIColor.Clear;
			
			activity = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White) {
				Frame = new RectangleF (0, 11.5f, 21, 21),
				HidesWhenStopped = false,
				Hidden = false,
			};
			AddSubview (activity);
			
			var label = new UILabel () {
				Text = text,
				TextColor = UIColor.White,
				Font = UIFont.BoldSystemFontOfSize (20),
				BackgroundColor = UIColor.Clear,
				Frame = new CGRect (25, 0, Frame.Width - 25, 44),
			};
			AddSubview (label);
			
			var f = Frame;
			f.Width = label.Frame.X + UIStringDrawing.StringSize (label.Text, label.Font).Width;
			Frame = f;
		}
		
		public void StartAnimating ()
		{
			activity.StartAnimating ();
		}
		
		public void StopAnimating ()
		{
			activity.StopAnimating ();
		}
	}
}

