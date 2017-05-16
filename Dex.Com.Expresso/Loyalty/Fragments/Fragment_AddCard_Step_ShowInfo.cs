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
using FR.Ganfra.Materialspinner;
using Loyalty.Models.ServiceOutput;
using Loyalty.Threads;
using Dex.Com.Expresso.Loyalty.Activities;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class Fragment_AddCard_Step_ShowInfo : BaseFragment
    {
        private TextView mTxtName, mTxtMobile, mTxtICNo;
        private MaterialSpinner mSpnRefType;
        private LinearLayout mLnlLoading;
        private Button mBtnOK;
        private LinearLayout prbLoading;
        private MValidatePlusMileCard mData;
        private CheckMasterAccountExistResult mMasterAccount;
        // private CardTypeAdapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public static Fragment_AddCard_Step_ShowInfo NewInstance(CheckMasterAccountExistResult data)
        {
            var frag1 = new Fragment_AddCard_Step_ShowInfo { Arguments = new Bundle(), mMasterAccount = data };
            return frag1;
        }

        public static Fragment_AddCard_Step_ShowInfo NewInstance(MValidatePlusMileCard data)
        {
            var frag1 = new Fragment_AddCard_Step_ShowInfo { Arguments = new Bundle() , mData = data };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_addcard_step_showInfo, null);

            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mTxtName = view.FindViewById<TextView>(Resource.Id.txtName);
            mTxtICNo = view.FindViewById<TextView>(Resource.Id.txtICNo);
            mTxtMobile = view.FindViewById<TextView>(Resource.Id.txtMobile);
            mBtnOK = view.FindViewById<Button>(Resource.Id.btnOK);
            mBtnOK.Click += MBtnOK_Click;

            if (mMasterAccount == null)
            {
                mTxtName.Text = "Name: " + mData.Name;
                mTxtICNo.Text = "Reference No: " + mData.RefNo;
                mTxtMobile.Text = "Mobile: " + mData.Mobile;
            }
            else
            {
                mTxtName.Text = "Name: " + mMasterAccount.Name;
                mTxtICNo.Text = "Reference No: " + mMasterAccount.RefNo;
                mTxtMobile.Text = "Mobile: " + mMasterAccount.Mobile;
                //mMasterAccount
            }

           

            mLnlLoading.Visibility = ViewStates.Gone;
            //LoadData();
            return view;
        }

        private void MBtnOK_Click(object sender, EventArgs e)
        {
            mLnlLoading.Visibility = ViewStates.Visible;
            AddCardThreads thread = new AddCardThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                mLnlLoading.Visibility = ViewStates.Gone;
                if (result.StatusCode == 1)
                {
                    SentOTPResult data = result.Data as SentOTPResult;
                    ((AddCardActivity)this.Activity).ChangeFragment(3, data);
                }
                else
                {
                    Toast.MakeText(this.Activity, result.Mess, ToastLength.Short).Show();
                }
            };
            if (mMasterAccount == null)
            {
                thread.SendOTP(mData.idMasterAccount, mData.Mobile);
            }
            else
            {
                thread.SendOTP(mMasterAccount.idMasterAccount, mMasterAccount.Mobile);
            }
            
        }
    }
}