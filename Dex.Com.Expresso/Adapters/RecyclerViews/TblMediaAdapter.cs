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
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Activities;
using Square.Picasso;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Dex.Com.Expresso.Adapters.RecyclerViews
{
    public class TblMediaAdapter : RecyclerView.Adapter
    {
        public event EventHandler<TblMedia> ItemClick;
        private Context mContext;
        private List<TblMedia> mLstItem;
        public delegate void onLoadedImage(Bitmap bitmap, int position);
        public onLoadedImage OnLoadedImage;

        public List<TblMedia> getListItems()
        {
            return mLstItem;
        }

        public TblMediaAdapter(Context conext, List<TblMedia> lstItem)
        {
            this.mContext = conext;
            mLstItem = lstItem;
            //mLstItem = mLstItem.Where(p => p.isEnable == true).ToList();
        }

        public override int ItemCount
        {
            get
            {
                return mLstItem.Count;
            }
        }



        public TblMedia GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public void AddMedia(TblMedia media)
        {
            this.mLstItem.Add(media);
        }

        public void UpdateMedia(TblMedia media)
        {
            var item = mLstItem.Where(p => p.idMedia == media.idMedia).FirstOrDefault();
            item.mStrComment = media.mStrComment;
        }

        public void DeleteItem(TblMedia media)
        {
            mLstItem.Remove(media);
            this.NotifyDataSetChanged();
        }


        private class BaseViewHolder : RecyclerView.ViewHolder
        {

            //public event EventHandler<TblRSA> ItemClick;
            public CardView Root;
            public ImageView imgImage;
            public TblMedia mBaseItem;
            public RecyclerView.Adapter mAdapter;

            public BaseViewHolder(View itemView, Action<int> itemClick) : base(itemView)
            {
                // Locate and cache view references:
                this.Root = itemView.FindViewById<CardView>(Resource.Id.cvRoot);
                this.imgImage = itemView.FindViewById<ImageView>(Resource.Id.imgImage);
                itemView.Click += (sender, e) => itemClick(base.Position);

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

            if (item.strURL.StartsWith("http"))
            {
                Picasso.With(mContext).Load(item.strURL).Error(Resource.Drawable.img_error).Into(new LoaderTarget(baseHolder.imgImage, position, OnLoadedImage));
            }
            else
            {
                Picasso.With(mContext).Load(Android.Net.Uri.Parse(item.strURL)).Error(Resource.Drawable.img_error).Into(new LoaderTarget(baseHolder.imgImage, position, OnLoadedImage));
            }
        }

        private class LoaderTarget : Java.Lang.Object, ITarget
        {
            private ImageView img;
            private int intPosition;
            private onLoadedImage OnLoadedImage;
            public LoaderTarget(ImageView view, int pos, onLoadedImage OnLoadedImage)
            {
                img = view;
                this.intPosition = pos;
                this.OnLoadedImage = OnLoadedImage;
            }


            public void OnBitmapFailed(Drawable p0)
            {
                img.SetImageResource(Resource.Drawable.img_error);
                if (OnLoadedImage != null)
                {
                    OnLoadedImage(null, intPosition);
                }
            }

            public void OnBitmapLoaded(Bitmap p0, Picasso.LoadedFrom p1)
            {
                img.SetImageBitmap(p0);
                if (OnLoadedImage != null)
                {
                    OnLoadedImage(p0, intPosition);
                }
            }


            public void OnPrepareLoad(Drawable p0)
            {

            }

        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_image, parent, false);
            BaseViewHolder holder = new BaseViewHolder(itemView, OnClick);
            return holder;
        }

    }
}