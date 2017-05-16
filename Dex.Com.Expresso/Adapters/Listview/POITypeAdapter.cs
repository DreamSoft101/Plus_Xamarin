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
using Dex.Com.Expresso.Activities;
using EXPRESSO.Models.Database;
using Square.Picasso;
using EXPRESSO.Utils;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class POITypeAdapter : BaseAdapter
    {
        private Context mContext;
        private List<POITypeSetting> mLstItem;
        private List<POITypeSetting> mLstSettings;
        private Settings set;
        public POITypeAdapter(Context conext, List<POITypeSetting> lstitem)
        {
            this.mContext = conext;
            this.mLstItem = lstitem;// ((BaseActivity)mContext).getAllPOIType();
            mLstSettings = ((BaseActivity)mContext).getFavoritePOIType();
           
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

        public POITypeSetting GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView imIcon;
            public TextView mTxtName;
            public CheckBox mCkeChoose;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.exp_item_facilities_type, null);
                holder.imIcon = convertView.FindViewById<ImageView>(Resource.Id.imgIcon);
                holder.mCkeChoose = convertView.FindViewById<CheckBox>(Resource.Id.ckeChoose);
                holder.mTxtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                holder.mCkeChoose.CheckedChange += MCkeChoose_CheckedChange;
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }

            if (string.IsNullOrEmpty(item.strURL))
            {
                holder.imIcon.Visibility = ViewStates.Invisible;
            }
            else
            {
                holder.imIcon.Visibility = ViewStates.Visible;
                Picasso.With(mContext).Load(item.strURL).Error(Resource.Drawable.img_error).Into(holder.imIcon);
            }

            

            var setting = mLstSettings.Where(p => p.intID == item.intID && p.intType == item.intType).FirstOrDefault();

            holder.mTxtName.Text = item.strName;
            holder.mCkeChoose.Tag = position;
            holder.mCkeChoose.Checked = setting == null ? false : true;
            return convertView;
        }

        private void MCkeChoose_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            int pos = (int)(sender as CheckBox).Tag;
            var chooseItem = mLstItem[pos];
            var item = mLstSettings.Where(p => p.intID == chooseItem.intID && p.intNearby == chooseItem.intNearby).FirstOrDefault();
            if (e.IsChecked == (item == null ? false : true))
            {
                return;
            }

        

            if (item == null && e.IsChecked)
            {
                POITypeSetting type = new POITypeSetting();
                type.intID = chooseItem.intID;
                type.strName = chooseItem.strName;
                type.intType = chooseItem.intType;
                type.intDataPOIType = chooseItem.intDataPOIType;
                type.intNearby = chooseItem.intNearby;
                type.intRSAType = chooseItem.intRSAType;
                mLstSettings.Add(type);
            }
            else
            {
                if (item != null)
                {
                    if (!e.IsChecked)
                    {
                        mLstSettings.Remove(item);
                    }
                }
                else
                {

                }

            }

            (mContext as BaseActivity).setFavoritePOIType(mLstSettings);

        }
    }
}