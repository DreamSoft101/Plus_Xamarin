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

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "TermsActivity")]
    public class TermsActivity : BaseActivity
    {
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.exp_activity_terms;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = GetString(Resource.String.text_terms);
            WebView termsWv = FindViewById<WebView>(Resource.Id.webView1);
            termsWv.LoadUrl("file:///android_asset/terms.html");
            // Create your application here
        }
    }
}