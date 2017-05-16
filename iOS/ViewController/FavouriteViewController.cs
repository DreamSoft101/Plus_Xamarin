using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class FavouriteViewController : UIViewController
    {
        public FavouriteViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Favourites";
		}
    }
}