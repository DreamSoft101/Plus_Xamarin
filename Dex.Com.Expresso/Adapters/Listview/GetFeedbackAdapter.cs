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

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class GetFeedbackAdapter : RecyclerView.Adapter
    {
        public event EventHandler<GetFeedback> ItemClick;
        private Context mContext;
        private List<GetFeedback> mLstItem;

        public GetFeedbackAdapter(Context conext, List<GetFeedback> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
        }

        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
        }

        public void addItem(GetFeedback feedBack)
        {
            mLstItem.Add(feedBack);
            this.NotifyItemChanged(mLstItem.Count - 1);
        }



        public GetFeedback GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class BaseViewHolder : RecyclerView.ViewHolder
        {
            public CardView Root;
            public TextView txtName;
            public TextView txtContent;
            public TextView txtTime;
            public GetFeedback mBaseItem;
            public RecyclerView.Adapter mAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.Root = itemView.FindViewById<CardView>(Resource.Id.cvRoot);
                this.txtName = itemView.FindViewById<TextView>(Resource.Id.txtCreateBy);
                this.txtContent = itemView.FindViewById<TextView>(Resource.Id.txtContent);
                this.txtTime = itemView.FindViewById<TextView>(Resource.Id.txtTime);
                itemView.Click += (sender, e) => itemClick(base.Position);

            }
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
          
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mAdapter = this;
            baseHolder.mBaseItem = item;
            baseHolder.txtContent.Text = item.strContent;
            baseHolder.txtName.Text = item.strCreatedBy;
            baseHolder.txtTime.Text = item.dtCreatedDate;
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_comment, parent, false);
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