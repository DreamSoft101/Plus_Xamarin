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
using Java.Lang;
using Android.Support.V4.App;
using Dex.Com.Expresso.Fragments;
using static EXPRESSO.Models.EnumType;

namespace Dex.Com.Expresso.Adapters.Viewpager
{
    public class FavoritesAdapter : FragmentStatePagerAdapter
    {
        private List<string> titles;
        private Context mContext;
        public FavoritesAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }


        public FavoritesAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            mContext = context;
            titles = new List<string>();
            titles.Add(mContext.GetString(Resource.String.facilities_type_rsa));
            titles.Add(mContext.GetString(Resource.String.facilities_type_toll_plaza));
            titles.Add(mContext.GetString(Resource.String.facilities_type_petrol_station));
            titles.Add(mContext.GetString(Resource.String.facilities_type_csc));
            titles.Add(mContext.GetString(Resource.String.facilities_type_facilities));
            titles.Add(mContext.GetString(Resource.String.facilities_type_nearby));
            //titles.Add(mContext.GetString(Resource.String.facilities_type_plussmie));
            //titles.Add(mContext.GetString(Resource.String.facilities_type_ssk));

        }

        public override int Count
        {
            get
            {
                return titles.Count;
            }
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(titles[position]);
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if (position == 0)
            {
                return Fragment_Favorite_RSA.NewInstance();
            }
            else if (position == 1)
            {
                return Fragment_Favorite_TollPlaza.NewInstance();
            }
            else if (position == 2)
            {
                return Fragment_Favorites_PertrolStation.NewInstance();
            }
            else if (position == 3)
            {
                return Fragment_Favorites_CSC.NewInstance();
            }
            else if (position == 4)
            {
                return Fragment_Favorite_Faclities.NewInstance();
            }
            else if (position == 5)
            {
                return Fragment_Favorites_Nearby.NewInstance();
            }
            return null;
        }
    }
}