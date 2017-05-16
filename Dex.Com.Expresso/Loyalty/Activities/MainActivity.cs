using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;

using Dex.Com.Expresso.Loyalty.Droid.Fragments;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Newtonsoft.Json;
using Android.Content;
using System;
using Dex.Com.Expresso.Loyalty.Droid.Dialogs;
using System.Collections.Generic;
using Loyalty.Models;
using Loyalty.Models.Database;
using Android.Runtime;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.AutoCompleteTextView;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    //[Activity(Label = "Home")]
    //[Activity(Label = "Beetle buck", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/Icon")]
    [Activity(Label = "@string/app_name", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity 
    {
        private const int Redeem_ChooAdd = 101;
        private const int Redeem_Success = 102;
        private IMenu mIMenu;
        //private SearchView searchView;
        private int RESULT_LOGIN = 99;
        private bool mIsLogined = false;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        private DateTime mdtStartUpdate;
        private bool isNoticeUpdate = false;
        private SearchAdapter adapterSearch;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_main;
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            //if (mCurrentFragment is CartFragment)
            //{
            //    if (requestCode == MemberRedeemInfoProductAdapters.REQUEST_UNLOCK)
            //    {
            //        if (resultCode == Result.Ok)
            //        {
            //            int id = data.GetIntExtra(AuthencationEvoucher.ID, 0);
            //            ((CartFragment)mCurrentFragment).UnLock(id);
            //        }
            //    }
            //}
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            this.IsHomePage = true;
            base.OnCreate(savedInstanceState);
           



            drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //Set hamburger items menu
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            //setup navigation view
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            //handle navigation
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_merchant_product:
                        ListItemClicked(0);
                        break;
                    case Resource.Id.nav_redemption:
                        ListItemClicked(Resource.Id.nav_redemption);
                        break;
                    case Resource.Id.nav_login:
                        {
                            if (mIsLogined)
                            {
                                mIsLogined = false;
                                setCacheString(MyAuth, "");
                                OnResume();
                            }
                            else
                            {
                                e.MenuItem.SetChecked(false);
                                Intent intent = new Intent(this, typeof(LoginActivity));
                                StartActivityForResult(intent, RESULT_LOGIN);
                            }
                            
                            break;
                        }
                    case Resource.Id.nav_aboutprogram:
                        {
                            ListItemClicked(Resource.Id.nav_aboutprogram);
                            break;
                        }
                    case Resource.Id.nav_reference:
                        {
                            ListItemClicked(Resource.Id.nav_reference);
                            break;
                        }
                    case Resource.Id.nav_myfavorite:
                        {
                            ListItemClicked(Resource.Id.nav_myfavorite);
                            break;
                        }
                    case Resource.Id.nav_recently_view:
                        {
                            ListItemClicked(Resource.Id.nav_recently_view);
                            break;
                        }
                    case Resource.Id.nav_preference:
                        {
                            ListItemClicked(Resource.Id.nav_preference);
                            break;
                        }
                    case Resource.Id.nav_whathot:
                        {

                            ListItemClicked(Resource.Id.nav_whathot);
                            break;
                        }
                    case Resource.Id.nav_accountinformation:
                        {
                            ListItemClicked(Resource.Id.nav_accountinformation);
                            break;
                        }
                    case Resource.Id.nav_mycart:
                        {
                           
                            ListItemClicked(Resource.Id.nav_mycart);
                            break;
                        }
                    //case Resource.Id.nav_evouchergallery:
                    //    {
                    //        ListItemClicked(Resource.Id.nav_evouchergallery);
                    //        break;
                    //    }
                    default:
                        {
                            Toast.MakeText(this, Resource.String.loy_mess_function_not_available, ToastLength.Short).Show();
                            return;
                        }
                }

                //Snackbar.Make(drawerLayout, "You selected: " + e.MenuItem.TitleFormatted, Snackbar.LengthLong)
                //    .Show();

                drawerLayout.CloseDrawers();
            };

            if (savedInstanceState == null)
            {
                ListItemClicked(0);
            }

        

            

            bool isFirst = getCacheInt(IsFistTime) == int.MinValue;
           
            if (isFirst)
            {
                Intent intent1 = new Intent(this, typeof(SplashActivity));
                StartActivity(intent1);
            }

            LoadSearchData();
            //ContentThreads contentThread = new ContentThreads
        }

        private void SearchText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            //OnItemClick(null);
            //throw new NotImplementedException();
            if (mCurrentFragment != null)
            {
                if (mCurrentFragment is MerchantOfferFragment)
                {
                    MerchantOfferFragment merchant = (MerchantOfferFragment)mCurrentFragment;
                    merchant.Filter(null);
                }
                else if (mCurrentFragment is MerchantOfferMapsFragment)
                {
                    MerchantOfferMapsFragment merchant = (MerchantOfferMapsFragment)mCurrentFragment;
                    merchant.Filter(null);
                }
            }
        }
        private MDeliveryInfo mAddressOrder;
        private Android.Support.V4.App.Fragment mCurrentFragment;
        int oldPosition = -1;
        private void ListItemClicked(int position, bool isforce = false)
        {
            //this way we don't load twice, but you might want to modify this a bit.
            if (position == oldPosition && isforce == false)
                return;

            oldPosition = position;

            Android.Support.V4.App.Fragment fragment = null;
            switch (position)
            {
                case 0:
                    ShowHideMenu(true);
                    fragment = MerchantOfferFragment.NewInstance();
                    break;
                case 1:
                    ShowHideMenu(true);
                    fragment = MerchantOfferMapsFragment.NewInstance();
                    break;
                case Resource.Id.nav_redemption:
                    this.Title = GetString(Resource.String.loy_title_redemption);
                    ShowHideMenu(false);
                    fragment = RedemptionListFragment.NewInstance();
                    break;
                case Resource.Id.nav_aboutprogram:
                    ShowHideMenu(false);
                    this.Title = GetString(Resource.String.loy_title_aboutprogram);
                    fragment = AboutProgramFragment.NewInstance();
                    break;
                case Resource.Id.nav_reference:
                    {
                        Intent intent = new Intent(this, typeof(LinkAccountActivity));
                        StartActivity(intent);
                        return;
                    }
                case Resource.Id.nav_myfavorite:
                    this.Title = GetString(Resource.String.loy_title_favorite);
                    ShowHideMenu(false);
                    fragment = MerchantOfferFavoritesFragment.NewInstance();
                    break;
                case Resource.Id.nav_recently_view:
                    this.Title = GetString(Resource.String.loy_title_recent);
                    ShowHideMenu(false);
                    fragment = MerchantOfferRecentFragment.NewInstance();
                    break;
                case Resource.Id.nav_whathot:
                    this.Title = GetString(Resource.String.loy_title_whathot);
                    ShowHideMenu(false);
                    fragment = MerchantOfferWhatHotFragment.NewInstance();
                    break;
                case Resource.Id.nav_preference:
                    ShowHideMenu(false);
                    fragment = PreferenceFragment.NewInstance();
                    break;
                case Resource.Id.nav_accountinformation:
                    ShowHideMenu(false);
                    fragment = AccountInfomationFragment.NewInstance();
                    break;
                case Resource.Id.nav_mycart:
                    ShowHideMenu(false);
                    this.Title = GetString(Resource.String.loy_title_mycart);
                    fragment = CartFragment.NewInstance();
                    if ( ( fragment as CartFragment).OnNext == null)
                    {
                        (fragment as CartFragment).OnNext += (int type) =>
                        {
                            ListItemClicked(Redeem_ChooAdd);
                        };
                    }
                    break;
                case Redeem_ChooAdd:
                    ShowHideMenu(false);
                    fragment = ChooseAddressRedeemFragment.NewInstance();
                    if ( (fragment as ChooseAddressRedeemFragment).OnPrevious == null)
                    {
                        (fragment as ChooseAddressRedeemFragment).OnPrevious += () =>
                        {
                            ListItemClicked(Resource.Id.nav_mycart);
                        };
                    }
                    if ((fragment as ChooseAddressRedeemFragment).OnNext == null)
                    {
                        (fragment as ChooseAddressRedeemFragment).OnNext += (MDeliveryInfo address) =>
                        {
                            mAddressOrder = address;
                            ListItemClicked(Redeem_Success);
                        };
                    }
                    break;
                case Redeem_Success:
                    ShowHideMenu(false);
                    fragment = RedempOrderSuccess.NewInstance(mAddressOrder);
                    break;
                //case Resource.Id.nav_evouchergallery:
                //    fragment = EVoucherListFragment.NewInstance();
                //    break;
            }
            mCurrentFragment = fragment;
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
                //case Resource.Id.switch_view:
                //    if (oldPosition == 0)
                //    {
                //        item.SetIcon(Resource.Drawable.loy_ic_list);
                //        ListItemClicked(1);
                //    }
                //    else
                //    {
                //        item.SetIcon(Resource.Drawable.loy_ic_maps);
                //        ListItemClicked(0);
                //    }
                    
                    break;
            }
            
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            base.OnResume();
            //navigationView.Menu.FindItem(Resource.Id.nav_).SetTitle(GetString(Resource.String.loy_menu_logout));

            //navigationView.Menu.FindItem(Resource.Id.nav_aboutprogram).SetVisible(false);
            //navigationView.Menu.FindItem(Resource.Id.nav_redemption).SetVisible(false);
            navigationView.Menu.FindItem(Resource.Id.nav_accountinformation).SetVisible(false);

            string myAuth = getCacheString(MyAuth);
            try
            {
                if (!string.IsNullOrEmpty(myAuth))
                {
                    //navigationView.items
                    navigationView.Menu.FindItem(Resource.Id.nav_login).SetTitle(GetString(Resource.String.loy_menu_logout));
                    mIsLogined = true;
                    MValidateMemberCredentials authData = JsonConvert.DeserializeObject<MValidateMemberCredentials>(myAuth);
                    navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.txtUserName).Text = authData.LoginParams.strLoginId;

                    navigationView.Menu.FindItem(Resource.Id.nav_reference).SetVisible(true);

                    if (authData.MemberProfile != null)
                    {
                        navigationView.Menu.FindItem(Resource.Id.nav_accountinformation).SetVisible(true);
                        navigationView.Menu.FindItem(Resource.Id.nav_reference).SetVisible(false);
                    }
                    else
                    {
                        navigationView.Menu.FindItem(Resource.Id.nav_accountinformation).SetVisible(false);
                        navigationView.Menu.FindItem(Resource.Id.nav_reference).SetVisible(true);
                    }

                    //navigationView.get
                    //navigationView.Menu.RemoveItem(Resource.Id.nav_login);
                }
                else
                {
                    mIsLogined = false;
                    navigationView.Menu.FindItem(Resource.Id.nav_reference).SetVisible(false);
                    navigationView.Menu.FindItem(Resource.Id.nav_login).SetTitle(GetString(Resource.String.loy_menu_login));
                }
            }
            catch (Exception ex)
            {

            }

            bool isFirst = getCacheInt(IsFistTime) == int.MinValue;
            setCacheInt(IsFistTime, int.MaxValue);
            //return;
            if (getLastUpdate() == FirstUpdate && !isFirst)
            {
                
                //Not init before
                FragmentTransaction ft = FragmentManager.BeginTransaction();
                Fragment prev = FragmentManager.FindFragmentByTag("update");
                if (prev != null)
                {
                    // UpdateDialog newFragment = (UpdateDialog)prev;
                    //newFragment.Show(ft, "update");
                    //ft.Remove(prev);
                }
                else
                {
                    ft.AddToBackStack(null);
                    UpdateDialog newFragment = UpdateDialog.NewInstance(null);
                    newFragment.EventOnDismiss += () =>
                    {
                        LoadSearchData();
                        ListItemClicked(0,true);
                    };
                    newFragment.Show(ft, "update");

                }
            }
            else
            {
                if (isNoticeUpdate)
                    return;
                isNoticeUpdate = true;
                //Check update data
                MasterThreads thread = new MasterThreads();
                thread.OnResult += (ServiceResult result) =>
                {
                    if (result.StatusCode == 1)
                    {
                        var data = result.Data as MBB_GetData;
                        bool isHaveData = false;
                        var lstItem = new List<BaseItem>();
                        if (data.MemberTypes != null)
                        {
                            foreach (var item in data.MemberTypes)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.Merchants != null)
                        {
                            foreach (var item in data.Merchants)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MerchantCategories != null)
                        {
                            foreach (var item in data.MerchantCategories)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MerchantProducts != null)
                        {
                            foreach (var item in data.MerchantProducts)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.RedemptionCategories != null)
                        {
                            foreach (var item in data.RedemptionCategories)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.RedemptionProducts != null)
                        {
                            foreach (var item in data.RedemptionProducts)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.RedemptionProductDetails != null)
                        {
                            foreach (var item in data.RedemptionProductDetails)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.RedemptionPartners != null)
                        {
                            foreach (var item in data.RedemptionPartners)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MerchantLocations != null)
                        {
                            foreach (var item in data.MerchantLocations)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MerchantProductMemberTypes != null)
                        {
                            foreach(var item in data.MerchantProductMemberTypes)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MemberGroups != null)
                        {
                            foreach(var item in data.MemberGroups)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MemberGroupDetails != null)
                        {
                            foreach (var item in data.MemberGroupDetails)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.States != null)
                        {
                            foreach (var item in data.States)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.Countries != null)
                        {
                            foreach (var item in data.Countries)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (lstItem.Count > 0)
                        {
                            FragmentTransaction ft = FragmentManager.BeginTransaction();
                            Fragment prev = FragmentManager.FindFragmentByTag("update");
                            if (prev != null)
                            {
                                ft.Remove(prev);
                            }
                            ft.AddToBackStack(null);
                            UpdateDialog newFragment = UpdateDialog.NewInstance(null, lstItem, data.LastGet);
                            newFragment.EventOnDismiss += () =>
                            {
                                
                                // ListItemClicked(Resource.Id.nav_pointsofinterest);
                                LoadSearchData();
                                ListItemClicked(0, true);
                            };
                            newFragment.Show(ft, "update");
                        }
                    }
                };
                //mdtStartUpdate = DateTime.Now;
                thread.GetData(getLastUpdate(), new M_BBGetDataDeletedID());

            }

            
        }
        private AutoCompleteTextView searchText;


        private void LoadSearchData()
        {
            MerchantThreads threadmerchant = new MerchantThreads();

            List<Merchant> lstMerchant = null;
            List<Document> lstDocument = null;
            List<MerchantLocation> lstLocation = null;
            List<Favorites> lstFavorite = null;
            List<MerchantProductMemberType> lstProductMemberType = null;
            //FavoriteThreads thread = new FavoriteThreads();

            List<string> mStrSearchItem = new List<string>();
            threadmerchant.OnResult += (ServiceResult result) =>
            {
                if (result.Data is List<MerchantProduct>)
                {
                    var lstproduct = result.Data as List<MerchantProduct>;

                    adapterSearch = new SearchAdapter(this, lstproduct, lstMerchant, lstLocation, lstProductMemberType);
                    searchText.Adapter = adapterSearch;
                    searchText.TextChanged += SearchText_TextChanged;
                    //searchText.ItemClick += SearchText_ItemClick;
                    //searchText.OnItemClickListener = this;
                    adapterSearch.OnItemClick = OnItemClick;

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
            };
            threadmerchant.HomMerchantOffer();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.loy_search_menu, menu);
            IMenuItem itemSearch = menu.FindItem(Resource.Id.search_view);
            var searchView = MenuItemCompat.GetActionView(itemSearch);
            searchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();
            
            searchText = searchView.FindViewById<AutoCompleteTextView>(Resource.Id.search_src_text);
            searchText.SetHintTextColor(Resources.GetColor(Resource.Color.White));
            searchText.SetTextColor(Resources.GetColor(Resource.Color.White));

            if (adapterSearch == null)
            {
                adapterSearch = new SearchAdapter(this, new List<MerchantProduct>(), new List<Merchant>(), new List<MerchantLocation>(), new List<MerchantProductMemberType>());
            }
            searchText.Threshold = 1;
            searchText.Adapter = adapterSearch;

            this.mIMenu = menu;
            return base.OnCreateOptionsMenu(menu);
        }

        public bool OnQueryTextChange(string newText)
        {
            return true;
        }

        public bool OnQueryTextSubmit(string query)
        {
            return true;
        }

        public void OnItemClick(BaseItem item)
        {
            //throw new NotImplementedException();
            //BaseItem item = adapterSearch.GetBaseItem(position);
            if (item.Item is Merchant)
            {
                searchText.Text = (item.Item as Merchant).MerchantName;
            }
            else if (item.Item is MerchantLocation)
            {
                searchText.Text = (item.Item as MerchantLocation).strLocationName;
            }
            else if (item.Item is MerchantProduct)
            {
                searchText.Text = (item.Item as MerchantProduct).ProductName;
            }
            //searchText.hid
            searchText.DismissDropDown();
            if (mCurrentFragment != null)
            {
                if (mCurrentFragment is MerchantOfferFragment)
                {
                    MerchantOfferFragment merchant = (MerchantOfferFragment)mCurrentFragment;
                    merchant.Filter(item);
                }
                else if (mCurrentFragment is MerchantOfferMapsFragment)
                {
                    MerchantOfferMapsFragment merchant = (MerchantOfferMapsFragment)mCurrentFragment;
                    merchant.Filter(item);
                }
            }
        }

        private void ShowHideMenu(bool isShow)
        {
            if (mIMenu != null)
            {
               // this.mIMenu.FindItem(Resource.Id.switch_view).SetVisible(isShow);
                this.mIMenu.FindItem(Resource.Id.search_view).SetVisible(isShow);
            }
            
        }
    }
}

