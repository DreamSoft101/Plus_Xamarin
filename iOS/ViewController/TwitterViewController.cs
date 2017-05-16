using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class TwitterViewController : UIViewController
    {
        public TwitterViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Twitter";
		}
    }
}