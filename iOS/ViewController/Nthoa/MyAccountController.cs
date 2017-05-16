using Foundation;
using PLUS.iOS.Sources.PageViews;
using System;
using UIKit;
using System.Collections.Generic;

namespace PLUS.iOS
{
    public partial class MyAccountController : UIPageViewController
    {
        public MyAccountController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "Account Information";

            var board1 = UIStoryboard.FromName("nthoa", null);
            var controll1 = board1.InstantiateViewController("ListAccountController");
            var board2 = UIStoryboard.FromName("duc", null);
            var controll2 = board2.InstantiateViewController("RedeemHistoryViewController");


            var listView = new List<UIViewController>() { controll1, controll2 };

            MyAccountSource source = new MyAccountSource(this,
                                                        new List<string>() { "My Account", "Redemption History" },
                                                        listView
                                                         );
            this.DataSource = source;
            this.SetViewControllers(new UIViewController[] { controll1 }, UIPageViewControllerNavigationDirection.Forward, true, null);
            //var content = Instantiate("MainStoryboard", "TutorialContentViewController") as TutorialContentViewController;
        }
    }
}