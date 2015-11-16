
using System;
using System.Collections.Generic;

#if ! __CLASSIC__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using nint=System.Int32;
#endif

namespace Xamarin.Auth.Sample.XamarinIOS
{
	public class TestProvidersSource : UITableViewSource
	{
		private List<string> _items;
    	private string _section1CellId;
	
		public TestProvidersSource ()
		{
			_section1CellId = "cellid";
 	       _items = new List<string>()
	        {   
				"Facebook OAuth2",
				"Twitter OAuth1",
				"Google OAuth2",
				"Microsoft Live OAuth2",
				"LinkedIn OAuth1",
				"LinkedIn OAuth2",
				"Github OAuth2",
				"Instagram OAuth2", 
	        };
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			// TODO: return the actual number of sections
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			// TODO: return the actual number of items in the section
			return _items.Count;
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
			return "OAuth Providers";
		}

		public override string TitleForFooter (UITableView tableView, nint section)
		{
			return "OAuth Providers";
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (TestProvidersCell.Key) as TestProvidersCell;
			if (cell == null)
				cell = new TestProvidersCell ();
			
			// TODO: populate the cell with the appropriate data based on the indexPath
			cell.TextLabel.Text = _items[indexPath.Row];
			//cell.DetailTextLabel.Text = 
			
			return cell;
		}
	}
}

