
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
using Android.Support.V7.Widget.Helper;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews;
using Android.Support.V7.Widget;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Loyalty.Models.Database;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using XamarinItemTouchHelper;
using Dex.Com.Expresso;
using Loyalty.Utils;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class CartFragment : BaseFragment
    {
        private int RESULT_LOGIN = 99;
        public delegate void onNext(int type);
        public onNext OnNext;
        private ItemTouchHelper mItemTouchHelper;
        private MemberRedeemInfoProductAdapters mAdapter;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private TextView mTxtBalance, mTxtPointRedeem;
        private LinearLayout mLnlData;
        private Button mBtnNext;
        private ProgressBar prbLoading;
        private int intMyBalance;
        private int intTotalPoints;
        private Button mBtnLogin;
        private View lnlLogin, lnlData;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static CartFragment NewInstance()
        {
            var frag1 = new CartFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_cart, null);
            lnlLogin = view.FindViewById<View>(Resource.Id.lnlLogin);
            lnlData = view.FindViewById<View>(Resource.Id.lnlData);
            mBtnLogin = view.FindViewById<Button>(Resource.Id.btnLogin);

            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            //mLstItems.
            // ItemTouchHelper.Callback callback = new ItemTouchHelper.SimpleCallback()
            mLnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            mTxtBalance = view.FindViewById<TextView>(Resource.Id.txtBalance);
            mTxtPointRedeem = view.FindViewById<TextView>(Resource.Id.txtPointRedeem);
            mBtnNext = view.FindViewById<Button>(Resource.Id.btnNext);
            mBtnNext.Click += MBtnNext_Click;
            prbLoading = view.FindViewById<ProgressBar>(Resource.Id.prbLoading);
            mBtnLogin.Click += MBtnLogin_Click;
            //LoadData();
            return view;
        }

        private void MBtnLogin_Click(object sender, EventArgs e)
        {
            //Intent intent = new Intent(this.getActivity(), typeof(LoginActivity));
            //StartActivityForResult(intent, RESULT_LOGIN);

            //((Dex.Com.Expresso.Activities.MainActivity)this.getActivity()).ListItemClicked(Resource.Id.nav_myaccount);
        }

        private void MBtnNext_Click(object sender, EventArgs e)
        {
            if (!mAdapter.IsUnLockAll())
            {
                Toast.MakeText(this.Activity, Resource.String.loy_authen_mess_unlock, ToastLength.Short).Show();
                return;
            }
            //if (!mAdapter.IsCanRedeemp())
            //{
            //    Toast.MakeText(this.Activity, Resource.String.loy_authen_mess_cannot, ToastLength.Short).Show();
            //    return;
            //}
            if (intMyBalance < intTotalPoints)
            {
                Toast.MakeText(this.Activity, Resource.String.loy_redemption_mess_notenoughtpoints, ToastLength.Short).Show();
                return;
            }
            //redemption_mess_notenoughtpoints
            //throw new NotImplementedException();
            if (mAdapter.ItemCount == 0)
            {
                return;
            }

            if (OnNext != null)
            {
                OnNext(mAdapter.ProductType());
            }
        }

        private void LoadData()
        {
            mLnlData.Visibility = ViewStates.Gone;
            RedemptionThreads thread = new RedemptionThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.Data is MGetRewardAccountSummary)
                {
                    var data = result.Data as MGetRewardAccountSummary;
                    mTxtBalance.Text = string.Format(GetString(Resource.String.loy_format_point), data.EndBalancePoints);
                    intMyBalance = data.EndBalancePoints;
                }
                else if (result.Data is List<MemberRedeemInfoProduct>)
                {
                    var data = result.Data as List<MemberRedeemInfoProduct>;
                    mAdapter = new MemberRedeemInfoProductAdapters(this.Activity, data);
                    mAdapter.OnStartUnlock += (Intent intent) =>
                    {
                        this.getActivity().StartActivityForResult(intent, MemberRedeemInfoProductAdapters.REQUEST_UNLOCK);
                    };
                    mAdapter.OnCountChange += (int totalPoints) =>
                    {
                        mTxtPointRedeem.Text = string.Format(GetString(Resource.String.loy_format_point), totalPoints);
                        intTotalPoints = totalPoints;

                    };
                    mLstItems.SetAdapter(mAdapter);
                    mTxtPointRedeem.Text = string.Format(GetString(Resource.String.loy_format_point), data.Sum(p => (p.Quantity * p.Points)));
                    intTotalPoints = data.Sum(p => (p.Quantity * p.Points));
                    mLnlData.Visibility = ViewStates.Visible;
                    if (data.Count == 0)
                    {
                        mBtnNext.Visibility = ViewStates.Gone;
                    }

                    ItemTouchHelper.Callback callback = new SimpleItemTouchHelperCallback(mAdapter);
                    mItemTouchHelper = new ItemTouchHelper(callback);
                    mItemTouchHelper.AttachToRecyclerView(mLstItems);
                }
              
            };
            thread.FragmentCart();
        }

        public void UnLock(int id)
        {
            mAdapter.UnLock(id);
        }

        //public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);
        //    if (requestCode == MemberRedeemInfoProductAdapters.REQUEST_UNLOCK)
        //    {
        //        if (resultCode == (int)Result.Ok)
        //        {
        //            int id = data.GetIntExtra(AuthencationEvoucher.ID, 0);
        //            mAdapter.UnLock(id);
        //        }
        //    }
        //}

        public override void OnResume()
        {
            base.OnResume();
            if (Cons.mMemberCredentials == null)
            {
                ((Dex.Com.Expresso.Activities.MainActivity)this.getActivity()).ListItemClicked(Resource.Id.nav_myaccount);
                //lnlLogin.Visibility = ViewStates.Visible;
                //lnlData.Visibility = ViewStates.Gone;
                //prbLoading.Visibility = ViewStates.Gone;
            }
            else
            {

                if (Cons.mMemberCredentials.MemberProfile.idMasterAccount == 0)
                {
                    ((Dex.Com.Expresso.Activities.MainActivity)this.getActivity()).ListItemClicked(Resource.Id.nav_myaccount);
                }

                lnlData.Visibility = ViewStates.Visible;
                lnlLogin.Visibility = ViewStates.Gone;
                LoadData();
            }
        }


        //public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);
        //    if (requestCode == MemberRedeemInfoProductAdapters.REQUEST_UNLOCK)
        //    {
        //        int id = data.GetIntExtra(AuthencationEvoucher.ID, 0);
        //        UnLock(id);
        //    }
        //}
    }
}