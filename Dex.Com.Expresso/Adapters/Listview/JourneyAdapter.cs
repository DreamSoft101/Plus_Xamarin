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

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class JourneyAdapter : BaseAdapter
    {
        private Context mContext;
        private List<Journey> mLstItem;

        public JourneyAdapter(Context conext, List<Journey> lstItem)
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

        public Journey GetJourney(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public TextView nameTv;
            public TextView typeTv;
            public TextView locationTv;
            public TextView exitTv;
            public ImageView triangleIv;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = GetJourney(position);
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.item_journey, null);
                holder.nameTv = convertView.FindViewById<TextView>(Resource.Id.tv_name);
                holder.typeTv = convertView.FindViewById<TextView>(Resource.Id.tv_parent);
                holder.locationTv = convertView.FindViewById<TextView>(Resource.Id.tv_location);
                holder.exitTv = convertView.FindViewById<TextView>(Resource.Id.tv_exit);
                holder.triangleIv = convertView.FindViewById<ImageView>(Resource.Id.iv_triangle);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            holder.nameTv.Text = item.mStrParentName;
            holder.typeTv.Text = item.mStrParentTypeName;
            holder.locationTv.Text = String.Format(mContext.GetString(Resource.String.toll_fare_km_format), item.mTblRouteDetail.decLocation);
            holder.triangleIv.Visibility = position == 0 ? ViewStates.Visible : ViewStates.Invisible;
            if (!string.IsNullOrEmpty(item.mTblRouteDetail.strExit))
            {
                holder.exitTv.Visibility = ViewStates.Visible;
                holder.exitTv.Text = item.mTblRouteDetail.strExit;
            }
            else
            {
                holder.exitTv.Visibility = ViewStates.Invisible;
            }

            return convertView;
        }
    }
}