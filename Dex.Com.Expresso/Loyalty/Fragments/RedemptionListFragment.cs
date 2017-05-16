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
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Spinners;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews;
using Dex.Com.Expresso.Loyalty.Droid.Dialogs;
using static Android.Widget.AdapterView;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class RedemptionListFragment : BaseFragment
    {
        private List<RedemptionProduct> mLstProduct;
        private List<RedemptionProductDetail> mLstDetail;
        private List<RedemptionCategory> mLstCategory;
        private List<Document> mLstDocument;
        private Spinner mSpnCategory;
        private GridView mGrvProduct;
        private RedemptionCategoryAdapter mCategoryAdapter;
        private RedemptionProductAdapter mProductAdapter;
        private ImageView mImgCart;
        private TextView mtxtCount;

        public delegate void onGotoCart();
        public onGotoCart OnGotoCart;

        //private Spinner
        //private MerchantProductsItemsAdapter adapter;
        public static RedemptionListFragment NewInstance()
        {
            var frag1 = new RedemptionListFragment { Arguments = new Bundle() };
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
            View view = inflater.Inflate(Resource.Layout.loy_fragment_redemption, null);
            //mLstView = view.FindViewById<ListView>(Resource.Id.lstItems);
            mSpnCategory = view.FindViewById<Spinner>(Resource.Id.spnCategory);
            mGrvProduct = view.FindViewById<GridView>(Resource.Id.grvData);
            mtxtCount = view.FindViewById<TextView>(Resource.Id.txtCount);
            mGrvProduct.NumColumns = 2;
            mImgCart = view.FindViewById<ImageView>(Resource.Id.imgCart);
            //mGrvProduct.ItemClick += MGrvProduct_ItemClick;
            mImgCart.Click += MImgCart_Click;
            mSpnCategory.ItemSelected += MSpnCategory_ItemSelected;
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
                    foreach (var item in mLstCategory)
                    {
                        item.intSorting = item.intSorting == null ? 1 : item.intSorting;
                    }
                    mLstCategory.Add(new RedemptionCategory() { RedemptionCategoryID = Guid.Empty, CategoryName = "All Product Categories" , intSorting = 0});
                    mLstCategory = mLstCategory.OrderBy(p => p.intSorting).ToList();
                    mCategoryAdapter = new RedemptionCategoryAdapter(getActivity(), mLstCategory);
                    mSpnCategory.Adapter = mCategoryAdapter;
                }
                else if (result.Data is List<Document>)
                {
                    mLstDocument = result.Data as List<Document>;
                }
            };
            thread.RedemptionHomePage();



            thread.OnGetCartCount += (int count) =>
            {
                if (count == 0)
                {
                    mtxtCount.Visibility = ViewStates.Gone;
                }
                else
                {
                    mtxtCount.Text = count + "";
                    mtxtCount.Visibility = ViewStates.Visible;
                }
            };
            thread.GetCartCount();

            return view;

            
        }

        private void MImgCart_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            int count = 0;
            try
            {
                count = Convert.ToInt32(mtxtCount.Text);
            }
            catch (Exception ex)
            {

            }
          
            if (count > 0)
            {
                if (OnGotoCart != null)
                {
                    OnGotoCart();
                }
            }
        }

        private void MSpnCategory_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //throw new NotImplementedException();
            var category = mCategoryAdapter.GetBaseItem(e.Position);
            if (category != null)
            {
                var lstItem = category.RedemptionCategoryID != Guid.Empty ? mLstProduct.Where(p => p.RedemptionCategoryID == category.RedemptionCategoryID) : mLstProduct.ToList();
                lstItem = lstItem.Where(p => p.Available == true).ToList();
                lstItem = lstItem.Where(p => p.intProductType == 6 || (p.intProductType == 1 && ( p.TnGoVoucher == null || p.TnGoVoucher == false ))).ToList();
                mProductAdapter = new RedemptionProductAdapter(getActivity(), lstItem.ToList(), mLstDetail, mLstDocument);
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
                                    mtxtCount.Visibility = ViewStates.Gone;
                                }
                                else
                                {
                                    mtxtCount.Text = count + "";
                                    mtxtCount.Visibility = ViewStates.Visible;
                                }
                            };
                            thread.GetCartCount();
                        };
                        newFragment.Show(ft, "redemption_product");

                    }


                };

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