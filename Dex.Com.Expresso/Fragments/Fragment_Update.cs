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
using Dex.Com.Expresso.Adapters.Listview;
using EXPRESSO.Models;
using EXPRESSO.Threads;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Update : BaseFragment
    {
        private int mIntResult = 99;
        private RecyclerView mLstItems;
        RecyclerView.LayoutManager mLayoutManager;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Update NewInstance()
        {
            var frag1 = new Fragment_Update { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_update, null);
            //mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);

            // mLstItems.SetLayoutManager(mLayoutManager);

            // UpdateThread thread = new UpdateThread();

            // thread.getNewData(new DateTime());
            UpdateThread thread = new UpdateThread();
            thread.getNewData(new DateTime(1990, 11, 2));
            return view;
        }

        private void Adapter_ItemClick(object sender, BaseItem e)
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