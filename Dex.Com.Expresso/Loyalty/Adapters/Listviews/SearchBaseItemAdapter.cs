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
using Loyalty.Models;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews
{
    public class SearchBaseItemAdapter : MyBaseAdapter
    {
        private List<BaseItem> mLstItem;
        public override int Count
        {
            get
            {
                return mLstItem.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            //throw new NotImplementedException();
            return null;
        }

        public BaseItem getBaseItem(int position)
        {
            return mLstItem[position];
        }

        public override long GetItemId(int position)
        {
            // throw new NotImplementedException();
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }
    }
}