using System;

using UIKit;

namespace PLUS.iOS
{
	public partial class ViewController : UIViewController
	{
		int count = 1;
		public ViewController sideVC { get; private set; }

		public ViewController(IntPtr handle) : base(handle)
		{
			//sideVC = new ViewController(this, new ViewController(), new SideMenuController());
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Perform any additional setup after loading the view, typically from a nib.
			Button.AccessibilityIdentifier = "myButton";
			Button.TouchUpInside += delegate
			{
				var title = string.Format("{0} clicks!", count++);
				Button.SetTitle(title, UIControlState.Normal);
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.		
		}
	}
}
