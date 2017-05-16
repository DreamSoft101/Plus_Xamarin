using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class NotificationViewController : BaseViewController
    {
        public NotificationViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Notifications";
		}
    }
}