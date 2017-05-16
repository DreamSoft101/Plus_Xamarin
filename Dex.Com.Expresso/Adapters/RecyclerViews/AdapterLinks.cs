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

namespace Dex.Com.Expresso.Adapters.RecyclerViews
{
    public class Link
    {
        public int Icon { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
    }
    public class AdapterLinks : RecyclerView.Adapter
    {
        public event EventHandler<Link> ItemClick;
        private Context mContext;
        private List<Link> mLstItem;
        public AdapterLinks(Context conext)
        {
            this.mContext = conext;
            mLstItem = new List<Link>();
            mLstItem.Add(new Link() { Icon = Resource.Drawable.ic_link_plus, Title = "PLUS", URL = "www.plus.com.my" });
            mLstItem.Add(new Link() { Icon = Resource.Drawable.ic_link_plusmiles, Title = "PLUSMiles", URL = "www.plusmiles.com.my" });
            mLstItem.Add(new Link() { Icon = Resource.Drawable.ic_link_facebook, Title = "PLUSMiles Facebook", URL = "www.facebook.com" });
            mLstItem.Add(new Link() { Icon = Resource.Drawable.ic_link_waze, Title = "Waze", URL = "www.waze.com" });
            mLstItem.Add(new Link() { Icon = Resource.Drawable.ic_link_mufors, Title = "Mufors", URL ="www.mufors.org.my" });

        }

        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
        }



        public Link GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class BaseViewHolder : RecyclerView.ViewHolder
        {

            public Link mBaseItem;
            public TextView mTxtTitle;
            public TextView mTxtURL;
            public ImageView mImgIcon;
            public RecyclerView.Adapter mAdapter;

            public BaseViewHolder(View itemView, Action<Link> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.mImgIcon = itemView.FindViewById<ImageView>(Resource.Id.imgIcon);
                this.mTxtTitle = itemView.FindViewById<TextView>(Resource.Id.txtTitle);
                this.mTxtURL = itemView.FindViewById<TextView>(Resource.Id.txtURL);

                itemView.Click += (sender, e) => itemClick(mBaseItem);

            }
        }


        void OnClick(Link position)
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
            baseHolder.mTxtTitle.Text = item.Title;
            baseHolder.mTxtURL.Text = item.URL;

            baseHolder.mImgIcon.SetImageResource(item.Icon);
            //baseHolder.txtName.Text = name;
            
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_link, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

    }
}