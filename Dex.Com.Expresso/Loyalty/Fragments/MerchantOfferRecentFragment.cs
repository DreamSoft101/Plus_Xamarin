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
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews;
using Loyalty.Threads;
using Loyalty.Models.Database;
using Dex.Com.Expresso.Loyalty.Droid.Utils;
using Loyalty.Models.ServiceOutput;
using Loyalty.Models;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    class MerchantOfferRecentFragment : BaseFragment
    {
        private ListView mLstView;
        private MerchantProductsItemsAdapter adapter;
        public static MerchantOfferRecentFragment NewInstance()
        {
            var frag1 = new MerchantOfferRecentFragment { Arguments = new Bundle() };
            return frag1;
        }

        public void Filter(BaseItem item)
        {
            if (adapter != null)
            {
                adapter.Filter(item);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.loy_YourFragment, container, false);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_merchantoffer, null);

            mLstView = view.FindViewById<ListView>(Resource.Id.lstItems);

            MerchantThreads thread = new MerchantThreads();
            List<Merchant> lstMerchant = null;
            List<Document> lstDocument = null;
            List<MerchantLocation> lstLocation = null;
            List<Favorites> lstFavorite = null;
            List<MerchantProductMemberType> lstProductMemberType = null;
            List<MemberGroup> lstMemberGroup = null;
            List<MemberGroupDetail> lstMemberGroupDetail = null;
            //FavoriteThreads thread = new FavoriteThreads();

            thread.OnResult += (ServiceResult result) =>
            {
                if (result.Data is List<MerchantProduct>)
                {
                    List<MerchantProduct> lstItem = result.Data as List<MerchantProduct>;
                    lstItem = lstItem.OrderByDescending(p => p.decRating).ToList();
                    adapter = new MerchantProductsItemsAdapter(getActivity(), lstItem, lstMerchant, lstDocument, lstLocation, lstFavorite, lstProductMemberType, lstMemberGroup, lstMemberGroupDetail);
                    mLstView.Adapter = adapter;

                    var loc = GPSUtils.getLastBestLocation(getActivity());
                    if (loc != null)
                    {
                        adapter.UpdateMyLocation(loc);
                    }
                }
                else if (result.Data is List<Document>)
                {
                    lstDocument = result.Data as List<Document>;
                }
                else if (result.Data is List<Merchant>)
                {
                    lstMerchant = result.Data as List<Merchant>;
                }
                else if (result.Data is List<MerchantLocation>)
                {
                    lstLocation = result.Data as List<MerchantLocation>;
                }
                else if (result.Data is List<Favorites>)
                {
                    lstFavorite = result.Data as List<Favorites>;
                }
                else if (result.Data is List<MerchantProductMemberType>)
                {
                    lstProductMemberType = result.Data as List<MerchantProductMemberType>;
                }
                else if (result.Data is List<MemberGroupDetail>)
                {
                    lstMemberGroupDetail = result.Data as List<MemberGroupDetail>;
                }
                else if (result.Data is List<MemberGroup>)
                {
                    lstMemberGroup = result.Data as List<MemberGroup>;
                }

            };
            thread.RecentMerchantOffer();

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            if (adapter != null)
            {
                adapter.UpdateFavorite();
                var loc = GPSUtils.getLastBestLocation(getActivity());
                if (loc != null)
                {
                    adapter.UpdateMyLocation(loc);
                }
            }
        }
    }
}