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
    public  class LinearLayoutPoolObjectFactory : PoolObjectFactory<LinearLayout>
    {
        private Context context;

        public LinearLayoutPoolObjectFactory(Context context)
        {
            this.context = context;
        }

        public LinearLayout createObject()
        {
            return new LinearLayout(context, null);
        }
    }
}