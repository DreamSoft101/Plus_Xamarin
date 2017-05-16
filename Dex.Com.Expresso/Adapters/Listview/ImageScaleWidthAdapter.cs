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
using Square.Picasso;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class ImageScaleWidthAdapter : BaseAdapter
    {
        private Context mContext;
        private List<string> mLstItem;
        public ImageScaleWidthAdapter(Context conext, List<string> lstItem)
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


        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }

        public string getImage(int pos)
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
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.item_image_scale_width, null);
                holder.imgImage = convertView.FindViewById<ImageView>(Resource.Id.imgImage);


                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }

            Picasso.With(mContext).Load(item).Error(Resource.Drawable.img_error).Into(holder.imgImage);

            return convertView;
        }


    }
}