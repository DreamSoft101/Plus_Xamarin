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
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.Spinners
{
    public class ReferenceTypeAdapter : MyBaseAdapter
    {

        public class ReferenceType
        {
            public int Type;
            public string Name;
        }

        private List<ReferenceType> mLstItem;
        //private List<MerchantProduct> mLstItem;
        public ReferenceTypeAdapter(Context conext)
        {
            this.mContext = conext;
            this.mLstItem = new List<ReferenceType>();
            this.mLstItem.Add(new ReferenceType() { Type = 1, Name = "IC No" });
            this.mLstItem.Add(new ReferenceType() { Type = 2, Name = "Old IC No" });
            this.mLstItem.Add(new ReferenceType() { Type = 3, Name = "Passpord" });
            this.mLstItem.Add(new ReferenceType() { Type = 4, Name = "Police Card" });
            this.mLstItem.Add(new ReferenceType() { Type = 5, Name = "Army Card" });
            this.mLstItem.Add(new ReferenceType() { Type = 6, Name = "Others" });

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

        public ReferenceType GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return mLstItem[position].Type;
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

            viewHoder.mTxtName.Text = item.Name;
            return convertView;
        }
    }
}