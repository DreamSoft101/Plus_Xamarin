using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class LiveTrafficViewController : UIViewController
    {
        public LiveTrafficViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Live Traffic";
		}
    }
}