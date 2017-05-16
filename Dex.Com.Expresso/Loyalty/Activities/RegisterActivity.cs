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
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;
using Android.Gms.Common;
using System.Threading;
using Dex.Com.Expresso.Loyalty.Droid.Dialogs;
using Loyalty.Utils;
using Android.Gms.Auth;
using Xamarin.Auth;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : BaseActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        private Spinner mSpnEntity;
        private EditText mTxtUserName;
        private EditText mTxtPassword;
        private EditText mTxtConfirmPassword;
        private EditText mTxtMobileNo;
        private EditText mTxtFirstName;
        private EditText mTxtLastName;
        private Button mbtnRegister;
        private Button mBtnCancel;
        private EditText mTxtEmail;
        private ImageView mImgFB;
        private const int RC_SIGN_IN = 9001;
        private GoogleApiClient mGoogleApiClient;

        public static string USERNAME = "USERNAME";
        public static string PASSWORD = "PASSWORD";
        public static string TOKEN = "TOKEN";
        public static string TYPE = "TYPE";

        private bool mIsResolving = false;
        private bool mShouldResolve = false;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_register;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            mbtnRegister = FindViewById<Button>(Resource.Id.btnRegister);
            mBtnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            mTxtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            mTxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mTxtConfirmPassword = FindViewById<EditText>(Resource.Id.txtConfirmPassword);
            mTxtMobileNo = FindViewById<EditText>(Resource.Id.txtMobileNo);
            mTxtFirstName = FindViewById<EditText>(Resource.Id.txtFirstName);
            mTxtLastName = FindViewById<EditText>(Resource.Id.txtLastName);
            mTxtEmail = FindViewById<EditText>(Resource.Id.txEmail);
            mImgFB = FindViewById<ImageView>(Resource.Id.imgFB);
            // Create your application here

            mbtnRegister.Click += MbtnRegister_Click;
            mBtnCancel.Click += MBtnCancel_Click;
            FindViewById<View>(Resource.Id.imgGPlus).Click += GplusLogin_Click; 

            mGoogleApiClient = new GoogleApiClient.Builder(this)
               .AddConnectionCallbacks(this)
               .AddOnConnectionFailedListener(this)
               .AddApi(PlusClass.API)
               .AddScope(new Scope(Scopes.Profile))
               .Build();

            mImgFB.Click += MImgFB_Click;
        }

        private void MImgFB_Click(object sender, EventArgs e)
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
                        var expiresIn = Convert.ToDouble(ee.Account.Properties["expires_in"]);

                        FragmentTransaction ft = FragmentManager.BeginTransaction();
                        Fragment prev = FragmentManager.FindFragmentByTag("gplus");
                        ft.AddToBackStack(null);
                        RegisterSocialDialog dialog = RegisterSocialDialog.NewInstance(null, accessToken, 1);
                        dialog.OnRegisted += (string strToken, MBB_Registration data) =>
                        {
                            Intent intentResult = new Intent();
                            intentResult.PutExtra(TYPE, 1);
                            intentResult.PutExtra(TOKEN, strToken);
                            SetResult(Result.Ok, intentResult);
                            Finish();
                        };
                        dialog.Show(ft, "gplus");
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

        private void MBtnCancel_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Intent intent = new Intent();
            SetResult(Result.Canceled, intent);
            Finish();
        }

        private void MbtnRegister_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            string strUserName = mTxtUserName.Text;
            string strPassword = mTxtPassword.Text;
            string strPasswordConfirm = mTxtConfirmPassword.Text;
            string strMobileNo = mTxtMobileNo.Text;
            string strFirstName = mTxtFirstName.Text;
            string strLastName = mTxtLastName.Text;
            string strEmail = mTxtEmail.Text;
            if (string.IsNullOrEmpty(strPassword))
            {
                Toast.MakeText(this, Resource.String.loy_reg_mess_input_password, ToastLength.Short).Show();
                return;
            }
            if (strPassword != strPasswordConfirm)
            {
                Toast.MakeText(this, Resource.String.loy_reg_mess_confirmpass, ToastLength.Short).Show();
                return;
            }
            if (string.IsNullOrEmpty(strUserName))
            {
                Toast.MakeText(this, Resource.String.loy_reg_mess_input_email, ToastLength.Short).Show();
                return;
            }
            

            UserThreads thread = new UserThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.StatusCode == 1)
                {
                    MBB_Registration data = result.Data as MBB_Registration;
                    Intent intentResult = new Intent();
                    intentResult.PutExtra(TYPE, 0);
                    intentResult.PutExtra(USERNAME, mTxtUserName.Text);
                    intentResult.PutExtra(PASSWORD, mTxtPassword.Text);
                    SetResult(Result.Ok, intentResult);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.Register(0, strUserName, "", strPassword, strFirstName, strLastName, strMobileNo, strEmail);
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
                    LogUtils.WriteLog("TOKEN GPLUS:", token);
                    if (mGoogleApiClient.IsConnected)
                    {
                        PlusClass.AccountApi.ClearDefaultAccount(mGoogleApiClient);
                        mGoogleApiClient.Disconnect();
                    }
                    // Send code back to server.
                    FragmentTransaction ft = FragmentManager.BeginTransaction();
                    Fragment prev = FragmentManager.FindFragmentByTag("gplus");
                    ft.AddToBackStack(null);
                    RegisterSocialDialog dialog = RegisterSocialDialog.NewInstance(null, token, 2);
                    dialog.OnRegisted += (string strToken, MBB_Registration data) =>
                    {
                        Intent intentResult = new Intent();
                        intentResult.PutExtra(TYPE, 2);
                        intentResult.PutExtra(TOKEN, strToken);
                        SetResult(Result.Ok, intentResult);
                        Finish();
                    };
                    dialog.Show(ft, "gplus");
                }
                catch (Exception e)
                {
                    LogUtils.WriteError("Get GPlus Token", e.Message);
                    // Network or server error.
                    //throw;
                }

            });

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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == RC_SIGN_IN)
            {
                if (resultCode != Result.Ok)
                {
                    mShouldResolve = false;
                }

                mIsResolving = false;
                mGoogleApiClient.Connect();
            }
        }
    }
}