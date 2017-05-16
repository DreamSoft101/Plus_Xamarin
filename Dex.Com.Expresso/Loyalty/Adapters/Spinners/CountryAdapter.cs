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
using Loyalty.Models.Database;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.Spinners
{
    public class CountryAdapter : MyBaseAdapter
    {

        public class ReferenceType
        {
            public int Type;
            public string Name;
        }

        private List<Country> mLstItem;
        //private List<MerchantProduct> mLstItem;
        public CountryAdapter(Context conext, List<Country> lstItem)
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
            return null;// mLstItem[position].ToJavaObject();
        }

        public Country GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            //return mLstItem[position];
            return 0;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView mTxtName;

        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_spinner, null);
                viewHoder = new ViewHolder();
                viewHoder.mTxtName = convertView.FindViewById<TextView>(Resource.Id.txtName);

                convertView.Tag = viewHoder;
            }
            else
            {
                viewHoder = convertView.Tag as ViewHolder;
            }

            viewHoder.mTxtName.Text = item.RegionName;
            return convertView;
        }
    }
}