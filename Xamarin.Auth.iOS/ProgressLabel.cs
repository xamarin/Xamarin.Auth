using System;
using MonoTouch.UIKit;
using System.Drawing;

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
				Frame = new RectangleF (25, 0, Frame.Width - 25, 44),
			};
			AddSubview (label);
			
			var f = Frame;
			f.Width = label.Frame.X + label.StringSize (label.Text, label.Font).Width;
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

