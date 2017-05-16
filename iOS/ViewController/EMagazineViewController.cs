using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class EMagazineViewController : UIViewController
    {
        public EMagazineViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "E-Magazine";
		}
    }
}