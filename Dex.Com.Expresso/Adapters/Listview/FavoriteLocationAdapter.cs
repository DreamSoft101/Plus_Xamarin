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

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class FavoriteLocationAdapter : BaseAdapter
    {
        private Context mContext;
        private List<FavoriteLocation> mLstItem;
        private List<FavoriteLocation> mLstSettings;
        public FavoriteLocationAdapter(Context conext, List<FavoriteLocation> lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
            mLstSettings = (conext as BaseActivity).getFavoriteLocation();
          
        }

      

        public List<FavoriteLocation> getSaveData()
        {
            return mLstSettings;
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

        public FavoriteLocation GetHighway(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public TextView txtName;
            public CheckBox swtEnable;
            public bool IsAuto = false;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = GetHighway(position);
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.exp_item_favorite_location_setting, null);
                holder.txtName = convertView.FindViewById<TextView>(Resource.Id.txtName);
                holder.swtEnable = convertView.FindViewById<CheckBox>(Resource.Id.swtEnable);
                holder.swtEnable.CheckedChange += SwtEnable_CheckedChange;
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            holder.txtName.Text = item.strFavoriteLocationName;
            var setting = mLstSettings.Where(p => p.idFavoriteLocation == item.idFavoriteLocation).FirstOrDefault();
            holder.swtEnable.Tag = position;
            holder.swtEnable.Checked = setting == null ? false : true;

            return convertView;
        }


        private void SwtEnable_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            int pos = (int)(sender as CheckBox).Tag;
            var chooseItem = mLstItem[pos];
            var item = mLstSettings.Where(p => p.idFavoriteLocation == chooseItem.idFavoriteLocation).FirstOrDefault();
            if (e.IsChecked == (item == null ? false : true))
            {
                return;
            }

            if (item == null && e.IsChecked)
            {
                item = new FavoriteLocation();
                item.idHighway = chooseItem.idHighway;
                item.idFavoriteLocation = chooseItem.idFavoriteLocation;
                item.strFavoriteLocationName = chooseItem.strFavoriteLocationName;
                item.detail = chooseItem.detail;
                mLstSettings.Add(item);
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

            (mContext as BaseActivity).setFavoriteLocation(mLstSettings);

        }
    }
}