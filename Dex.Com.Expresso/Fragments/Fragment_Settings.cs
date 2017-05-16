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
using Dex.Com.Expresso.Activities;
using EXPRESSO.Threads;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Settings : BaseFragment
    {
        private ListView mLstEntity;
        private HighwaySettingAdapter adapter;
        private Button mBtnSave;
        public static string IDENTITY = "IDENTITY";
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment_Settings NewInstance()
        {
            var frag1 = new Fragment_Settings { Arguments = new Bundle() };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_settings, null);

            mLstEntity = view.FindViewById<ListView>(Resource.Id.lstHighway);
            this.mBtnSave = view.FindViewById<Button>(Resource.Id.btnSave);
            MastersThread thread = new MastersThread();
            
            thread.OnLoadListHighway += (result) =>
            {
                adapter = new HighwaySettingAdapter(getActivity(), result);
                this.mLstEntity.Adapter = adapter;
            };
            thread.loadListHighway();


            this.mBtnSave.Click += MBtnSave_Click;

            mLstEntity.ItemClick += MLstEntity_ItemClick;
            return view;
        }

        private void MBtnSave_Click(object sender, EventArgs e)
        {
            var save = adapter.getSaveData();
            string strSave = EXPRESSO.Utils.StringUtils.Object2String(save);
            getActivity().saveMySetting(strSave);
            Toast.MakeText(getActivity(), GetString(Resource.String.setting_mess_saved), ToastLength.Short).Show();

        }

        private void MLstEntity_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            adapter.Toggle(e.Position);
        }
    }
}