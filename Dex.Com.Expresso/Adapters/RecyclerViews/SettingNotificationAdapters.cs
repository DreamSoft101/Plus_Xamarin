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
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Activities;
using Dex.Com.Expresso.Dialogs;

namespace Dex.Com.Expresso.Adapters.RecyclerViews
{
    public class SettingNotificationAdapters : RecyclerView.Adapter
    {
        public event EventHandler<BaseItem> ItemClick;
        private Context mContext;
        private List<BaseItem> mLstItem;
        public SettingNotificationAdapters(Context conext, List<BaseItem> lstItems)
        {
            this.mContext = conext;
            this.mLstItem = lstItems;
            // mLstItem = (conext as BaseActivity).getFavoriteLocation();

        }

        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
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

            //public event EventHandler<TblRSA> ItemClick;
            public LinearLayout mLnlHighway;
            public LinearLayout mLnlData;
            public TextView mTxtHighwayName;
            public TextView mTxtTitle;
            public ImageView mImgAdd;
            public BaseItem mBaseItem;
            public RecyclerView.Adapter mAdapter;
            public RecyclerView mLstItems;
            public Context mContext;
            private RecyclerView.LayoutManager mLayoutManager;
            public int Position;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.mLnlHighway = itemView.FindViewById<LinearLayout>(Resource.Id.lnlHighway);
                this.mLnlData = itemView.FindViewById<LinearLayout>(Resource.Id.lnlData);
                this.mLnlData = itemView.FindViewById<LinearLayout>(Resource.Id.lnlData);
                this.mLstItems = ItemView.FindViewById<RecyclerView>(Resource.Id.lstItems);
                this.mTxtHighwayName = itemView.FindViewById<TextView>(Resource.Id.txtHighwayName);
                this.mTxtTitle = itemView.FindViewById<TextView>(Resource.Id.txtTitle);
                this.mImgAdd = itemView.FindViewById<ImageView>(Resource.Id.imgAdd);
                this.mLayoutManager = new LinearLayoutManager(mContext, LinearLayoutManager.Horizontal, false);
                this.mLstItems.SetLayoutManager(mLayoutManager);
                itemView.Click += (sender, e) => itemClick(base.Position);

                this.mImgAdd.Click += MImgAdd_Click;

            }

            

