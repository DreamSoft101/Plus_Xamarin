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
using Java.Lang;
using Dex.Com.Expresso.Fragments;

namespace Dex.Com.Expresso.Adapters.Viewpager
{
    public class RSAAdapter : FragmentStatePagerAdapter
    {
        private List<string> titles;
        private Context mContext;
        public delegate void onInitData(Filter filter);
        public onInitData OnInitData;
        public RSAAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }


        public RSAAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            mContext = context;
            titles = new List<string>();
            titles.Add(mContext.GetString(Resource.String.facilities_type_rsa));
            titles.Add(mContext.GetString(Resource.String.text_lay_by));
            //titles.Add(mContext.GetString(Resource.String.facilities_type_plussmie));
            //titles.Add(mContext.GetString(Resource.String.facilities_type_ssk));

        }

        public override int Count
        {
            get
            {
                return 2;
            }
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(titles[position]);
        }

        private Fragment_RSA f1;
        private Fragment_Lay_By f2;

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if (position == 0)
            {
                if (f1 == null)
                {
                    f1 = Fragment_RSA.NewInstance();
                    f1.OnInitData += (Dex.Com.Expresso.Adapters.Listview.RSAAdapter adapter) =>
                    {
                        if (OnInitData != null)
                        {
                            OnInitData(f1.getAdapter().Filter);
                        }
                    };
                }
                return f1;
            }
            else if (position == 1)
            {
                if (f2 == null)
                {
                    f2 = Fragment_Lay_By.NewInstance();
                }
                return f2;
            }
            return null;
        }
    }
}