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
using Loyalty.Utils;
using Loyalty.Models;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Loyalty.Models.Database;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Spinners;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class ChooseAddressRedeemFragment : BaseFragment
    {
        public delegate void onPrevious();
        public onPrevious OnPrevious;
        public delegate void onNext(MDeliveryInfo address);
        public onNext OnNext;
        private TextView mTxtBalance, mTxtPointRedeem;
        private LinearLayout mLnlData;
        private EditText mTxtContactName;
        private EditText mTxtAddress1, mTxtAddress2, mTxtAddress3;
        private EditText mTxtPostCode, mTxtCity;
        private Spinner mSpnState, mSpnCountry;
        private Button mBtnNext, mBtnPrevious;
        private List<Country> mLstCountries;
        private List<State> mLstState;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
          
            // Create your fragment here
        }

        public static ChooseAddressRedeemFragment NewInstance()
        {
            var frag1 = new ChooseAddressRedeemFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_redemption_chooseaddress, null);
            this.mTxtContactName = view.FindViewById<EditText>(Resource.Id.txtContactName);
            this.mTxtAddress1 = view.FindViewById<EditText>(Resource.Id.txtAddress1);
            this.mTxtAddress2 = view.FindViewById<EditText>(Resource.Id.txtAddress2);
            this.mTxtAddress3 = view.FindViewById<EditText>(Resource.Id.txtAddress3);
            this.mTxtPostCode = view.FindViewById<EditText>(Resource.Id.txtPostcode);
            this.mTxtCity = view.FindViewById<EditText>(Resource.Id.txtCity);
            this.mBtnNext = view.FindViewById<Button>(Resource.Id.btnNext);
            this.mBtnPrevious = view.FindViewById<Button>(Resource.Id.btnPrevious);
            this.mSpnCountry = view.FindViewById<Spinner>(Resource.Id.spnCountry);
            this.mSpnState = view.FindViewById<Spinner>(Resource.Id.spnState);


            this.mSpnCountry.ItemSelected += MSpnCountry_ItemSelected;
            this.mBtnPrevious.Click += MBtnPrevious_Click;
            this.mBtnNext.Click += MBtnNext_Click;

            view.FindViewById<View>(Resource.Id.lnlLoading).Visibility = ViewStates.Visible;
            MasterThreads thread = new MasterThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.Data is List<Country>)
                {
                    List<Country> countries = result.Data as List<Country>;
                    mLstCountries = countries;
                    CountryAdapter adapter = new CountryAdapter(this.Activity, countries);
                    mSpnCountry.Adapter = adapter;
                    
                }
                else if (result.Data is List<State>)
                {
                    List<State> state = result.Data as List<State>;
                    mLstState = state;
                    //StateAdapter adapter = new StateAdapter(this.Activity, state);
                    //mSpnState.Adapter = adapter;
                }
                else  if (result.Data is MGetMemberProfile)
                {
                    var profile = result.Data as MGetMemberProfile;
                    if (profile.DeliveryAddress != null)
                    {
                        Cons.mMemberCredentials.MemberProfile = profile;
                        LoadData();
                    }

                    view.FindViewById<View>(Resource.Id.lnlLoading).Visibility = ViewStates.Gone;
                }
            };
            thread.ChoosAddressCart();
            //LoadData();
            return view;
        }

        private void MSpnCountry_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //throw new NotImplementedException();
            var country = mLstCountries[e.Position];
            var lstState = mLstState.Where(p => p.RegionParentID == country.RegionID).ToList();
            StateAdapter adapter = new StateAdapter(this.Activity, lstState);
            mSpnState.Adapter = adapter;
        }

        private void MBtnNext_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            MDeliveryInfo address = new MDeliveryInfo();
            address.ContactName = mTxtContactName.Text;
            address.Address1 = mTxtAddress1.Text;
            address.Address2 = mTxtAddress2.Text;
            address.Address3 = mTxtAddress3.Text;
            address.DeliverCity = mTxtCity.Text;
            address.PostalCode = mTxtPostCode.Text;
            address.DeliverCountryId = mLstCountries[mSpnCountry.SelectedItemPosition].RegionID;
            try
            {
                if (mSpnState.SelectedItemPosition != -1)
                {
                    address.DeliverStateId = mLstCountries[mSpnState.SelectedItemPosition].RegionID;
                }
            }
            catch (Exception ex)
            {

            }
            if (OnNext != null)
            {
                OnNext(address);
            }
        }

        private void MBtnPrevious_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (OnPrevious != null)
            {
                OnPrevious();
            }
        }

        private void LoadData()
        {
            this.mTxtContactName.Text = Cons.mMemberCredentials.MemberProfile.strDisplayName;
            this.mTxtAddress1.Text = Cons.mMemberCredentials.MemberProfile.DeliveryAddress == null ? "" : Cons.mMemberCredentials.MemberProfile.DeliveryAddress.Address1;
            this.mTxtAddress2.Text = Cons.mMemberCredentials.MemberProfile.DeliveryAddress == null ? "" : Cons.mMemberCredentials.MemberProfile.DeliveryAddress.Address2;
            this.mTxtAddress3.Text = Cons.mMemberCredentials.MemberProfile.DeliveryAddress == null ? "" : Cons.mMemberCredentials.MemberProfile.DeliveryAddress.Address3;
            this.mTxtCity.Text = Cons.mMemberCredentials.MemberProfile.DeliveryAddress == null ? "" : Cons.mMemberCredentials.MemberProfile.DeliveryAddress.City;
            this.mTxtPostCode.Text = Cons.mMemberCredentials.MemberProfile.DeliveryAddress == null ? "" : Cons.mMemberCredentials.MemberProfile.DeliveryAddress.PostCode;

            try
            {
                Guid idCountry = new Guid(Cons.mMemberCredentials.MemberProfile.DeliveryAddress.Country);
                mSpnCountry.SetSelection(mLstCountries.IndexOf(mLstCountries.Where(p => p.RegionID == idCountry).FirstOrDefault()));

                try
                {
                    if (string.IsNullOrEmpty(Cons.mMemberCredentials.MemberProfile.DeliveryAddress.State))
                    {
                        Guid idState = new Guid(Cons.mMemberCredentials.MemberProfile.DeliveryAddress.State);
                        mSpnState.SetSelection(mLstState.IndexOf(mLstState.Where(p => p.RegionID == idState).FirstOrDefault()));

                    }
                }
                catch (Exception ex)
                {

                }
               
                
            }
            catch (Exception ex)
            {

            }
            
        }


        public override void OnResume()
        {
            base.OnResume();
        }
    }
}