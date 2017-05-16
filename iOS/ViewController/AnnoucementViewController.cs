using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class AnnoucementViewController : UIViewController
    {
        public AnnoucementViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Annoucement";
		}
    }
}