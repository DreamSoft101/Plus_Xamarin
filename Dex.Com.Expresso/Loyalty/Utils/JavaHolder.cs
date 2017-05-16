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

namespace Dex.Com.Expresso.Loyalty.Droid.Utils
{
    public class JavaHolder : Java.Lang.Object
    {
        public readonly object Instance;
        public JavaHolder(object instance)
        {
            Instance = instance;
        }
        
    }
}