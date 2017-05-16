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
using Android.Content.PM;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Newtonsoft.Json;
using Android.Gms.Plus.Model.People;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;
using Android.Gms.Auth;
using Dex.Com.Expresso.Loyalty.Droid.Dialogs;
using System.Threading;
using Loyalty.Utils;
using Xamarin.Auth;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "LoginActivity")]
   // [Activity(Label = "LoginActivity", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/Icon")]
    public class LoginActivity : BaseActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {

        public static string LOGINWITH = "LOGINWITH";
        public static int LOGINWITH_FB = 1;
        public static int LOGINWITH_GP = 2;
        public static int LOGINWITH_REGISTER = 3;

        private Spinner mSpnEntity;
        private EditText mTxtUserName;
        private EditText mTxtPassword;
        private Button mBtnLogin;
        private Button mBtnGuest;
        private SignInButton mGoogleSignIn;
        private const int mIntRequestRequest = 99;
        private const int RC_SIGN_IN = 9001;
        private GoogleApiClient mGoogleApiClient;
        private bool mIsResolving = false;
        private bool mShouldResolve = false;
        private ImageView mImgGPlus;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.Title = GetString(Resource.String.loy_title_login);
            mBtnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            mTxtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            mTxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);

            mBtnLogin.Click += MBtnLogin_Click;
            FindViewById<View>(Resource.Id.lbeRegister).Click += LoginActivity_Click;
            FindViewById<View>(Resource.Id.imgGPlus).Click += GplusLogin_Click;
            FindViewById<View>(Resource.Id.imgFB).Click += FBLogin_Click;
            // Create your application here

            mGoogleApiClient = new GoogleApiClient.Builder(this)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(PlusClass.API)
                .AddScope(new Scope(Scopes.Profile))
                .Build();

            int loginWith = this.Intent.GetIntExtra(LOGINWITH,-1);
            if (loginWith == LOGINWITH_FB)
            {
                FBLogin_Click(null, null);
            }
            else if (loginWith == LOGINWITH_GP)
            {
                GplusLogin_Click(null, null);
            }
            else if (loginWith == LOGINWITH_REGISTER)
            {
                LoginActivity_Click(null, null);
            }
        }

        private void FBLogin_Click(object sender, EventArgs e)
        {
            var auth = new OAuth2Authenticator(
                         clientId: "176502389360054",
                         scope: "",
                         authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                         redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

                auth.Completed += (s, ee) => {
                if (ee.IsAuthenticated)
                {
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        var accessToken = ee.Account.Properties["access_token"].ToString();
                        UserThreads userthread = new UserThreads();
                        userthread.OnResult += (ServiceResult result) =>
                        {
                            if (result.StatusCode == 1)
                            {
                                MValidateMemberCredentials dataCredential = result.Data as MValidateMemberCredentials;
                                string jsonData = JsonConvert.SerializeObject(dataCredential);
                                setCacheString(MyAuth, jsonData);
                                Cons.mMemberCredentials = dataCredential;
                                Finish();
                            }
                            else
                            {
                                Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                            }
                        };
                        userthread.Login( "", accessToken, "");
                    });


                }

            };
            StartActivity(auth.GetUI(this));
        }

        private void GplusLogin_Click(object sender, EventArgs e)
        {
            mShouldResolve = true;
            mGoogleApiClient.Connect();
        }

        protected override void OnStart()
        {
            base.OnStart();
            mGoogleApiClient.Connect();
        }

        protected override void OnStop()
        {
            base.OnStop();
            mGoogleApiClient.Disconnect();
        }

        private void LoginActivity_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));
            StartActivityForResult(intent, mIntRequestRequest);
        }

        private void MBtnLogin_Click(object sender, EventArgs e)
        {
            string strUserName = mTxtUserName.Text;
            string strPassword = mTxtPassword.Text;
            UserThreads thread = new UserThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.StatusCode == 1)
                {
                    MValidateMemberCredentials data = result.Data as MValidateMemberCredentials;

                   

                    string jsonData = JsonConvert.SerializeObject(data);
                    setCacheString(MyAuth, jsonData);
                    Cons.mMemberCredentials = data;
                    //Intent intentResult = new Intent();
                    //intentResult.PutExtra()
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.Login( strUserName,"", strPassword);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntRequestRequest)
            {
                if (resultCode == Result.Ok)
                {
                    
                    Toast.MakeText(this, Resource.String.loy_reg_mess_success, ToastLength.Short).Show();

                    int type = data.Extras.GetInt(RegisterActivity.TYPE);

                    if (type == 0)
                    {
                        string username = data.Extras.GetString(RegisterActivity.USERNAME);
                        string password = data.Extras.GetString(RegisterActivity.PASSWORD);

                        mTxtUserName.Text = username;
                        mTxtPassword.Text = password;
                    }
                    else
                    {
                        string token = data.Extras.GetString(RegisterActivity.TOKEN);
                        UserThreads thread = new UserThreads();
                        thread.OnResult += (ServiceResult result) =>
                        {
                            if (result.StatusCode == 1)
                            {
                                MValidateMemberCredentials dataCredential = result.Data as MValidateMemberCredentials;
                                string jsonData = JsonConvert.SerializeObject(dataCredential);
                                setCacheString(MyAuth, jsonData);
                                Finish();
                            }
                            else
                            {
                                Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                            }
                            
                        };
                        thread.Login( "", token, "");
                    }
                    
                }
            }
            else if (requestCode == RC_SIGN_IN)
            {
                if (resultCode != Result.Ok)
                {
                    mShouldResolve = false;
                }

                mIsResolving = false;
                mGoogleApiClient.Connect();
            }
        }


        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_login;
            }
        }


        private void LoginGPlus()
        {
            var person = PlusClass.AccountApi.GetAccountName(mGoogleApiClient);
            //string scopes = "oauth2:" + Scopes.Profile;
            string scopes = "oauth2:" + Scopes.Profile;
            //string token = GoogleAuthUtil.GetToken(this, person, scopes);

            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    string token = GoogleAuthUtil.GetToken(this, person, scopes);
                    if (mGoogleApiClient.IsConnected)
                    {
                        PlusClass.AccountApi.ClearDefaultAccount(mGoogleApiClient);
                        mGoogleApiClient.Disconnect();
                    }
                    UserThreads userthread = new UserThreads();
                    userthread.OnResult += (ServiceResult result) =>
                    {
                        if (result.StatusCode == 1)
                        {
                            MValidateMemberCredentials dataCredential = result.Data as MValidateMemberCredentials;
                            string jsonData = JsonConvert.SerializeObject(dataCredential);
                            setCacheString(MyAuth, jsonData);
                            Cons.mMemberCredentials = dataCredential;
                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                        }
                    };
                    userthread.Login( "", token, "");
                }
                catch (Exception e)
                {
                    LogUtils.WriteError("Get GPlus Token", e.Message);
                    // Network or server error.
                    //throw;
                }
               
            });
            
        }

        private void LoginWithToken(int type, string token)
        {
            UserThreads thread = new UserThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.StatusCode == 1)
                {
                    MValidateMemberCredentials data = result.Data as MValidateMemberCredentials;
                    string jsonData = JsonConvert.SerializeObject(data);
                    setCacheString(MyAuth, jsonData);
                    Cons.mMemberCredentials = data;
                    //Intent intentResult = new Intent();
                    //intentResult.PutExtra()
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.Login( "", token,"");
        }

        public void OnConnected(Bundle connectionHint)
        {
            //throw new NotImplementedException();
            //Login();
            LoginGPlus();
        }
        public void OnConnectionSuspended(int cause)
        {
            //throw new NotImplementedException();
        }

        

        public void OnConnectionFailed(ConnectionResult result)
        {
            if (!mIsResolving && mShouldResolve)
            {
                if (result.HasResolution)
                {
                    try
                    {
                        result.StartResolutionForResult(this, RC_SIGN_IN);
                        mIsResolving = true;
                    }
                    catch (IntentSender.SendIntentException e)
                    {
                        //Log.Error(TAG, "Could not resolve ConnectionResult.", e);
                        mIsResolving = false;
                        mGoogleApiClient.Connect();
                    }
                }
                else
                {
                    //ShowErrorDialog(result);
                }
            }
            else
            {
                //UpdateUI(false);
            }
        }
    }
}