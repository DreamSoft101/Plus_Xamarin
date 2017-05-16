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
using static Android.Support.V7.Widget.RecyclerView;
using Dex.Com.Expresso.Adapters.Listview;
using Dex.Com.Expresso.Activities;
using EXPRESSO.Models.Database;
using EXPRESSO.Threads;
using Newtonsoft.Json;
using EXPRESSO.Utils;

namespace Dex.Com.Expresso.Adapters.RecyclerViews
{
    public class FavoriteAdapters : RecyclerView.Adapter
    {
        private int mIntResult = 99;
        public event EventHandler<BaseItem> ItemClick;
        private Context mContext;
        private List<BaseItem> mLstItem;
        private BaseItem mCurentItem;
        private LiveTrafficAdapter mAdapterLiveTraffic;
        private NearbyAdapter mAdapterNearby;
        private LiveFeedAdapter mAdapterLiveFeed;
        private TollPlazaAdapter mAdapterTollPlaza;
        private RSAAdapter mRSAAdapter;
        private RecyclerView.Adapter mCurrentAdapter;

        public delegate void onStartActivity(Intent intent);
        public onStartActivity OnStartActivity;


        public FavoriteAdapters(Context conext, List<BaseItem> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
            //mLstItem = new List<BaseItem>();

        }

        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
        }

        public void UpdateTrafficUpdate(BaseItem detail)
        {
            mLstItem[1] = detail;
            this.NotifyDataSetChanged();
        }

        public void UpdateCCTV(List<TrafficUpdate> lstTrafficUpdate)
        {
            var livefeed = mLstItem.Where(p => p.Item is LiveFeedFavorite).FirstOrDefault();
            if (livefeed != null)
            {
                foreach (var item in (livefeed.Item as LiveFeedFavorite).LiveFeed)
                {
                    if (item.Item is TrafficUpdate)
                    {
                        try
                        {
                            (item.Item as TrafficUpdate).strURL = lstTrafficUpdate.Where(p => p.idTrafficUpdate == (item.Item as TrafficUpdate).idTrafficUpdate).FirstOrDefault().strURL;
                            
                        }
                        catch (Exception ex)
                        {

                        }
                        
                    }
                }
            }
            this.NotifyDataSetChanged();
        }

