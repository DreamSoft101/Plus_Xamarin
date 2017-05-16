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
using Android.Webkit;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_PlusTwitter : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment_PlusTwitter NewInstance()
        {
            var frag1 = new Fragment_PlusTwitter { Arguments = new Bundle() };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.exp_fragment_twitter, null);

            WebView webiew = view.FindViewById<WebView>(Resource.Id.webView1);

            webiew.Settings.JavaScriptEnabled = true;

            webiew.SetWebViewClient(new WebViewClient());
            webiew.LoadUrl("https://twitter.com/plustrafik");
            
            return view;
        }
    }
}