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
using Dex.Com.Expresso.Loyalty.Droid.Adapters;
using Loyalty.Models;

namespace Dex.Com.Expresso.Loyalty.Adapters.Spinners
{
    public class CardTypeAdapter : MyBaseAdapter
    {


        private List<CardType> mLstItem;
        //private List<MerchantProduct> mLstItem;
        public CardTypeAdapter(Context conext)
        {
            this.mContext = conext;
            this.mLstItem = new List<CardType>();
            this.mLstItem.Add(new CardType() { ID = 1, Name = "RFID TAG" });
            this.mLstItem.Add(new CardType() { ID = 2, Name = "PLUSMILES MFG NUMBER" });

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

        public CardType GetBaseItem(int pos)
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

            viewHoder.mTxtName.Text = item.Name;
            return convertView;
        }
    }
}