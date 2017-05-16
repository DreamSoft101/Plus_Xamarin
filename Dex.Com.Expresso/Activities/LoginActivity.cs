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
using EXPRESSO.Threads;
using Dex.Com.Expresso.Adapters.Spinner;
using EXPRESSO.Models.Database;
using EXPRESSO.Models;
using EXPRESSO.Utils;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;
using Xamarin.Auth;
using System.Threading;
using Android.Gms.Auth;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "LoginActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LoginActivity : BaseActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        private Spinner mSpnEntity;
        private EditText mTxtUserName;
        private EditText mTxtPassword;
        private Button mBtnLogin;
        private Button mBtnGuest;
        private EntitiesAdapter adapter;
        private List<TblEntities> mlstEntity;
        private TextView mTxtForgetpassword;
        private int mIntRequestRequest = 99;

        private SignInButton mGoogleSignIn;
        private GoogleApiClient mGoogleApiClient;
        private bool mIsResolving = false;
        private bool mShouldResolve = false;
        private ImageView mImgGPlus;

        private const int RC_SIGN_IN = 9001;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_login;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = GetString(Resource.String.title_login);
            mSpnEntity = FindViewById<Spinner>(Resource.Id.spnEntities);
            mBtnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            mBtnGuest = FindViewById<Button>(Resource.Id.btnGuest);
            mTxtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            mTxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mTxtForgetpassword = FindViewById<TextView>(Resource.Id.txtforgetpassword);
            EntitiesAdapter adaptertmp = new EntitiesAdapter(this, new List<EXPRESSO.Models.Database.TblEntities>());
            mSpnEntity.Adapter = adaptertmp;

            UsersThreads userThread = new UsersThreads();
            userThread.OnGetListEntitiesResult += (result) =>
            {
                if (result.intStatus == 1)
                {
                    adapter = new EntitiesAdapter(this, result.Data as List<TblEntities>);
                    mSpnEntity.Adapter = adapter;
                    mlstEntity = result.Data as List<TblEntities>;

                    if (getMyEntity().Count == 0)
                    {
                        UsersThreads thread = new UsersThreads();
                        thread.OnLoadAPIKey += (ServiceResult result1) =>
                        {
                            if (result1.intStatus == 1)
                            {
                                List<MyEntity> lstMyEntity = getMyEntity();
                                MyEntity myentity = result1.Data as MyEntity;
                                myentity.Entity = mlstEntity[0];
                                myentity.User = new UserInfos();
                                myentity.User.strUserName = string.Format(GetString(Resource.String.useasanonymouse), mlstEntity[0].strName);
                                lstMyEntity.Add(myentity);
                                saveMyEntity(lstMyEntity);
                                saveCurrentMyEntity(myentity);

                                Finish();
                            }
                            else
                            {
                                Toast.MakeText(this, result1.strMess, ToastLength.Short).Show();
                            }
                        };
                        thread.loadEntityInfo(mlstEntity[0].idEntity);
                    }
                }
                else
                {
                    Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                }

            };
            userThread.loadGetListEntities();

            // Create your application here

            mBtnGuest.Click += MBtnGuest_Click;
            mBtnLogin.Click += MBtnLogin_Click;
            mTxtForgetpassword.Click += MTxtForgetpassword_Click;
            FindViewById<View>(Resource.Id.lbeRegister).Click += LoginActivity_Click;

            FindViewById<View>(Resource.Id.lbeRegister).Click += LoginActivity_Click;
            FindViewById<View>(Resource.Id.imgGPlus).Click += GplusLogin_Click;
            FindViewById<View>(Resource.Id.imgFB).Click += FBLogin_Click;

            mGoogleApiClient = new GoogleApiClient.Builder(this)
              .AddConnectionCallbacks(this)
              .AddOnConnectionFailedListener(this)
              .AddApi(PlusClass.API)
              .AddScope(new Scope(Scopes.Profile))
              .Build();

        }

        private void GplusLogin_Click(object sender, EventArgs e)
        {
            mShouldResolve = true;
            mGoogleApiClient.Connect();
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
                        //UserThreads userthread = new UserThreads();
                        //userthread.OnResult += (ServiceResult result) =>
                        //{
                        //    if (result.StatusCode == 1)
                        //    {
                        //        BB_MValidateMemberCredentials dataCredential = result.Data as BB_MValidateMemberCredentials;
                        //        string jsonData = JsonConvert.SerializeObject(dataCredential);
                        //        setCacheString(MyAuth, jsonData);
                        //        Cons.mMemberCredentials = dataCredential;
                        //        Finish();
                        //    }
                        //    else
                        //    {
                        //        Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                        //    }
                        //};
                        //userthread.Login(1, "", accessToken, "");
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

        private void LoginActivity_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));
            if (mlstEntity != null)
            {
                intent.PutExtra(RegisterActivity.MYENTITY, StringUtils.Object2String(mlstEntity));
            }
            else
            {
                intent.PutExtra(RegisterActivity.MYENTITY, "");
            }
            StartActivityForResult(intent, mIntRequestRequest);
        }

        private void MBtnLogin_Click(object sender, EventArgs e)
        {
            if (mSpnEntity.SelectedItemPosition == 0)
            {
                Toast.MakeText(this, Resource.String.mess_choose_entity, ToastLength.Short).Show();
                return;
            }
            TblEntities entity = adapter.GetEntity(mSpnEntity.SelectedItemPosition - 1);

            string strUserName = mTxtUserName.Text;
            string strPassword = mTxtPassword.Text;

            List<MyEntity> lstMyEntity = getMyEntity();
            if (lstMyEntity != null)
            {
                foreach (var item in lstMyEntity)
                {
                    if (item.User != null)
                    {
                        if (item.User.strUserName.ToUpper() == strUserName.ToUpper())
                        {
                            Toast.MakeText(this, Resource.String.mess_already_login, ToastLength.Short).Show();
                            return;
                        }
                    }
                }
            }

            UsersThreads thread = new UsersThreads();
            thread.OnLoginSusscess += (ServiceResult resultEntity, ServiceResult resultLogin) =>
            {
                if (resultEntity.intStatus == 1)
                {
                    if (resultLogin.intStatus == 1)
                    {
                        MyEntity myentity = resultEntity.Data as MyEntity;
                        //List<MyEntity> lstMyEntity = getMyEntity();
                        if (lstMyEntity == null)
                            lstMyEntity = new List<MyEntity>();
                        myentity.Entity = entity;
                        myentity.User = resultLogin.Data as UserInfos;
                        lstMyEntity.Add(myentity);
                        saveMyEntity(lstMyEntity);
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
            thread.doLogin(entity.idEntity, strUserName, strPassword);

        }

        private void MBtnGuest_Click(object sender, EventArgs e)
        {
            if (mSpnEntity.SelectedItemPosition == 0)
            {
                Toast.MakeText(this, Resource.String.mess_choose_entity, ToastLength.Short).Show();
                return;
            }

            TblEntities entity = adapter.GetEntity(mSpnEntity.SelectedItemPosition - 1);

            List<MyEntity> lstEntity = getMyEntity();
            if (lstEntity.Where(p => p.idEntity == entity.idEntity).Count() > 0)
            {
                Toast.MakeText(this, Resource.String.mess_already_login, ToastLength.Short).Show();
                //Exists
                return;
            }
            else
            {
                UsersThreads thread = new UsersThreads();
                thread.OnLoadAPIKey += (ServiceResult result) =>
                {
                    if (result.intStatus == 1)
                    {
                        if (lstEntity == null)
                            lstEntity = new List<MyEntity>();
                        MyEntity myentity = result.Data as MyEntity;
                        myentity.Entity = entity;
                        myentity.User = new UserInfos();
                        myentity.User.strUserName = string.Format(GetString(Resource.String.useasanonymouse), entity.strName);
                        lstEntity.Add(myentity);
                        saveMyEntity(lstEntity);
                        saveCurrentMyEntity(myentity);
                        
                        Finish();
                    }
                    else
                    {
                        Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                    }
                };
                thread.loadEntityInfo(entity.idEntity);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntRequestRequest)
            {
                if (resultCode == Result.Ok)
                {
                    Finish();
                }
            }
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
                    //UserThreads userthread = new UserThreads();
                    //userthread.OnResult += (ServiceResult result) =>
                    //{
                    //    if (result.StatusCode == 1)
                    //    {
                    //        BB_MValidateMemberCredentials dataCredential = result.Data as BB_MValidateMemberCredentials;
                    //        string jsonData = JsonConvert.SerializeObject(dataCredential);
                    //        setCacheString(MyAuth, jsonData);
                    //        Cons.mMemberCredentials = dataCredential;
                    //        Finish();
                    //    }
                    //    else
                    //    {
                    //        Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                    //    }
                    //};
                    //userthread.Login(2, "", token, "");
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
            //UserThreads thread = new UserThreads();
            //thread.OnResult += (ServiceResult result) =>
            //{
            //    if (result.StatusCode == 1)
            //    {
            //        BB_MValidateMemberCredentials data = result.Data as BB_MValidateMemberCredentials;
            //        string jsonData = JsonConvert.SerializeObject(data);
            //        setCacheString(MyAuth, jsonData);
            //        Cons.mMemberCredentials = data;
            //        //Intent intentResult = new Intent();
            //        //intentResult.PutExtra()
            //        Finish();
            //    }
            //    else
            //    {
            //        Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
            //    }
            //};
            //thread.Login(type, "", token, "");
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