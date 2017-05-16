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
using Dex.Com.Expresso.Activities;
using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using RecyclerViewAnimators.Adapters;
using Dex.Com.Expresso.Adapters.RecyclerViews;
using Android.Support.V4.Widget;
using Dex.Com.Expresso.Dialogs;
using Newtonsoft.Json;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_TollPlaza : BaseFragment
    {

        private RecyclerView.LayoutManager mLayoutManager1;
        private Adapters.RecyclerViews.SettingHighwayNameAdapter mHighwayFilterAdapter;
        private ImageView imgHighwayDropdown;
        private RecyclerView lstHighway;
        private int mIntResult = 1000;
        private BaseItem mCurentItem;
        private RecyclerView mLstItems;
        RecyclerView.LayoutManager mLayoutManager;
        private TollPlazaAdapter mAdapter;

        private SwipeRefreshLayout swipeRefreshLayout;
        private LinearLayout lnlNoData, lnlData;
        private TextView mTxtSelectHighway;
        private LinearLayout lnlLoading;
        private LinearLayout mLnlHighway;

        public delegate void onInitSearch(TollPlazaAdapter data);
        public onInitSearch OnInitSearch;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_TollPlaza NewInstance()
        {
            var frag1 = new Fragment_TollPlaza { Arguments = new Bundle() };
            return frag1;
        }

        public string[] getAllName()
        {
            return mAdapter.getFullName();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_tollplaza, null);

            mLnlHighway = view.FindViewById<LinearLayout>(Resource.Id.lnlHighway);

            lnlNoData = view.FindViewById<LinearLayout>(Resource.Id.lnlNoData);
            lnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            swipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            lnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            lstHighway = view.FindViewById<RecyclerView>(Resource.Id.lstHighway);
            mLayoutManager1 = new LinearLayoutManager(getActivity(), LinearLayoutManager.Horizontal, false);
            lstHighway.SetLayoutManager(mLayoutManager1);
            mTxtSelectHighway = view.FindViewById<TextView>(Resource.Id.txtSelectHighway);
            imgHighwayDropdown = view.FindViewById<ImageView>(Resource.Id.imgHighwayDropDown);

            mLnlHighway.Click += MLnlHighway_Click; ;

            mHighwayFilterAdapter = new Adapters.RecyclerViews.SettingHighwayNameAdapter(getActivity());
            lstHighway.SetAdapter(mHighwayFilterAdapter);
            if (mHighwayFilterAdapter.ItemCount > 0)
            {
                mTxtSelectHighway.Visibility = ViewStates.Gone;
            }

            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            swipeRefreshLayout.Refresh += SwipeRefreshLayout_Refresh;
            LoadData();
            return view;
        }

        private void SwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            swipeRefreshLayout.Refreshing = true;
            LoadData();
        }

        private void LoadData()
        {
            lnlNoData.Visibility = ViewStates.Gone;
            lnlData.Visibility = ViewStates.Gone;
            swipeRefreshLayout.Refreshing = true;

            PointOfInterestThread thread = new PointOfInterestThread();
            thread.OnLoadTollPlaza += (ServiceResult result) =>
            {
                lnlLoading.Visibility = ViewStates.Gone;
                swipeRefreshLayout.Refreshing = false;

                if (result.intStatus == 1)
                {
                    List<BaseItem> lstItems = result.Data as List<BaseItem>;
                    if (lstItems.Count > 0)
                    {
                        lnlNoData.Visibility = ViewStates.Gone;
                        lnlData.Visibility = ViewStates.Visible;

                        mAdapter = new TollPlazaAdapter(getActivity(), lstItems);
                        var adapterAnimator = new ScaleInAnimationAdapter(mAdapter);
                        mLstItems.SetAdapter(adapterAnimator);
                        mAdapter.ItemClick += Adapter_ItemClick;

                        if (OnInitSearch != null)
                        {
                            OnInitSearch(mAdapter);
                        }
                    }
                    else
                    {
                        lnlNoData.Visibility = ViewStates.Visible;
                        lnlData.Visibility = ViewStates.Gone;

                    }
                       
                   
                }
                else
                {
                    lnlNoData.Visibility = ViewStates.Visible;
                    lnlData.Visibility = ViewStates.Gone;
                }

            };

            var highways = getActivity().getMySetting().Where(p => p.isEnable == true).Select(p => p.idHighway).ToList();
            if (highways.Count == 0)
            {
                MastersThread threadM = new MastersThread();
                threadM.OnLoadListHighway += (List<TblHighway> result) =>
                {
                    highways = result.Select(p => p.idHighway).ToList();
                    thread.loadTollPlaza(highways);
                };
                threadM.loadListHighway();
            }
            else
            {
                thread.loadTollPlaza(highways);
            }

           

        }

        private void MLnlHighway_Click(object sender, EventArgs e)
        {
            FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
            Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog");
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
                //LoadLiveTraffic();
            };
            newFragment.Show(ft, "dialog");
        }

        private void Adapter_ItemClick(object sender, BaseItem e)
        {
            mCurentItem = e;
            /*
            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.TollPlaza);
            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_TOLLPLAZA);
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblTollPlaza));
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            StartActivityForResult(intent, mIntResult);*/

            var tollPlaza = e.Item as TblTollPlaza;
            var cctvs = tollPlaza.getCCTV();

            Intent intent = new Intent(getActivity(), typeof(TollPlazaActivity));
            intent.PutExtra(TollPlazaActivity.DATA, JsonConvert.SerializeObject(tollPlaza));
            intent.PutExtra(TollPlazaActivity.DATA_ISFAVORITE, (bool)e.getTag(BaseItem.TagName.IsFavorite));
            intent.PutExtra(TollPlazaActivity.DATA_CCTV, JsonConvert.SerializeObject(cctvs));
            StartActivityForResult(intent, mIntResult);

        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntResult)
            {
                bool isFavorite = data.GetBooleanExtra(TollPlazaActivity.DATA_ISFAVORITE, false);
                mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                mAdapter.NotifyDataSetChanged();
                //FavoriteThread thread = new FavoriteThread();
                //thread.IsToggle(mCurentItem.Item as TblTollPlaza);
            }
        }

       
    }
}