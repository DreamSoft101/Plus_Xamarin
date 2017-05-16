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
using Dex.Com.Expresso.Fragments;
using Java.Lang;
using EXPRESSO.Threads;
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Activities;
using static EXPRESSO.Models.EnumType;

namespace Dex.Com.Expresso.Adapters.Viewpager
{
    public class PointOfInterestAdapter : FragmentStatePagerAdapter
    {
        private List<string> titles;
        private Context mContext;
        private List<TblNearbyCatg> mLstNearbyCatg;
        private List<TblFacilityType> mLstFacilityType;
        public PointOfInterestAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            
        }

        public override int GetItemPosition(Java.Lang.Object objectValue)
        {
            return PositionNone;
        }
        

        public PointOfInterestAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            mContext = context;
            titles = new List<string>();
            titles.Add(mContext.GetString(Resource.String.facilities_type_rsa));
            titles.Add(mContext.GetString(Resource.String.facilities_type_lay_by));
            titles.Add(mContext.GetString(Resource.String.facilities_type_toll_plaza));
            titles.Add(mContext.GetString(Resource.String.facilities_type_petrol_station));
            titles.Add(mContext.GetString(Resource.String.facilities_type_csc));
            titles.Add(mContext.GetString(Resource.String.facilities_type_plussmie));
            titles.Add(mContext.GetString(Resource.String.facilities_type_ssk));
            titles.Add(mContext.GetString(Resource.String.facilities_type_interchange));
            titles.Add(mContext.GetString(Resource.String.facilities_type_vistapoint));
            titles.Add(mContext.GetString(Resource.String.facilities_type_tunnel));

            PointOfInterestThread thread = new PointOfInterestThread();
            thread.OnLoadNearbyCategory += (result) =>
            {
                mLstNearbyCatg = result;
                foreach (TblNearbyCatg item in result)
                {
                    titles.Add(item.strNearbyCatgName);
                }
            };
            thread.OnLoadFacilityType += (result) =>
            {
                mLstFacilityType = result;
                //foreach (var item in mLstFacilityType)
                //{
                //    titles.Add
                //}
            };
            thread.loadHomePointOfInterest();
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
                return Fragment_RSA.NewInstance();
            }
            else if (position == 1)
            {
                return Fragment_Lay_By.NewInstance();
            }
            else if (position == 2)
            {
                return Fragment_TollPlaza.NewInstance();
            }
            else if (position == 3)
            {
                return Fragment_PertrolStation.NewInstance();
            }
            else if (position == 4)
            {
                return Fragment_CSC.NewInstance();
            }
            else if (position == 5)
            {
                return Fragment_Facilities_Others.NewInstance(FacilitiesType.PLUSSmile);
            }
            else if (position == 6)
            {
                return Fragment_Facilities_Others.NewInstance(FacilitiesType.SSK);
            }
            else if (position == 7)
            {
                return Fragment_Facilities_Others.NewInstance(FacilitiesType.INTERCHANGE);
            }
            else if (position == 8)
            {
                return Fragment_Facilities_Others.NewInstance(FacilitiesType.VISTAPOINT);
            }
            else if (position == 9)
            {
                return Fragment_Facilities_Others.NewInstance(FacilitiesType.TUNNEL);
            }
            else
            {
                position -= 10;
                return Fragment_Nearby.NewInstance(mLstNearbyCatg[position].idNearbyCatg, mLstNearbyCatg[position].strNearbyCatgName);
            }
        }
    }
}