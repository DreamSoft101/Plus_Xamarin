using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Loyalty.Utils;
using EXPRESSO.Models;
using EXPRESSO.Threads;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Plus;
using Xamarin.Auth;
using System.Threading;
using Android.Gms.Auth;
using Newtonsoft.Json;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "PLUSRangerLogin")]
    public class PLUSRangerLogin : BaseActivity
    {

        private EditText mTxtUserName;
        private EditText mTxtPassword;
        private Button mBtnLogin;
        private TextView mTxtForgetpassword;

        private int mIntRequestRequest = 99;

        private SignInButton mGoogleSignIn;
        private GoogleApiClient mGoogleApiClient;
        private bool mIsResolving = false;
        private bool mShouldResolve = false;
        private ImageView mImgGPlus;
        private ProgressBar mPrbLoading;

        private const int RC_SIGN_IN = 9001;


        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.exp_activity_plusranger_login;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = GetString(Resource.String.title_login);
         
            mBtnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            
            mTxtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            mTxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mTxtForgetpassword = FindViewById<TextView>(Resource.Id.txtforgetpassword);
            mPrbLoading = FindViewById<ProgressBar>(Resource.Id.prbLoading);
            mPrbLoading.Visibility = ViewStates.Gone;
            mBtnLogin.Click += MBtnLogin_Click;
            mTxtForgetpassword.Click += MTxtForgetpassword_Click;
            FindViewById<View>(Resource.Id.lbeRegister).Click += RegisterActivity_Click;

            FindViewById<View>(Resource.Id.lnlGPlus).Click += GplusLogin_Click;
            FindViewById<View>(Resource.Id.lnlFB).Click += FBLogin_Click;



        }

        private void GplusLogin_Click(object sender, EventArgs e)
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

            
            auth.Completed += (s, ee) => {
                if (ee.IsAuthenticated)
                {
                    RunOnUiThread(new Action(() =>
                    {
                        mPrbLoading.Visibility = ViewStates.Visible;
                        mBtnLogin.Clickable = false;
                    }));
                 

                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        var accessToken = ee.Account.Properties["access_token"].ToString();
                        
                        UsersThreads thread = new UsersThreads();
                        thread.OnLoginSusscess += (ServiceResult resultEntity, ServiceResult result) =>
                        {
                            try
                            {
                                mBtnLogin.Clickable = true;
                                mPrbLoading.Visibility = ViewStates.Gone;
                            }
                            catch (Exception ex)
                            {

                            }
                            if (result.intStatus == 1)
                            {
                                RunOnUiThread(new Action(() =>
                                {
                                    MyEntity myentity = resultEntity.Data as MyEntity;
                                    var lstMyEntity = new List<MyEntity>();
                                    myentity.Entity = new EXPRESSO.Models.Database.TblEntities() { idEntity = EXPRESSO.Utils.Cons.PLUSEntity.ToString(), strName = "PLUS" };
                                    myentity.User = result.Data as UserInfos;
                                    EXPRESSO.Utils.Cons.myEntity = myentity;
                                    lstMyEntity.Add(myentity);
                                    saveMyEntity(lstMyEntity);
                                    saveCurrentMyEntity(myentity);
                                    //Cons.IdEntity = myentity.idEntity;
                                    string jsonData = JsonConvert.SerializeObject((result.Data as UserInfos).LoyaltyAccount);
                                    ((Loyalty.Droid.Activities.BaseActivity)this).setCacheString(MyAuth, jsonData);


                                    Finish();
                                }));

                            }
                            else
                            {
                                Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                            }
                        };
                        thread.doLoginSocial(EXPRESSO.Utils.Cons.PLUSEntity.ToString(), "Google", accessToken);

                    });


                }

            };

            StartActivity(auth.GetUI(this));
        }

        private void FBLogin_Click(object sender, EventArgs e)
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
                    RunOnUiThread(new Action(() =>
                    {
                        mPrbLoading.Visibility = ViewStates.Visible;
                        mBtnLogin.Clickable = false;
                    }));
                    ThreadPool.QueueUserWorkItem(o =>
                {
                    var accessToken = ee.Account.Properties["access_token"].ToString();

                    UsersThreads thread = new UsersThreads();
                    thread.OnLoginSusscess += (ServiceResult resultEntity, ServiceResult result) =>
                    {

                       
                        try
                        {
                            mBtnLogin.Clickable = true;
                            mPrbLoading.Visibility = ViewStates.Gone;
                        }
                        catch (Exception ex)
                        {

                        }
                      
                        if (result.intStatus == 1)
                        {
                            MyEntity myentity = resultEntity.Data as MyEntity;
                                //List<MyEntity> lstMyEntity = getMyEntity();
                                var lstMyEntity = new List<MyEntity>();
                            myentity.Entity = new EXPRESSO.Models.Database.TblEntities() { idEntity = EXPRESSO.Utils.Cons.PLUSEntity.ToString(), strName = "PLUS" };
                            myentity.User = result.Data as UserInfos;
                            EXPRESSO.Utils.Cons.myEntity = myentity;
                            //Cons.IdEntity = myentity.idEntity;
                            lstMyEntity.Add(myentity);
                            saveMyEntity(lstMyEntity);
                            saveCurrentMyEntity(myentity);



                            string jsonData = JsonConvert.SerializeObject((result.Data as UserInfos).LoyaltyAccount);
                            ((Loyalty.Droid.Activities.BaseActivity)this).setCacheString(MyAuth, jsonData);


                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                        }
                    };
                    thread.doLoginSocial(EXPRESSO.Utils.Cons.PLUSEntity.ToString(), "Facebook", accessToken);

                });


                }

            };
            StartActivity(auth.GetUI(this));
        }

        private void MTxtForgetpassword_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Intent intent = new Intent(this, typeof(ForgetpasswordActivity));
            StartActivity(intent);
        }

        private void RegisterActivity_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PLUSRangerRegisterAcitivity));
            StartActivityForResult(intent, mIntRequestRequest);
        }

        private void MBtnLogin_Click(object sender, EventArgs e)
        {
           
            string strUserName = mTxtUserName.Text;
            string strPassword = mTxtPassword.Text;

            mBtnLogin.Clickable = false;
            mPrbLoading.Visibility = ViewStates.Visible;
            UsersThreads thread = new UsersThreads();
            thread.OnLoginSusscess += (ServiceResult resultEntity, ServiceResult resultLogin) =>
            {
                if (resultEntity.intStatus == 1)
                {
                    mBtnLogin.Clickable = true;
                    mPrbLoading.Visibility = ViewStates.Gone;
                    if (resultLogin.intStatus == 1)
                    {
                        
                        MyEntity myentity = resultEntity.Data as MyEntity;
                        //List<MyEntity> lstMyEntity = getMyEntity();
                        var lstMyEntity = new List<MyEntity>();
                        myentity.Entity = new EXPRESSO.Models.Database.TblEntities() { idEntity = EXPRESSO.Utils.Cons.PLUSEntity.ToString(), strName = "PLUS" };
                        myentity.User = resultLogin.Data as UserInfos;
                        EXPRESSO.Utils.Cons.myEntity = myentity;
                        //Cons.IdEntity = myentity.idEntity;
                        lstMyEntity.Add(myentity);
                        saveMyEntity(lstMyEntity);
                        saveCurrentMyEntity(myentity);

                        string jsonData = JsonConvert.SerializeObject((resultLogin.Data as UserInfos).LoyaltyAccount);
                        ((Loyalty.Droid.Activities.BaseActivity)this).setCacheString(MyAuth, jsonData);

                        Cons.mMemberCredentials = (resultLogin.Data as UserInfos).LoyaltyAccount;

                        Finish();
                    }
                    else
                    {
                        Toast.MakeText(this, resultLogin.strMess, ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, resultEntity.strMess, ToastLength.Short).Show();
                }

            };
            thread.doLogin(EXPRESSO.Utils.Cons.PLUSEntity.ToString(), strUserName, strPassword);

        }

    }
}