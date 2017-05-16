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
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.Listview;
using Android.Support.V7.Widget;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Activities;
using Newtonsoft.Json;
using Android.Support.V4.Widget;
using Dex.Com.Expresso.Widgets;
using RecyclerViewAnimators.Animators;
using Android.Views.Animations;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Announcement : BaseFragment 
    {
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private View mLnlLoadingData;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private SwipeRefreshLayout swipeRefreshLayout;
        private AnnouncementAdapter mAdapter;
        private EndlessScrollRecyclListener scrollDown;

        public delegate void onInitSearch(Filter fiter);
        public onInitSearch OnInitSearch;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Announcement NewInstance()
        {
            var frag1 = new Fragment_Announcement { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_announcement, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);

            mAdapter = new AnnouncementAdapter(getActivity(), new List<Announcement>());
            if (OnInitSearch != null)
            {
                OnInitSearch(mAdapter.Filter);
            }
            mAdapter.ItemClick += Adapter_ItemClick;

            var animator = new SlideInUpAnimator(new OvershootInterpolator(1f));
            mLstItems.SetItemAnimator(animator);


            mLstItems.SetAdapter(mAdapter);

            mLnlLoadingData = view.FindViewById<View>(Resource.Id.lnlLoading);
            swipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            swipeRefreshLayout.Refresh += SwipeRefreshLayout_Refresh;


            scrollDown = new EndlessScrollRecyclListener();
            scrollDown.onLoadMore += (int page, int totalitem) =>
            {
                if (string.IsNullOrEmpty(((MainActivity)this.Activity).GetSearch()))
                {
                    LoadData(page);
                }
                
            };
            mLstItems.AddOnScrollListener(scrollDown);
          
            LoadData(1);
            return view;
        }

     

        private void LoadData(int page)
        {
            swipeRefreshLayout.Refreshing = true;
            mLnlLoadingData.Visibility = ViewStates.Visible;
            MastersThread thread = new MastersThread();
            thread.OnLoadListAnnoucement += (ServiceResult result) =>
            {
                swipeRefreshLayout.Refreshing = false;
                mLnlLoadingData.Visibility = ViewStates.Gone;
                if (result.intStatus == 1)
                {
                    List<Announcement> lstItems = result.Data as List<Announcement>;
                    foreach (var item in lstItems)
                    {
                        mAdapter.addItem(item);
                    }
                }
                else
                {
                    Toast.MakeText(getActivity(), result.strMess, ToastLength.Short).Show();
                }
            };
            thread.loadAnnoucement(page);

        }

        private void SwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            mAdapter.Clear();
            scrollDown.Clear();
            LoadData(1);
        }

        private void Adapter_ItemClick(object sender, Announcement e)
        {
            Intent intent = new Intent(this.getActivity(), typeof(AnnouncementDetailActivity));
            intent.PutExtra(AnnouncementDetailActivity.ANNOUNCEMENT, JsonConvert.SerializeObject(e));
            StartActivity(intent);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            
        }
    }
}