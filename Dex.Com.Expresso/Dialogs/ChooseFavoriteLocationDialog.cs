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
using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Adapters.Spinner;

namespace Dex.Com.Expresso.Dialogs
{
    public class ChooseFavoriteLocationDialog : DialogFragment
    {
        private FavoriteLocationAdapter adapter;
        private Context mContext;
        public delegate void onChange();
        public onChange OnChange;
        private Spinner mSpnHighway;
        private ListView lstView;
        public delegate void onDismiss();
        public onDismiss OnDismess;
        private ProgressBar prbLoading;
        private HighwayAdapter highwayAdapter;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            mContext = this.Activity;
            return base.OnCreateDialog(savedInstanceState);
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
            //var save = adapter.getSaveData();
            //((BaseActivity)this.Activity).setFavoriteLocation(save);
            if (OnDismess != null)
            {
                OnDismess();
            }
        }



        public static ChooseFavoriteLocationDialog NewInstance(Bundle bundle)
        {
            ChooseFavoriteLocationDialog fragment = new ChooseFavoriteLocationDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.SetTitle(Resource.String.exp_dialog_title);
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.exp_dialog_choosefavoritelocation, container, false);
            lstView = view.FindViewById<ListView>(Resource.Id.lstItems);
            prbLoading = view.FindViewById<ProgressBar>(Resource.Id.prbLoading);
            Spinner spnHighway = view.FindViewById<Spinner>(Resource.Id.spnHighway);
            spnHighway.ItemSelected += SpnHighway_ItemSelected;
            MastersThread thread = new MastersThread();
            thread.OnLoadListHighway += (List<TblHighway> result) =>
            {
                highwayAdapter = new HighwayAdapter(this.Activity, result);
                spnHighway.Adapter = highwayAdapter;

            };
            thread.loadListHighway();

            //FavoriteThread thread = new FavoriteThread();
            //thread.OnGetFavoriteLocation += (ServiceResult result) =>
            //{
            //    //adapter = new HighwaySettingAdapter(mContext, result);
            //    //lstView.Adapter = adapter;
            //};
            //thread.GetFavoriteLocation(null);
            return view;
        }

        private void SpnHighway_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            lstView.Visibility = ViewStates.Gone;
            prbLoading.Visibility = ViewStates.Visible;
            var item = highwayAdapter.GetHighway(e.Position);
            FavoriteThread thread = new FavoriteThread();
            thread.OnGetFavoriteLocation += (ServiceResult result) =>
            {
                
                prbLoading.Visibility = ViewStates.Gone;
                if (result.intStatus == 1)
                {
                    lstView.Visibility = ViewStates.Visible;
                    adapter = new FavoriteLocationAdapter(mContext, result.Data as List<FavoriteLocation>);
                    lstView.Adapter = adapter;
                }
                else
                {
                    Toast.MakeText(this.Activity, result.strMess, ToastLength.Short).Show();
                }
            };
            thread.GetFavoriteLocation(item.idHighway);
        }

        private void LstView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

        }
    }
}