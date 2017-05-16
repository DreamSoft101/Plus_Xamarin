using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class TollFareViewController : BaseViewController
    {
        public TollFareViewController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Toll Fare";
		}
    }
}