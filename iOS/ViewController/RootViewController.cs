using EXPRESSO.Models;
using EXPRESSO.Threads;
using EXPRESSO.Utils;
using Foundation;
using Newtonsoft.Json;
using PLUS.iOS.Helpers;
using System;
using System.Collections.Generic;
using UIKit;
using Xamarin.Auth;

namespace PLUS.iOS
{
    public partial class RootViewController : UIViewController
    {
        public SidebarNavigation.SidebarController SidebarController { get; private set; }
        private UINavigationController homeNav;

        private int currentSelectedRow = -1;
        public RootViewController(IntPtr handle) : base(handle)
        {
            //sideVC = new ViewController(this, new ViewController(), new SideMenuController());
            //InitData();

        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (AccountHelper.getMyEntity() == null)
            {
                UsersThreads thread = new UsersThreads();
                thread.OnLoadAPIKey += (ServiceResult result) =>
                {
                    if (result.intStatus == 1)
                    {
                        try
                        {
                            var lstEntity = new List<MyEntity>();
                            MyEntity myentity = result.Data as MyEntity;
                            myentity.Entity = new EXPRESSO.Models.Database.TblEntities() { idEntity = myentity.idEntity, strName = "PLUS" };
                            myentity.User = new UserInfos();
                            myentity.User.strUserName = "PLUS";
                            lstEntity.Add(myentity);

                            string strJson = JsonConvert.SerializeObject(myentity);
                            NSUserDefaults.StandardUserDefaults.SetString(strJson, "MYENTITY");
                            NSUserDefaults.StandardUserDefaults.Synchronize();
                            //var data = NSUserDefaults.StandardUserDefaults.StringForKey("MYENTITY");


                            loadLayout();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        //Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                    }
                };
                thread.loadEntityInfo(Cons.PLUSEntity.ToString());
            }
            else
            {
                loadLayout();
            }


        }

        private void loadLayout()
        {
            UIStoryboard board = UIStoryboard.FromName("Main", null);
            homeNav = (UINavigationController)board.InstantiateViewController("NavHome");
            LeftMenuViewController leftVC = (LeftMenuViewController)board.InstantiateViewController("LeftMenuViewController");
            leftVC.rootVC = this;

            SidebarController = new SidebarNavigation.SidebarController(this, homeNav, leftVC);
            SidebarController.MenuWidth = 220;
            SidebarController.ReopenOnRotate = false;
            SidebarController.MenuLocation = SidebarNavigation.MenuLocations.Left;

        }

     

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public void selectLeftMenuItem(int idx)
        {
            Console.WriteLine("click left menu: " + idx);
            if (idx == currentSelectedRow)
            {
                SidebarController.ToggleMenu();
                return;
            }
            currentSelectedRow = idx;
            UIStoryboard board = UIStoryboard.FromName("Main", null);
            switch (idx)
            {
                case 0:
                    {
                        if (AccountHelper.getMyEntity() != null)
                        {
                          
                            if (string.IsNullOrEmpty(AccountHelper.getMyEntity().User.strToken))
                            {
                                currentSelectedRow = -1;
                                var vc = board.InstantiateViewController("LoginController");
                                //this.NavigationController.PushViewController(vc, true);
                                homeNav.PushViewController(vc, true);
                                SidebarController.CloseMenu();
                            }
                            else
                            {
                                board = UIStoryboard.FromName("nthoa", null);
                                if (AccountHelper.getMyEntity().User.LoyaltyAccount.MemberProfile == null)
                                {
                                    currentSelectedRow = -1;
                                    var navLinkMasterAccountController = board.InstantiateViewController("LinkMasterAccountController");
                                    homeNav.PushViewController(navLinkMasterAccountController, true);
                                    SidebarController.CloseMenu();
                                    return;
                                }
                                else if (AccountHelper.getMyEntity().User.LoyaltyAccount.MemberProfile.idMasterAccount == 0)
                                {
                                    currentSelectedRow = -1;
                                    var navLinkMasterAccountController = board.InstantiateViewController("LinkMasterAccountController");
                                    homeNav.PushViewController(navLinkMasterAccountController, true);
                                    SidebarController.CloseMenu();
                                    return;
                                }

                                var nav = board.InstantiateViewController("navMyAccount");
                                SidebarController.ChangeContentView(nav);
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        UINavigationController nav = (UINavigationController)board.InstantiateViewController("NavTollFare");
                        SidebarController.ChangeContentView(nav);

                    }
                    break;
                case 2:
                    {
                        UINavigationController nav = (UINavigationController)board.InstantiateViewController("NavNotifications");
                        SidebarController.ChangeContentView(nav);

                    }
                    break;
                case 3:
                    {
                        UINavigationController nav = (UINavigationController)board.InstantiateViewController("NavUpdate");
                        SidebarController.ChangeContentView(nav);

                    }
                    break;
                case 4:
                    {
                        UINavigationController nav = (UINavigationController)board.InstantiateViewController("NavTwitter");
                        SidebarController.ChangeContentView(nav);

                    }
                    break;
                case 5:
                    {
                        UINavigationController nav = (UINavigationController)board.InstantiateViewController("NavEmagazine");
                        SidebarController.ChangeContentView(nav);

                    }
                    break;
                case 6:
                    {
                        UINavigationController nav = (UINavigationController)board.InstantiateViewController("NavMore");
                        SidebarController.ChangeContentView(nav);

                    }
                    break;
                case 7:
                    {
                        LoginController vc = (LoginController)board.InstantiateViewController("LoginController");
                        //this.NavigationController.PushViewController(vc, true);
                        SidebarController.ChangeContentView(vc);
                    }
                    break;
                case 8:
                    {



                    }
                    break;
            }
        }
    }
}