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
using EXPRESSO.Threads;
using EXPRESSO.Models;
using Dex.Com.Expresso.Adapters.Spinner;
using EXPRESSO.Models.Database;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "ForgetpasswordActivity")]
    public class ForgetpasswordActivity : BaseActivity
    {
        private Spinner spnEntities;
        private EditText mTxtEmailAddress;
        private Button mBtnForgetpassword;
        private EntitiesAdapter mAdapter;
        private ProgressBar mPrbLoading;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_forgetpassword;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mTxtEmailAddress = FindViewById<EditText>(Resource.Id.txtUserName);
            mBtnForgetpassword = FindViewById<Button>(Resource.Id.btnForgetpassword);
            spnEntities = FindViewById<Spinner>(Resource.Id.spnEntities);
            mPrbLoading = FindViewById<ProgressBar>(Resource.Id.prbLoading);
            this.Title = GetString(Resource.String.title_forgetpassword);

            this.mBtnForgetpassword.Click += MBtnForgetpassword_Click;

            UsersThreads thread = new UsersThreads();
            thread.OnGetListEntitiesResult += (ServiceResult result) =>
            {
                if (result.intStatus == 1)
                {
                    List<TblEntities> lstData = result.Data as List<TblEntities>;
                    mAdapter = new EntitiesAdapter(this, lstData);
                    spnEntities.Adapter = mAdapter;
                }
                else
                {
                    Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                }
            };
            thread.loadGetListEntities();

            // Create your application here
        }

        private void MBtnForgetpassword_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (string.IsNullOrEmpty(this.mTxtEmailAddress.Text))
            {
                Toast.MakeText(this, Resource.String.mess_forgetpassword_pleaseinputemail, ToastLength.Short).Show();
                return;
            }

            //if (spnEntities.SelectedItemPosition == 0)
            //{
            //    Toast.MakeText(this, Resource.String.mess_choose_entity, ToastLength.Short).Show();
            //    return;
            //}

            mBtnForgetpassword.Clickable = false;
            mPrbLoading.Visibility = ViewStates.Visible;
            UsersThreads thread = new UsersThreads();
            thread.OnForgetpassword += (EXPRESSO.Models.ServiceResult result) =>
            {
                mBtnForgetpassword.Clickable = true;
                mPrbLoading.Visibility = ViewStates.Gone;
                if (result.intStatus == 1)
                {
                    Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                }
            };
            thread.doForgetpassword(this.mTxtEmailAddress.Text, EXPRESSO.Utils.Cons.PLUSEntity.ToString());
        }
    }
}