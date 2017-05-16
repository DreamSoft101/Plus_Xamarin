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

namespace Dex.Com.Expresso.Activities
{
    [Activity(Label = "PLUSRangerRegisterAcitivity")]
    public class PLUSRangerRegisterAcitivity : BaseActivity
    {
   
        private EditText mTxtUserName;
        private EditText mTxtPassword;
        private EditText mTxtConfirmPassword;
        private EditText mTxtMobileNo;
        private EditText mTxtFirstName;
        private EditText mTxtLastName;
        private Button mbtnRegister;
        private Button mBtnCancel;
  

        private static string LISTENTITY = "LISTENTITY";

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.exp_activity_plusranger_register;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = GetString(Resource.String.title_register);
            mbtnRegister = FindViewById<Button>(Resource.Id.btnRegister);
            mBtnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            mTxtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            mTxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mTxtConfirmPassword = FindViewById<EditText>(Resource.Id.txtConfirmPassword);
            mTxtMobileNo = FindViewById<EditText>(Resource.Id.txtMobileNo);
            mTxtFirstName = FindViewById<EditText>(Resource.Id.txtFirstName);
            mTxtLastName = FindViewById<EditText>(Resource.Id.txtLastName);

           
            mbtnRegister.Click += MbtnRegister_Click;
            // Create your application here
        }

        private void MbtnRegister_Click(object sender, EventArgs e)
        {
           

            string strUserName = mTxtUserName.Text;
            string strPassword = mTxtPassword.Text;
            string strPasswordConfirm = mTxtConfirmPassword.Text;
            string strMobileNo = mTxtMobileNo.Text;
            string strFirstName = mTxtFirstName.Text;
            string strLastName = mTxtLastName.Text;

            if (strPassword != strPasswordConfirm)
            {
                Toast.MakeText(this, Resource.String.loy_reg_mess_input_password, ToastLength.Short).Show();
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
                        myentity.Entity = new EXPRESSO.Models.Database.TblEntities() { idEntity = EXPRESSO.Utils.Cons.PLUSEntity.ToString(), strName = "PLUS" };
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
            userThread.doRegister(EXPRESSO.Utils.Cons.PLUSEntity.ToString(), strUserName, strPassword, strMobileNo, strFirstName, strLastName);
        }
    }
}