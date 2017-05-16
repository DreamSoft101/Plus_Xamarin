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

namespace Dex.Com.Expresso.Dialogs
{
    public class FilterLiveTrafficDialog : DialogFragment
    {
        private Context mContext;
        public delegate void onSave(int[] listid);
        public onSave OnSave;
        private List<int> mLstChecked;
        CheckBox cke9;
        CheckBox cke1;
        CheckBox cke2;
        CheckBox cke3;
        CheckBox cke4;
        CheckBox cke5;
        CheckBox cke6;
        CheckBox cke7;
        CheckBox cke8;
        CheckBox cke10;
        CheckBox cke11;
        CheckBox cke12;
        CheckBox cke14;
        private List<CheckBox> lstCheck = new List<CheckBox>();
        public static FilterLiveTrafficDialog NewInstance(Bundle bundle, List<int> lstChecked)
        {
            FilterLiveTrafficDialog fragment = new FilterLiveTrafficDialog() { mLstChecked = lstChecked };
            fragment.Arguments = bundle;
            return fragment;
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            FilterLiveTrafficDialog_Click(null, null);
        }

        public override void OnStart()
        {
            base.OnStart();
           
            int width = (int)(this.Activity.Resources.DisplayMetrics.WidthPixels * 0.75);
            //int height = (int)(this.Activity.Resources.DisplayMetrics.HeightPixels * 0.75);
            this.Dialog.Window.SetLayout(width, WindowManagerLayoutParams.WrapContent);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.Dialog.SetTitle(Resource.String.title_choose_entity);
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            this.Cancelable = true;
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.dialog_filter_livetraffic, container, false);

            cke9 = view.FindViewById<CheckBox>(Resource.Id.cke9);
            cke1 = view.FindViewById<CheckBox>(Resource.Id.cke1);
            cke2 = view.FindViewById<CheckBox>(Resource.Id.cke2);
            cke3 = view.FindViewById<CheckBox>(Resource.Id.cke3);
            cke4 = view.FindViewById<CheckBox>(Resource.Id.cke4);
            cke5 = view.FindViewById<CheckBox>(Resource.Id.cke5);
            cke6 = view.FindViewById<CheckBox>(Resource.Id.cke6);
            cke7 = view.FindViewById<CheckBox>(Resource.Id.cke7);
            cke8 = view.FindViewById<CheckBox>(Resource.Id.cke8);
            cke10 = view.FindViewById<CheckBox>(Resource.Id.cke10);
            cke11 = view.FindViewById<CheckBox>(Resource.Id.cke11);
            cke12 = view.FindViewById<CheckBox>(Resource.Id.cke12);
            cke14 = view.FindViewById<CheckBox>(Resource.Id.cke14);
            lstCheck.Add(cke1);
            lstCheck.Add(cke2);
            lstCheck.Add(cke3);
            lstCheck.Add(cke4);
            lstCheck.Add(cke5);
            lstCheck.Add(cke6);
            lstCheck.Add(cke7);
            lstCheck.Add(cke8);
            lstCheck.Add(cke9);
            lstCheck.Add(cke10);
            lstCheck.Add(cke11);
            lstCheck.Add(cke12);
            lstCheck.Add(null);
            lstCheck.Add(cke14);
            LinearLayout lnl1 = view.FindViewById<LinearLayout>(Resource.Id.lnl1);
            LinearLayout lnl2 = view.FindViewById<LinearLayout>(Resource.Id.lnl2);
            LinearLayout lnl3 = view.FindViewById<LinearLayout>(Resource.Id.lnl3);
            LinearLayout lnl4 = view.FindViewById<LinearLayout>(Resource.Id.lnl4);
            LinearLayout lnl5 = view.FindViewById<LinearLayout>(Resource.Id.lnl5);
            LinearLayout lnl6 = view.FindViewById<LinearLayout>(Resource.Id.lnl6);
            LinearLayout lnl7 = view.FindViewById<LinearLayout>(Resource.Id.lnl7);
            LinearLayout lnl8 = view.FindViewById<LinearLayout>(Resource.Id.lnl8);
            LinearLayout lnl9 = view.FindViewById<LinearLayout>(Resource.Id.lnl9);
            LinearLayout lnl10 = view.FindViewById<LinearLayout>(Resource.Id.lnl10);
            LinearLayout lnl11 = view.FindViewById<LinearLayout>(Resource.Id.lnl11);
            LinearLayout lnl12 = view.FindViewById<LinearLayout>(Resource.Id.lnl12);
            LinearLayout lnl14 = view.FindViewById<LinearLayout>(Resource.Id.lnl14);
            lnl9.Click += Lnl9_Click;
            lnl1.Click += Lnl1_Click;
            lnl2.Click += Lnl2_Click;
            lnl3.Click += Lnl3_Click;
            lnl4.Click += Lnl4_Click;
            lnl5.Click += Lnl5_Click;
            lnl6.Click += Lnl6_Click;
            lnl7.Click += Lnl7_Click;
            lnl8.Click += Lnl8_Click;
            lnl10.Click += Lnl10_Click;
            lnl11.Click += Lnl11_Click;
            lnl12.Click += Lnl12_Click;
            lnl14.Click += Lnl14_Click;



            if (mLstChecked != null)
            {
                for (int i=0; i < mLstChecked.Count; i ++)
                {
                    lstCheck[mLstChecked[i] - 1].Checked = true;
                }
            }

            view.FindViewById(Resource.Id.btnSave).Click += FilterLiveTrafficDialog_Click;
            return view;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return base.OnCreateDialog(savedInstanceState);
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
        }

        private void Lnl14_Click(object sender, EventArgs e)
        {
            ChangePos(14);
        }

        private void FilterLiveTrafficDialog_Click(object sender, EventArgs e)
        {
            int[] saved = null;
            int count = lstCheck.Where(p => p != null).Where(p => p.Checked == true).Count();
            saved = new int[count];
            int index = 0;
            for (int i = 0; i < lstCheck.Count; i++)
            {
                if (lstCheck[i] == null)
                    continue;
                if (lstCheck[i].Checked)
                {
                    saved[index] = i+1;
                    index++;
                }
            }
            if (OnSave != null)
            {
                OnSave(saved);
            }
            //this.Dismiss();

        }

       


        private void ChangePos(int pos)
        {
            lstCheck[pos - 1].Checked = !lstCheck[pos - 1].Checked;
        }

        private void Lnl12_Click(object sender, EventArgs e)
        {
            ChangePos(12);
        }

        private void Lnl11_Click(object sender, EventArgs e)
        {
            ChangePos(11);
        }

        private void Lnl10_Click(object sender, EventArgs e)
        {
            ChangePos(10);
        }
        private void Lnl9_Click(object sender, EventArgs e)
        {
            ChangePos(9);
        }

        private void Lnl8_Click(object sender, EventArgs e)
        {
            ChangePos(8);
        }

        private void Lnl7_Click(object sender, EventArgs e)
        {
            ChangePos(7);
        }

        private void Lnl6_Click(object sender, EventArgs e)
        {
            ChangePos(6);
        }

        private void Lnl5_Click(object sender, EventArgs e)
        {
            ChangePos(5);
        }

        private void Lnl4_Click(object sender, EventArgs e)
        {
            ChangePos(4);
        }

        private void Lnl3_Click(object sender, EventArgs e)
        {
            ChangePos(3);
        }

        private void Lnl2_Click(object sender, EventArgs e)
        {
            ChangePos(2);
        }

        private void Lnl1_Click(object sender, EventArgs e)
        {
            ChangePos(1);
        }

       
    }
}