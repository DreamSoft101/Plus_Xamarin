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
//using Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Loyalty.Models.Database;
using Loyalty.Models;
using Dex.Com.Expresso.Loyalty.Droid.Dialogs;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.RecyclerViews;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using XamarinItemTouchHelper;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Fragments
{
    public class PreferenceFragment : BaseFragment
    {
        private ItemTouchHelper mItemTouchHelper;
        private ItemPreferencesAdapters mAdapter;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static PreferenceFragment NewInstance()
        {
            var frag1 = new PreferenceFragment { Arguments = new Bundle() };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.loy_fragment_preference, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            //mLstItems.
            // ItemTouchHelper.Callback callback = new ItemTouchHelper.SimpleCallback()

          

            LoadData();

            view.FindViewById<View>(Resource.Id.imgAdd).Click += PreferenceFragment_Click;
            return view;
        }

        private void PreferenceFragment_Click(object sender, EventArgs e)
        {
            FragmentTransaction ft =  this.Activity.FragmentManager.BeginTransaction();
            Fragment prev = this.Activity.FragmentManager.FindFragmentByTag("add");
            if (prev != null)
            {
                // UpdateDialog newFragment = (UpdateDialog)prev;
                //newFragment.Show(ft, "update");
                //ft.Remove(prev);
            }
            else
            {
                ft.AddToBackStack(null);
                AddPreferencesDialog newFragment = AddPreferencesDialog.NewInstance(null);
                newFragment.EventOnDismiss += () =>
                {
                    //ListItemClicked(0, true);
                    //LoadData();
                };
                newFragment.Show(ft, "add");
                newFragment.OnAdd += (ItemPreferences item, AddPreferencesDialog dialog) =>
                {
                    if (mAdapter.IsExist(item))
                    {
                        Toast.MakeText(this.Activity, Resource.String.loy_mess_preferences_exists, ToastLength.Short).Show();
                        return;
                    }
                    else
                    {
                        mAdapter.AddItem(item);
                        dialog.Dismiss();
                    }
                    
                };
            }
        }

        private void LoadData()
        {
            var data = this.getActivity().getSettingMemberType();
            mAdapter = new ItemPreferencesAdapters(this.Activity, data);
            mLstItems.SetAdapter(mAdapter);
            mLstItems.HasFixedSize = true;

            ItemTouchHelper.Callback callback = new SimpleItemTouchHelperCallback(mAdapter);
            mItemTouchHelper = new ItemTouchHelper(callback);
            mItemTouchHelper.AttachToRecyclerView(mLstItems);
            //mAdapter.NotifyDataSetChanged();
        }


        public override void OnResume()
        {
            base.OnResume();
        }
    }
}