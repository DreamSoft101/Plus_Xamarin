using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using Google.SignIn;
using Xamarin.Auth;
using EXPRESSO.Threads;
using EXPRESSO.Models;
using Newtonsoft.Json;
using ToastIOS;
using System.Threading;
using EXPRESSO.Utils;
using PLUS.iOS.Helpers;

namespace PLUS.iOS
{
    public partial class LoginController : BaseViewController
    {
        List<string> readPermissions = new List<string> { "public_profile" };
        private UIStoryboard board = UIStoryboard.FromName("Main", null);
        public LoginController (IntPtr handle) : base (handle)
        {

        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.Title = "Login";

            var imageView = new UIImageView(new CGRect(0, 5, 30, 20));
            imageView.Image = UIImage.FromFile("ic_username.png");
            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;


            //txtEmail.BackgroundColor = UIColor.Brown;
            txtEmail.RightViewMode = UITextFieldViewMode.Always;
            txtEmail.RightView = imageView;

            var imageViewPassword = new UIImageView(new CGRect(0, 5, 30, 20));
            imageViewPassword.Image = UIImage.FromFile("ic_pwd.png");
            imageViewPassword.ContentMode = UIViewContentMode.ScaleAspectFit;
            txtPassword.RightViewMode = UITextFieldViewMode.Always;
            txtPassword.RightView = imageViewPassword;


            //btnLoginGPlus

            var buttonRect = UIButton.FromType(UIButtonType.RoundedRect);

            btnLoginGPlus.SetBackgroundImage(UIImage.FromFile("ic_google_circle.png"), UIControlState.Normal);
            btnLoginFB.SetBackgroundImage(UIImage.FromFile("ic_facebook_circle.png"), UIControlState.Normal);

            



        }
      
        public void LoginSuccess()
        {
            //this.NavigationController.RemoveFromParentViewController();
            this.NavigationController.ViewControllers[1] = this;
            var myentity = AccountHelper.getMyEntity();
            if (myentity.User.LoyaltyAccount.MemberProfile == null)
            {
                var navLinkMasterAccountController = board.InstantiateViewController("LinkMasterAccountController");
                
                this.NavigationController.PushViewController(navLinkMasterAccountController, true);
                return;
            }
            if (myentity.User.LoyaltyAccount.MemberProfile.idMasterAccount == 0)
            {
                var navLinkMasterAccountController = board.InstantiateViewController("LinkMasterAccountController");
                this.NavigationController.PushViewController(navLinkMasterAccountController, true);
                return;
            }

            var home  = board.InstantiateViewController("HomeViewController");
            this.NavigationController.PushViewController(home, true);
            //this.DismissViewController(true, null);
        }

        partial void btnLoginClick(UIButton sender)
        {
            string strUserName = txtEmail.Text;
            string strPassword = txtPassword.Text;

            this.showLoading();

            btnLoginFB.Enabled = false;
     
            UsersThreads thread = new UsersThreads();
            thread.OnLoginSusscess += (ServiceResult resultEntity, ServiceResult resultLogin) =>
            {
                if (resultEntity.intStatus == 1)
                {
                    btnLoginFB.Enabled = true;
                    this.hideLoading();
                    if (resultLogin.intStatus == 1)
                    {

                        MyEntity myentity = resultEntity.Data as MyEntity;
                        myentity.Entity = new EXPRESSO.Models.Database.TblEntities() { idEntity = EXPRESSO.Utils.Cons.PLUSEntity.ToString(), strName = "PLUS" };
                        myentity.User = resultLogin.Data as UserInfos;
                        AccountHelper.setMyEntity(myentity);
                        LoginSuccess();


                    }
                    else
                    {
                    }
                }
                else
                {
                }

            };
            thread.doLogin(EXPRESSO.Utils.Cons.PLUSEntity.ToString(), strUserName, strPassword);
        }

        partial void btnLoginFBClick(UIButton sender)
        {
            var auth = new OAuth2Authenticator(
                          clientId: "318581368494042",
                          scope: "email, user_about_me, user_birthday, user_hometown",
                          authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                          redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));


            auth.Completed += (s, ee) =>
            {
                if (ee.IsAuthenticated)
                {
                    this.showLoading();
                    var accessToken = ee.Account.Properties["access_token"].ToString();
                    UsersThreads thread = new UsersThreads();
                    thread.OnLoginSusscess += (ServiceResult resultEntity, ServiceResult result) =>
                    {
                        this.hideLoading();
                        if (result.intStatus == 1)
                        {
                           
                            MyEntity myentity = resultEntity.Data as MyEntity;
                            myentity.Entity = new EXPRESSO.Models.Database.TblEntities() { idEntity = EXPRESSO.Utils.Cons.PLUSEntity.ToString(), strName = "PLUS" };
                            myentity.User = result.Data as UserInfos;
                            AccountHelper.setMyEntity(myentity);
                            LoginSuccess();
                        }
                        else
                        {
                            Toast.MakeText(result.strMess).Show();
                        }
                    };
                    thread.doLoginSocial(EXPRESSO.Utils.Cons.PLUSEntity + "", "Facebook", accessToken);
                }
            };
            PresentViewController(auth.GetUI(), true, null);
        }

        partial void btnLoginGPlusClick(UIButton sender)
        {
            var auth = new OAuth2Authenticator(
                clientId: "419784069151-jtdcf5kq8gnhpas5td07n002fuq02l7g.apps.googleusercontent.com",
                clientSecret: "XUURq8PFN_3peRK06bAobC1n",
                scope: "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email",
                authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
                redirectUrl: new Uri("http://expresso.expresso.cloud/auth/socialendpoint?hauth.done=nthoa"),
                accessTokenUrl: new Uri("https://accounts.google.com/o/oauth2/token"),
                getUsernameAsync: null
            );


            auth.Completed += (s, ee) =>
            {
                if (ee.IsAuthenticated)
                {
                    this.showLoading();
                    var accessToken = ee.Account.Properties["access_token"].ToString();
                    UsersThreads thread = new UsersThreads();
                    thread.OnLoginSusscess += (ServiceResult resultEntity, ServiceResult result) =>
                    {
                        this.hideLoading();
                        if (result.intStatus == 1)
                        {

                            MyEntity myentity = resultEntity.Data as MyEntity;
                            myentity.Entity = new EXPRESSO.Models.Database.TblEntities() { idEntity = EXPRESSO.Utils.Cons.PLUSEntity.ToString(), strName = "PLUS" };
                            myentity.User = result.Data as UserInfos;
                            EXPRESSO.Utils.Cons.myEntity = myentity;
                            AccountHelper.setMyEntity(myentity);
                            LoginSuccess();
                        }
                        else
                        {
                            Toast.MakeText(result.strMess).Show();
                        }
                    };
                    thread.doLoginSocial(EXPRESSO.Utils.Cons.PLUSEntity + "", "Google", accessToken);
                }
            };
            PresentViewController(auth.GetUI(), true, null);
        }
        

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}