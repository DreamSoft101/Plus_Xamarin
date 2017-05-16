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
using Loyalty.Models;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class RedempOrderSuccess : BaseFragment
    {
        public delegate void onNext();
        public onNext OnNext;
        private MDeliveryInfo mAddress;
        private LinearLayout mLnlData;
        private ProgressBar mPrbLoading;
        private Button mBtnNext;
        private Button mBtnViewDetail;
        private TextView mTxtMess, mTxtTotalPoints, mTxtRedeemID;
        private MMemberRedeem redeem;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public static RedempOrderSuccess NewInstance(MDeliveryInfo address)
        {
            var frag1 = new RedempOrderSuccess { Arguments = new Bundle() , mAddress = address  };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_redeemp_success, null);
            mLnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            mPrbLoading = view.FindViewById<ProgressBar>(Resource.Id.prbLoading);
            mTxtMess = view.FindViewById<TextView>(Resource.Id.txtMess);
            mBtnViewDetail = view.FindViewById<Button>(Resource.Id.btnDetail);
            mTxtTotalPoints = view.FindViewById<TextView>(Resource.Id.txtTotalPoint);
            mTxtRedeemID = view.FindViewById<TextView>(Resource.Id.txtRedeemID);

            mBtnViewDetail.Click += MBtnViewDetail_Click;
            LoadData();
            return view;
        }

        private void MBtnViewDetail_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (redeem != null)
            {
                Intent intent = new Intent(this.Activity, typeof(RedemptionDetailsActivity));
                intent.PutExtra(RedemptionDetailsActivity.RedemptionID, redeem.RedemptionID.ToString());
                StartActivity(intent);
            }
        }

        private void MBtnNext_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (OnNext != null)
            {
                OnNext();
            }
        }

        private void LoadData()
        {
            mLnlData.Visibility = ViewStates.Gone;
            mPrbLoading.Visibility = ViewStates.Visible;

            RedemptionThreads thread = new RedemptionThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.StatusCode == 1)
                {
                    redeem = result.Data as MMemberRedeem;
                    mTxtTotalPoints.Text += redeem.inTotalPointsRedeem;
                    mTxtRedeemID.Text += redeem.RedeemCode;


                    mLnlData.Visibility = ViewStates.Visible;
                    mPrbLoading.Visibility = ViewStates.Gone;
                    thread.OnResult = null;
                    thread.ClearCart();
                }
                else
                {
                    mBtnViewDetail.Visibility = ViewStates.Gone;
                    mTxtMess.Text = result.Mess;
                }
                
            };
            thread.Redeem(mAddress);
        }


        public override void OnResume()
        {
            base.OnResume();
        }
    }
}