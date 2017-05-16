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
using Android.Support.V7.Widget;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Loyalty.Utils;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class AccountSummaryFragment : BaseFragment
    {
        private ItemTouchHelper mItemTouchHelper;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private ProgressBar mPrbLoading;
        private LinearLayout mLnlData;
        private TextView mTxtOpenBalancePoints, mTxtEndBalancePoints, mTxtPointsEarned, mTxtPointsRedeem, mTxtPointsAdjusted, mTxtPointsTransfer, mTxtStartBalanceDatem ,ntxtPointsExpired;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static AccountSummaryFragment NewInstance()
        {
            var frag1 = new AccountSummaryFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_account_summary, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            mPrbLoading = view.FindViewById<ProgressBar>(Resource.Id.prbLoading);

            mLnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            mTxtOpenBalancePoints = view.FindViewById<TextView>(Resource.Id.txtOpenPoints);
            mTxtEndBalancePoints = view.FindViewById<TextView>(Resource.Id.txtEndBalancePoints);
            mTxtPointsEarned = view.FindViewById<TextView>(Resource.Id.txtPointsEarn);
            mTxtPointsRedeem = view.FindViewById<TextView>(Resource.Id.txtPointsRedeem);
            mTxtPointsAdjusted = view.FindViewById<TextView>(Resource.Id.txtPointsAdjusted);
            mTxtPointsTransfer = view.FindViewById<TextView>(Resource.Id.txtPointsTransfer);
            mTxtStartBalanceDatem = view.FindViewById<TextView>(Resource.Id.txtStarDate);
            ntxtPointsExpired = view.FindViewById<TextView>(Resource.Id.txtPointExpired);
        

            LoadData();
            return view;
        }

        private void LoadData()
        {
            mLnlData.Visibility = ViewStates.Gone;
            mPrbLoading.Visibility = ViewStates.Visible;
            UserThreads thread = new UserThreads();
            thread.OnResult += (ServiceResult result) =>
            {

                if (result.StatusCode == 1)
                {
                    mLnlData.Visibility = ViewStates.Visible;
                    mPrbLoading.Visibility = ViewStates.Gone;
                    var data = result.Data as MGetRewardAccountSummary;
                    mTxtOpenBalancePoints.Text = String.Format(GetString(Resource.String.loy_format_point), data.OpenBalancePoints);
                    mTxtEndBalancePoints.Text = String.Format(GetString(Resource.String.loy_format_point), data.EndBalancePoints);
                    mTxtPointsEarned.Text = String.Format(GetString(Resource.String.loy_format_point), data.PointsEarned);
                    mTxtPointsRedeem.Text = String.Format(GetString(Resource.String.loy_format_point), data.PointsRedeem);
                    mTxtPointsAdjusted.Text = String.Format(GetString(Resource.String.loy_format_point), data.PointsAdjusted);
                    mTxtPointsTransfer.Text = String.Format(GetString(Resource.String.loy_format_point), data.PointsTransfer);
                    mTxtStartBalanceDatem.Text = data.StartBalanceDate.ToString(GetString(Resource.String.loy_format_date_time));
                    ntxtPointsExpired.Text = String.Format(GetString(Resource.String.loy_format_point), data.PointsExpired);


                    MRewardDetailsAdapters adapter = new MRewardDetailsAdapters(this.Activity, data.RewardDetails.ToList());
                    mLstItems.SetAdapter(adapter);
                }
                else
                {
                    Toast.MakeText(this.Activity, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.GetRewardAccountSummary(null);
        }


        public override void OnResume()
        {
            base.OnResume();
        }
    }
}