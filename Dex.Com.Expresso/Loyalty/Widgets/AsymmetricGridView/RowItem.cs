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

namespace Dex.Com.Expresso.Loyalty.Droid.Widgets.AsymmetricGridView
{
    public class RowItem :  IParcelable
    {
        private AsymmetricItem item;
        private int index;

        public IntPtr Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public RowItem(int index, AsymmetricItem item)
        {
            this.item = item;
            this.index = index;
        }
        public AsymmetricItem getItem()
        {
            return item;
        }
        public int getIndex()
        {
            return index;
        }

        public int DescribeContents()
        {
            //throw new NotImplementedException();
            return 0;
        }

        public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
        {
            //dest.WriteParcelable
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}