        public void UpdateCCTV(List<TollPlazaCCTV> lstTrafficUpdate)
        {
            var livefeed = mLstItem.Where(p => p.Item is LiveFeedFavorite).FirstOrDefault();
            if (livefeed != null)
            {
                foreach (var item in (livefeed.Item as LiveFeedFavorite).LiveFeed)
                {
                    if (item.Item is TollPlazaCCTV)
                    {
                        try
                        {
                            (item.Item as TollPlazaCCTV).strCCTVImage = lstTrafficUpdate.Where(p => p.idTollPlazaCctv == (item.Item as TollPlazaCCTV).idTollPlazaCctv).FirstOrDefault().strCCTVImage;
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
            }
        }

        public void HideTrafficUpdate()
        {
            mLstItem[0] = null;
            this.NotifyDataSetChanged();
        }

        public BaseItem GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class BaseViewHolder : RecyclerView.ViewHolder
        {
            public Context mContext;
            public BaseItem mBaseItem;
            public TextView mTxtType;
            public LinearLayout mLnlTitle, mLnlAllData, mLnlData, mLnlLoading;
            public RecyclerView mRecyclerView;
            public RecyclerView.Adapter mAdapter;
            public LayoutManager mLnlLayoutManager;
            public BaseViewHolder(View itemView, Action<BaseItem> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.mTxtType = itemView.FindViewById<TextView>(Resource.Id.txtType);
                this.mLnlTitle = itemView.FindViewById<LinearLayout>(Resource.Id.lnlTitle);
                this.mLnlAllData = itemView.FindViewById<LinearLayout>(Resource.Id.lnlAlldata);
                this.mLnlData = itemView.FindViewById<LinearLayout>(Resource.Id.lnlData);
                this.mLnlLoading = itemView.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
                this.mRecyclerView = itemView.FindViewById<RecyclerView>(Resource.Id.lstItems);
                this.mLnlLayoutManager = new LinearLayoutManager(mContext);
                this.mRecyclerView.SetLayoutManager(mLnlLayoutManager);
                itemView.Click += (sender, e) => itemClick(mBaseItem);

            }
        }


        void OnClick(BaseItem position)
        {
            if (ItemClick != null)
            {
               ItemClick(this, position);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;

            baseHolder.mLnlAllData.Visibility = ViewStates.Gone;
            baseHolder.mLnlData.Visibility = ViewStates.Gone;
            baseHolder.mLnlLoading.Visibility = ViewStates.Gone;
            baseHolder.mLnlTitle.Visibility = ViewStates.Gone;
            if (item == null)
            {
                baseHolder.mLnlData.Visibility = ViewStates.Visible;
                baseHolder.mLnlLoading.Visibility = ViewStates.Visible;
            }
            else if (item.Item is FavoriteHeader)
            {
                baseHolder.mLnlTitle.Visibility = ViewStates.Visible;
                var data = item.Item as FavoriteHeader;
                baseHolder.mTxtType.Text = data.strType;
            }
            else  if (item.Item is RSAFavorite)
            {
                baseHolder.mLnlData.Visibility = ViewStates.Visible;
                baseHolder.mLnlAllData.Visibility = ViewStates.Visible;
                var lstItem = (item.Item as RSAFavorite).RSA;
                mRSAAdapter = new RSAAdapter(mContext, lstItem);
                baseHolder.mRecyclerView.SetAdapter(mRSAAdapter);
                mRSAAdapter.ItemClick += RSAClick;
            }
            else if (item.Item is TollPlazaFavorite)
            {
                baseHolder.mLnlData.Visibility = ViewStates.Visible;
                baseHolder.mLnlAllData.Visibility = ViewStates.Visible;
                var lstItem = (item.Item as TollPlazaFavorite).TollPlaza;
                mAdapterTollPlaza = new TollPlazaAdapter(mContext, lstItem);
                baseHolder.mRecyclerView.SetAdapter(mAdapterTollPlaza);
                mAdapterTollPlaza.ItemClick += TollPlazaClick;
            }
            else if (item.Item is LiveFeedFavorite)
            {
                baseHolder.mLnlData.Visibility = ViewStates.Visible;
                baseHolder.mLnlAllData.Visibility = ViewStates.Visible;
                var lstItem = (item.Item as LiveFeedFavorite).LiveFeed;
                mAdapterLiveFeed = new LiveFeedAdapter(mContext, lstItem);
                baseHolder.mRecyclerView.SetAdapter(mAdapterLiveFeed);
                mAdapterLiveFeed.ItemClick += LiveFeedClick;
            }
            else if (item.Item is TrafficUpdateFavorite)
            {
                baseHolder.mLnlData.Visibility = ViewStates.Visible;
                baseHolder.mLnlAllData.Visibility = ViewStates.Visible;
                var lstItem = (item.Item as TrafficUpdateFavorite).TrafficUpdate;
                mAdapterLiveTraffic = new LiveTrafficAdapter(mContext, lstItem);
                baseHolder.mRecyclerView.SetAdapter(mAdapterLiveTraffic);
                mAdapterLiveTraffic.ItemClick += TrafficUpdateClick;
            }
            else if (item.Item is NearbyFavorite)
            {
                baseHolder.mLnlData.Visibility = ViewStates.Visible;
                baseHolder.mLnlAllData.Visibility = ViewStates.Visible;
                var lstItem = (item.Item as NearbyFavorite).Nearby;
                mAdapterNearby = new NearbyAdapter(mContext, lstItem);
                baseHolder.mRecyclerView.SetAdapter(mAdapterNearby);
                mAdapterNearby.ItemClick += MAdapterNearby_ItemClick;
            }
        }

        private void MAdapterNearby_ItemClick(object sender, BaseItem e)
        {
            //throw new NotImplementedException();
            mCurentItem = e;
            mCurrentAdapter = mAdapterNearby;
            if (e.Item is TblNearby)
            {
                var itemNearby = e.Item as TblNearby;
                PointOfInterestThread thread = new PointOfInterestThread();
                thread.OnGetNearbyCategoryItem += (TblNearbyCatg itemc) =>
                {
                    Intent intent = new Intent(mContext, typeof(NearbyDetailActivity));
                    intent.PutExtra(NearbyDetailActivity.ModelData, StringUtils.Object2String(itemNearby));
                    intent.PutExtra(NearbyDetailActivity.ModelType, itemc.strNearbyCatgName);
                    intent.PutExtra(NearbyDetailActivity.MODELISFAVORITE, (bool)e.getTag(BaseItem.TagName.IsFavorite));
                    if (OnStartActivity != null)
                    {
                        OnStartActivity(intent);

                    }
                };
                thread.getNearbyCategoryItem(itemNearby.idNearbyCatg);
               
            }
        }

        private void TrafficUpdateClick(object sender, BaseItem e)
        {
            mCurrentAdapter = mAdapterLiveTraffic;
            mCurentItem = e;
            if (e.Item is TrafficUpdate)
            {
                var item = e.Item as TrafficUpdate;
                Intent intent = new Intent(mContext, typeof(LiveTrafficDetailActivity));
                intent.PutExtra(LiveTrafficDetailActivity.DATA, JsonConvert.SerializeObject(item));
                if (OnStartActivity != null)
                {
                    OnStartActivity(intent);

                }
                //((BaseActivity)mContext).StartActivityForResult(intent, mIntResult);
            }
        }

        private void LiveFeedClick(object sender, BaseItem e)
        {
            mCurrentAdapter = mAdapterLiveFeed;
            mCurentItem = e;
            if (e.Item is TrafficUpdate)
            {
                mCurentItem = e;
                Intent intent = new Intent(mContext, typeof(LiveFeedDetailActivity));
                intent.PutExtra(LiveFeedDetailActivity.DATA, JsonConvert.SerializeObject(e.Item as TrafficUpdate));
                intent.PutExtra(LiveFeedDetailActivity.MODETYPE, LiveFeedDetailActivity.MODETYPE_HIGHWAY);
                intent.PutExtra(LiveFeedDetailActivity.DATA_FAVORITE, (bool)e.getTag(BaseItem.TagName.IsFavorite));
                //((BaseActivity)mContext).StartActivityForResult(intent, mIntResult);
                if (OnStartActivity != null)
                {
                    OnStartActivity(intent);

                }

            }
            else if (e.Item is TollPlazaCCTV)
            {
                mCurentItem = e;
                Intent intent = new Intent(mContext, typeof(LiveFeedDetailActivity));
                intent.PutExtra(LiveFeedDetailActivity.DATA, JsonConvert.SerializeObject(e.Item as TollPlazaCCTV));
                intent.PutExtra(LiveFeedDetailActivity.MODETYPE, LiveFeedDetailActivity.MODETYPE_TOLLPLAZA);
                intent.PutExtra(LiveFeedDetailActivity.DATA_FAVORITE, (bool)e.getTag(BaseItem.TagName.IsFavorite));
                //((BaseActivity)mContext).StartActivityForResult(intent, mIntResult);
                if (OnStartActivity != null)
                {
                    OnStartActivity(intent);

                }
            }
        }

        private void TollPlazaClick(object sender, BaseItem e)
        {
            mCurrentAdapter = mAdapterLiveFeed;
            mCurentItem = e;
            if (e.Item is TblTollPlaza)
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

                Intent intent = new Intent(mContext, typeof(TollPlazaActivity));
                intent.PutExtra(TollPlazaActivity.DATA, JsonConvert.SerializeObject(tollPlaza));
                intent.PutExtra(TollPlazaActivity.DATA_ISFAVORITE, (bool)e.getTag(BaseItem.TagName.IsFavorite));
                intent.PutExtra(TollPlazaActivity.DATA_CCTV, JsonConvert.SerializeObject(cctvs));
                //((BaseActivity)mContext).StartActivityForResult(intent, mIntResult);
                if (OnStartActivity != null)
                {
                    OnStartActivity(intent);

                }
            }
        }

        public void ReUpdateIsFavorite(bool isFavorite)
        {
            if (mCurentItem != null)
            {
                //if (mCurentItem.Item is )
                mCurentItem.setTag(BaseItem.TagName.IsFavorite, isFavorite);
                if (mAdapterLiveFeed != null)
                    mAdapterLiveFeed.NotifyDataSetChanged();
                if (mAdapterTollPlaza!= null)
                    mAdapterTollPlaza.NotifyDataSetChanged();
                if (mRSAAdapter != null)
                    mRSAAdapter.NotifyDataSetChanged();
                if (mAdapterNearby != null)
                    mAdapterNearby.NotifyDataSetChanged();
                if (mCurrentAdapter != null)
                    mCurrentAdapter.NotifyDataSetChanged();
                
            }
        }

        private void RSAClick(object sender, BaseItem e)
        {
            mCurentItem = e;
            if (e.Item is TblRSA)
            {
                Intent intent = new Intent(mContext, typeof(FacilityDetailActivity));
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
                    //((BaseActivity)mContext).StartActivityForResult(intent, mIntResult);
                    if (OnStartActivity != null)
                    {
                        OnStartActivity(intent);
                    }
                };
                thread.LoadFacilityImagesRSA((e.Item as TblRSA).idRSA);
            }
           
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_favorite_v2, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

        
    }
}