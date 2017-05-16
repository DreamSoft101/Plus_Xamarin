using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;

using Dex.Com.Expresso.Fragments;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using EXPRESSO.Threads;
using Android.Content;
using Dex.Com.Expresso.Dialogs;
using EXPRESSO.Models;
using EXPRESSO.Utils;
using System.Collections.Generic;
using System;
using EXPRESSO.Models.Database;
using System.IO;
using Android.Views.InputMethods;
using Dex.Com.Expresso.Utils;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using Android.Runtime;
using Dex.Com.Expresso.Services;
using Square.Picasso;
using static Android.App.ActionBar;
using Dex.Com.Expresso.Loyalty.Fragments;
using Dex.Com.Expresso.Loyalty.Droid.Fragments;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews;
using Loyalty.Models;
using Dex.Com.Expresso.Adapters.Spinner;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/Icon", Theme = "@style/MyTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
   
    public class MainActivity : BaseActivity , IDialogInterfaceOnClickListener
    {
        private const int Redeem_ChooAdd = 101;
        private const int Redeem_Success = 102;
        private MDeliveryInfo mAddressOrder;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        private DateTime mdtStartUpdate;
        private bool isNoticeUpdate = false;
        private AutoCompleteTextView searchText;
        private IMenu mIMenu;
        private ImageView mImgAvatar;
        private TextView mTxtEmail;
        private Filter mFilter;

        public enum MenuType
        {
            Setting = 9,
            About = 10
        }

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.main;
            }
        }

        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            this.isHomePage = true;
            base.OnCreate(savedInstanceState);
            StartService(new Intent(this, typeof(SynceService)));

            //return;
            drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //Set hamburger items menu
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            //setup navigation view
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            //handle navigation
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                if (e.MenuItem.ItemId != Resource.Id.nav_addaccount)
                {
                    e.MenuItem.SetChecked(true);
                }
                
                ListItemClicked(e.MenuItem.ItemId);
                //Snackbar.Make(drawerLayout, "You selected: " + e.MenuItem.TitleFormatted, Snackbar.LengthLong).Show();
                drawerLayout.CloseDrawers();
            };


            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                ListItemClicked(Resource.Id.nav_home);
            }

            //navigationView.GetHeaderView(0).FindViewById(Resource.Id.imgSetting).Click += MainActivity_Click;


            //PLUSRangerThreads thread = new PLUSRangerThreads();
            //thread.OnGetSetting += (ServiceResult result) =>
            //{
            //    if (result.intStatus == 1)
            //    {
            //        FtpSettings setting = result.Data as FtpSettings;
            //        Cons.mFtpSettings = setting;
            //        //FtpClient.sendAPicture("test_nthoa.jpg");
            //    }
            //};
            //thread.GetSetting();

            mTxtEmail = FindViewById<TextView>(Resource.Id.txtEmail);
            mImgAvatar = FindViewById<ImageView>(Resource.Id.imgAvatar);
            mImgAvatar.Click += MImgAvatar_Click;

            FindViewById<View>(Resource.Id.rllHome).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllMyFavorite).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllLiveTraffic).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllLiveFeed).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllRSA).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllTollPlaza).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllFacilities).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllAnnouncement).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllTwister).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllEMagazine).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllMyAccount).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllMyVouchers).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllPromotions).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllRedemption).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllRedemptionCart).Click += MenuClick;   
            FindViewById<View>(Resource.Id.rllAbout1).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllMyReport).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllTwister).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllLiveFeed).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllSettings).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllPlusLink).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllAbout1).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllAbout).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllTollFare).Click += MenuClick;
            FindViewById<View>(Resource.Id.rllAddCard).Click += MenuClick;
            //Fragment_AboutProgram

        }

        private void MImgAvatar_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            var entity = getCurrentMyEntity();
            if (string.IsNullOrEmpty(entity.User.strToken))
            {
                Intent intent = new Intent(this, typeof(PLUSRangerLogin));
                StartActivity(intent);
            }
            else
            {
                var dialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                dialog.SetTitle(GetString(Resource.String.mess_confirm));
                dialog.SetMessage(GetString(Resource.String.mess_confirm_logout));
                dialog.SetPositiveButton(Android.Resource.String.Yes, this);
                dialog.SetNegativeButton(Android.Resource.String.No, handler: null);
                dialog.Show();
            }
        }

        private void MenuClick(object sender, EventArgs e)
        {
            var view = sender as View;
            switch (view.Id)
            {
                case Resource.Id.rllHome:
                    ListItemClicked(Resource.Id.nav_home);
                    break;
                case Resource.Id.rllMyFavorite:
                    ListItemClicked(Resource.Id.nav_favorite);
                    break;
                case Resource.Id.rllLiveTraffic:
                    ListItemClicked(Resource.Id.nav_live_traffic);
                    break;
                case Resource.Id.rllLiveFeed:
                    ListItemClicked(Resource.Id.rllLiveFeed);
                    break;
                case Resource.Id.rllRSA:
                    ListItemClicked(Resource.Id.layout_rsa);
                    break;
                case Resource.Id.rllTollPlaza:
                    ListItemClicked(Resource.Id.layout_toll_plaza);
                    break;
                case Resource.Id.rllFacilities:
                    ListItemClicked(Resource.Id.nav_pointsofinterest);
                    break;
                case Resource.Id.rllAnnouncement:
                    ListItemClicked(Resource.Id.nav_announcement);
                    break;
                case Resource.Id.rllTwister:
                    ListItemClicked(Resource.Id.rllTwister);
                    //ListItemClicked(Resource.Id.nav_);
                    break;
                case Resource.Id.rllEMagazine:
                    ListItemClicked(Resource.Id.rllEMagazine);
                    break;
                case Resource.Id.rllMyAccount:
                    ListItemClicked(Resource.Id.nav_myaccount);
                    break;
                case Resource.Id.rllMyVouchers:
                    ListItemClicked(Resource.Id.nav_myevoucher);
                    break;
                case Resource.Id.rllPromotions:
                    ListItemClicked(Resource.Id.nav_promotions);
                    break;
                case Resource.Id.rllRedemptionCart:
                    ListItemClicked(Resource.Id.nav_cart);
                    break;
                case Resource.Id.rllRedemption:
                    ListItemClicked(Resource.Id.nav_redemption);
                    break;
                case Resource.Id.rllMyReport:
                    ListItemClicked(Resource.Id.nav_myreport);
                    break;
                case Resource.Id.rllSettings:
                    ListItemClicked(Resource.Id.nav_setting);
                    break;
                //case Resource.Id.rllLiveFeed:
                //    ListItemClicked(Resource.Id.rllLiveFeed);
                //    break;
                case Resource.Id.rllAbout1:
                    ListItemClicked(Resource.Id.rllAbout1);
                    break;
                case Resource.Id.rllAbout:
                    ListItemClicked(Resource.Id.rllAbout);
                    break;
                case Resource.Id.rllPlusLink:
                    ListItemClicked(Resource.Id.rllPlusLink);
                    break;
                case Resource.Id.rllTollFare:
                    ListItemClicked(Resource.Id.rllTollFare);
                    break;
                case Resource.Id.rllAddCard:
                    ListItemClicked(Resource.Id.rllAddCard);
                    break;
            }
            drawerLayout.CloseDrawers();
            //throw new NotImplementedException();
        }

        private MyEntity oldEntity;
        private bool isLoaded = false;
        protected override void OnResume()
        {
            base.OnResume();

         

            if (getMyEntity().Count == 0)
            {
                //Intent intent = new Intent(this, typeof(LoginActivity));
                Intent intent = new Intent(this, typeof(InitActivity));
                StartActivity(intent);
            }
            else
            {
              

                MyEntity currentEntity = getCurrentMyEntity();

                if (string.IsNullOrEmpty(currentEntity.User.strToken))
                {
                    mTxtEmail.Text = "";

                }
                else
                {
                    mTxtEmail.Text = currentEntity.User.strFirstName + " " + currentEntity.User.strLastName;
                    if (!string.IsNullOrEmpty(currentEntity.User.strAvatar))
                    {
                        Picasso.With(this).Load(currentEntity.User.strAvatar).Error(Resource.Drawable.ic_profile).Into(mImgAvatar);
                    }
                    
                }

                //oldEntity = currentEntity;
                if (currentEntity == null)
                {
                    currentEntity = getMyEntity()[0];
                    saveCurrentMyEntity(currentEntity);
                }
                //navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.txtEntityName).Text = currentEntity.Entity.strName;
                //navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.txtEmail).Text = currentEntity.User.strUserName;
                Cons.IdEntity = currentEntity.idEntity;
                Cons.myEntity = currentEntity;

                isLoaded = true;
                if (oldEntity != null)
                {
                    if (oldEntity.idEntity == currentEntity.idEntity)
                    {
                        if (oldEntity.User.strUserName == currentEntity.User.strUserName)
                        {
                            isLoaded = false;
                        }
                        else
                        {

                        }
                    }
                }
                if (isLoaded)
                {
                    oldEntity = currentEntity;
                    ListItemClicked(Resource.Id.nav_home);
                }
            }
        }

        private void MainActivity_Click(object sender, System.EventArgs e)
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            ChooseEntitiesDialog newFragment = ChooseEntitiesDialog.NewInstance(null);
            newFragment.OnChangeEntity += (MyEntity result) =>
            {
                saveCurrentMyEntity(result);
            };
            newFragment.Show(ft, "dialog");
        }

        private string SaveToSd(string data)
        {
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
            var file = System.IO.Path.Combine(sdCardPath, "ExporessLog.txt");
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
            }
            using (System.IO.StreamWriter write = new System.IO.StreamWriter(file, true))
            {
                write.Write(data);
                return file;
            }
        }

        public string GetSearch()
        {
            return searchText.Text;
        }


        public override void OnBackPressed()
        {
          
            if (oldPosition != Resource.Id.nav_home)
            {
                ListItemClicked(Resource.Id.nav_home);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        

        ProgressDialog progress;
        int oldPosition = -1;
        public void ListItemClicked(int position)
        {
            //this way we don't load twice, but you might want to modify this a bit.
            //if (position == oldPosition)
                //return;

            oldPosition = position;

            Android.Support.V4.App.Fragment fragment = null;
            switch (position)
            {
                case Resource.Id.nav_pointsofinterest:
                    {
                        this.Toolbar.Title = GetString(Resource.String.title_point_of_interest);
                        fragment = Fragment_Facilities_V2.NewInstance();
                        (fragment as Fragment_Facilities_V2).OnInitSearch += (Filter filter) =>
                        {
                            mFilter = filter;
                        };
                        break;
                    }
                case Resource.Id.nav_tollfare:
                    {
                        this.Toolbar.Title = GetString(Resource.String.title_tollfare);
                        fragment = Fragment_TollFare.NewInstance();
                        break;
                    }
                case Resource.Id.nav_setting:
                    {
                        this.Toolbar.Title = GetString(Resource.String.title_setting);
                        fragment = Fragment_SettingNotification.NewInstance();
                        break;
                    }
                case Resource.Id.nav_about:
                    {
                        this.Toolbar.Title = GetString(Resource.String.title_about);
                        fragment = Fragment_About.NewInstance();
                        break;
                    }
                case Resource.Id.nav_favorite:
                    {
                        this.Toolbar.Title = GetString(Resource.String.title_favorites);
                        fragment = Fragment_Favorite_v2.NewInstance();
                        break;
                    }
                case Resource.Id.nav_addaccount:
                    {
                        Intent intent = new Intent(this, typeof(LoginActivity));
                        StartActivity(intent);
                        break;
                    }
                case Resource.Id.nav_live_traffic:
                    {
                        this.Toolbar.Title = GetString(Resource.String.title_livetraffic);
                        fragment = Fragment_LiveTraffice_v2.NewInstance();
                        (fragment as Fragment_LiveTraffice_v2).OnInitSearch += (Filter filter) =>
                        {
                            mFilter = filter;
                        };
                        break;
                    }
                case Resource.Id.nav_send_log:
                    {
                        MastersThread thread = new MastersThread();
                        thread.OnLoadLog += (List<TblLog> lst) =>
                        {
                            
                            
                            string data = "";
                            foreach (var item in lst)
                            {
                                data += item.strDate + "\t" + item.intType + "\t" + item.strContent + "\r\n";
                            }

                            string file = SaveToSd(data);
                            //progress.Dismiss();
                           
                            var email = new Intent(Intent.ActionSend);
                            email.SetType("message/rfc822");
                            email.PutExtra(Intent.ExtraEmail, new String[] { "nthoa.it@gmail.com" });
                            email.PutExtra(Intent.ExtraSubject, "[Log]Expresso " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            email.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + file));
                            RunOnUiThread(() =>
                            {
                                
                                try
                                {
                                    StartActivity(email);
                                }
                                catch (Exception ex)
                                {

                                }
                                
                            });
                        };
                        thread.loadAllLog();
                        Toast.MakeText(this,"Please wait", ToastLength.Short).Show();
                        break;
                    }
                case Resource.Id.nav__clear_log:
                    {
                        MastersThread thread = new MastersThread();
                        thread.clearAllLog();
                        break;
                    }
                case Resource.Id.nav_announcement:
                    {
                        this.Toolbar.Title = GetString(Resource.String.title_announcement);
                        fragment = Fragment_Announcement.NewInstance();
                        (fragment as Fragment_Announcement).OnInitSearch += (Filter fiter) =>
                        {
                            mFilter = fiter;
                        };
                        break;
                    }
                case Resource.Id.nav_myreport:
                    {
                        if (IsLogined())
                        {
                            this.Toolbar.Title = GetString(Resource.String.title_plusranger);
                            fragment = Fragment_PLUSRanger.NewInstance();
                            break;
                        }
                        else
                        {
                            Intent intent = new Intent(this, typeof(PLUSRangerLogin));
                            StartActivity(intent);
                            break;
                        }
                      
                    }
                case Resource.Id.nav_myaccount:
                    {
                        if (IsLogined())
                        {
                            this.Title = GetString(Resource.String.loy_title_plusmile_account);
                            fragment = Loyalty.Droid.Fragments.AccountInfomationFragment.NewInstance();
                            break;
                        }
                        else
                        {
                            Intent intent = new Intent(this, typeof(PLUSRangerLogin));
                            StartActivity(intent);
                            break;
                        }

                    }
                case Resource.Id.nav_promotions:
                    {
                        this.Title = GetString(Resource.String.loy_title_plusmile_promotions);
                        fragment = Loyalty.Droid.Fragments.MerchantOfferFragment.NewInstance();
                        break;
                    }
                case Resource.Id.nav_redemption:
                    {
                        this.Title = GetString(Resource.String.loy_title_plusmile_redemption);
                        fragment = Loyalty.Droid.Fragments.RedemptionListFragment.NewInstance();
                        (fragment as RedemptionListFragment).OnGotoCart += () =>
                        {
                            ListItemClicked(Resource.Id.nav_cart);
                        };
                        break;
                    }
                case Resource.Id.nav_cart:
                    {
                        //Intent intent = new Intent(this, typeof(MyAccount));
                        //StartActivity(intent);
                        if (IsLogined())
                        {
                            this.Title = GetString(Resource.String.title_cart);
                            fragment = Loyalty.Droid.Fragments.CartFragment.NewInstance();

                            if ((fragment as CartFragment).OnNext == null)
                            {
                                (fragment as CartFragment).OnNext += (int type) =>
                                {
                                    if (type != 6)
                                    {
                                        ListItemClicked(Redeem_ChooAdd);
                                    }
                                    else
                                    {
                                        //mAddressOrder = address;
                                        ListItemClicked(Redeem_Success);
                                    }
                                };
                            }


                            break;
                        }
                        else
                        {
                            Intent intent = new Intent(this, typeof(PLUSRangerLogin));
                            StartActivity(intent);
                            break;
                        }


                       
                    }
                case Redeem_ChooAdd:
                    {
                        this.Title = GetString(Resource.String.title_cart);
                        // fragment = Loyalty.Droid.Fragments.ChooseAddressRedeemFragment.NewInstance();
                        fragment = ChooseAddressRedeemFragment.NewInstance();
                        if ((fragment as ChooseAddressRedeemFragment).OnPrevious == null)
                        {
                            (fragment as ChooseAddressRedeemFragment).OnPrevious += () =>
                            {
                                ListItemClicked(Resource.Id.nav_cart);
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
                    }
                case Redeem_Success:
                    this.Title = GetString(Resource.String.title_cart);
                    fragment = RedempOrderSuccess.NewInstance(mAddressOrder);
                    break;
                case Resource.Id.nav_home:
                    {
                        this.Title = GetString(Resource.String.home_menu_home);
                        fragment = Fragment_Home.NewInstance();
                        break;
                    }
                case Resource.Id.layout_toll_plaza:
                    {
                        this.Title = GetString(Resource.String.text_toll_plaza);
                        fragment = Fragment_TollPlaza.NewInstance();
                        (fragment as Fragment_TollPlaza).OnInitSearch += (Dex.Com.Expresso.Adapters.Listview.TollPlazaAdapter data) =>
                        {
                            mFilter = data.Filter;
                        };
                        break;
                    }
                case Resource.Id.layout_rsa:
                    {
                        this.Title = GetString(Resource.String.text_rsa_layby);
                        fragment = Fragment_RSA_Main.NewInstance();
                        (fragment as Fragment_RSA_Main).OnInitSearch += (Filter fil) =>
                        {
                            mFilter = fil;
                        };
                        break;
                    }
                case Resource.Id.layout_live_traffic:
                    {
                        this.Title = GetString(Resource.String.text_live_traffic);
                        fragment = Fragment_LiveTraffice_v2.NewInstance();
                        (fragment as Fragment_LiveTraffice_v2).OnInitSearch += (Filter filter) =>
                        {
                            mFilter = filter;
                        };
                        break;
                    }
                case Resource.Id.layout_live_feed:
                    {
                        
                        break;
                    }
                case Resource.Id.layout_plusmiles:
                    {
                        if (IsLogined())
                        {
                            this.Title = GetString(Resource.String.loy_title_plusmile_account);
                            fragment = Loyalty.Droid.Fragments.AccountInfomationFragment.NewInstance();
                            break;
                        }
                        else
                        {
                            Intent intent = new Intent(this, typeof(PLUSRangerLogin));
                            StartActivity(intent);
                            break;
                        }

                    }
                case Resource.Id.layout_plusranger:
                    {
                        if (IsLogined())
                        {
                            this.Title = GetString(Resource.String.title_plusranger);
                            fragment = Fragment_PLUSRanger.NewInstance();
                            break;
                        }
                        else
                        {
                            Intent intent = new Intent(this, typeof(PLUSRangerLogin));
                            StartActivity(intent);
                            break;
                        }

                    }
                case Resource.Id.layout_favorite:
                    {
                        this.Title = GetString(Resource.String.title_favorites);
                        fragment = Fragment_Favorite_v2.NewInstance();
                        break;
                    }
                case Resource.Id.layout_announcement:
                    {
                        this.Title = GetString(Resource.String.title_announcement);
                        fragment = Fragment_Announcement.NewInstance();
                        (fragment as Fragment_Announcement).OnInitSearch += (Filter fiter) =>
                        {
                            mFilter = fiter;
                        };
                        break;
                    }
                case Resource.Id.rllTwister:
                    {
                        this.Title = GetString(Resource.String.text_plus_twitter);
                        fragment = Fragment_PlusTwitter.NewInstance();
                        break;
                    }
                case Resource.Id.rllLiveFeed:
                    {
                        this.Title = GetString(Resource.String.txt_livefeed);
                        fragment = Fragment_LiveFeed.NewInstance();
                        (fragment as Fragment_LiveFeed).OnInitSearch += (Filter filter) =>
                        {
                            searchText.Text = "";
                            mFilter = filter;
                        };
                        break;
                    }
                case Resource.Id.rllAbout1:
                    {
                        this.Title = GetString(Resource.String.home_menu_about);
                        fragment = Fragment_About.NewInstance();
                        break;
                    }
                case Resource.Id.rllPlusLink:
                    {
                        this.Title = GetString(Resource.String.text_plus_links);
                        fragment = Fragment_Links.NewInstance();
                        break;
                    }

                case Resource.Id.rllAbout:
                    {
                        this.Title = GetString(Resource.String.title_about);
                        fragment = Fragment_AboutProgram.NewInstance();
                        break;
                    }
                case Resource.Id.nav_myevoucher:
                    {
                        //FragmentMyEvoucher
                        if (IsLogined())
                        {
                            this.Title = GetString(Resource.String.title_evoucher);
                            fragment = FragmentEVoucher.NewInstance();
                            break;
                        }
                        else
                        {
                            Intent intent = new Intent(this, typeof(PLUSRangerLogin));
                            StartActivity(intent);
                            break;
                        }

                    }
                case Resource.Id.rllTollFare:
                    {
                        this.Title = GetString(Resource.String.toll_fare);
                        fragment = Fragment_TollFare.NewInstance();
                        break;
                    }
                case Resource.Id.rllEMagazine:
                    {
                        //FragmentEMagazine
                        this.Title = GetString(Resource.String.text_emagazine);
                        fragment = FragmentEMagazine.NewInstance();
                        break;
                    }
                case Resource.Id.rllAddCard:
                    {
                        if (IsLogined())
                        {
                            Intent intent = new Intent(this, typeof(Loyalty.Activities.AddCardActivity));
                            StartActivity(intent);
                            break;
                        }
                        else
                        {
                            Intent intent = new Intent(this, typeof(PLUSRangerLogin));
                            StartActivity(intent);
                            break;
                        }
                    }
                    //rllAbout
            }
            if (fragment == null)
            {
                return;
            }
            if (searchText != null)
            {
                searchText.Text = "";
            }
            
            mCurrentFragment = fragment;
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }


        public bool IsLogined()
        {
            var entity = getCurrentMyEntity();
            if (string.IsNullOrEmpty(entity.User.strToken))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.loy_search_menu, menu);
            IMenuItem itemSearch = menu.FindItem(Resource.Id.search_view);
            var searchView = MenuItemCompat.GetActionView(itemSearch);
            searchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();

            LayoutParams param = new Android.App.ActionBar.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            searchView.LayoutParameters = param;
            //searchView.SetMinimumWidth(int.MaxValue);
            searchText = searchView.FindViewById<AutoCompleteTextView>(Resource.Id.search_src_text);
            searchText.SetHintTextColor(Resources.GetColor(Resource.Color.White));
            searchText.SetTextColor(Resources.GetColor(Resource.Color.White));
            searchText.Threshold = 1;

            searchText.TextChanged += SearchText_TextChanged;

            this.mIMenu = menu;
            return base.OnCreateOptionsMenu(menu);
        }

        private void SearchText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            //throw new NotImplementedException();
            if (mFilter != null)
            {
                try
                {
                    mFilter.InvokeFilter(e.Text.ToString());
                }
                catch (Exception ex)
                {

                }
                
            }
        }

        protected override Dialog OnCreateDialog(int id)
        {
            if (id == 0)
                return new TimePickerDialog(this, TimePickerCallback, DateTime.Now.Hour, 0, false);

            return null;
        }

        private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            //hour = e.HourOfDay;
            //minute = e.Minute;
            //UpdateDisplay();
        }

        private Android.Support.V4.App.Fragment mCurrentFragment;
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

        public void OnClick(IDialogInterface dialog, int which)
        {
            //throw new NotImplementedException();
            setCacheString(MyAuth, "");
            ((Loyalty.Droid.Activities.BaseActivity)this).LogOut();
            
            Intent intent = new Intent(this, typeof(InitActivity));
            StartActivity(intent);

            mImgAvatar.SetImageResource(Resource.Drawable.img_default_avatar);
            //Cons.myEntity = 

        }
    }
}

