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
using Dex.Com.Expresso.Activities;
using Dex.Com.Expresso.Animations;

namespace Dex.Com.Expresso.Dialogs
{
    public class UpdateDialog : DialogFragment
    {
        private Context mContext;
        public delegate void onDismiss();
        public onDismiss EventOnDismiss;

        private Button btnUpdate, btnCancel;
        private TextView txtMainTask, txtSubTask;
        private ProgressBar progressBar;

        private List<BaseItem> lstItem;
        private ProgressBarAnimation progressBarAnimation;
        private UpdateThread thread;
        private TextView txtPercent, txtIndex;

        private DateTime mDtStart;

        public static UpdateDialog NewInstance(Bundle bundle)
        {
            UpdateDialog fragment = new UpdateDialog() {  };
            fragment.Arguments = bundle;
            return fragment;
            
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            if (thread != null)
            {
                //thread.Cancel();
            }
            if (EventOnDismiss != null)
            {
                EventOnDismiss();
            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return base.OnCreateDialog(savedInstanceState);
        }

        public static UpdateDialog NewInstance(Bundle bundle, List<BaseItem> items, DateTime dtStart)
        {
            UpdateDialog fragment = new UpdateDialog() { lstItem = items, mDtStart = dtStart };
            fragment.Arguments = bundle;
            return fragment;

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
            this.Dialog.SetTitle(Resource.String.title_choose_entity);
            
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.dialog_update, container, false);
            btnUpdate = view.FindViewById<Button>(Resource.Id.btnUpdate);
            btnCancel = view.FindViewById<Button>(Resource.Id.btnCancel);
            txtMainTask = view.FindViewById<TextView>(Resource.Id.txtMainTask);
            txtSubTask = view.FindViewById<TextView>(Resource.Id.txtSubTask);
             progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
            progressBarAnimation = new ProgressBarAnimation(progressBar, 100);
            txtPercent = view.FindViewById<TextView>(Resource.Id.txtPercent);
            txtIndex = view.FindViewById<TextView>(Resource.Id.txtIndex);
            btnUpdate.Enabled = false;

            txtPercent.Text = "0 %";
            txtIndex.Text = "0/0";
            txtPercent.Visibility = ViewStates.Invisible;
            txtIndex.Visibility = ViewStates.Invisible;

            if ( ( (BaseActivity) this.Activity).getLastUpdate() == new DateTime(1990, 11, 2))
            {
                btnUpdate.Visibility = ViewStates.Gone;
                btnCancel.Visibility = ViewStates.Gone;
                this.Cancelable = false;
            }

            if (lstItem != null)
            {
                txtPercent.Visibility = ViewStates.Visible;
                txtIndex.Visibility = ViewStates.Visible;
                txtIndex.Text = "0/" + lstItem.Count;
                txtSubTask.Visibility = ViewStates.Visible;
                txtMainTask.Text = this.Activity.GetString(Resource.String.mess_update_downloadfinish);
                if (lstItem.Count == 0)
                {
                    txtSubTask.Text = this.Activity.GetString(Resource.String.mess_update_no_newdata);
                }
                else
                {
                    if (((BaseActivity)this.Activity).getLastUpdate() == new DateTime(1990, 11, 2))
                    {
                        BtnUpdate_Click(null, null);
                    }
                    btnUpdate.Enabled = true;

                    txtSubTask.Text = string.Format(this.Activity.GetString(Resource.String.mess_update_number_newdata), lstItem.Count);
                }
            }
            else
            {
                UpdateThread thread = new UpdateThread();
                thread.OnGetNewData += (ServiceResult result) =>
                {
                    if (result.intStatus == 1)
                    {
                        txtSubTask.Visibility = ViewStates.Visible;
                        lstItem = result.Data as List<BaseItem>;
                        txtMainTask.Text = this.Activity.GetString(Resource.String.mess_update_downloadfinish);
                        if (lstItem.Count == 0)
                        {
                            txtSubTask.Text = this.Activity.GetString(Resource.String.mess_update_no_newdata);
                        }
                        else
                        {
                            if (((BaseActivity)this.Activity).getLastUpdate() == new DateTime(1990, 11, 2))
                            {
                                BtnUpdate_Click(null, null);
                            }
                            btnUpdate.Enabled = true;

                            txtSubTask.Text = string.Format(this.Activity.GetString(Resource.String.mess_update_number_newdata), lstItem.Count);
                            txtPercent.Visibility = ViewStates.Visible;
                            txtIndex.Visibility = ViewStates.Visible;
                            txtIndex.Text = "0/" + lstItem.Count;
                        }
                    }
                    else
                    {
                        txtMainTask.Text = result.strMess;
                    }

                };
                txtSubTask.Visibility = ViewStates.Gone;
                txtMainTask.Text = this.Activity.GetString(Resource.String.mess_update_download);
                mDtStart = DateTime.UtcNow;
                thread.getNewData(((BaseActivity)this.Activity).getLastUpdate());
            }
            btnCancel.Click += BtnCancel_Click;
            btnUpdate.Click += BtnUpdate_Click;
            return view;
        }


        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (thread == null)
            {
                thread = new UpdateThread();
                btnUpdate.Enabled = false;
                thread.OnDataChange += (string table, UpdateThread.Action action, int index,  int count) =>
                {
                    txtMainTask.Text = table;
                    txtSubTask.Visibility = ViewStates.Visible;
                    switch (action)
                    {
                        case UpdateThread.Action.AddNew:
                            txtSubTask.Text = this.Activity.GetString(Resource.String.mess_update_add);
                            break;
                        case UpdateThread.Action.Update:
                            txtSubTask.Text = this.Activity.GetString(Resource.String.mess_update_update);
                            break;
                        case UpdateThread.Action.Delete:
                            txtSubTask.Text = this.Activity.GetString(Resource.String.mess_update_delete);
                            break;
                    }
                    int percent = index * 100 / lstItem.Count;
                    txtIndex.Text = index + "/" + lstItem.Count;
                    txtPercent.Text = percent + " %";
                    progressBarAnimation.Progress = percent;
                };
                thread.OnUpdateComplate += () =>
                {
                    bool isDismiss = false;
                    if (((BaseActivity)this.Activity).getLastUpdate() == new DateTime(1990, 11, 2))
                    {
                        isDismiss = true;

                    }
                     ((BaseActivity)this.Activity).setLastUpdate(mDtStart);
                    if (isDismiss)
                    {
                        this.Dismiss();
                    }
                    txtPercent.Text = "100 %";
                    txtIndex.Text = lstItem.Count + "/" + lstItem.Count;
                    progressBarAnimation.Progress = 100;
                    btnUpdate.Enabled = true;
                    txtMainTask.Text = this.Activity.GetString(Resource.String.mess_update_finish);
                    txtSubTask.Visibility = ViewStates.Gone;
                    btnUpdate.Enabled = false;
                };
                thread.UpdateData(lstItem);
            }
            
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }
    }
}