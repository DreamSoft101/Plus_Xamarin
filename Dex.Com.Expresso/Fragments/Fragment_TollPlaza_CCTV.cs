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
using Square.Picasso;
using Android.Graphics;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_TollPlaza_CCTV : BaseFragment
    {
        private ImageView mImgCamera1, mImgCamera2, mImgCameraMain;
        private List<TollPlazaCCTV> mLstCCTV;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment_TollPlaza_CCTV NewInstance(List<TollPlazaCCTV> lstCCTV)
        {
            var frag1 = new Fragment_TollPlaza_CCTV { Arguments = new Bundle(), mLstCCTV = lstCCTV };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_tollplaza_cctv, null);
            mImgCameraMain = view.FindViewById<ImageView>(Resource.Id.imgCameraMain);
            mImgCamera1 = view.FindViewById<ImageView>(Resource.Id.imgCamera1);
            mImgCamera2 = view.FindViewById<ImageView>(Resource.Id.imgCamera2);

            if (mLstCCTV.Count > 0)
            {
                Picasso.With(getActivity()).Load(mLstCCTV[0].strCCTVImage).Error(Resource.Drawable.img_error).Into(mImgCameraMain);
                Picasso.With(getActivity()).Load(mLstCCTV[0].strCCTVImage).Error(Resource.Drawable.img_error).Into(mImgCamera1);
                Picasso.With(getActivity()).Load(mLstCCTV[1].strCCTVImage).Error(Resource.Drawable.img_error).Into(mImgCamera2);
            }
            else
            {

            }
            mImgCamera1.Click += MImgCamera1_Click;
            mImgCamera2.Click += MImgCamera2_Click;


            return view;
        }

        private void MImgCamera2_Click(object sender, EventArgs e)
        {
            if (mLstCCTV.Count > 0)
            {
                Picasso.With(getActivity()).Load(mLstCCTV[1].strCCTVImage).Error(Resource.Drawable.img_error).Into(mImgCameraMain);
            }
        }

        private void MImgCamera1_Click(object sender, EventArgs e)
        {
            if (mLstCCTV.Count > 0)
            {
                Picasso.With(getActivity()).Load(mLstCCTV[0].strCCTVImage).Error(Resource.Drawable.img_error).Into(mImgCameraMain);
            }
        }

        public Bitmap getBitmap()
        {
            mImgCameraMain.BuildDrawingCache(true);
            // throw new NotImplementedException();
            Bitmap bitmap1 = mImgCameraMain.GetDrawingCache(true);
            return bitmap1;
        }
    }
}