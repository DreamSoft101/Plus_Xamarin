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
using Android.Support.V7.Widget;
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Adapters.RecyclerViews;
using EXPRESSO.Threads;
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.Spinner;
using Dex.Com.Expresso.Activities;
using Dex.Com.Expresso.Utils;
using EXPRESSO.Utils;
using Dex.Com.Expresso.Widgets;
using RecyclerViewAnimators.Animators;
using Android.Views.Animations;
using Dex.Com.Expresso.Dialogs;
using Android.Support.V4.Widget;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_PLUSRanger : BaseFragment
    {
        private int mIntResult = 99;
        private TblPost mCurentItem;
        private RecyclerView mLstItems;
        private RecyclerView mLstCategory;
        private LinearLayout mLnlCategory;
        RecyclerView.LayoutManager mLayoutManager;
        RecyclerView.LayoutManager mLayoutManagerDropdown;
        private SwipeRefreshLayout mSwipe;
        private TblPostAdapter adapter;
        private PLUSRangerCategoryAdapter mCateAdapter;
        private LinearLayout mLnlLoading;
        private LinearLayout mLnlLogin, mLnlData;
        private int lstItemClick = -1;
        private Button mBtnLogin;
        private EndlessScrollRecyclListener endload;
        private string idCategory;
        private TextView mTxtSelectCategory;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
          
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_PLUSRanger NewInstance()
        {
            var frag1 = new Fragment_PLUSRanger { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_plusranger, null);

            mSwipe = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            mSwipe.Refresh += MSwipe_Refresh;
            mLnlLogin = view.FindViewById<LinearLayout>(Resource.Id.lnlLogin);
            mLnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mTxtSelectCategory = view.FindViewById<TextView>(Resource.Id.txtSelectCategory);
            adapter = new TblPostAdapter(this.Activity, new List<TblPost>());
            this.mLstItems.SetAdapter(adapter);
            endload = new  EndlessScrollRecyclListener();
            endload.onLoadMore += (int page, int totalitem) =>
            {
                LoadData(page);
            };
            mLstItems.AddOnScrollListener(endload);

            var animator = new SlideInUpAnimator(new OvershootInterpolator(1f));
            mLstItems.SetItemAnimator(animator);

            
            mLayoutManager = new LinearLayoutManager(getActivity());
            mLstItems.SetLayoutManager(mLayoutManager);
            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mLnlLoading.Visibility = ViewStates.Visible;
            mBtnLogin = view.FindViewById<Button>(Resource.Id.btnLogin);

            mLayoutManagerDropdown = new LinearLayoutManager(this.getActivity(), LinearLayoutManager.Horizontal, false);
            mLnlCategory = view.FindViewById<LinearLayout>(Resource.Id.lnlCategory);
            mLstCategory = view.FindViewById<RecyclerView>(Resource.Id.lstCategory);
            mLstCategory.SetLayoutManager(mLayoutManagerDropdown);
            mLnlCategory.Click += MLnlCategory_Click;
            
            mBtnLogin.Click += MBtnLogin_Click;
            view.FindViewById<View>(Resource.Id.floatingAdd).Click += Fragment_PLUSRanger_Click;

            return view;
        }

        private void MSwipe_Refresh(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            endload.Clear();
            adapter.Clear();
            LoadData(1);
        }

        private void MLnlCategory_Click(object sender, EventArgs e)
        {
            FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
            Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog_category");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            ChoosePlusRangerCategory newFragment = ChoosePlusRangerCategory.NewInstance(null);
            newFragment.OnDismess += () =>
            {
                Expresso.Adapters.RecyclerViews.PlusRangerCategoryAdapter adapter = new PlusRangerCategoryAdapter(this.Activity);
                mLstCategory.SetAdapter(adapter);
                if (adapter.ItemCount > 0)
                {
                    mTxtSelectCategory.Visibility = ViewStates.Gone;
                }
                else
                {
                    mTxtSelectCategory.Visibility = ViewStates.Visible;
                }


                //mHighwayFilterAdapter = new Adapters.RecyclerViews.SettingHighwayNameAdapter(getActivity());
                //lstHighway.SetAdapter(mHighwayFilterAdapter);

                //if (mHighwayFilterAdapter.ItemCount > 0)
                //{
                //    mTxtSelectHighway.Visibility = ViewStates.Gone;
                //}
                //else
                //{
                //    mTxtSelectHighway.Visibility = ViewStates.Visible;
                //}

                MSwipe_Refresh(null, null);
            };
            newFragment.Show(ft, "dialog_category");
        }

        private void MBtnLogin_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this.getActivity(), typeof(PLUSRangerLogin));
            StartActivity(intent);
        }

        private void Fragment_PLUSRanger_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (!GPSUtils.IsEnable(this.Activity))
            {
                Toast.MakeText(this.Activity, Resource.String.text_gps, ToastLength.Short).Show();
                return;
            }
            if (!Net.IsEnable(this.Activity))
            {
                Toast.MakeText(this.Activity, Resource.String.text_network, ToastLength.Short).Show();
                return;
            }
            Intent intent = new Intent(this.Activity, typeof(PLUSRangerAddNewActivity));
            StartActivity(intent);
        }
        
       
        private void LoadData(int page)
        {
            if (adapter.ItemCount == 0)
            {
                mLnlLoading.Visibility = ViewStates.Visible;
                mSwipe.Refreshing = true;
            }

            mSwipe.Refreshing = true;

             PLUSRangerThreads thread = new PLUSRangerThreads();
            thread.OnGetListOpsComm += (ServiceResult result) =>
            {
                mSwipe.Refreshing = false;
                if (result.intStatus == 1)
                {
                    mLnlLoading.Visibility = ViewStates.Gone;
                    List<TblPost> posts = result.Data as List<TblPost>;
                    foreach (var item in posts)
                    {
                        adapter.AddItem(item);
                    }
                    //adapter.ItemClick += Adapter_ItemClick;
                }
                else
                {
                    Toast.MakeText(this.Activity, result.strMess, ToastLength.Short).Show();
                }
            };
            thread.GetListOpsComm(getActivity().getPlusRangerCategory().Select(p => p.idCategory).ToList() , page);
        }

        public override void OnResume()
        {
            base.OnResume();
            var myentity = this.getActivity(). getMyEntity().Where(p => p.User.strToken != null).FirstOrDefault();
            if (myentity == null)
            {
                mLnlLogin.Visibility = ViewStates.Visible;
                mLnlData.Visibility = ViewStates.Gone;

            }
            else
            {

                var xe = this.getActivity(). getMyEntity().Where(p => p.User.strToken != null).FirstOrDefault();
                Cons.myEntity = xe;

                //mLnlLogin.Visibility = ViewStates.Gone;
                //mLnlData.Visibility = ViewStates.Visible;

                Expresso.Adapters.RecyclerViews.PlusRangerCategoryAdapter adapter1 = new PlusRangerCategoryAdapter(this.Activity);
                mLstCategory.SetAdapter(adapter1);
                if (adapter1.ItemCount > 0)
                {
                    mTxtSelectCategory.Visibility = ViewStates.Gone;
                }
                else
                {
                    mTxtSelectCategory.Visibility = ViewStates.Visible;
                }

                LoadData(1);

            }
        }

        private void Adapter_ItemClick(object sender, TblPost e)
        {
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == mIntResult)
            {
              
            }
        }
    }
}