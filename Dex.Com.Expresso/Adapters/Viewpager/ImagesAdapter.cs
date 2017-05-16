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
using Android.Support.V4.App;
using Java.Lang;
using Dex.Com.Expresso.Fragments;
using Android.Graphics;

namespace Dex.Com.Expresso.Adapters.Viewpager
{
    public class ImagesAdapter : FragmentStatePagerAdapter
    {
        private List<string> urls;
        private Context mContext;
        private FragmentImages fstFragment;
        private FragmentImages[] mDictionary;
        public delegate void onClick(int position);
        public onClick OnClick;
        public ImagesAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public void Remove(int position)
        {
            try
            {
                this.urls.RemoveAt(position);
                this.NotifyDataSetChanged();
            }
            catch (System.Exception ex)
            {

            }
            
        }
        public ImagesAdapter(Context context, Android.Support.V4.App.FragmentManager fm, List<string> urls) : base(fm)
        {
            mContext = context;
            this.urls = urls;
            mDictionary = new FragmentImages[urls.Count];
        }

        public override int Count
        {
            get
            {
                return urls.Count;
            }
        }

        public Bitmap GetBitmap(int position = 0)
        {
            return mDictionary[position].GetBitmap();
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            var fragment = mDictionary[position];
            if (fragment == null)
            {
                fragment = FragmentImages.NewInstance(urls[position], position);
                fragment.OnClick += (int postion) =>
                {
                    if (OnClick != null)
                    {
                        OnClick(position);
                    }
                };
                mDictionary[position] = fragment;
            }
            return fragment;
            
            
        }
    }
}