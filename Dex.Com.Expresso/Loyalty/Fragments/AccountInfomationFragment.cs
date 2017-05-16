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
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Viewpager;
using Dex.Com.Expresso;
using Loyalty.Utils;
using Dex.Com.Expresso.Loyalty.Droid.Activities;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class AccountInfomationFragment : BaseFragment
    {
        private int RESULT_LOGIN = 99;
        private ViewPager mVppTab;
        private TabLayout mTabLayout;
        private View lnlLogin, lnlData, lnlLinkAccount;
        private AccountInformationAdapters adapter;
        private Button mBtnLogin, mBtnLinkAccount;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static AccountInfomationFragment NewInstance()
        {
            var frag1 = new AccountInfomationFragment { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_accountinformation, null);
            lnlLogin = view.FindViewById<View>(Resource.Id.lnlLogin);
            lnlData = view.FindViewById<View>(Resource.Id.lnlData);
            lnlLinkAccount = view.FindViewById<View>(Resource.Id.lnlLinkAccount);
            mBtnLinkAccount = view.FindViewById<Button>(Resource.Id.btnLinkAccunt);
            mBtnLogin = view.FindViewById<Button>(Resource.Id.btnLogin);
            mVppTab = view.FindViewById<ViewPager>(Resource.Id.pager);
            mTabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);
            mTabLayout.TabMode = TabLayout.ModeScrollable;
            mVppTab.OffscreenPageLimit = 0;

            mBtnLogin.Click += MBtnLogin_Click;
            mBtnLinkAccount.Click += MBtnLinkAccount_Click;
            return view;
        }

        private void MBtnLinkAccount_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this.getActivity(), typeof(LinkAccountActivity));
            StartActivityForResult(intent, RESULT_LOGIN);
        }

        private void MBtnLogin_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this.getActivity(), typeof(Dex.Com.Expresso.Activities.PLUSRangerLogin));
            StartActivityForResult(intent, RESULT_LOGIN);
        }

        public override void OnResume()
        {
            base.OnResume();

            if (Cons.mMemberCredentials == null)
            {
                lnlLinkAccount.Visibility = ViewStates.Gone;
                lnlLogin.Visibility = ViewStates.Visible;
                lnlData.Visibility = ViewStates.Gone;
            }
            else
            {
                if (Cons.mMemberCredentials.MemberProfile == null)
                {

                    lnlLinkAccount.Visibility = ViewStates.Visible;
                    lnlLogin.Visibility = ViewStates.Gone;
                    lnlData.Visibility = ViewStates.Gone;
                    return;
                }
                if (Cons.mMemberCredentials.MemberProfile.idMasterAccount == 0)
                {
                    lnlLinkAccount.Visibility = ViewStates.Visible;
                    lnlLogin.Visibility = ViewStates.Gone;
                    lnlData.Visibility = ViewStates.Gone;
                    return;
                }
                lnlData.Visibility = ViewStates.Visible;
                lnlLogin.Visibility = ViewStates.Gone;
                lnlLinkAccount.Visibility = ViewStates.Gone;
                if (adapter == null)
                {
                    AccountInformationAdapters adapter = new AccountInformationAdapters(getActivity(), getActivity().SupportFragmentManager);
                    mVppTab.Adapter = adapter;
                    mTabLayout.SetupWithViewPager(mVppTab);
                }
            }
        }
    }
}