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

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews
{
    public class MerchantProductAdapter : MyBaseAdapter
    {
        private List<MerchantProduct> mLstItem;
        //private List<MerchantProduct> mLstItem;
        public MerchantProductAdapter(Context conext, List<MerchantProduct> lstItem)
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

        public MerchantProduct GetBaseItem(int pos)
        {
            return mLstItem[pos];
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public ImageView mImgPicture;
            public TextView mTxtOffer;
            
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHoder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.loy_item_merchantoffer_2x2, null);
                viewHoder = new ViewHolder();
                viewHoder.mTxtOffer = convertView.FindViewById<TextView>(Resource.Id.txtOffer);

                convertView.Tag = viewHoder;
            }
            else
            {
                viewHoder = convertView.Tag as ViewHolder;
            }

            viewHoder.mTxtOffer.Rotation = -45;
            return convertView;
        }
    }
}