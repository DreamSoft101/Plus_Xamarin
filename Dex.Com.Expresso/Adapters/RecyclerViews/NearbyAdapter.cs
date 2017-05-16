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
using EXPRESSO.Threads;
using Square.Picasso;

namespace Dex.Com.Expresso.Adapters.RecyclerViews
{
    public class NearbyAdapter : RecyclerView.Adapter
    {
        public event EventHandler<BaseItem> ItemClick;
        private Context mContext;
        private List<BaseItem> mLstItem;

        public delegate void onErrorImage(string uri, int position);
        public onErrorImage OnErroImage;

        public NearbyAdapter(Context conext, List<BaseItem> urls)
        {
            this.mContext = conext;
            mLstItem = urls;

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
            public Context mContext;
            public LinearLayout mLnlHighway, mLnlContent;
            public TextView mTxtHighwayName, mTxtName;
            public ImageView mImgPicture;
            public RecyclerView mLstIcon;
            public LinearLayoutManager mLnlManager;
            public ImageView mImgFavorite;
            public BaseItem mBaseItem;
            public RecyclerView.Adapter mAdapter;
            public IconFacilitiesAdapter mIconAdapter;
            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.mImgPicture = itemView.FindViewById<ImageView>(Resource.Id.imgIcon);
                this.mLnlHighway = itemView.FindViewById<LinearLayout>(Resource.Id.lnlHighway);
                this.mLnlContent = itemView.FindViewById<LinearLayout>(Resource.Id.lnlContent);
                this.mTxtHighwayName = ItemView.FindViewById<TextView>(Resource.Id.txtHighwayName);
                this.mTxtName = ItemView.FindViewById<TextView>(Resource.Id.txtName);
                this.mImgPicture = ItemView.FindViewById<ImageView>(Resource.Id.imgMain);
                this.mLstIcon = ItemView.FindViewById<RecyclerView>(Resource.Id.lstItems);
                this.mLnlManager = new LinearLayoutManager(mContext, LinearLayoutManager.Horizontal, false);
                this.mLstIcon.SetLayoutManager(this.mLnlManager);
                this.mImgFavorite = ItemView.FindViewById<ImageView>(Resource.Id.imgFavorite);
                this.mImgFavorite.Click += MImgFavorite_Click;
                itemView.Click += (sender, e) => itemClick(base.Position);


            }

            private void MImgFavorite_Click(object sender, EventArgs e)
            {
                //throw new NotImplementedException();
                var row = Convert.ToInt32((sender as ImageView).Tag.ToString());
                if (mBaseItem.Item is TblNearby)
                {
                    var nearby = mBaseItem.Item as TblNearby;
                    FavoriteThread thread = new FavoriteThread();
                    thread.IsToggle(nearby);

                    var isfavorite = (bool)mBaseItem.getTag(BaseItem.TagName.IsFavorite);
                    isfavorite = !isfavorite;
                    mBaseItem.setTag(BaseItem.TagName.IsFavorite, isfavorite);

                    if (isfavorite)
                    {
                        this.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
                    }
                    else
                    {
                        this.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
                    }

                    mAdapter.NotifyItemChanged(row);

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
            baseHolder.mBaseItem = item;
            baseHolder.mAdapter = this;
            baseHolder.mContext = mContext;
            baseHolder.mImgFavorite.Tag = position;
            if (item.Item is TblHighway)
            {
                baseHolder.mLnlHighway.Visibility = ViewStates.Visible;
                baseHolder.mLnlContent.Visibility = ViewStates.Gone;
                baseHolder.mTxtHighwayName.Text = (item.Item as TblHighway).strName;
            }
            else
            {
                var itemf = item.Item as TblNearby;
                var isfavorite = (bool)item.getTag(BaseItem.TagName.IsFavorite);
                baseHolder.mLnlHighway.Visibility = ViewStates.Gone;
                baseHolder.mLnlContent.Visibility = ViewStates.Visible;
                baseHolder.mTxtName.Text = itemf.strTitle;


                PointOfInterestThread thread = new PointOfInterestThread();
                thread.OnGetNearbyCategoryItem += (TblNearbyCatg catItem) =>
                {
                    if (string.IsNullOrEmpty(catItem.strNearbyCatgImg))
                    {
                        Picasso.With(mContext).Load(Resource.Drawable.img_error).Into(baseHolder.mImgPicture);
                    }
                    else
                    {
                        Picasso.With(mContext).Load(catItem.strNearbyCatgImg).Error(Resource.Drawable.img_error).Into(baseHolder.mImgPicture);
                    }

                };
                thread.getNearbyCategoryItem(itemf.idNearbyCatg);

                
                if (isfavorite)
                {
                    baseHolder.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite);
                }
                else
                {
                    baseHolder.mImgFavorite.SetImageResource(Resource.Drawable.loy_ic_favorite_un);
                }

                baseHolder.mLstIcon.Visibility = ViewStates.Gone;
                //if (itemf.SubUrlImg != null)
                //{
                //    if (itemf.SubUrlImg.Count > 0)
                //    {
                //        if (baseHolder.mIconAdapter == null)
                //        {
                //            baseHolder.mIconAdapter = new IconFacilitiesAdapter(mContext, itemf.SubUrlImg);
                //            baseHolder.mLstIcon.SetAdapter(baseHolder.mIconAdapter);

                //        }
                //        else
                //        {
                //            baseHolder.mIconAdapter.SetData(itemf.SubUrlImg);
                //        }

                //        baseHolder.mLstIcon.Visibility = ViewStates.Visible;
                //    }
                //}
            }


        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.exp_item_facilities, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

    }
}