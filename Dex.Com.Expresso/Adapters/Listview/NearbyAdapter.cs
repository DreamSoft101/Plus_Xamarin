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
using EXPRESSO.Models;
using Android.Support.V7.Widget;
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Utils;
using Square.Picasso;
using EXPRESSO.Threads;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class NearbyAdapter : RecyclerView.Adapter
    {
        public event EventHandler<BaseItem> ItemClick;
        private Context mContext;
        private List<BaseItem> mLstItem;

        public NearbyAdapter(Context conext, List<BaseItem> lstItem)
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
            public CardView Root;
            public TextView txtName;
            public TextView txtBrand;
            public LinearLayout lnlFavorite;
            public TextView txtHeaderName;
            public LinearLayout lnlHeader;
            public LinearLayout lnlContent;
            public ImageView mImgLogo;
            public ImageView imgFavorite;
            public BaseItem mBaseItem;
            public RecyclerView.Adapter mAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.Root = itemView.FindViewById<CardView>(Resource.Id.cvRoot);
                this.txtHeaderName = itemView.FindViewById<TextView>(Resource.Id.txtHeaderName);
                this.txtName = itemView.FindViewById<TextView>(Resource.Id.txtName);
                this.txtBrand = itemView.FindViewById<TextView>(Resource.Id.txtBrand);
                this.mImgLogo = itemView.FindViewById<ImageView>(Resource.Id.imgLogo);

                this.lnlFavorite = itemView.FindViewById<LinearLayout>(Resource.Id.lnlFavorite);
                this.lnlHeader = itemView.FindViewById<LinearLayout>(Resource.Id.lnlHeader);
                this.lnlContent = itemView.FindViewById<LinearLayout>(Resource.Id.lnlContent);

                this.imgFavorite = itemView.FindViewById<ImageView>(Resource.Id.imgFavorite);
               
                itemView.Click += (sender, e) => itemClick(base.Position);

                this.lnlFavorite.Click += Favorite_Click;
            }

            private void Favorite_Click(object sender, EventArgs e)
            {
                if (mBaseItem.Item is TblNearby)
                {
                    var item = mBaseItem.Item as TblNearby;
                    var baseItem = mBaseItem;
                    if ((bool)baseItem.getTag(BaseItem.TagName.IsFavorite))
                    {
                        baseItem.setTag(BaseItem.TagName.IsFavorite, false);
                    }
                    else
                    {
                        baseItem.setTag(BaseItem.TagName.IsFavorite, true);
                    }
                    FavoriteThread thread = new FavoriteThread();
                    thread.IsToggle(item);
                }
                this.mAdapter.NotifyItemChanged(this.Position);
            }
        }
    

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = GetBaseItem(position);
            item.setTag(BaseItem.TagName.Position, position);
            BaseViewHolder baseHolder = holder as BaseViewHolder;
            baseHolder.mAdapter = this;
            baseHolder.mBaseItem = item;
            baseHolder.lnlHeader.Visibility = ViewStates.Gone;
            baseHolder.lnlContent.Visibility = ViewStates.Gone;
            if (item.Item is TblHighway)
            {
                //Header
                baseHolder.Root.SetCardBackgroundColor(mContext.getColor(Resource.Color.primary));
                var itemHighway = item.Item as TblHighway;
                baseHolder.txtHeaderName.Text = itemHighway.strName;
                baseHolder.lnlHeader.Visibility = ViewStates.Visible;
            }
            else if (item.Item is TblNearby)
            {
                baseHolder.Root.SetCardBackgroundColor(mContext.getColor(Resource.Color.White));
                baseHolder.lnlContent.Visibility = ViewStates.Visible;
                var itemNearby = item.Item as TblNearby;
                baseHolder.txtName.Text = itemNearby.strAddress;
                baseHolder.txtBrand.Text = itemNearby.strTitle;
                Picasso.With(mContext).Load(itemNearby.strLocationImg).Into(baseHolder.mImgLogo);

                if ((bool)item.getTag(BaseItem.TagName.IsFavorite))
                {
                    baseHolder.imgFavorite.SetImageResource(Resource.Drawable.ic_favorite_black_48dp);
                }
                else
                {
                    baseHolder.imgFavorite.SetImageResource(Resource.Drawable.ic_favorite_border_black_48dp);
                }

                
            }
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_nearby, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }
        void OnClick(int position)
        {
            if (ItemClick != null)
            {
                if (GetBaseItem(position).Item is TblHighway)
                {

                }
                else
                {
                    ItemClick(this, GetBaseItem(position));
                }
            }
        }
    }
}