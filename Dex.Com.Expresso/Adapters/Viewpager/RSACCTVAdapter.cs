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
using Android.Support.V4.App;
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Fragments;

namespace Dex.Com.Expresso.Adapters.Viewpager
{
    public class RSACCTVAdapter : FragmentStatePagerAdapter
    {
        private List<TblRSACctv> mLstItem;
        private Context mContext;
        public override int Count
        {
            get
            {
                return mLstItem.Count;
            }
        }

        public RSACCTVAdapter(Context context, List<TblRSACctv> lstItem, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            this.mContext = context;
            this.mLstItem = lstItem;
        }

        

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return Fragment_Facility_Detail_CCTV_Item.NewInstance(mLstItem[position].strURL);
        }
    }
}