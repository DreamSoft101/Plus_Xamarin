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
using Dex.Com.Expresso.Adapters.Listview;
using Dex.Com.Expresso.Activities;
using EXPRESSO.Threads;
using EXPRESSO.Models.Database;

namespace Dex.Com.Expresso.Dialogs
{
    public class TollPlazaDialog : DialogFragment
    {
        private Context mContext;
        public delegate void onChange();
        public onChange OnChange;
        private Adapters.Spinner.TollPlazaAdapter adapter;

        public delegate void onDismiss();
        public onDismiss OnDismess;

        public EditText mTxtFiter;

        public delegate void onChoose(TblTollPlaza toll);
        public onChoose OnChoose;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            mContext = this.Activity;
            return base.OnCreateDialog(savedInstanceState);
        }

        public override void OnStart()
        {
            base.OnStart();

            int width = (int)(this.Activity.Resources.DisplayMetrics.WidthPixels * 0.95);
            int height = (int)(this.Activity.Resources.DisplayMetrics.HeightPixels * 0.95);
            this.Dialog.Window.SetLayout(width, WindowManagerLayoutParams.WrapContent);
        }


        //RelativeLayout root = new RelativeLayout(getActivity());
        //root.setLayoutParams(new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
        //final Dialog dialog = new Dialog(getActivity());
        //dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        //dialog.setContentView(root);
        //dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.WHITE));
        //dialog.getWindow().setLayout(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT);

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            
        }



        public static TollPlazaDialog NewInstance(Bundle bundle)
        {
            TollPlazaDialog fragment = new TollPlazaDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.SetTitle(Resource.String.exp_dialog_title);
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.exp_dialog_tollplaza, container, false);
            ListView lstView = view.FindViewById<ListView>(Resource.Id.lstItems);
            mTxtFiter = view.FindViewById<EditText>(Resource.Id.txtFiler);
            mTxtFiter.AfterTextChanged += MTxtFiter_AfterTextChanged;
            TollFareThread thread = new TollFareThread();
            thread.OnGetTollPlaza += (List<TblTollPlaza> toll) =>
            {
                adapter = new Adapters.Spinner.TollPlazaAdapter(this.Activity,toll);
                lstView.Adapter = adapter;

            };
            thread.loadTollPlaza();
            lstView.ItemClick += LstView_ItemClick1;
            return view;
        }

        private void LstView_ItemClick1(object sender, AdapterView.ItemClickEventArgs e)
        {
            //throw new NotImplementedException();
            var item = adapter.GetTollPlaza(e.Position);
            if (OnChoose != null)
            {
                OnChoose(item);
            }
            this.Dismiss();
        }

        private void MTxtFiter_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            //throw new NotImplementedException();
            string text = e.Editable.ToString();
            adapter.Filter.InvokeFilter(text);
        }

        private void LstView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

        }
    }
}