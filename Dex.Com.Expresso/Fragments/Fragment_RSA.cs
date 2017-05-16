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
using RecyclerViewAnimators.Animators;
using Android.Views.Animations;
using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using RecyclerViewAnimators.Adapters;
using static EXPRESSO.Models.EnumType;
using Android.Support.V4.Widget;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_RSA : BaseFragment
    {
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private RecyclerView mLstItems;
        RecyclerView.LayoutManager mLayoutManager;
        private RSAAdapter adapter;
        private RecyclerView.LayoutManager mLayoutManager1;
        private Adapters.RecyclerViews.SettingHighwayNameAdapter mHighwayFilterAdapter;
        private SwipeRefreshLayout swipeRefreshLayout;
        private LinearLayout lnlNoData, lnlData;
        private TextView mTxtSelectHighway;
        private LinearLayout lnlLoading;
        private LinearLayout mLnlHighway;
        private BaseItem mCurrentItem;

        public RSAAdapter getAdapter()
        {
            return adapter;
        }

        public delegate void onInitData(RSAAdapter a);
        public onInitData OnInitData;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_RSA NewInstance()
        {
            var frag1 = new Fragment_RSA { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_rsa_rsa, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            lnlNoData = view.FindViewById<LinearLayout>(Resource.Id.lnlNoData);
            lnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            swipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            lnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);

            mLstItems.SetLayoutManager(mLayoutManager);
            

            swipeRefreshLayout.Refresh += SwipeRefreshLayout_Refresh;
            LoadData();
            return view;
        }


        private void LoadData()
        {
            lnlNoData.Visibility = ViewStates.Gone;
            lnlData.Visibility = ViewStates.Gone;
            swipeRefreshLayout.Refreshing = true;
            lnlLoading.Visibility = ViewStates.Visible;

            //throw new NotImplementedException();
            PointOfInterestThread thread = new PointOfInterestThread();
            thread.OnLoadRSA += (result) =>
            {
                lnlLoading.Visibility = ViewStates.Gone;
                swipeRefreshLayout.Refreshing = false;
                if (result.Count > 0)
                {


                    lnlNoData.Visibility = ViewStates.Gone;
                    lnlData.Visibility = ViewStates.Visible;
                   

                    adapter = new RSAAdapter(getActivity(), result);
                    var wrapped = new ScaleInAnimationAdapter(adapter);
                    mLstItems.SetAdapter(wrapped);
                    //mLstItems.SetAdapter(adapter);
                    adapter.ItemClick += Adapter_ItemClick;
                    if (OnInitData != null)
                    {
                        OnInitData(adapter);
                    }
                }
                else
                {
                    lnlNoData.Visibility = ViewStates.Visible;
                    lnlData.Visibility = ViewStates.Gone;
                }
            };
            thread.loadRSA(FacilitiesType.RSA);
        }

        private void SwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Adapter_ItemClick(object sender, BaseItem e)
        {
            mCurentItem = e;

            Intent intent = new Intent(getActivity(), typeof(FacilityDetailActivity));
            intent.PutExtra(FacilityDetailActivity.MODELTYPE, (int)FacilityDetailActivity.ModelType.RSA);
            intent.PutExtra(FacilityDetailActivity.MODELSOURCE, FacilityDetailActivity.MODEL_RSA);
            intent.PutExtra(FacilityDetailActivity.MODELISFAVORITE, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.IsFavorite));
            intent.PutExtra(FacilityDetailActivity.MODELData, EXPRESSO.Utils.StringUtils.Object2String(e.Item as TblRSA));
            intent.PutExtra(FacilityDetailActivity.MODEL_HASCCTV, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.RSA_IsCCTV));

            PointOfInterestThread thread = new PointOfInterestThread();
            thread.OnGetFacilityImageRSA += (List<TblFacilityImage> result) =>
            {
                if (result.Count > 0)
                {
                    intent.PutExtra(FacilityDetailActivity.MODEL_HASFOOD, (bool)e.getTag(EXPRESSO.Models.BaseItem.TagName.RSA_IsFood));
                }
                StartActivityForResult(intent, mIntResult);
            };
            thread.LoadFacilityImagesRSA((e.Item as TblRSA).idRSA);
            
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntResult)
            {
                bool isFavorite = data.GetBooleanExtra(FacilityDetailActivity.MODELISFAVORITE, false);
                mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                adapter.NotifyItemChanged((int)mCurentItem.getTag(BaseItem.TagName.Position));

                FavoriteThread thread = new FavoriteThread();
                thread.IsToggle(mCurentItem.Item as TblRSA);
            }
        }
    }
}