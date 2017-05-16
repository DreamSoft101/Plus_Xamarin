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
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.Listview;
using Dex.Com.Expresso.Adapters.RecyclerViews;
using Android.Support.V4.Widget;
using EXPRESSO.Threads;
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Activities;
using Newtonsoft.Json;
using Dex.Com.Expresso.Dialogs;
using static EXPRESSO.Models.EnumType;
using EXPRESSO.Utils;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Facilities_V2 : BaseFragment
    {
        private RecyclerView mLstItems;
        private RecyclerView mLstPOIType;
        private RecyclerView mLstFacilityType;
        private RecyclerView.LayoutManager mLayoutManager_lstPOI;
        private RecyclerView.LayoutManager mLayoutManager_lstHighway;
        private RecyclerView.LayoutManager mLayoutManager_lstItem;
        private RecyclerView.LayoutManager mLayoutManager_lstFacility;
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private FacilitiesAdapter adapter;
        private ImageView imgHighwayDropdown;
        private ImageView imgPOIType;
        private ImageView imgFacilityType;
        private ListView mLstHighway;

        public delegate void onInitSearch(Filter filter);
        public onInitSearch OnInitSearch;

        private Adapters.RecyclerViews.SettingHighwayNameAdapter mHighwayFilterAdapter;
        private RecyclerView lstHighway;
        private List<ImageView> mLstCheckBoxTypes = new List<ImageView>();
        private Adapters.RecyclerViews.FacilitiesTypeAdapter mFacilitiesTypeAdapter;
        private Adapters.RecyclerViews.POITypeAdapter mPOITypeAdapter;
        private SwipeRefreshLayout swipeRefreshLayout;
        private LinearLayout lnlNoData, lnlData;
        private TextView mTxtSelectHighway,  mTxtSelectPOI, mTxtSelectFacility;
        private LinearLayout lnlLoading;
        private LinearLayout mLnlHighway, mLnlFacilityType, mlnlPOIType;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager_lstPOI = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Facilities_V2 NewInstance()
        {
            var frag1 = new Fragment_Facilities_V2 { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_facilities_v2, null);
            mLnlHighway = view.FindViewById<LinearLayout>(Resource.Id.lnlHighway);
            mlnlPOIType = view.FindViewById<LinearLayout>(Resource.Id.lnlPOIType);
            mLnlFacilityType = view.FindViewById<LinearLayout>(Resource.Id.lnlFacilitie);
            lnlNoData = view.FindViewById<LinearLayout>(Resource.Id.lnlNoData);
            lnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);

            swipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            lnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            lstHighway = view.FindViewById<RecyclerView>(Resource.Id.lstHighway);
            mLstFacilityType = view.FindViewById<RecyclerView>(Resource.Id.lstFacilitie);
            mLstPOIType = view.FindViewById<RecyclerView>(Resource.Id.lstPOIType);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);

            mLayoutManager_lstItem = new LinearLayoutManager(getActivity());
            mLayoutManager_lstPOI = new LinearLayoutManager(getActivity(), LinearLayoutManager.Horizontal, false);
            mLayoutManager_lstHighway = new LinearLayoutManager(getActivity(), LinearLayoutManager.Horizontal, false);
            mLayoutManager_lstFacility = new LinearLayoutManager(getActivity(), LinearLayoutManager.Horizontal, false); 

            mLstPOIType.SetLayoutManager(mLayoutManager_lstPOI);
            lstHighway.SetLayoutManager(mLayoutManager_lstHighway);
            mLstItems.SetLayoutManager(mLayoutManager_lstItem);
            mLstFacilityType.SetLayoutManager(mLayoutManager_lstFacility);
            //mLstHighway.Click += MLstHighway_Click;

            mTxtSelectPOI = view.FindViewById<TextView>(Resource.Id.txtSelectPOI);
            mTxtSelectHighway = view.FindViewById<TextView>(Resource.Id.txtSelectHighway);
            mTxtSelectFacility = view.FindViewById<TextView>(Resource.Id.txtSelectFacility);

            imgPOIType = view.FindViewById<ImageView>(Resource.Id.imgFavoriteDropDown);
            imgHighwayDropdown = view.FindViewById<ImageView>(Resource.Id.imgHighwayDropDown);
            mLnlHighway.Click += ImgHighwayDropdown_Click;
            mlnlPOIType.Click += ImgPOIType_Click;
            mLnlFacilityType.Click += ImgFacilityType_Click;

            mHighwayFilterAdapter = new Adapters.RecyclerViews.SettingHighwayNameAdapter(getActivity());
            lstHighway.SetAdapter(mHighwayFilterAdapter);
            if (mHighwayFilterAdapter.ItemCount > 0)
            {
                mTxtSelectHighway.Visibility = ViewStates.Gone;
            }


            mPOITypeAdapter = new Adapters.RecyclerViews.POITypeAdapter(this.getActivity());
            mLstPOIType.SetAdapter(mPOITypeAdapter);
            if (mPOITypeAdapter.ItemCount > 0)
            {
                mTxtSelectPOI.Visibility = ViewStates.Gone;
            }

            mFacilitiesTypeAdapter = new Adapters.RecyclerViews.FacilitiesTypeAdapter(this.getActivity());
            mLstFacilityType.SetAdapter(mFacilitiesTypeAdapter);
            if (mFacilitiesTypeAdapter.ItemCount > 0)
            {
                mTxtSelectFacility.Visibility = ViewStates.Gone;
            }

            LoadData();
       

            view.FindViewById<View>(Resource.Id.cvHighway).Click += ImgHighwayDropdown_Click;
            view.FindViewById<View>(Resource.Id.cvPOIType).Click += ImgPOIType_Click;
            view.FindViewById<View>(Resource.Id.cvFacilities).Click += ImgFacilityType_Click;

            swipeRefreshLayout.Refresh += SwipeRefreshLayout_Refresh;
            return view;
        }

        private void SwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            swipeRefreshLayout.Refreshing = true;
            LoadData();
        }

        private void LoadData()
        {
            lnlNoData.Visibility = ViewStates.Gone;
            lnlData.Visibility = ViewStates.Gone;
            swipeRefreshLayout.Refreshing = true;
            lnlLoading.Visibility = ViewStates.Visible;
            var Highway = this.getActivity().getMySetting().Where(p => p.isEnable == true).Select(p => p.idHighway).ToList();
            var POI = this.getActivity().getFavoritePOIType();
            var Fac = this.getActivity().getFacilitiesType().Select(p => p.intID).ToList();

            PointOfInterestThread thread = new PointOfInterestThread();
            thread.OnGetFacilities += (List<BaseItem> lstItems) =>
            {
                if (lstItems.Count > 0)
                {
                    adapter = new FacilitiesAdapter(this.getActivity(), lstItems);
                    adapter.ItemClick += Adapter_ItemClick2;
                    mLstItems.SetAdapter(adapter);
                    lnlNoData.Visibility = ViewStates.Gone;
                    lnlData.Visibility = ViewStates.Visible;
                    swipeRefreshLayout.Refreshing = false;
                    lnlLoading.Visibility = ViewStates.Gone;
                    if (OnInitSearch != null)
                    {
                        OnInitSearch(adapter.Filter);
                    }
                }
                else
                {
                    lnlNoData.Visibility = ViewStates.Visible;
                    lnlData.Visibility = ViewStates.Gone;
                    swipeRefreshLayout.Refreshing = false;
                }
               
            };
            thread.GetFacilities(Highway, Fac, POI);
        }

        private void Adapter_ItemClick2(object sender, BaseItem e)
        {
            //throw new NotImplementedException();
            if (e.Item is FacilityItem)
            {
                
                var item = e.Item as FacilityItem;
                mCurentItem = e;
                if (item.IsNearby == 0)
                {
                    //
                    if (item.Data is TblRSA)
                    {
                        var itemrsa = item.Data as TblRSA;
                        Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
                        intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.RSA);
                        if (itemrsa.strType == "0" || itemrsa.strType=="1")
                        {
                            //rsa
                            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_RSA);
                        }
                        else if (itemrsa.strType == "2" || itemrsa.strType == "3")
                        {
                            //layby
                            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_LAYBY);
                        }
                        else if (itemrsa.strType == "4")
                        {
                            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_INTERCHANGE);
                        }
                        else if (itemrsa.strType == "5")
                        {
                            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_TUNNEL);
                        }
                        else if (itemrsa.strType == "6")
                        {
                            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_VISTAPOINT);
                        }

                        PointOfInterestThread threadRSA = new PointOfInterestThread();
                        threadRSA.OnCheckCCTVRSA += (bool result) =>
                        {

                            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, item.IsFavorite);
                            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(item.Data as TblRSA));
                            intent.PutExtra(FacilityDetailActivity.MODEL_HASCCTV, result);
                            StartActivityForResult(intent, mIntResult);

                        };
                        threadRSA.isCCTVRSA(itemrsa.idRSA);
                    }
                    else if (item.Data is TblTollPlaza)
                    {
                        var itemTollPlaza = item.Data as TblTollPlaza;
                        Intent intent = new Intent(this.getActivity(), typeof(TollPlazaActivity));
                        intent.PutExtra(TollPlazaActivity.DATA, JsonConvert.SerializeObject(itemTollPlaza));
                        intent.PutExtra(TollPlazaActivity.DATA_ISFAVORITE, item.IsFavorite);
                        PointOfInterestThread thread = new PointOfInterestThread();
                        thread.OnLoadTollPlazaInfo += (TblTollPlaza result) =>
                        {
                            try
                            {
                                var cctvs = result.getCCTV(); ;
                                intent.PutExtra(TollPlazaActivity.DATA_CCTV, JsonConvert.SerializeObject(cctvs));
                                StartActivityForResult(intent, mIntResult);
                            }
                            catch (Exception ex)
                            {
                                intent.PutExtra(TollPlazaActivity.DATA_CCTV, JsonConvert.SerializeObject(new List<TollPlazaCCTV>()));
                                StartActivityForResult(intent, mIntResult);
                            }
                            
                        };
                        thread.loadTollPlaza( itemTollPlaza.idHighway ,itemTollPlaza.idTollPlaza);
                       
                    }
                    else if (item.Data is TblCSC)
                    {
                        var itemCSC = item.Data as TblCSC;
                        Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
                        intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.CSC);
                        intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_CSC);
                        intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, item.IsFavorite);
                        intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(item.Data as TblCSC));
                        StartActivityForResult(intent, mIntResult);
                    }
                    else if (item.Data is TblPetrolStation)
                    {
                        var itemPertrol = item.Data as TblPetrolStation;
                        Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
                        intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.Petrol);
                        intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_PETROLSTATION);
                        intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, item.IsFavorite);
                        intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(item.Data as TblPetrolStation));
                        StartActivityForResult(intent, mIntResult);
                    }
                }
                else
                {
                    if (item.Data is TblNearby)
                    {
                        var itemNearby = item.Data as TblNearby;
                        Intent intent = new Intent(getActivity(), typeof(NearbyDetailActivity));
                        intent.PutExtra(NearbyDetailActivity.ModelData, StringUtils.Object2String(itemNearby));
                        intent.PutExtra(NearbyDetailActivity.ModelType, item.strCategoryNearbyName);
                        intent.PutExtra(NearbyDetailActivity.MODELISFAVORITE, item.IsFavorite);
                        StartActivityForResult(intent, mIntResult);
                    }
                }
            }
        }

        //private void Adapter_ItemClick1(object sender, BaseItem e)
        //{
        //    //throw new NotImplementedException();
        //    var item = e.Item as TrafficUpdate;
        //    Intent intent = new Intent(getActivity(), typeof(LiveTrafficDetailActivity));
        //    intent.PutExtra(LiveTrafficDetailActivity.DATA, JsonConvert.SerializeObject(item));
        //    StartActivity(intent);
        //}

        private void ImgPOIType_Click(object sender, EventArgs e)
        {
          
            FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
            Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog_poi");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            ChoosePOITypeDialog newFragment = ChoosePOITypeDialog.NewInstance(null);
            newFragment.OnDismess += () =>
            {
                //mHighwayFilterAdapter = new Adapters.RecyclerViews.SettingHighwayNameAdapter(getActivity());
                //lstHighway.SetAdapter(mHighwayFilterAdapter);
                mPOITypeAdapter = new Adapters.RecyclerViews.POITypeAdapter(this.getActivity());
                if (mPOITypeAdapter.ItemCount > 0)
                {
                    mTxtSelectPOI.Visibility = ViewStates.Gone;
                }
                else
                {
                    mTxtSelectPOI.Visibility = ViewStates.Visible;
                }
                mLstPOIType.SetAdapter(mPOITypeAdapter);
                LoadData();
            };
            newFragment.Show(ft, "dialog_poi");
        }

        private void ImgFacilityType_Click(object sender, EventArgs e)
        {

            FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
            Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog_fac");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            ChooseFacilityTypeDialog newFragment = ChooseFacilityTypeDialog.NewInstance(null);
            newFragment.OnDismess += () =>
            {
                //mHighwayFilterAdapter = new Adapters.RecyclerViews.SettingHighwayNameAdapter(getActivity());
                //lstHighway.SetAdapter(mHighwayFilterAdapter);
                mFacilitiesTypeAdapter = new Adapters.RecyclerViews.FacilitiesTypeAdapter(this.getActivity());
                if (mFacilitiesTypeAdapter.ItemCount > 0)
                {
                    mTxtSelectFacility.Visibility = ViewStates.Gone;
                }
                else
                {
                    mTxtSelectFacility.Visibility = ViewStates.Visible;
                }
                mLstFacilityType.SetAdapter(mFacilitiesTypeAdapter);
                LoadData();
            };
            newFragment.Show(ft, "dialog_fac");
        }


        private void ImgHighwayDropdown_Click(object sender, EventArgs e)
        {
            FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
            Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog_highway");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            ChooseHighwayDialog newFragment = ChooseHighwayDialog.NewInstance(null);
            newFragment.OnDismess += () =>
            {
                mHighwayFilterAdapter = new Adapters.RecyclerViews.SettingHighwayNameAdapter(getActivity());
                lstHighway.SetAdapter(mHighwayFilterAdapter);

                if (mHighwayFilterAdapter.ItemCount > 0)
                {
                    mTxtSelectHighway.Visibility = ViewStates.Gone;
                }
                else
                {
                    mTxtSelectHighway.Visibility = ViewStates.Visible;
                }

                LoadData();
            };
            newFragment.Show(ft, "dialog_highway");
        }


        private void Adapter_ItemClick(object sender, EXPRESSO.Models.BaseItem e)
        {
            mCurentItem = e;
            //Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            //intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)Activities.FacilityDetailActivity.ModelType.CSC);
            //intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_CSC);
            //intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            //intent.PutExtra(FacilityDetailActivity.MODELPOSITION, (int)e.getTag(EXPRESSO.Models.BaseItem.TagName.Position));
            //intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblCSC));
            //StartActivityForResult(intent, mIntResult);
            //StartActivity(intent);
        }

     
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntResult)
            {
                if (data != null)
                {
                    bool isFavorite = data.GetBooleanExtra(FacilityDetailActivity.MODELISFAVORITE, false);
                    (mCurentItem.Item as FacilityItem).IsFavorite = isFavorite;
                    adapter.NotifyDataSetChanged();
                }
                //bool isFavorite = data.GetBooleanExtra(FacilityDetailActivity.MODELISFAVORITE, false);
                //mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                //adapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));
            }
        }
    }
}
