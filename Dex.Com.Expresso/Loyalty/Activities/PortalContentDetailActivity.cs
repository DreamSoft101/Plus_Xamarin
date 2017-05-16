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
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Android.Webkit;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "PortalContentDetailActivity")]
    public class PortalContentDetailActivity : BaseActivity
    {
        public static string IDCONTENT = "IDContent";
        public static string NAMECONTENT = "NAMEContent";
        private View lnlLoading;
        private WebView mWebView;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_portalcontentdetail;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            lnlLoading = FindViewById<View>(Resource.Id.lnlLoading);
            mWebView = FindViewById<WebView>(Resource.Id.webView1);

            var idMenu = new Guid(this.Intent.GetStringExtra(IDCONTENT));
            this.Title = this.Intent.GetStringExtra(NAMECONTENT);
            ContentThreads thread = new ContentThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.StatusCode == 1)
                {
                    MGetPortalMenu data = result.Data as MGetPortalMenu;
                    if (data.PortalMenuDetails.Count > 0)
                    {
                        PortalMenuDetail detail = data.PortalMenuDetails[0];
                        mWebView.LoadData( detail.strBody, "text/html", "UTF-8");
                        lnlLoading.Visibility = ViewStates.Gone;
                    }
                }
                else
                {
                    Toast.MakeText(this, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.GetPortalContent(idMenu,null);
            //thread.onl
            // Create your application here
        }
    }
}