using EXPRESSO.Models;
using Foundation;
using Newtonsoft.Json;
using PLUS.iOS.Helpers;
using System;

using UIKit;

namespace PLUS.iOS
{
    public partial class BaseViewController : UIViewController
    {
        LoadingOverlay loadingOverlay;
        public void showLoading()
        {
            var bounds = UIScreen.MainScreen.Bounds;
            loadingOverlay = new LoadingOverlay(bounds);
            View.Add(loadingOverlay);
        }
        public void hideLoading()
        {
            loadingOverlay.Hide();
        }

        public MyEntity getMyEntity()
        {
            NSUserDefaults.StandardUserDefaults.Synchronize();
            var data = NSUserDefaults.StandardUserDefaults.StringForKey("MYENTITY");
            return JsonConvert.DeserializeObject<MyEntity>(data);
        }

        // provide access to the sidebar controller to all inheriting controllers
        protected SidebarNavigation.SidebarController SidebarController
        {
            get
            {
                return (UIApplication.SharedApplication.KeyWindow.RootViewController as RootViewController).SidebarController;

            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (this.NavigationController != null)
            {
                var navigationItem = this.NavigationController.TopViewController.NavigationItem;
                navigationItem.SetLeftBarButtonItem(
                    new UIBarButtonItem(UIImage.FromBundle("ic_menu")
                        , UIBarButtonItemStyle.Plain
                        , (sender, args) => {
                            SidebarController.ToggleMenu();
                        }), true);
            }
        }

        public BaseViewController(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}