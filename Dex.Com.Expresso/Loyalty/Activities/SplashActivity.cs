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
using Android.Text;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "SplashActivity")]
    public class SplashActivity : BaseActivity
    {
        private const int mIntRequestRequest = 99;

        private TextView mTxtSignup, mTxtSignEmail, mTxtNoSigin;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_splash;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mTxtSignEmail = this.FindViewById<TextView>(Resource.Id.txtSignin);
            mTxtSignEmail.TextFormatted = Html.FromHtml(GetString(Resource.String.loy_mess_splash_signinwithemail));


            mTxtNoSigin = this.FindViewById<TextView>(Resource.Id.txtNoSigning);
            mTxtNoSigin.TextFormatted = Html.FromHtml(GetString(Resource.String.loy_mess_splash_withoutsignin));

            mTxtSignup = this.FindViewById<TextView>(Resource.Id.txtCreateAccount);
            mTxtSignup.TextFormatted = Html.FromHtml(GetString(Resource.String.loy_mess_splash_signig_createaccount));


            mTxtNoSigin.Click += MTxtNoSigin_Click;

            // Create your application here

            mTxtSignup.Click += MTxtSignup_Click;
            mTxtSignEmail.Click += MTxtSignEmail_Click;

            FindViewById<View>(Resource.Id.imgGPlus).Click += GPlusClick;
            FindViewById<View>(Resource.Id.imgFB).Click += FBClick;
        }

        private void FBClick(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            intent.PutExtra(LoginActivity.LOGINWITH, LoginActivity.LOGINWITH_FB);
            StartActivity(intent);
            Finish();
        }

        private void GPlusClick(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            intent.PutExtra(LoginActivity.LOGINWITH, LoginActivity.LOGINWITH_GP);
            StartActivity(intent);
            Finish();
        }

        private void MTxtSignEmail_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
            Finish();
        }

        private void MTxtSignup_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            intent.PutExtra(LoginActivity.LOGINWITH, LoginActivity.LOGINWITH_REGISTER);
            StartActivity(intent);
            Finish();
        }

        private void MTxtNoSigin_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}