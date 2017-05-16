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
using Dex.Com.Expresso.Adapters.RecyclerViews;
using RecyclerViewAnimators.Animators;
using Android.Views.Animations;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Links : BaseFragment
    {
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private AdapterLinks adapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Links NewInstance()
        {
            var frag1 = new Fragment_Links { Arguments = new Bundle() };
            return frag1;
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_list, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);
            var animator = new SlideInUpAnimator(new OvershootInterpolator(1f));
            mLstItems.SetItemAnimator(animator);
            adapter = new AdapterLinks(this.Activity);
            mLstItems.SetAdapter(adapter);
            adapter.ItemClick += Adapter_ItemClick;
            return view;
        }

        private void Adapter_ItemClick(object sender, Link e)
        {
            //throw new NotImplementedException();
            if (e.Title.ToLower() == "waze")
            {
                try
                {
                    Intent LaunchIntent = this.getActivity().PackageManager.GetLaunchIntentForPackage("com.waze");
                    this.getActivity().StartActivity(LaunchIntent);
                }
                catch (Exception ex)
                {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=com.waze"));
                    StartActivity(intent);
                }
               
            }
            else
            {
                Intent intent = new Intent(Intent.ActionView);
                intent.SetData(Android.Net.Uri.Parse("http://" + e.URL));
                StartActivity(intent);
            }
           
        }
    }
}