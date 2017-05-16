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
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using Dex.Com.Expresso.Loyalty.Fragments;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso.Loyalty.Droid.Fragments;
using Loyalty.Models;

namespace Dex.Com.Expresso.Loyalty.Activities
{
    [Activity(Label = "Add Card")]
    public class AddCardActivity : BaseActivity
    {
        private MValidatePlusMileCard mData;
        private MasterAccountInfo mInfo;
        private CheckMasterAccountExistResult mMaster;

        public void setInfo(MasterAccountInfo info)
        {
            this.mInfo = info;
        }

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_addcard;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ChangeFragment(1, null);
        }


        public void Success()
        {
            //Intent intent = new Intent();
            SetResult(Result.Ok);
            Finish();
        }

        public void ChangeFragment(int position, object data)
        {

            Android.Support.V4.App.Fragment fragment = null;
            switch (position)
            {
                case 1:
                    {
                        fragment = Fragment_AddCard_Step1.NewInstance();
                        break;
                    }
                case 2:
                    {
                        if (data is MValidatePlusMileCard)
                        {
                            MValidatePlusMileCard mobjData = data as MValidatePlusMileCard;
                            mData = mobjData;
                            fragment = Fragment_AddCard_Step_ShowInfo.NewInstance(mobjData);
                        }
                        else
                        {
                            CheckMasterAccountExistResult mobjData = data as CheckMasterAccountExistResult;
                            //mData = mobjData;
                            mMaster = mobjData;
                            fragment = Fragment_AddCard_Step_ShowInfo.NewInstance(mobjData); 
                        }
                       
                        break;
                    }
                case 3:
                    {
                        if (mData != null)
                        {
                            SentOTPResult mobjData = data as SentOTPResult;
                            fragment = Fragment_AddCard_ValidateOTP.NewInstance(mData, mobjData);
                            break;
                        }
                        else 
                        {
                            SentOTPResult mobjData = data as SentOTPResult;
                            fragment = Fragment_AddCard_ValidateOTP.NewInstance(mInfo, mobjData);
                            break;
                        }
                    }
                case 4:
                    {
                        MValidatePlusMileCard mobjData = data as MValidatePlusMileCard;
                        fragment = Fragment_AddCard_Step_EnterInfo.NewInstance(mobjData);
                        break;
                    }
            }

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }
    }
}