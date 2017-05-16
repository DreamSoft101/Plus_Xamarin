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
using Square.Picasso;
using Android.Graphics;
using Android.Graphics.Drawables;
using EXPRESSO.Utils;

namespace Dex.Com.Expresso.Adapters.Galerry
{
    public class Operations_MediaAdapter : BaseAdapter
    {
        private Context mContext;
        private List<Operations_Media> mLstItem;
        public delegate void onLoadedImage(Bitmap bitmap, int position);
        public onLoadedImage OnLoadedImage;
        public Operations_MediaAdapter(Context conext, List<Operations_Media> lstItem)
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

        public void SetData(List<Operations_Media> lstItem)
        {
            this.mLstItem = lstItem;
            this.NotifyDataSetChanged();
        }
        public List<Operations_Media> getListItems()
        {
            return mLstItem;
        }
        

        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }


        public Operations_Media getImage(int pos)
        {
            if (pos >= mLstItem.Count)
            {
                return null;
            }
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView imgImage;
            public View mImgMainView;
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
                holder.mImgMainView = convertView;
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            if (item == null)
            {
                holder.mImgMainView.Visibility = ViewStates.Gone;
            }
            else
            {
                holder.mImgMainView.Visibility = ViewStates.Visible;
                if (item.media_url.StartsWith("http"))
                {
                    Picasso.With(mContext).Load(item.media_url).Error(Resource.Drawable.img_error).Into(new LoaderTarget(holder.imgImage, position, OnLoadedImage));
                }
                else
                {
                    Picasso.With(mContext).Load(Cons.IMG_URL_PLUS + item.media_url).Error(Resource.Drawable.img_error).Into(new LoaderTarget(holder.imgImage, position, OnLoadedImage));

                }
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