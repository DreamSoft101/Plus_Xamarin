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
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews
{
    public class MStatementFileAdapters : RecyclerView.Adapter
    {
        public event EventHandler<MStatementFile> ItemClick;
        private Context mContext;
        private List<MStatementFile> mLstItems;
        private int intPositionRemoved = -1;

        public MStatementFileAdapters(Context conext, List<MStatementFile> lstItem)
        {
            this.mContext = conext;
            this.mLstItems = lstItem;
        }

        public override int ItemCount
        {
            get
            {
                return mLstItems.Count;
            }
        }



        public MStatementFile GetBaseItem(int pos)
        {
            return mLstItems[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }


        private class BaseViewHolder : RecyclerView.ViewHolder
        {
            public Context mContext;
            public MStatementFile mBaseItem;
            public TextView txtName;
            public RecyclerView.Adapter mAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.txtName = itemView.FindViewById<TextView>(Resource.Id.txtName);
                itemView.Click += (sender, e) => itemClick(base.Position);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mContext = mContext;
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            baseHolder.txtName.Text = item.Name;

            //var gdetail = mLstMemberGroupDetail.Where(p => p.MemberTypeID == item.idMemberType).FirstOrDefault();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.loy_item_estatementfile, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, GetBaseItem(position));
            }
        }

    }


}