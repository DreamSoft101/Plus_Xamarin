using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Dex.Com.Expresso;
using Loyalty.Models.ServiceOutput;
using Loyalty.Threads;
using System;

namespace Dex.Com.Expresso.Loyalty.Droid.Dialogs
{
    public class RegisterSocialDialog : DialogFragment
    {
        private Context mContext;
        public delegate void onDismiss();
        public onDismiss EventOnDismiss;
        private string mStrToken;


        public delegate void onRegisted(string token, MBB_Registration data);
        public onRegisted OnRegisted;
     

        private Button mBtnCancel;
        private Button mbtnRegister;

        private EditText mTxtUserName;
        private EditText mTxtPassword;
        private EditText mTxtConfirmPassword;
        private int Type = 0;
        public static RegisterSocialDialog NewInstance(Bundle bundle)
        {
            RegisterSocialDialog fragment = new RegisterSocialDialog() { };
            fragment.Arguments = bundle;
            return fragment;

        }

        public static RegisterSocialDialog NewInstance(Bundle bundle, string strToken, int type)
        {
            RegisterSocialDialog fragment = new RegisterSocialDialog() { mStrToken = strToken , Type = type};
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
            //this.Dialog.SetTitle(Resource.String.loy_title_choose_entity);

            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.loy_dialog_register_social, container, false);
            mbtnRegister = view.FindViewById<Button>(Resource.Id.btnRegister);
            mBtnCancel = view.FindViewById<Button>(Resource.Id.btnCancel);
            mTxtUserName = view.FindViewById<EditText>(Resource.Id.txtUserName);
            mTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mTxtConfirmPassword = view.FindViewById<EditText>(Resource.Id.txtConfirmPassword);

            mBtnCancel.Click += MBtnCancel_Click;
            mbtnRegister.Click += MbtnRegister_Click;
            return view;
        }

        private void MbtnRegister_Click(object sender, EventArgs e)
        {
            string strUserName = mTxtUserName.Text;
            string strPassword = mTxtPassword.Text;
            string strPasswordConfirm = mTxtConfirmPassword.Text;
        
            if (string.IsNullOrEmpty(strPassword))
            {
                Toast.MakeText(mContext, Resource.String.loy_reg_mess_input_password, ToastLength.Short).Show();
                return;
            }
            if (strPassword != strPasswordConfirm)
            {
                Toast.MakeText(mContext, Resource.String.loy_reg_mess_confirmpass, ToastLength.Short).Show();
                return;
            }
            if (string.IsNullOrEmpty(strUserName))
            {
                Toast.MakeText(mContext, Resource.String.loy_reg_mess_input_email, ToastLength.Short).Show();
                return;
            }

            UserThreads thread = new UserThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                if (result.StatusCode == 1)
                {
                    MBB_Registration data = result.Data as MBB_Registration;
                    if (OnRegisted != null)
                    {
                        OnRegisted(mStrToken, data);
                    }
                    this.Dismiss();
                }
                else
                {
                    Toast.MakeText(mContext, result.Mess, ToastLength.Short).Show();
                }
            };
            thread.Register(Type, strUserName, mStrToken, strPassword, "", "", "", "");
        }

        private void MBtnCancel_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }
    }
}
