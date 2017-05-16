using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class FacilitiesViewController : UIViewController
    {
        public FacilitiesViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Facility";
		}
    }
}