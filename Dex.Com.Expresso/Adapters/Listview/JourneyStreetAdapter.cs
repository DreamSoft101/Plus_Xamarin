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

namespace Dex.Com.Expresso.Adapters.Listview
{
    public class JourneyStreetAdapter : BaseAdapter
    {
        private Context mContext;

        public JourneyStreetAdapter(Context conext)
        {
            this.mContext = conext;
        }

        public override int Count
        {
            get
            {
                return 30;
            }
        }


        public override Java.Lang.Object GetItem(int position)
        {
            //return (mLstItem[position]).Cast<VehicleClasses>();
            return null;
        }

        public Journey GetTollPlaza(int pos)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.item_journey_street, null);
            }
           
            return convertView;
        }
    }
}