using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Dex.Com.Expresso.Activities;

namespace Dex.Com.Expresso.Fragments
{
    public class BaseFragment : Fragment
    {
        public BaseActivity getActivity()
        {
            return (BaseActivity)this.Activity;
        }

        
    }
}