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
using Dex.Com.Expresso.Adapters.Spinner;
using EXPRESSO.Models.Database;
using EXPRESSO.Utils;
using EXPRESSO.Threads;
using EXPRESSO.Models;

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "RegisterActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RegisterActivity : BaseActivity
    {
        private Spinner mSpnEntity;
        private EditText mTxtUserName;
        private EditText mTxtPassword;
        private EditText mTxtConfirmPassword;
        private EditText mTxtMobileNo;
        private EditText mTxtFirstName;
        private EditText mTxtLastName;
        private Button mbtnRegister;
        private Button mBtnCancel;
        private EntitiesAdapter adapter;

        private static string LISTENTITY = "LISTENTITY";

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.activity_register;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mSpnEntity = FindViewById<Spinner>(Resource.Id.spnEntities);
            mbtnRegister = FindViewById<Button>(Resource.Id.btnRegister);
            mBtnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            mTxtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            mTxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mTxtConfirmPassword = FindViewById<EditText>(Resource.Id.txtConfirmPassword);
            mTxtMobileNo = FindViewById<EditText>(Resource.Id.txtMobileNo);
            mTxtFirstName = FindViewById<EditText>(Resource.Id.txtFirstName);
            mTxtLastName = FindViewById<EditText>(Resource.Id.txtLastName);

            string strListEntity = this.Intent.GetStringExtra(LISTENTITY);
            if (!string.IsNullOrEmpty(strListEntity))
            {
                List<TblEntities> lstEntity = StringUtils.String2Object<List<TblEntities>>(strListEntity);
                adapter = new EntitiesAdapter(this, lstEntity);
                mSpnEntity.Adapter = adapter;
            }
            else
            {
                adapter = new EntitiesAdapter(this, new List<TblEntities>());
                mSpnEntity.Adapter = adapter;
                UsersThreads userThread = new UsersThreads();
                userThread.OnGetListEntitiesResult += (ServiceResult result) =>
                {
                    if (result.intStatus == 1)
                    {
                        adapter = new EntitiesAdapter(this, result.Data as List<TblEntities>);
                        mSpnEntity.Adapter = adapter;
                    }
                    else
                    {
                        Toast.MakeText(this, result.strMess, ToastLength.Short).Show();
                    }
                    
                };
                userThread.loadGetListEntities();
            }

            mbtnRegister.Click += MbtnRegister_Click;
            // Create your application here
        }

        private void MbtnRegister_Click(object sender, EventArgs e)
        {
            if (mSpnEntity.SelectedItemPosition == 0)
            {
                Toast.MakeText(this, Resource.String.mess_choose_entity, ToastLength.Short).Show();
                return;
            }
            TblEntities entity = adapter.GetEntity(mSpnEntity.SelectedItemPosition - 1);

            string strUserName = mTxtUserName.Text;
            string strPassword = mTxtPassword.Text;
            string strPasswordConfirm = mTxtConfirmPassword.Text;
            string strMobileNo = mTxtMobileNo.Text;
            string strFirstName = mTxtFirstName.Text;
            string strLastName = mTxtLastName.Text;

            if (strPassword != strPasswordConfirm)
            {
                Toast.MakeText(this, Resource.String.mess_choose_entity, ToastLength.Short).Show();
                return;
            }
            if (string.IsNullOrEmpty(strUserName))
            {
                Toast.MakeText(this, Resource.String.mess_input_email, ToastLength.Short).Show();
                return;
            }
            if (string.IsNullOrEmpty(strPassword))
            {
                Toast.MakeText(this, Resource.String.mess_input_password, ToastLength.Short).Show();
                return;
            }
            UsersThreads userThread = new UsersThreads();
            userThread.OnRegisterSusscess += (ServiceResult entityinfo, ServiceResult userinfo) =>
            {
                if (entityinfo.intStatus == 1)
                {
                    if (userinfo.intStatus == 1)
                    {
                        List<MyEntity> lstMyEntity = getMyEntity();
                        MyEntity myentity = entityinfo.Data as MyEntity;
                        myentity.Entity = entity;
                        myentity.User = userinfo.Data as UserInfos;
                        lstMyEntity.Add(myentity);
                        saveMyEntity(lstMyEntity);

                        Intent intent = new Intent();
                        SetResult(Result.Ok, intent);
                        Finish();
                    }
                    else
                    {
                        Toast.MakeText(this, userinfo.strMess, ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, entityinfo.strMess, ToastLength.Short).Show();
                }
                
            };
            userThread.doRegister(entity.idEntity, strUserName, strPassword, strMobileNo, strFirstName, strLastName);
        }
    }
}