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
using Dex.Com.Expresso.Loyalty.Droid.Fragments;
using FR.Ganfra.Materialspinner;
using Dex.Com.Expresso.Loyalty.Adapters.Spinners;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso.Loyalty.Activities;

namespace Dex.Com.Expresso.Loyalty.Fragments
{
    public class Fragment_AddCard_Step1 : BaseFragment
    {
        private TextView mTxtCardNo;
        private MaterialSpinner mSpnCardType;
        private LinearLayout mLnlLoading;
        private Button mBtnOK;
        private LinearLayout prbLoading;
        private CardTypeAdapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
        }

        public static Fragment_AddCard_Step1 NewInstance()
        {
            var frag1 = new Fragment_AddCard_Step1 { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_addcard_step1, null);

            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mTxtCardNo = view.FindViewById<TextView>(Resource.Id.txtCardNo);
            mSpnCardType = view.FindViewById<MaterialSpinner>(Resource.Id.spnCardType);
            mBtnOK = view.FindViewById<Button>(Resource.Id.btnOK);

            mBtnOK.Click += MBtnOK_Click;

            mAdapter = new CardTypeAdapter(this.getActivity());
            mSpnCardType.Adapter = mAdapter;

            mLnlLoading.Visibility = ViewStates.Gone;
            //LoadData();
            return view;
        }

        private void MBtnOK_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            if (mSpnCardType.SelectedItemPosition == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(mTxtCardNo.Text))
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
                    MValidatePlusMileCard data = result.Data as MValidatePlusMileCard;
                    if (data.AddCardState == 1)
                    {
                        ((AddCardActivity)this.Activity).ChangeFragment(2, data);
                    }
                    else if (data.AddCardState == 2)
                    {
                        ((AddCardActivity)this.Activity).Success();
                    }
                    else if (data.AddCardState == 3)
                    {
                        ((AddCardActivity)this.Activity).ChangeFragment(4, data);
                    }
                    
                }
                else
                {
                    Toast.MakeText(this.getActivity(), result.Mess, ToastLength.Short).Show();
                }
            };

            thread.Common_ValidatePlusMileCard(mTxtCardNo.Text, mSpnCardType.SelectedItemPosition);
        }
    }
}