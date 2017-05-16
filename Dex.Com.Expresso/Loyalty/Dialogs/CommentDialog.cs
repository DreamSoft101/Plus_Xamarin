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
using Loyalty.Models.ServiceOutput;
using Loyalty.Models.Database;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Dialogs
{
    public class CommentDialog : DialogFragment
    {
        private Context mContext;
        public delegate void onDismiss();
        public onDismiss EventOnDismiss;
        private string mStrComment;
        private int mIntRating;
        private MerchantProduct mProduct;
        public delegate void onSubmited(string comment, int rating);
        public onSubmited OnSubmited;

        private Button mBtnSubmit;
        private EditText mTxtComment;
        private RatingBar mRtbRating;
        public static CommentDialog NewInstance(Bundle bundle)
        {
            CommentDialog fragment = new CommentDialog() { };
            fragment.Arguments = bundle;
            return fragment;

        }

        public static CommentDialog NewInstance(Bundle bundle, int intRating, MerchantProduct pro)
        {
            CommentDialog fragment = new CommentDialog() { mIntRating = intRating, mProduct = pro };
            fragment.Arguments = bundle;
            return fragment;
        }

        public static CommentDialog NewInstance(Bundle bundle, int intRating, string strComment, MerchantProduct pro)
        {
            CommentDialog fragment = new CommentDialog() { mIntRating = intRating , mStrComment  = strComment, mProduct = pro };
            fragment.Arguments = bundle;
            return fragment;
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            if (EventOnDismiss != null)
            {
                EventOnDismiss();
            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return base.OnCreateDialog(savedInstanceState);
        }

        public override void OnResume()
        {
            base.OnResume();

            try
            {
                this.Dialog.Window.SetLayout(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
            }
            catch (Exception ex)
            {

            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            View view = inflater.Inflate(Resource.Layout.loy_dialog_comment, container, false);
            view.FindViewById<Button>(Resource.Id.btnSubmit).Click += btnSubmit_Click;
            mRtbRating = view.FindViewById<RatingBar>(Resource.Id.rtbRating);
            mTxtComment = view.FindViewById<EditText>(Resource.Id.txtComment);
            mBtnSubmit = view.FindViewById<Button>(Resource.Id.btnSubmit);
            mTxtComment.Text = mStrComment == null ? "": mStrComment;
            mRtbRating.Rating = mIntRating;

            return view;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            
            MerchantThreads thread = new MerchantThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                this.Cancelable = true;
                mBtnSubmit.Enabled = true;
                if (result.StatusCode == 1)
                {
                    if (OnSubmited != null)
                    {
                        OnSubmited(mTxtComment.Text, (int)mRtbRating.Rating);
                    }
                    this.Dismiss();
                }
                else
                {
                    Toast.MakeText(this.Activity, result.Mess, ToastLength.Short).Show();
                }

            };
            this.Cancelable = false;
            mBtnSubmit.Enabled = false;
            thread.Raiting(mProduct.MerchantProductID, (int)mRtbRating.Rating, mTxtComment.Text);

        }

    }
}
