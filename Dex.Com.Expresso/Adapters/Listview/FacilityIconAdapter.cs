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
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Utils;

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class FacilityIconAdapter : BaseAdapter
    {
        private Context mContext;
        private List<BaseItem> mLstItem;
        public FacilityIconAdapter(Context conext, List<BaseItem> lstItem)
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

        public BaseItem GetBaseItem(int pos)
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
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            var item = GetBaseItem(position);
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.item_icon, null);
                holder.imIcon = convertView.FindViewById<ImageView>(Resource.Id.imgIcon);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            
            if (item.Item is TblFacilities)
            {
                var itemFac = item.Item as TblFacilities;
                int idIcon = mContext.GetResourceID("ic_facility_type" + itemFac.intFacilityType);
                if (itemFac.intFacilityType == 0 || itemFac.intFacilityType == 2)
                {
                    string idBrand = ((Java.Lang.String)item.getTag(BaseItem.TagName.Facility_BrandID)).ToString();
                    idIcon = mContext.GetResourceID(idBrand.ToLower());
                }
                holder.imIcon.SetImageResource(idIcon);
            }
            return convertView;
        }
    }
}