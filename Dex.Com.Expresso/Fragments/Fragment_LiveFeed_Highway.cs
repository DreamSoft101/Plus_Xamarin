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
using Dex.Com.Expresso.Dialogs;
using Dex.Com.Expresso.Activities;
using Newtonsoft.Json;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_LiveFeed_Highway : BaseFragment
    {
        private RecyclerView mLstItems;
        private RecyclerView mLstLiveTraffic;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.LayoutManager mLayoutManager1;
        private int mIntResult = 1000;
        private BaseItem mCurentItem;
        private LiveFeedAdapter adapter;
        private ImageView imgHighwayDropdown;
        private ImageView imgFavoriteLocationDropdown;
        private ListView mLstHighway;
        private Adapters.RecyclerViews.SettingHighwayNameAdapter mHighwayFilterAdapter;
        private RecyclerView lstHighway;
        private List<int> mLstFilter = new List<int>() { 13 };
        private SwipeRefreshLayout swipeRefreshLayout;
        private LinearLayout lnlNoData, lnlData;
        private TextView mTxtSelectHighway;
        private LinearLayout lnlLoading;
        private LinearLayout mLnlHighway;
        private BaseItem mCurrentItem;

        public delegate void onInitData();
        public onInitData OnInitData;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_LiveFeed_Highway NewInstance()
        {
            var frag1 = new Fragment_LiveFeed_Highway { Arguments = new Bundle() };
            return frag1;
        }

        public LiveFeedAdapter getAdapter()
        {
            return adapter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_livefeed_highway, null);
            mLnlHighway = view.FindViewById<LinearLayout>(Resource.Id.lnlHighway);

            lnlNoData = view.FindViewById<LinearLayout>(Resource.Id.lnlNoData);
            lnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            swipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            lnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            lstHighway = view.FindViewById<RecyclerView>(Resource.Id.lstHighway);
            mLstLiveTraffic = view.FindViewById<RecyclerView>(Resource.Id.lstLiveTraffic);

            mLayoutManager= new LinearLayoutManager(getActivity());
            mLayoutManager1 = new LinearLayoutManager(getActivity(), LinearLayoutManager.Horizontal, false);

            mLstLiveTraffic.SetLayoutManager(mLayoutManager);
            lstHighway.SetLayoutManager(mLayoutManager1);

            mTxtSelectHighway = view.FindViewById<TextView>(Resource.Id.txtSelectHighway);

            imgFavoriteLocationDropdown = view.FindViewById<ImageView>(Resource.Id.imgFavoriteDropDown);
            imgHighwayDropdown = view.FindViewById<ImageView>(Resource.Id.imgHighwayDropDown);
        
            mLnlHighway.Click += ImgHighwayDropdown_Click;

            mHighwayFilterAdapter = new Adapters.RecyclerViews.SettingHighwayNameAdapter(getActivity());
            lstHighway.SetAdapter(mHighwayFilterAdapter);
            if (mHighwayFilterAdapter.ItemCount > 0)
            {
                mTxtSelectHighway.Visibility = ViewStates.Gone;
            }


         
            LoadLiveTraffic();
          

            view.FindViewById<View>(Resource.Id.cvHighway).Click += ImgHighwayDropdown_Click;

            swipeRefreshLayout.Refresh += SwipeRefreshLayout_Refresh;
            return view;
        }

        private void SwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            swipeRefreshLayout.Refreshing = true;
            LoadLiveTraffic();
        }

        private void LoadLiveTraffic()
        {
            lnlNoData.Visibility = ViewStates.Gone;
            lnlData.Visibility = ViewStates.Gone;

            swipeRefreshLayout.Refreshing = true;




            LiveTrafficThread thread = new LiveTrafficThread();
            thread.OnLoadLiveTrafficResult += (ServiceResult resultx) =>
            {
                lnlLoading.Visibility = ViewStates.Gone;
                //setDataResult(resultx);

                swipeRefreshLayout.Refreshing = false;

                List<FavoriteLocation> mLocation = getActivity().getFavoriteLocation();

                if (resultx.intStatus == 1)
                {
                    List<BaseItem> lstItem = resultx.Data as List<BaseItem>;
                    if (lstItem.Count > 0)
                    {
                        if (lstItem.Count > 0)
                        {
                            //lstItem = lstItem.OrderBy(p => p.Item as )
                            lnlNoData.Visibility = ViewStates.Gone;
                            lnlData.Visibility = ViewStates.Visible;

                            adapter = new LiveFeedAdapter(getActivity(), lstItem);
                            adapter.ItemClick += Adapter_ItemClick1;
                            mLstLiveTraffic.SetAdapter(adapter);
                            if (OnInitData != null)
                            {
                                OnInitData();
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

                }
                else
                {
                    lnlNoData.Visibility = ViewStates.Visible;
                    lnlData.Visibility = ViewStates.Gone;
                }

            };
            //mLstItems.Visibility = ViewStates.Invisible;
            //prbLoading.Visibility = ViewStates.Visible;

            var highways = getActivity().getMySetting().Where(p => p.isEnable == true).Select(p => p.idHighway).ToList();
            if (highways.Count == 0)
            {
                MastersThread threadM = new MastersThread();
                threadM.OnLoadListHighway += (List<TblHighway> result) =>
                {
                    highways = result.Select(p => p.idHighway).ToList();
                    thread.loadLiveTraffic(new List<int>() { 13 }, highways, null);
                };
                threadM.loadListHighway();
            }
            else
            {
                thread.loadLiveTraffic(new List<int>() { 13 }, highways, null);
            }



           
        }

        private void Adapter_ItemClick1(object sender, BaseItem e)
        {
            if (e.Item is TrafficUpdate)
            {
                mCurrentItem = e;
                Intent intent = new Intent(this.Activity, typeof(LiveFeedDetailActivity));
                intent.PutExtra(LiveFeedDetailActivity.DATA, JsonConvert.SerializeObject(e.Item as TrafficUpdate));
                intent.PutExtra(LiveFeedDetailActivity.MODETYPE, LiveFeedDetailActivity.MODETYPE_HIGHWAY);
                intent.PutExtra(LiveFeedDetailActivity.DATA_FAVORITE, (bool)e.getTag(BaseItem.TagName.IsFavorite));
                StartActivityForResult(intent, mIntResult);
                
            }
        }




        private void ImgHighwayDropdown_Click(object sender, EventArgs e)
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

                LoadLiveTraffic();
            };
            newFragment.Show(ft, "dialog");
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
                bool isFavorite = data.GetBooleanExtra(LiveFeedDetailActivity.MODELISFAVORITE, false);
                mCurrentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                adapter.NotifyDataSetChanged();
            }
        }
    }
}
