using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class LiveFeedViewController : UIViewController
    {
        public LiveFeedViewController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Live Feed";
		}
    }
}