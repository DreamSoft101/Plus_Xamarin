using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class PlusMilesViewController : UIViewController
    {
        public PlusMilesViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Plus Miles";
		}
    }
}