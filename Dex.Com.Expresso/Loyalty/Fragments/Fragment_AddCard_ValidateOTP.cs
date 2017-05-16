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
using Loyalty.Models.ServiceOutput;
using Loyalty.Threads;
using Dex.Com.Expresso.Loyalty.Activities;
using Loyalty.Models;
using Dex.Com.Expresso.Loyalty.Droid.Fragments;

namespace Dex.Com.Expresso.Loyalty.Fragments
{
    public class Fragment_AddCard_ValidateOTP : BaseFragment
    {
        private TextView mTxtSecurityCode;
        private EditText mTxtOTPCode;
        private LinearLayout mLnlLoading;
        private Button mBtnOK;
        private SentOTPResult mData;
        private MValidatePlusMileCard mCardInfo;
        private MasterAccountInfo mInfo;
        //private CheckMasterAccountExistResult mMasterAccount;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public static Fragment_AddCard_ValidateOTP NewInstance(MValidatePlusMileCard cardinfo, SentOTPResult data)
        {
            var frag1 = new Fragment_AddCard_ValidateOTP { Arguments = new Bundle(), mData = data , mCardInfo = cardinfo };
            return frag1;
        }

        public static Fragment_AddCard_ValidateOTP NewInstance(MasterAccountInfo info, SentOTPResult data)
        {
            var frag1 = new Fragment_AddCard_ValidateOTP { Arguments = new Bundle(), mData = data, mInfo = info };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_addcard_step_validateOTP, null);
            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mTxtSecurityCode = view.FindViewById<TextView>(Resource.Id.txtSecurityCode);
            mTxtOTPCode  = view.FindViewById<EditText>(Resource.Id.txtOTPCode);
            mBtnOK = view.FindViewById<Button>(Resource.Id.btnOK);

            mBtnOK.Click += MBtnOK_Click;

            mTxtSecurityCode.Text = "Security Code: " + mData.SecurityKey;

            mLnlLoading.Visibility = ViewStates.Gone;
            //LoadData();
            return view;
        }

        private void MBtnOK_Click(object sender, EventArgs e)
        {

            string otp = mTxtOTPCode.Text;
            if (string.IsNullOrEmpty(otp))
            {
                return;
            }
            mLnlLoading.Visibility = ViewStates.Visible;
            AddCardThreads thread = new AddCardThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                mLnlLoading.Visibility = ViewStates.Gone;
                if (result.StatusCode == 1)
                {
                    ((AddCardActivity)this.Activity).Success();
                }
                else
                {
                    Toast.MakeText(this.getActivity(), result.Mess, ToastLength.Short).Show();
                }
            };
            if (mInfo == null)
            {
                thread.ValidateOTP(mCardInfo, mData, otp);
            }
            else
            {
                thread.ValidateOTP(mInfo, mData, otp);
            }
            
        }
    }
}