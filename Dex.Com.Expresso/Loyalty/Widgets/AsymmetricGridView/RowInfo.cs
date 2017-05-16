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

namespace Dex.Com.Expresso.Loyalty.Droid.Widgets.AsymmetricGridView
{
    public class RowInfo : IParcelable
    {
        private List<RowItem> items;
        private int rowHeight;
        private float spaceLeft;

   
        public RowInfo(int rowHeight, List<RowItem> items, float spaceLeft)
        {
            this.rowHeight = rowHeight;
            this.items = items;
            this.spaceLeft = spaceLeft;
        }

        public RowInfo(Parcel input)
        {
            rowHeight = input.ReadInt();
            spaceLeft = input.ReadFloat();
            int totalItems = input.ReadInt();

            items = new List<RowItem>();
            ClassLoader classLoader = Java.Lang.Class.FromType(typeof(AsymmetricItem)).ClassLoader;

            for (int i = 0; i < totalItems; i++)
            {
                items.Add(new RowItem(input.ReadInt(), (AsymmetricItem)input.ReadParcelable(classLoader)));
            }
        }

        public List<RowItem> getItems()
        {
            return items;
        }

        public int getRowHeight()
        {
            return rowHeight;
        }
        public float getSpaceLeft()
        {
            return spaceLeft;
        }
        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Parcel dest, int flags)
        {
            dest.WriteInt(rowHeight);
            dest.WriteFloat(spaceLeft);
            dest.WriteInt(items.Count);

            foreach (RowItem rowItem in items)
            {
                dest.WriteInt(rowItem.getIndex());
                dest.WriteParcelable((IParcelable)rowItem.getItem(), 0);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
        public RowInfo[] newArray(int size)
        {
            return new RowInfo[size];
        }
    }
}