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
using Dex.Com.Expresso.Activities;

namespace Dex.Com.Expresso.Adapters.RecyclerViews
{
    public class SettingNotificationDetailAdapter : RecyclerView.Adapter
    {
        public event EventHandler<SettingNotification> ItemClick;
        private Context mContext;
        private List<SettingNotification> mLstItem;
        private SettingNotificationAdapters mAdater;

        public SettingNotificationDetailAdapter(Context conext, List<SettingNotification> lstItems, SettingNotificationAdapters adapter)
        {
            this.mContext = conext;
            this.mLstItem = lstItems;
            this.mAdater = adapter;
           // mLstItem = (conext as BaseActivity).getFavoriteLocation();

        }

        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
        }



        public SettingNotification GetBaseItem(int pos)
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
            public CardView Root;
            public TextView txtName;
            public TextView txtDelete;
            public SettingNotification mBaseItem;
            public RecyclerView.Adapter mAdapter;
            public Context mContext;
            public SettingNotificationAdapters mPAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.Root = itemView.FindViewById<CardView>(Resource.Id.cvRoot);
                this.txtDelete = itemView.FindViewById<TextView>(Resource.Id.txtDelete);
                this.txtName = itemView.FindViewById<TextView>(Resource.Id.txtName);

                txtDelete.Click += TxtDelete_Click;
                itemView.Click += (sender, e) => itemClick(base.Position);

            }

            private void TxtDelete_Click(object sender, EventArgs e)
            {
                var lst = ((BaseActivity)mContext).getSettingNotification();
                lst.Remove(lst.Where(p => p.Tick == mBaseItem.Tick).FirstOrDefault());
                ((BaseActivity)mContext).setSettingNotification(lst);
                mPAdapter.NotifyDataSetChanged();
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
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            baseHolder.mContext = mContext;
            string name = item.dtTime.ToString("HH:mm");
            baseHolder.txtName.Text = name;
            baseHolder.mPAdapter = mAdater;
            
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_notification_detail, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

    }
}