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
using Android.Support.V7.Widget;
using Dex.Com.Expresso.Adapters.Listview;
using Android.Support.V4.Widget;
using Dex.Com.Expresso.Adapters.RecyclerViews;
using EXPRESSO.Threads;
using EXPRESSO.Models.Database;
namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Notification_Setting_LiveFeed : BaseFragment
    {
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.LayoutManager mLayoutManager1;
        private Adapters.RecyclerViews.SettingHighwayNameAdapter mHighwayFilterAdapter;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Notification_Setting_LiveFeed NewInstance()
        {
            var frag1 = new Fragment_Notification_Setting_LiveFeed { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_setting_livetraffic, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);

            string idLast = "";
            List<BaseItem> lstData = new List<BaseItem>();


            MastersThread thread = new MastersThread();
            thread.OnLoadListHighway += (List<TblHighway> lst) =>
            {
                FavoriteThread threadF = new FavoriteThread();
                threadF.OnLoadCCTV += (List< BaseItem> lstresult) =>
                {
                    foreach (var item in lstresult)
                    {
                        var idHighway = item.getTag(BaseItem.TagName.IdHighway).ToString();
                        if (idLast != idHighway)
                        {
                            idLast = idHighway;
                            BaseItem itemB = new BaseItem();
                            //itemB.Item 
                            itemB.Item = lst.Where(p => p.idHighway == idLast).FirstOrDefault();
                            lstData.Add(itemB);

                        }
                        lstData.Add(item);
                    }
                    SettingNotificationAdapters adapter = new SettingNotificationAdapters(this.Activity, lstData);
                    mLstItems.SetAdapter(adapter);
                };
                threadF.loadCCTV();

            };
            thread.loadListHighway();

            LoadData();
            return view;
        }

        private void LoadData()
        {


        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {

        }
    }
}