            private void MImgAdd_Click(object sender, EventArgs e)
            {
                if (mBaseItem.Item is FavoriteLocation)
                {
                    var flocation = mBaseItem.Item as FavoriteLocation;
                    FragmentTransaction ft = ((BaseActivity)mContext).FragmentManager.BeginTransaction();
                    Fragment prev = ((BaseActivity)mContext).FragmentManager.FindFragmentByTag("dialog");
                    if (prev != null)
                    {
                        ft.Remove(prev);
                    }
                    ft.AddToBackStack(null);
                    ChooseTimeDialog newFragment = new ChooseTimeDialog(DateTime.Now.TimeOfDay.Hours, 0);
                    newFragment.OnChangeValue += (int hour, int miunite) =>
                    {
                        SettingNotification newsetting = new SettingNotification();
                        newsetting.dtTime = new DateTime(1990, 11, 2, hour, miunite, 0);
                        newsetting.idParent = flocation.idFavoriteLocation;
                        newsetting.intType = 1;
                        newsetting.Tick = DateTime.Now.Ticks;
                        var lstSetting = ((BaseActivity)mContext).getSettingNotification();
                        lstSetting.Add(newsetting);
                        ((BaseActivity)mContext).setSettingNotification(lstSetting);
                        mAdapter.NotifyItemChanged(Position);
                    };

                    newFragment.Show(ft, "dialog");
                }
                else if (mBaseItem.Item is TollPlazaCCTV)
                {
                    var tollPlaza = mBaseItem.Item as TollPlazaCCTV;
                    FragmentTransaction ft = ((BaseActivity)mContext).FragmentManager.BeginTransaction();
                    Fragment prev = ((BaseActivity)mContext).FragmentManager.FindFragmentByTag("dialog");
                    if (prev != null)
                    {
                        ft.Remove(prev);
                    }
                    ft.AddToBackStack(null);
                    ChooseTimeDialog newFragment = new ChooseTimeDialog(DateTime.Now.TimeOfDay.Hours, 0);
                    newFragment.OnChangeValue += (int hour, int miunite) =>
                    {
                        SettingNotification newsetting = new SettingNotification();
                        newsetting.dtTime = new DateTime(1990, 11, 2, hour, miunite, 0);
                        newsetting.idParent = tollPlaza.idTollPlazaCctv;
                        newsetting.intType = 2;
                        newsetting.Tick = DateTime.Now.Ticks;
                        var lstSetting = ((BaseActivity)mContext).getSettingNotification();
                        lstSetting.Add(newsetting);
                        ((BaseActivity)mContext).setSettingNotification(lstSetting);
                        mAdapter.NotifyItemChanged(Position);
                    };

                    newFragment.Show(ft, "dialog");
                }
                else if (mBaseItem.Item is TrafficUpdate)
                {
                    var traffic = mBaseItem.Item as TrafficUpdate;
                    FragmentTransaction ft = ((BaseActivity)mContext).FragmentManager.BeginTransaction();
                    Fragment prev = ((BaseActivity)mContext).FragmentManager.FindFragmentByTag("dialog");
                    if (prev != null)
                    {
                        ft.Remove(prev);
                    }
                    ft.AddToBackStack(null);
                    ChooseTimeDialog newFragment = new ChooseTimeDialog(DateTime.Now.TimeOfDay.Hours, 0);
                    newFragment.OnChangeValue += (int hour, int miunite) =>
                    {
                        SettingNotification newsetting = new SettingNotification();
                        newsetting.dtTime = new DateTime(1990, 11, 2, hour, miunite, 0);
                        newsetting.idParent = traffic.idTrafficUpdate;
                        newsetting.intType = 3;
                        newsetting.Tick = DateTime.Now.Ticks;
                        var lstSetting = ((BaseActivity)mContext).getSettingNotification();
                        lstSetting.Add(newsetting);
                        ((BaseActivity)mContext).setSettingNotification(lstSetting);
                        mAdapter.NotifyItemChanged(Position);
                    };

                    newFragment.Show(ft, "dialog");
                }
                else
                {

                }

            }
        }


        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, GetBaseItem(position));
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);

            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.Position = position;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            baseHolder.mContext = mContext;
            if (item.Item is TblHighway)
            {
                var highway = item.Item as TblHighway;
                baseHolder.mLnlData.Visibility = ViewStates.Gone;
                baseHolder.mLnlHighway.Visibility = ViewStates.Visible;
                baseHolder.mTxtHighwayName.Text = highway.strName;
            }
            else if (item.Item is FavoriteLocation)
            {
                var flocation = item.Item as FavoriteLocation;
                baseHolder.mLnlData.Visibility = ViewStates.Visible;
                baseHolder.mLnlHighway.Visibility = ViewStates.Gone;
                baseHolder.mTxtTitle.Text = flocation.strFavoriteLocationName;

                var settings = ((BaseActivity)mContext).getSettingNotification();
                settings = settings.Where(p => p.idParent == flocation.idFavoriteLocation && p.intType == 1).OrderBy(p => p.dtTime).ToList();

                SettingNotificationDetailAdapter detailAdapter = new SettingNotificationDetailAdapter(mContext, settings, this);
                baseHolder.mLstItems.SetAdapter(detailAdapter);
                detailAdapter.ItemClick += DetailAdapter_ItemClick;
            }
            else if (item.Item is TollPlazaCCTV)
            {
                var flocation = item.Item as TollPlazaCCTV;
                baseHolder.mLnlData.Visibility = ViewStates.Visible;
                baseHolder.mLnlHighway.Visibility = ViewStates.Gone;
                baseHolder.mTxtTitle.Text = flocation.strDescription;

                var settings = ((BaseActivity)mContext).getSettingNotification();
                settings = settings.Where(p => p.idParent == flocation.idTollPlazaCctv && p.intType == 2).OrderBy(p => p.dtTime).ToList();

                SettingNotificationDetailAdapter detailAdapter = new SettingNotificationDetailAdapter(mContext, settings, this);
                baseHolder.mLstItems.SetAdapter(detailAdapter);
                detailAdapter.ItemClick += DetailAdapter_ItemClick;
            }
            else if (item.Item is TrafficUpdate)
            {
                var traffic = item.Item as TrafficUpdate;
                baseHolder.mLnlData.Visibility = ViewStates.Visible;
                baseHolder.mLnlHighway.Visibility = ViewStates.Gone;
                baseHolder.mTxtTitle.Text = traffic.strDescription;

                var settings = ((BaseActivity)mContext).getSettingNotification();
                settings = settings.Where(p => p.idParent == traffic.idTrafficUpdate && p.intType == 3).OrderBy(p => p.dtTime).ToList();

                SettingNotificationDetailAdapter detailAdapter = new SettingNotificationDetailAdapter(mContext, settings, this);
                baseHolder.mLstItems.SetAdapter(detailAdapter);
                detailAdapter.ItemClick += DetailAdapter_ItemClick;
            }
        }

        private void DetailAdapter_ItemClick(object sender, SettingNotification e)
        {
            
            FragmentTransaction ft = ((BaseActivity)mContext).FragmentManager.BeginTransaction();
            Fragment prev = ((BaseActivity)mContext).FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            ChooseTimeDialog newFragment = new ChooseTimeDialog(e.dtTime.TimeOfDay.Hours, e.dtTime.Minute);
            newFragment.OnChangeValue += (int hour, int miunite) =>
            {
                SettingNotification newsetting = new SettingNotification();
                newsetting.dtTime = new DateTime(1990, 11, 2, hour, miunite, 0);
                newsetting.idParent = e.idParent;
                newsetting.intType = e.intType;
                newsetting.Tick = DateTime.Now.Ticks;
                var lstSetting = ((BaseActivity)mContext).getSettingNotification();
                lstSetting.Add(newsetting);
                lstSetting.Remove(lstSetting.Where(p => p.Tick == e.Tick && p.intType == e.intType).FirstOrDefault());
                ((BaseActivity)mContext).setSettingNotification(lstSetting);
                this.NotifyDataSetChanged();
            };

            newFragment.Show(ft, "dialog");
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_notification_setting, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

    }
}