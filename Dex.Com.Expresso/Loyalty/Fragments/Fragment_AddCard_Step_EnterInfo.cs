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
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Spinners;
using Loyalty.Models;
using Dex.Com.Expresso.Loyalty.Droid.Fragments;

namespace Dex.Com.Expresso.Loyalty.Fragments
{
    public class Fragment_AddCard_Step_EnterInfo : BaseFragment
    {
        private EditText mTxtName, mTxtMobile, mTxtICNo;
        private MaterialSpinner mSpnRefType;
        private LinearLayout mLnlLoading;
        private Button mBtnOK;
        private LinearLayout prbLoading;
        private MValidatePlusMileCard mData;
        private ReferenceTypeAdapter mAdapter;
        // private CardTypeAdapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public static Fragment_AddCard_Step_EnterInfo NewInstance(MValidatePlusMileCard data)
        {
            var frag1 = new Fragment_AddCard_Step_EnterInfo { Arguments = new Bundle(), mData = data };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_addcard_step_enterinfo, null);
            mSpnRefType = view.FindViewById<MaterialSpinner>(Resource.Id.spnRefType);
            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mTxtName = view.FindViewById<EditText>(Resource.Id.txtName);
            mTxtICNo = view.FindViewById<EditText>(Resource.Id.txtICNo);
            mTxtMobile = view.FindViewById<EditText>(Resource.Id.txtMobile);
            mBtnOK = view.FindViewById<Button>(Resource.Id.btnOK);
            mBtnOK.Click += MBtnOK_Click;

            //mTxtName.Text = mData.Name;
            //mTxtICNo.Text = mData.RefNo;
            //mTxtMobile.Text = mData.Mobile;

            mLnlLoading.Visibility = ViewStates.Gone;

            mAdapter = new ReferenceTypeAdapter(this.Activity);
            this.mSpnRefType.Adapter = mAdapter;

            //LoadData();
            return view;
        }

        private void MBtnOK_Click(object sender, EventArgs e)
        {
            if (mSpnRefType.SelectedItemPosition == 0)
            {
                return;
            }
            int refType = mAdapter.GetBaseItem(mSpnRefType.SelectedItemPosition - 1).Type;

            string icNo = mTxtICNo.Text;
            if (string.IsNullOrEmpty(icNo))
            {
                return;
            }

            MasterAccountInfo info = new MasterAccountInfo();
            info.Mobile = mTxtMobile.Text;
            info.Name = mTxtName.Text;
            info.RefNo = icNo;
            info.RefType = refType;
            info.MemberID = mData.MemberID;
            AddCardThreads thread = new AddCardThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                mLnlLoading.Visibility = ViewStates.Gone;
                if (result.StatusCode == 1)
                {
                    if (result.Data is CheckMasterAccountExistResult)
                    {


                        CheckMasterAccountExistResult data = result.Data as CheckMasterAccountExistResult;
                        if (data.idMasterAccount == 0)
                        {
                            thread.SendOTP(1, info.Mobile);
                        }
                        else
                        {
                            ((AddCardActivity)this.Activity).setInfo(info);
                            ((AddCardActivity)this.Activity).ChangeFragment(2, data);
                        }
                      
                    }
                    else
                    {

                        
                        ((AddCardActivity)this.Activity).setInfo(info);
                        SentOTPResult data = result.Data as SentOTPResult;
                        ((AddCardActivity)this.Activity).ChangeFragment(3, data);
                    }
                }
                else
                {

                  Toast.MakeText(this.getActivity(), result.Mess, ToastLength.Short).Show();
                }
            };
            thread.CheckMasterAccount(icNo, refType);
        }
    }
}