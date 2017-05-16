using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Square.Picasso;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Facility_Detail_CCTV_Item : BaseFragment
    {
        private string imgURL;
        public static Fragment_Facility_Detail_CCTV_Item NewInstance(string url)
        {
            var frag1 = new Fragment_Facility_Detail_CCTV_Item { imgURL = url };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.item_rsa_cctv, null);
            ImageView img = view.FindViewById<ImageView>(Resource.Id.imgCamera);

            Picasso.With(getActivity()).Load(imgURL).Error(Resource.Drawable.img_error).Into(img, null, new Action(() =>
            {
                view.Visibility = ViewStates.Gone;
            }));
            return view;
        }
    }
}