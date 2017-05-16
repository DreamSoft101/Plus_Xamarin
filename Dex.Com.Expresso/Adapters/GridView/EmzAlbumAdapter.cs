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

namespace Dex.Com.Expresso.Adapters.GridView
{
    public class EmzAlbumAdapter : BaseAdapter
    {
        private Context mContext;
        private List<EmzAlbum> mLstItem;

        public EmzAlbumAdapter(Context conext, List<EmzAlbum> lstItem)
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

        public List<EmzAlbum> getListItems()
        {
            return mLstItem;
        }

    

        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }

    

        public EmzAlbum getBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView iv_album;
            public TextView tv_name_manager;
            public TextView tv_munber;
            
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = getBaseItem(position);
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.emz_item_album, null);
                holder.iv_album = convertView.FindViewById<ImageView>(Resource.Id.iv_album);
                holder.tv_name_manager = convertView.FindViewById<TextView>(Resource.Id.tv_name_manager);
                holder.tv_munber = convertView.FindViewById<TextView>(Resource.Id.tv_munber);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            Picasso.With(mContext).Load(item.Picture).Error(Resource.Drawable.img_error).Into(holder.iv_album);
            holder.tv_munber.Text = item.Quantity + " books";
            holder.tv_name_manager.Text = item.Title;
            return convertView;
        }
    }
}