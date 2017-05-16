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
using Dex.Com.Expresso.Loyalty.Droid.Activities;

namespace Dex.Com.Expresso.Loyalty.Droid.Adapters
{
    public class MyBaseAdapter : BaseAdapter
    {
        protected Context mContext;

        public BaseActivity Activity
        {
            get
            {
                if (mContext != null)
                {
                    return (BaseActivity)mContext;
                }
                else
                {
                    return null;
                }
            }
            

        }

        public override int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            throw new NotImplementedException();
        }

        public override long GetItemId(int position)
        {
            throw new NotImplementedException();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }
    }
}