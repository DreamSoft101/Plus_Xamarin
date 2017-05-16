using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class UpdateViewController : UIViewController
    {
        public UpdateViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Updates";
		}
    }
}