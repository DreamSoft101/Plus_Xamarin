using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Dex.Com.Expresso.Activities;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_About : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment_About NewInstance()
        {
            var frag1 = new Fragment_About { Arguments = new Bundle() };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_about, null);
            view.FindViewById<TextView>(Resource.Id.txtTerms).Click += Fragment_About_Click;
            return view;
        }

        private void Fragment_About_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Intent intent = new Intent(this.Activity, typeof(TermsActivity));
            StartActivity(intent);
        }
    }
}