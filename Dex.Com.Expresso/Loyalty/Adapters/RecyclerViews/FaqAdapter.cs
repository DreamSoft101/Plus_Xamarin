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

namespace Dex.Com.Expresso.Loyalty.Adapters.RecyclerViews
{
    public class FaqAdapter : RecyclerView.Adapter
    {
        public event EventHandler<MFAQ> ItemClick;
        private Context mContext;
        private List<MFAQ> mLstItems;
        private int intPositionRemoved = -1;

        public FaqAdapter(Context conext, List<MFAQ> lstItem)
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



        public MFAQ GetBaseItem(int pos)
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
            public MFAQ mBaseItem;
            public TextView txtAnser;
            public TextView txtQuestion;
            public RecyclerView.Adapter mAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                this.txtAnser = itemView.FindViewById<TextView>(Resource.Id.txtAnser);
                this.txtQuestion = itemView.FindViewById<TextView>(Resource.Id.txtQuestion);
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

            baseHolder.txtAnser.Text = item.Answer;
            baseHolder.txtQuestion.Text = (position+1) + ". "  +  item.Question;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.loy_item_faq, parent, false);
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