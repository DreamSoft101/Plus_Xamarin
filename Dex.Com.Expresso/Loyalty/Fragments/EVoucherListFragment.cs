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
using Loyalty.Models.Database;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Spinners;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso.Loyalty.Droid.Dialogs;
using Dex.Com.Expresso.Loyalty.Adapters.Listviews;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews;
using Dex.Com.Expresso.Activities;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class EVoucherListFragment : BaseFragment
    {
        private List<RedemptionProduct> mLstProduct;
        private List<RedemptionProductDetail> mLstDetail;
        private List<RedemptionCategory> mLstCategory;
        private List<Document> mLstDocument;
        private Spinner mSpnCategory;
        private GridView mGrvProduct;
        private RedemptionCategoryAdapter mCategoryAdapter;
        private EVoucherProductAdapter mProductAdapter;
        private TextView mTxtCount;
        public delegate void onGoToCart();
        public onGoToCart OnGoToCart;
        //private Spinner
        //private MerchantProductsItemsAdapter adapter;
        public static EVoucherListFragment NewInstance()
        {
            var frag1 = new EVoucherListFragment { Arguments = new Bundle() };
            return frag1;
        }

     
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.loy_YourFragment, container, false);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_redemption, null);
            //mLstView = view.FindViewById<ListView>(Resource.Id.lstItems);
            mSpnCategory = view.FindViewById<Spinner>(Resource.Id.spnCategory);
            mGrvProduct = view.FindViewById<GridView>(Resource.Id.grvData);
            mGrvProduct.NumColumns = 1;
            //mGrvProduct.ItemClick += MGrvProduct_ItemClick;
            mTxtCount = view.FindViewById<TextView>(Resource.Id.txtCount);
            mSpnCategory.ItemSelected += MSpnCategory_ItemSelected;
            mSpnCategory.Visibility = ViewStates.Gone;
            RedemptionThreads thread = new RedemptionThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.Data is List<RedemptionProduct>)
                {
                    mLstProduct = result.Data as List<RedemptionProduct>;

                }
                else if (result.Data is List<RedemptionProductDetail>)
                {
                    mLstDetail = result.Data as List<RedemptionProductDetail>;
                }
                else if (result.Data is List<RedemptionCategory>)
                {
                    mLstCategory = result.Data as List<RedemptionCategory>;
                    mCategoryAdapter = new RedemptionCategoryAdapter(getActivity(), mLstCategory);
                    //mSpnCategory.Adapter = mCategoryAdapter;
                    LoadEVoucher();
                }
                else if (result.Data is List<Document>)
                {
                    mLstDocument = result.Data as List<Document>;
                }
            };
            thread.GetEVoucher();

            thread.OnGetCartCount += (int count) =>
            {
                if (count == 0)
                {
                    mTxtCount.Visibility = ViewStates.Gone;
                }
                else
                {
                    mTxtCount.Visibility = ViewStates.Visible;
                    mTxtCount.Text = count + "";
                }
            };
            thread.GetCartCount();
            mTxtCount.Click += MTxtCount_Click;
            return view;


        }

        private void MTxtCount_Click(object sender, EventArgs e)
        {
            if (mTxtCount.Visibility == ViewStates.Visible)
                (this.getActivity() as MainActivity).ListItemClicked(Resource.Id.nav_cart);
        }

        private void LoadEVoucher()
        {
            var lstItem = mLstProduct.Where(p =>  p.intProductType == 6);
            mProductAdapter = new EVoucherProductAdapter(getActivity(), lstItem.ToList(), mLstDetail, mLstDocument);
            mGrvProduct.Adapter = mProductAdapter;
            mProductAdapter.OnItemClick += (int position) =>
            {
                var product = mProductAdapter.GetBaseItem(position);
                var detail = mLstDetail.Where(p => p.RedemptionProductID == product.RedemptionProductID).FirstOrDefault();
                var document = mLstDocument.Where(p => p.ID == product.ImageID).FirstOrDefault();

                FragmentTransaction ft = this.Activity.FragmentManager.BeginTransaction();
                Fragment prev = this.Activity.FragmentManager.FindFragmentByTag("redemption_product");
                if (prev != null)
                {
                    // UpdateDialog newFragment = (UpdateDialog)prev;
                    //newFragment.Show(ft, "update");
                    //ft.Remove(prev);
                }
                else
                {
                    ft.AddToBackStack(null);
                    RedemptionDialog newFragment = RedemptionDialog.NewInstance(null, product, detail, document);
                    newFragment.OnChange += () =>
                    {
                        RedemptionThreads thread = new RedemptionThreads();
                        thread.OnGetCartCount += (int count) =>
                        {
                            if (count == 0)
                            {
                                mTxtCount.Visibility = ViewStates.Gone;
                            }
                            else
                            {
                                mTxtCount.Visibility = ViewStates.Visible;
                                mTxtCount.Text = count + "";
                            }
                        };
                        thread.GetCartCount();
                    };
                    newFragment.Show(ft, "redemption_product");

                }
            };

        }


        private void MSpnCategory_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //throw new NotImplementedException();
            var category = mCategoryAdapter.GetBaseItem(e.Position);
            if (category != null)
            {
               
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            //if (adapter != null)
            //{
            //    adapter.UpdateFavorite();
            //    var loc = GPSUtils.getLastBestLocation(getActivity());
            //    if (loc != null)
            //    {
            //        adapter.UpdateMyLocation(loc);
            //    }
            //}
        }

    }
}