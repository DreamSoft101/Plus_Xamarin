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
using Loyalty.Threads;
using Loyalty.Models.Database;
using Newtonsoft.Json;
using Loyalty.Models.ServiceOutput;
using static Android.Views.View;
using Dex.Com.Expresso.Loyalty.Droid.Dialogs;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews;
using static Android.Widget.RatingBar;
using Loyalty.Utils;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Activities
{
    [Activity(Label = "MerchantOfferDetail", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MerchantOfferCommentsActivity : BaseActivity, IOnTouchListener, IOnRatingBarChangeListener
    {
        public static string ProductData = "ProductData";
        public MerchantProduct mProduct;
        private Merchant_Rating_Info myComment;
        private TextView mTxtComment;
        private RatingBar mRtbRating;
        private ListView mLstData;
        private LinearLayout mLnlLoading;
        private MerchantOfferCommentsAdapters adapter;
        private LinearLayout lnlComment;
        private bool IsFirstLoad = true;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.loy_activity_merchantoffer_comment;
            }
        }


      

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = GetString(Resource.String.loy_title_review_rating);
            string jsonData = Intent.GetStringExtra(ProductData);
            mProduct = JsonConvert.DeserializeObject<MerchantProduct>(jsonData);

            mLnlLoading = FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            lnlComment = FindViewById<LinearLayout>(Resource.Id.lnlComments);
            //mLnlLoading.Visibility = ViewStates.Gone;
            if (Cons.mMemberCredentials == null)
            {
                lnlComment.Visibility = ViewStates.Gone;
            }

            mTxtComment = FindViewById<TextView>(Resource.Id.txtComment);
            mTxtComment.Visibility = ViewStates.Gone;
            mRtbRating = FindViewById<RatingBar>(Resource.Id.rtbRating);
           // mRtbRating.SetOnTouchListener(this);
            mRtbRating.OnRatingBarChangeListener = this;

            mLstData = FindViewById<ListView>(Resource.Id.lstData);

            MerchantThreads merchantthread = new MerchantThreads();
            merchantthread.OnResult += (ServiceResult result) =>
            {
                try
                {
                    mLnlLoading.Visibility = ViewStates.Gone;
                    MCommon_GetMerchantProductRatings data = result.Data as MCommon_GetMerchantProductRatings;
                    if (data.CurrentUserRating != null)
                    {
                        mRtbRating.Rating = data.CurrentUserRating.intRate;
                        mTxtComment.Text = data.CurrentUserRating.strReview;
                        mTxtComment.Visibility = ViewStates.Visible;
                        var item = data.RatingInfos.Where(p => p.UserName == Cons.mMemberCredentials.LoginParams.strLoginId).FirstOrDefault();
                        if (item != null)
                        {
                            data.RatingInfos.Remove(item);
                        }
                        myComment = data.CurrentUserRating;
                    }
                    else
                    {

                    }

                    adapter = new MerchantOfferCommentsAdapters(this, data.RatingInfos);
                    mLstData.Adapter = adapter;
                }
                catch(Exception ex)
                {

                }
            };
            merchantthread.GetCommentAndRating(mProduct.MerchantProductID);
            // Create your application here
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (e.Action != MotionEventActions.Up)
            {
                return true;
            }
            int value = (int)mRtbRating.Rating;
            
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            Fragment prev = FragmentManager.FindFragmentByTag("rating");
            if (prev != null)
            {
                // UpdateDialog newFragment = (UpdateDialog)prev;
                //newFragment.Show(ft, "update");
                //ft.Remove(prev);
            }
            else
            {
                ft.AddToBackStack(null);
                CommentDialog newFragment = null;
                if (myComment != null) 
                {
                    newFragment = CommentDialog.NewInstance(null, myComment.intRate, myComment.strReview, mProduct); 
                }
                else
                {
                    newFragment = CommentDialog.NewInstance(null, value, mProduct);
                }
                newFragment.OnSubmited += (string comment, int rating) =>
                {
                    IsFirstLoad = true;
                    mRtbRating.Rating = rating;
                    mTxtComment.Text = comment;
                    mTxtComment.Visibility = ViewStates.Visible;
                    if (myComment == null)
                    {
                        myComment = new Merchant_Rating_Info();
                    }
                    myComment.dtCreated = DateTime.Now;
                    myComment.intRate = (byte)rating;
                    myComment.strReview = comment;
                };
                newFragment.Show(ft, "rating");
                
            }
            return true;
        }

        private bool isChange = false;
        CommentDialog newFragment;
        public void OnRatingChanged(RatingBar ratingBar, float rating, bool fromUser)
        {
            if (newFragment != null)
            {
                if (newFragment.IsVisible)
                {
                    return;
                }
            }
            if (IsFirstLoad)
            {
                IsFirstLoad = false;
                return;
            }
            if (rating - (int)rating > 0)
            {
                ratingBar.Rating = (int)rating;
                isChange = true;
            }
            if (!isChange && !fromUser)
            {
                return;
            }
            int value = (int)rating;

            FragmentTransaction ft = FragmentManager.BeginTransaction();
            Fragment prev = FragmentManager.FindFragmentByTag("rating");
            if (prev != null)
            {
                // UpdateDialog newFragment = (UpdateDialog)prev;
                //newFragment.Show(ft, "update");
                //ft.Remove(prev);
            }
            else
            {
                ft.AddToBackStack(null);
                newFragment = null;
                if (myComment != null)
                {
                    newFragment = CommentDialog.NewInstance(null, value, myComment.strReview, mProduct);
                }
                else
                {
                    newFragment = CommentDialog.NewInstance(null, value, mProduct);
                }
                newFragment.OnSubmited += (string comment, int ratingx) =>
                {
                    isChange = true;
                    //IsFirstLoad = true;
                    mRtbRating.Rating = ratingx;
                    mTxtComment.Text = comment;
                    mTxtComment.Visibility = ViewStates.Visible;
                    if (myComment == null)
                    {
                        myComment = new Merchant_Rating_Info();
                    }
                    myComment.dtCreated = DateTime.Now;
                    myComment.intRate = (byte)rating;
                    myComment.strReview = comment;
                };
                newFragment.Show(ft, "rating");

            }
        }
    }
}