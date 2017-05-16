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
using EXPRESSO.Models.Database;

namespace Dex.Com.Expresso.Dialogs
{
    public class MediaDialog : DialogFragment
    {
        private TblMedia media;
        private Context mContext;
        private EditText mTxtComment;
        private Button mBtnSave, mBtnCancel, mBtnDelete;

        public delegate void onSave(string data);
        public onSave OnSave;

        public delegate void onDelete(string id);
        public onDelete OnDelete;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            mContext = this.Activity;
            return base.OnCreateDialog(savedInstanceState);
            
        }
        public override void OnStart()
        {
            base.OnStart();
            this.Dialog.Window.SetLayout(WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
        }
      

        public static MediaDialog NewInstance(Bundle bundle, TblMedia _media)
        {
            MediaDialog fragment = new MediaDialog() { media = _media };
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.SetTitle(Resource.String.title_choose_entity);
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.exp_dialog_media, container, false);
            mTxtComment = view.FindViewById<EditText>(Resource.Id.txtComment);

            mTxtComment.Text = media.mStrComment;

            mBtnSave = view.FindViewById<Button>(Resource.Id.btnSave);
            mBtnDelete = view.FindViewById<Button>(Resource.Id.btnDelete);
            mBtnCancel = view.FindViewById<Button>(Resource.Id.btnCancel);

            mBtnDelete.Click += MBtnDelete_Click;
            mBtnCancel.Click += MBtnCancel_Click;
            mBtnSave.Click += MBtnSave_Click;
            return view;
        }

        private void MBtnDelete_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (OnDelete != null)
            {
                OnDelete(media.idMedia);
                this.Dismiss();
            }
        }

        private void MBtnSave_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            if (OnSave != null)
            {
                string data = mTxtComment.Text;
                OnSave(data);
                this.Dismiss();
            }
        }

        private void MBtnCancel_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            this.Dismiss();
        }

        
    }
}