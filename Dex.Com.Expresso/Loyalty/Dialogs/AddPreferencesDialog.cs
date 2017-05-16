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
using Loyalty.Models;
using Dex.Com.Expresso.Loyalty.Droid.Adapters.Listviews;
using Android.Graphics;
using Dex.Com.Expresso;

namespace Dex.Com.Expresso.Loyalty.Droid.Dialogs
{
    public class AddPreferencesDialog : DialogFragment
    {
        public delegate void onDismiss();
        public onDismiss EventOnDismiss;

        public delegate void onAdd(ItemPreferences item, AddPreferencesDialog dialog);
        public onAdd OnAdd;

        private Button btnCancel;
        private ListView lstView;
        private EditText mTxtFilter;
        private List<ItemPreferences> mLstMemberType;
        private ItemPreferencesAdapters mAdapter;
        

        public static AddPreferencesDialog NewInstance(Bundle bundle)
        {
            AddPreferencesDialog fragment = new AddPreferencesDialog() { };
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
            View view = inflater.Inflate(Resource.Layout.loy_dialog_add_preferences, container, false);

            Rect displayRectangle = new Rect();
            Window window = this.Activity.Window;
            window.DecorView.GetWindowVisibleDisplayFrame(displayRectangle);
            view.SetMinimumHeight((int)(displayRectangle.Height() * 0.9));


            btnCancel = view.FindViewById<Button>(Resource.Id.btnCancel);
           
            lstView = view.FindViewById<ListView>(Resource.Id.lstData);
            mTxtFilter = view.FindViewById<EditText>(Resource.Id.txtFilter);
            mTxtFilter.TextChanged += MTxtFilter_TextChanged;
            lstView.ItemClick += LstView_ItemClick;

            UserThreads thread = new UserThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                mLstMemberType = result.Data as List<ItemPreferences>;
                mAdapter = new ItemPreferencesAdapters(this.Activity, mLstMemberType);
                lstView.Adapter = mAdapter;

            };
            thread.Preference();
            //thread.Preference

            btnCancel.Click += BtnCancel_Click;
            return view;
        }

        private void MTxtFilter_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            //throw new NotImplementedException();
            Java.Lang.ICharSequence strData =    new Java.Lang.String(mTxtFilter.Text);
            mAdapter.Filter.InvokeFilter(strData);
        }

        private void LstView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = mLstMemberType[e.Position];
            if (OnAdd != null)
            {
                OnAdd(item, this);
            }
            //Dismiss();
        }

       

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

    }
}