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
using Android.Widget;
using Java.Lang;

namespace Dex.Com.Expresso.Loyalty.Droid.Widgets.AsymmetricGridView
{
    public class AsymmetricViewImpl
    {
        private static int DEFAULT_COLUMN_COUNT = 2;
        protected int numColumns = DEFAULT_COLUMN_COUNT;
        protected int requestedHorizontalSpacing;
        protected int requestedColumnWidth;
        protected int requestedColumnCount;
        protected bool allowReordering;
        protected bool debugging;

        public AsymmetricViewImpl(Context context)
        {
            requestedHorizontalSpacing = Utils.dpToPx(context, 5);
        }

        public void setRequestedColumnWidth(int width)
        {
            requestedColumnWidth = width;
        }

        public void setRequestedColumnCount(int requestedColumnCount)
        {
            this.requestedColumnCount = requestedColumnCount;
        }

        public int getRequestedHorizontalSpacing()
        {
            return requestedHorizontalSpacing;
        }

        public void setRequestedHorizontalSpacing(int spacing)
        {
            requestedHorizontalSpacing = spacing;
        }

        public int determineColumns(int availableSpace)
        {
            int numColumns;

            if (requestedColumnWidth > 0)
            {
                numColumns = (availableSpace + requestedHorizontalSpacing) /
                    (requestedColumnWidth + requestedHorizontalSpacing);
            }
            else if (requestedColumnCount > 0)
            {
                numColumns = requestedColumnCount;
            }
            else
            {
                // Default to 2 columns
                numColumns = DEFAULT_COLUMN_COUNT;
            }

            if (numColumns <= 0)
            {
                numColumns = 1;
            }

            this.numColumns = numColumns;

            return numColumns;
        }

        public SavedState onSaveInstanceState(Parcel superState)
        {
            
            SavedState ss = new SavedState(superState);
            ss.allowReordering = allowReordering;
            ss.debugging = debugging;
            ss.numColumns = numColumns;
            ss.requestedColumnCount = requestedColumnCount;
            ss.requestedColumnWidth = requestedColumnWidth;
            ss.requestedHorizontalSpacing = requestedHorizontalSpacing;
            return ss;
        }

        public void onRestoreInstanceState(SavedState ss)
        {
            allowReordering = ss.allowReordering;
            debugging = ss.debugging;
            numColumns = ss.numColumns;
            requestedColumnCount = ss.requestedColumnCount;
            requestedColumnWidth = ss.requestedColumnWidth;
            requestedHorizontalSpacing = ss.requestedHorizontalSpacing;
        }

        public int getNumColumns()
        {
            return numColumns;
        }

        public int getColumnWidth(int availableSpace)
        {
            return (availableSpace - ((numColumns - 1) * requestedHorizontalSpacing)) / numColumns;
        }

        public bool isAllowReordering()
        {
            return allowReordering;
        }

        public void setAllowReordering(bool allowReordering)
        {
            this.allowReordering = allowReordering;
        }

        public bool isDebugging()
        {
            return debugging;
        }

        public void setDebugging(bool debugging)
        {
            this.debugging = debugging;
        }









        public class SavedState : View.BaseSavedState
        {
            public int numColumns;
            public int requestedColumnWidth;
            public int requestedColumnCount;
            public int requestedVerticalSpacing;
            public int requestedHorizontalSpacing;
            public int defaultPadding;
            public bool debugging;
            public bool allowReordering;
            public IParcelable adapterState;
            public ClassLoader loader;

            //public SavedState(Parcel superState) : base(superState)
            //{

            //}

            public SavedState(Parcel input) : base(input)
            {
                numColumns = input.ReadInt();
                requestedColumnWidth = input.ReadInt();
                requestedColumnCount = input.ReadInt();
                requestedVerticalSpacing = input.ReadInt();
                requestedHorizontalSpacing = input.ReadInt();
                defaultPadding = input.ReadInt();
                debugging = input.ReadByte() == 1;
                allowReordering = input.ReadByte() == 1;
                adapterState = (IParcelable)input.ReadParcelable(loader);
            }

            public void writeToParcel(Parcel dest, ParcelableWriteFlags flags)
            {
                base.WriteToParcel(dest, flags);

                dest.WriteInt(numColumns);
                dest.WriteInt(requestedColumnWidth);
                dest.WriteInt(requestedColumnCount);
                dest.WriteInt(requestedVerticalSpacing);
                dest.WriteInt(requestedHorizontalSpacing);
                dest.WriteInt(defaultPadding);
                dest.WriteByte((sbyte)(debugging ? 1 : 0));
                dest.WriteByte((sbyte)(allowReordering ? 1 : 0));
                dest.WriteParcelable(adapterState, flags);
            }
        }
    }
}