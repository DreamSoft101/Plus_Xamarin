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

namespace Dex.Com.Expresso.Adapters.Spinner
{
    public class SearchAdapter : BaseAdapter
    {
        private Context mContext;
        private string[] mLstItem;

        public SearchAdapter(Context conext, string[] lstItem)
        {
            this.mContext = conext;
            this.mLstItem = lstItem;
        }

        public override int Count
        {
            get
            {
                return mLstItem.Length;
            }
        }


        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }

        public string GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.spn_item, null);
            }
            TextView txtText = (TextView)convertView.FindViewById(Resource.Id.txtText);
            var item = GetBaseItem(position);
            txtText.Text = item;
            return convertView;
        }
    }
}