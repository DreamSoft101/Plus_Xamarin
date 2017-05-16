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
    public interface AsymmetricView
    {
        bool isDebugging();
        int getNumColumns();
        bool isAllowReordering();
        void fireOnItemClick(int index, View v);
        bool fireOnItemLongClick(int index, View v);
        int getColumnWidth();
        int getDividerHeight();
        int getRequestedHorizontalSpacing();
    }
}