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
using Dex.Com.Expresso.Activities;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Home : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public static Fragment_Home NewInstance()
        {
            var frag1 = new Fragment_Home { Arguments = new Bundle() };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_homepage, null);

            view.FindViewById<View>(Resource.Id.layout_toll_plaza).Click += TollPlaza_Home_Click;
            view.FindViewById<View>(Resource.Id.layout_rsa).Click += RSA_Home_Click;
            view.FindViewById<View>(Resource.Id.layout_live_traffic).Click += LiveTraffic_Click;
            view.FindViewById<View>(Resource.Id.layout_live_feed).Click += LiveFeed_Click;
            view.FindViewById<View>(Resource.Id.layout_plusmiles).Click += PLUSMileClick; ;
            view.FindViewById<View>(Resource.Id.layout_plusranger).Click += PLUS_Ranger_Click;
            view.FindViewById<View>(Resource.Id.layout_favorite).Click += favorite_click;
            view.FindViewById<View>(Resource.Id.layout_announcement).Click += Announcement_Click;
            view.FindViewById<View>(Resource.Id.layout_live_feed).Click += Fragment_Home_Click;
            return view;
        }

        private void Fragment_Home_Click(object sender, EventArgs e)
        {
            (this.getActivity() as MainActivity).ListItemClicked(Resource.Id.rllLiveFeed);
        }

        private void Announcement_Click(object sender, EventArgs e)
        {
            (this.getActivity() as MainActivity).ListItemClicked(Resource.Id.layout_announcement);
        }

        private void favorite_click(object sender, EventArgs e)
        {
            (this.getActivity() as MainActivity).ListItemClicked(Resource.Id.layout_favorite);
        }

        private void PLUS_Ranger_Click(object sender, EventArgs e)
        {
            (this.getActivity() as MainActivity).ListItemClicked(Resource.Id.layout_plusranger);
        }

        private void PLUSMileClick(object sender, EventArgs e)
        {
            (this.getActivity() as MainActivity).ListItemClicked(Resource.Id.layout_plusmiles);
        }

        private void LiveFeed_Click(object sender, EventArgs e)
        {
            (this.getActivity() as MainActivity).ListItemClicked(Resource.Id.layout_live_feed);
        }

        private void LiveTraffic_Click(object sender, EventArgs e)
        {
            (this.getActivity() as MainActivity).ListItemClicked(Resource.Id.layout_live_traffic);
        }

        private void RSA_Home_Click(object sender, EventArgs e)
        {
            (this.getActivity() as MainActivity).ListItemClicked(Resource.Id.layout_rsa);
        }

        private void TollPlaza_Home_Click(object sender, EventArgs e)
        {
            (this.getActivity() as MainActivity).ListItemClicked(Resource.Id.layout_toll_plaza);
        }
    }
}