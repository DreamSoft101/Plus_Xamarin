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
using EXPRESSO.Models.Database;
using Square.Picasso;
using Android.Graphics;
using Android.Graphics.Drawables;
using EXPRESSO.Utils;

namespace Dex.Com.Expresso.Adapters.Galerry
{
    public class TblMediaAdapter : BaseAdapter
    {
        private Context mContext;
        private List<TblMedia> mLstItem;
        public delegate void onLoadedImage(Bitmap bitmap, int position);
        public onLoadedImage OnLoadedImage;

        public TblMediaAdapter(Context conext, List<TblMedia> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
        }

        public override int Count
        {
            get
            {
                return mLstItem.Count;
            }
        }

        public List<TblMedia> getListItems()
        {
            return mLstItem;
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

        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }

        public void DeleteItem(TblMedia media)
        {
            mLstItem.Remove(media);
            this.NotifyDataSetChanged();
        }

        public TblMedia getImage(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView imgImage;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = getImage(position);
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.item_image, null);
                holder.imgImage = convertView.FindViewById<ImageView>(Resource.Id.imgImage);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            if (item.strURL.StartsWith("http"))
            {
                Picasso.With(mContext).Load(item.strURL).Error(Resource.Drawable.img_error).Into(new LoaderTarget(holder.imgImage, position, OnLoadedImage));
            }
            else
            {
                Picasso.With(mContext).Load(Android.Net.Uri.Parse(item.strURL)).Error(Resource.Drawable.img_error).Into(new LoaderTarget(holder.imgImage, position, OnLoadedImage));
            }
            

            return convertView;
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

    }
}