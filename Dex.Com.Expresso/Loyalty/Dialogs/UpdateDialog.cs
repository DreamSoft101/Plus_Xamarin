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
using Dex.Com.Expresso.Loyalty.Droid.Animations;
using Dex.Com.Expresso.Loyalty.Droid.Activities;
using Loyalty.Models;
using Loyalty.Threads;
using Loyalty.Models.ServiceOutput;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Dialogs
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
        private MasterThreads thread;
        private TextView txtPercent, txtIndex;
        //private DateTime lastTime;

        private DateTime mDtStart;

        public static UpdateDialog NewInstance(Bundle bundle)
        {
            UpdateDialog fragment = new UpdateDialog() { };
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
            //this.Dialog.SetTitle(Resource.String.loy_title_choose_entity);

            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.loy_dialog_update, container, false);
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

            if (((BaseActivity)this.Activity).getLastUpdate() == BaseActivity.FirstUpdate)
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
                txtMainTask.Text = this.Activity.GetString(Resource.String.loy_mess_update_downloadfinish);
                if (lstItem.Count == 0)
                {
                    txtSubTask.Text = this.Activity.GetString(Resource.String.loy_mess_update_no_newdata);
                }
                else
                {
                    if (((BaseActivity)this.Activity).getLastUpdate() == BaseActivity.FirstUpdate)
                    {
                        BtnUpdate_Click(null, null);
                    }
                    btnUpdate.Enabled = true;

                    txtSubTask.Text = string.Format(this.Activity.GetString(Resource.String.loy_mess_update_number_newdata), lstItem.Count);
                }
            }
            else
            {
                MasterThreads thread = new MasterThreads();
                thread.OnResult += (ServiceResult result) =>
                {
                    if (result.StatusCode == 1)
                    {
                        txtSubTask.Visibility = ViewStates.Visible;
                        // lstItem = result.Data as List<BaseItem>;
                        var data = result.Data as MBB_GetData;
                        mDtStart = data.LastGet;

                        lstItem = new List<BaseItem>();
                        if (data.MemberTypes != null)
                        {
                            foreach (var item in data.MemberTypes)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.Merchants != null)
                        {
                            foreach (var item in data.Merchants)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MerchantCategories != null)
                        {
                            foreach (var item in data.MerchantCategories)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MerchantProducts != null)
                        {
                            foreach (var item in data.MerchantProducts)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.RedemptionCategories != null)
                        {
                            foreach (var item in data.RedemptionCategories)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.RedemptionProducts != null)
                        {
                            foreach (var item in data.RedemptionProducts)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.RedemptionProductDetails != null)
                        {
                            foreach (var item in data.RedemptionProductDetails)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.RedemptionPartners != null)
                        {
                            foreach (var item in data.RedemptionPartners)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MerchantLocations != null)
                        {
                            //MerchantLocation
                            foreach (var item in data.MerchantLocations)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MerchantProductMemberTypes != null)
                        {
                            foreach (var item in data.MerchantProductMemberTypes)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }  
                        }
                        if (data.MemberGroups != null)
                        {
                            foreach (var item in data.MemberGroups)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.MemberGroupDetails != null)
                        {
                            foreach (var item in data.MemberGroupDetails)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.Countries != null)
                        {
                            foreach (var item in data.Countries)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        if (data.States != null)
                        {
                            foreach (var item in data.States)
                            {
                                BaseItem bitem = new BaseItem();
                                bitem.Item = item;
                                lstItem.Add(bitem);
                            }
                        }
                        txtMainTask.Text = this.Activity.GetString(Resource.String.loy_mess_update_downloadfinish);
                        if (lstItem.Count == 0)
                        {
                            txtSubTask.Text = this.Activity.GetString(Resource.String.loy_mess_update_no_newdata);
                        }
                        else
                        {
                            if (((BaseActivity)this.Activity).getLastUpdate() == BaseActivity.FirstUpdate)
                            {
                                BtnUpdate_Click(null, null);
                            }
                            btnUpdate.Enabled = true;

                            txtSubTask.Text = string.Format(this.Activity.GetString(Resource.String.loy_mess_update_number_newdata), lstItem.Count);
                            txtPercent.Visibility = ViewStates.Visible;
                            txtIndex.Visibility = ViewStates.Visible;
                            txtIndex.Text = "0/" + lstItem.Count;
                        }
                    }
                    else
                    {
                        txtMainTask.Text = result.Mess;
                    }

                };
                txtSubTask.Visibility = ViewStates.Gone;
                txtMainTask.Text = this.Activity.GetString(Resource.String.loy_mess_update_download);
                //mDtStart = DateTime.UtcNow;
                thread.GetData(((BaseActivity)this.Activity).getLastUpdate(), new M_BBGetDataDeletedID());
            }
            btnCancel.Click += BtnCancel_Click;
            btnUpdate.Click += BtnUpdate_Click;
            return view;
        }


        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (thread == null)
            {
                thread = new MasterThreads();
                btnUpdate.Enabled = false;
                thread.OnDataChange += (string table, MasterThreads.Action action, int index,int count) =>
                {
                    txtMainTask.Text = table;
                    txtSubTask.Visibility = ViewStates.Visible;
                    switch (action)
                    {
                        case MasterThreads.Action.AddNew:
                            txtSubTask.Text = this.Activity.GetString(Resource.String.loy_mess_update_add);
                            break;
                        case MasterThreads.Action.Update:
                            txtSubTask.Text = this.Activity.GetString(Resource.String.loy_mess_update_update);
                            break;
                        case MasterThreads.Action.Delete:
                            txtSubTask.Text = this.Activity.GetString(Resource.String.loy_mess_update_delete);
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
                    if (((BaseActivity)this.Activity).getLastUpdate() == BaseActivity.FirstUpdate)
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
                    txtMainTask.Text = this.Activity.GetString(Resource.String.loy_mess_update_finish);
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