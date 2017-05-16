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
using Android.Support.V7.Widget;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Adapters.Listview;
using EXPRESSO.Models;
using Dex.Com.Expresso.Activities;
using EXPRESSO.Models.Database;
using RecyclerViewAnimators.Adapters;
using static EXPRESSO.Models.EnumType;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Facilities_Others : BaseFragment
    {
        private FacilitiesType mOtherTypes;
        private RecyclerView mLstItems;
        RecyclerView.LayoutManager mLayoutManager;
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private BaseFacilityAdapter baseFacilityAdapter;
        private RSAAdapter rSAAdapter;
    
       
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Facilities_Others NewInstance(FacilitiesType type)
        {
           
            var frag1 = new Fragment_Facilities_Others { mOtherTypes = type };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_list, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            PointOfInterestThread thread = new PointOfInterestThread();
            List<BaseItem> lstItem;
            if (mOtherTypes == FacilitiesType.PLUSSmile)
            {
                thread.OnLoadPlusSmile += (result) =>
                {
                    lstItem = result;
                    baseFacilityAdapter = new BaseFacilityAdapter(getActivity(), lstItem);
                    baseFacilityAdapter.ItemClick += Adapter_ItemClick_PLUSSmile;
                    var adapterAnimator = new ScaleInAnimationAdapter(baseFacilityAdapter);
                    mLstItems.SetAdapter(adapterAnimator);
                };
                thread.loadPLUSSmile();
            }
            else if (mOtherTypes == FacilitiesType.SSK)
            {
                thread.OnLoadSSK += (result) =>
                {
                    lstItem = result;
                    lstItem = result;
                    baseFacilityAdapter = new BaseFacilityAdapter(getActivity(), lstItem);
                    baseFacilityAdapter.ItemClick += Adapter_ItemClick_SSK;
                    var adapterAnimator = new ScaleInAnimationAdapter(baseFacilityAdapter);
                    mLstItems.SetAdapter(adapterAnimator);
                };
                thread.loadSSK();
            }
            else if (mOtherTypes == FacilitiesType.INTERCHANGE)
            {
                thread.OnInterchange += (result) =>
                {
                    lstItem = result;
                    lstItem = result;
                    rSAAdapter = new RSAAdapter(getActivity(), lstItem);
                    var adapterAnimator = new ScaleInAnimationAdapter(rSAAdapter);
                    mLstItems.SetAdapter(adapterAnimator);
                    rSAAdapter.ItemClick += Adapter_ItemClick_Interchange;
                };
                thread.loadInterchange ();
            }
            else if (mOtherTypes == FacilitiesType.TUNNEL)
            {
                thread.OnLoadTunnel += (result) =>
                {
                    lstItem = result;
                    lstItem = result;
                    rSAAdapter = new RSAAdapter(getActivity(), lstItem);
                    var adapterAnimator = new ScaleInAnimationAdapter(rSAAdapter);
                    mLstItems.SetAdapter(adapterAnimator);
                    rSAAdapter.ItemClick += Adapter_ItemClick_Tunnel;
                };
                thread.loadTunnel();
            }
            else if (mOtherTypes == FacilitiesType.VISTAPOINT)
            {
               
                thread.OnLoadVistaPoint += (result) =>
                {
                    lstItem = result;
                    lstItem = result;
                    rSAAdapter = new RSAAdapter(getActivity(), lstItem);
                    var adapterAnimator = new ScaleInAnimationAdapter(rSAAdapter);
                    mLstItems.SetAdapter(adapterAnimator);
                    rSAAdapter.ItemClick += Adapter_ItemClick_VisaPoint;
                };
                thread.loadVistaPoint();
            }
            return view;
        }

        private void Adapter_ItemClick_VisaPoint(object sender, BaseItem e)
        {
            mCurentItem = e;
            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.RSA);
            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_VISTAPOINT);
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblRSA));
            StartActivityForResult(intent, mIntResult);
        }

        private void Adapter_ItemClick_Tunnel(object sender, BaseItem e)
        {
            mCurentItem = e;
            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.RSA);
            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_TUNNEL);
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblRSA));
            StartActivityForResult(intent, mIntResult);
        }

        private void Adapter_ItemClick_Interchange(object sender, BaseItem e)
        {
            mCurentItem = e;
            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.RSA);
            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_INTERCHANGE);
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblRSA));
            StartActivityForResult(intent, mIntResult);
        }

        private void Adapter_ItemClick_SSK(object sender, BaseItem e)
        {
            mCurentItem = e;
            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.Facility);
            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_SSK);
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblFacilities));
            StartActivityForResult(intent, mIntResult);
        }

        private void Adapter_ItemClick_PLUSSmile(object sender, BaseItem e)
        {
            mCurentItem = e;
            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.Facility);
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_PLUS_SMILE);
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblFacilities));
            StartActivityForResult(intent, mIntResult);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntResult)
            {
                bool isFavorite = data.GetBooleanExtra(FacilityDetailActivity.MODELISFAVORITE, false);
               
                if (rSAAdapter != null)
                {
                    if (isFavorite != (bool)mCurentItem.getTag(BaseItem.TagName.IsFavorite))
                    {
                        FavoriteThread thread = new FavoriteThread();
                        thread.IsToggle(mCurentItem.Item as TblRSA);
                    }
                    mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                    rSAAdapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
                }
                if (baseFacilityAdapter != null)
                {
                    if (isFavorite != (bool)mCurentItem.getTag(BaseItem.TagName.IsFavorite))
                    {
                        FavoriteThread thread = new FavoriteThread();
                        thread.IsToggle(mCurentItem.Item as TblFacilities);
                    }
                    mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                    baseFacilityAdapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
                }
               // adapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
            }
        }
    }
}