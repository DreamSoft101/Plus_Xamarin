using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class TollPlazaViewController : UIViewController
    {
        public TollPlazaViewController (IntPtr handle) : base (handle)
        {
			
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Toll Plaza";
		}
    }
}