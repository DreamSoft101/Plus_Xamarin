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
using Dex.Com.Expresso;
using Loyalty.Utils;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "MyAccount")]
    public class MyAccount : BaseActivity
    {
        private int RESULT_LOGIN = 99;
        private Button mBtnLogin;
        private View lnlLogin, frgData;
        protected override int LayoutResource
        {
            get
            {
                return Dex.Com.Expresso.Resource.Layout.loy_activity_myacount;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            lnlLogin = FindViewById<View>(Resource.Id.lnlLogin);
            frgData = FindViewById<View>(Resource.Id.frgContent);
            mBtnLogin = FindViewById<Button>(Resource.Id.btnLogin);

            // Create your application here

            mBtnLogin.Click += MBtnLogin_Click;

        }

        private void MBtnLogin_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivityForResult(intent, RESULT_LOGIN);
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (Cons.mMemberCredentials == null)
            {
                lnlLogin.Visibility = ViewStates.Visible;
                frgData.Visibility = ViewStates.Gone;
            }
            else
            {
                frgData.Visibility = ViewStates.Visible;
                lnlLogin.Visibility = ViewStates.Gone;
                Android.Support.V4.App.FragmentTransaction fragmentTransaction = SupportFragmentManager.BeginTransaction();
                fragmentTransaction.Replace(Resource.Id.frgContent, Fragments.AccountInfomationFragment.NewInstance());
                fragmentTransaction.Commit();
            }
        }
    }
}