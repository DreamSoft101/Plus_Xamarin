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
    class VehicleClassesAdapter : BaseAdapter
    {
        private Context mContext;
        private List<VehicleClasses> mLstItem;

        public VehicleClassesAdapter(Context conext, List<VehicleClasses> lstItem)
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

        public VehicleClasses GetVehicleClasses(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public ImageView mImgCar;
            public TextView mTxtName;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetVehicleClasses(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.item_vehicle_classes, null);
                viewHoder = new ViewHolder();
                viewHoder.mImgCar = convertView.FindViewById<ImageView>(Resource.Id.imgCar);
                viewHoder.mTxtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                convertView.Tag = viewHoder;
            }
            else
            {
                viewHoder = convertView.Tag as ViewHolder;
            }
            viewHoder.mTxtName.Text = item.strName;
            switch (item.intValue)
            {
                case 1:
                    viewHoder.mImgCar.SetImageResource(Resource.Drawable.car_class1);
                    break;
                case 2:
                    viewHoder.mImgCar.SetImageResource(Resource.Drawable.car_class2);
                    break;
                case 3:
                    viewHoder.mImgCar.SetImageResource(Resource.Drawable.car_class3);
                    break;
                case 4:
                    viewHoder.mImgCar.SetImageResource(Resource.Drawable.car_class4);
                    break;
                case 5:
                    viewHoder.mImgCar.SetImageResource(Resource.Drawable.car_class5);
                    break;
            }
            return convertView;
        }
    }
}