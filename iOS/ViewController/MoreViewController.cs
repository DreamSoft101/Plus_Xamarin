using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class MoreViewController : UIViewController
    {
        public MoreViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "More";
		}

    }
}