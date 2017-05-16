using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class RSAViewController : UIViewController
    {
        public RSAViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "RSA & Lay-by";
		}
    }
}