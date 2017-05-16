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
using static EXPRESSO.Models.EnumType;
using EXPRESSO.Models;
using EXPRESSO.Threads;
using Dex.Com.Expresso.Adapters.Listview;
using RecyclerViewAnimators.Adapters;
using RecyclerViewAnimators.Animators;
using Android.Views.Animations;
using Android.Views.InputMethods;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_Feedback : BaseFragment
    {
        private FacilitiesType mOtherTypes;
        private RecyclerView mLstItems;
        private RecyclerView.LayoutManager mLayoutManager;
        private int mIntResult = 99;
        private BaseItem mCurentItem;
        private LinearLayout mLnlLoading, mLnlData, mLnlComment;
        private TextView mTxtLogin;
        private EditText mTxtContent;
        private ImageView mImgSend;
        private GetFeedbackAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mLayoutManager = new LinearLayoutManager(getActivity());
            // Create your fragment here
        }

        public static Fragment_Feedback NewInstance()
        {

            var frag1 = new Fragment_Feedback { };
            return frag1;
        }

        public static Fragment_Feedback NewInstance(BaseItem item)
        {

            var frag1 = new Fragment_Feedback { mCurentItem = item };
            return frag1;
        }


        private void LoadFeedback(BaseItem bitem)
        {

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_feedback, null);
            mLstItems = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            mLstItems.SetLayoutManager(mLayoutManager);

            var animator = new SlideInUpAnimator(new OvershootInterpolator(1f));
            mLstItems.SetItemAnimator(animator);



            mLnlLoading = view.FindViewById<LinearLayout>(Resource.Id.lnlLoading);
            mLnlData = view.FindViewById<LinearLayout>(Resource.Id.lnlData);
            mTxtLogin = view.FindViewById<TextView>(Resource.Id.txtLogin);
            mImgSend = view.FindViewById<ImageView>(Resource.Id.imgSend);
            mTxtContent = view.FindViewById<EditText>(Resource.Id.txtComment);
            mLnlComment = view.FindViewById<LinearLayout>(Resource.Id.lnlComment);
            mImgSend.Click += MImgSend_Click;
            LoadingData();
            return view;
        }

        private void MImgSend_Click(object sender, EventArgs e)
        {
            MyEntity entity = this.getActivity().getCurrentMyEntity();
            if (mCurentItem == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(entity.User.strToken))
            {
                return;
            }
            if (adapter == null)
            {
                Toast.MakeText(this.getActivity(), Resource.String.mess_feedback_waiting, ToastLength.Short).Show();
                return;
            }
           
            if (!string.IsNullOrEmpty(mTxtContent.Text))
            {
                hideKeyboard(mTxtContent);
                CommunicationThreads thread = new CommunicationThreads();
                thread.OnPostFeedback += (ServiceResult result) =>
                {
                    if (result.intStatus == 1)
                    {
                        if (result.Data != null)
                        {
                            GetFeedback item = result.Data as GetFeedback;
                            adapter.addItem(item);
                            mLstItems.ScrollToPosition(adapter.ItemCount - 1);
                        }
                    }
                    else
                    {
                        Toast.MakeText(this.getActivity(), result.strMess, ToastLength.Short).Show();
                    } 
                };
                thread.postFeedback(mCurentItem, mTxtContent.Text);
                mTxtContent.Text = "";
            }
            else
            {
                Toast.MakeText(this.getActivity(), Resource.String.mess_feedback_inputcomment, ToastLength.Short).Show();
            }

        }

        public void hideKeyboard(View view)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)this.getActivity().GetSystemService(Android.App.Activity.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(view.WindowToken, 0);
        }


        private void LoadingData()
        {
            MyEntity entity = this.getActivity().getCurrentMyEntity();
            if (string.IsNullOrEmpty(entity.User.strToken))
            {
                mTxtLogin.Visibility = ViewStates.Visible;
                mLnlComment.Visibility = ViewStates.Gone;
            }
            else
            {
                mTxtLogin.Visibility = ViewStates.Gone;
                mLnlComment.Visibility = ViewStates.Visible;
            }

            mLnlLoading.Visibility = ViewStates.Visible;
            mLnlData.Visibility = ViewStates.Gone;
            if (mCurentItem != null)
            {
                CommunicationThreads thread = new CommunicationThreads();
                thread.OnGetFeedback += (ServiceResult result) =>
                {
                    mLnlLoading.Visibility = ViewStates.Gone;
                    mLnlData.Visibility = ViewStates.Visible;

                    adapter = new GetFeedbackAdapter(this.getActivity(), result.Data as List<GetFeedback>);
                    var adapterAnimator = new ScaleInAnimationAdapter(adapter);
                    mLstItems.SetAdapter(adapter);
                    mLstItems.ScrollToPosition(adapter.ItemCount - 1);
                };
                thread.getFeedback(mCurentItem);
            }


        }
    }
}