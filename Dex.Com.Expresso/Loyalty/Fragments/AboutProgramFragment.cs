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
using Loyalty.Models.ServiceOutput;
using Loyalty.Threads;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class AboutProgramFragment : BaseFragment
    {
        private LinearLayout mLnlLoading;
        private LinearLayout mLnlData;
        private List<MenuPortal> mLstProduct;
        private ListView mLstData;
        private ContentMenuAdapter mAdapter;
        //private Spinner
        //private MerchantProductsItemsAdapter adapter;
        public static AboutProgramFragment NewInstance()
        {
            var frag1 = new AboutProgramFragment { Arguments = new Bundle() };
            return frag1;
        }

        //public void Filter(BaseItem item)
        //{
        //    if (adapter != null)
        //    {
        //        adapter.Filter(item);
        //    }
        //}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.loy_YourFragment, container, false);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_aboutprogram, null);
            mLnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);

            mLstData = view.FindViewById<ListView>(Resource.Id.lstData);
            mLstData.ItemClick += MLstData_ItemClick;
            ContentThreads thread = new ContentThreads();

            mLnlData.Visibility = ViewStates.Gone;
            mLnlLoading.Visibility = ViewStates.Visible;

            thread.OnResult += (ServiceResult result) =>
            {
                if (result.StatusCode ==1)
                {
                    mLnlData.Visibility = ViewStates.Visible;
                    mLnlLoading.Visibility = ViewStates.Gone;

                    MGetMenuPortal lstdata = result.Data as MGetMenuPortal;
                    List<MenuPortal> data = lstdata.MenuPortals;
                    mAdapter = new ContentMenuAdapter(getActivity(), data);
                    mLstData.Adapter = mAdapter;
                }
                else
                {
                    Toast.MakeText(this.Activity, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.GetPortalMenu();

            return view;
        }

        private void MLstData_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //throw new NotImplementedException();
            var menu = mAdapter.GetBaseItem(e.Position);
            Intent intent = new Intent(getActivity(), typeof(PortalContentDetailActivity));
            intent.PutExtra(PortalContentDetailActivity.IDCONTENT, menu.idPortalMenu.ToString());
            intent.PutExtra(PortalContentDetailActivity.NAMECONTENT, menu.strName);
            StartActivity(intent);
        }

        public override void OnResume()
        {
            base.OnResume();
        }
    }
}