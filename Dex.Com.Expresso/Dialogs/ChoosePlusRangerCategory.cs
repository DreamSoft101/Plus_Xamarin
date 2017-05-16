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
using EXPRESSO.Threads;
using EXPRESSO.Models.Database;
using EXPRESSO.Models;

namespace Dex.Com.Expresso.Dialogs
{
    public class ChoosePlusRangerCategory : DialogFragment
    {
        private PlusRangerCategoryAdapter adapter;
        private Context mContext;
        public delegate void onChange();
        public onChange OnChange;


        public delegate void onDismiss();
        public onDismiss OnDismess;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            mContext = this.Activity;
            return base.OnCreateDialog(savedInstanceState);
        }

        public override void OnStart()
        {
            base.OnStart();

            int width = (int)(this.Activity.Resources.DisplayMetrics.WidthPixels * 0.75);
            //int height = (int)(this.Activity.Resources.DisplayMetrics.HeightPixels * 0.75);
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
            if (OnDismess != null)
            {
                OnDismess();
            }
        }



        public static ChoosePlusRangerCategory NewInstance(Bundle bundle)
        {
            ChoosePlusRangerCategory fragment = new ChoosePlusRangerCategory();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.SetTitle(Resource.String.exp_dialog_title);
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.exp_dialog_choose_plusranger_category, container, false);

            ProgressBar prbLoading = view.FindViewById<ProgressBar>(Resource.Id.prbLoading);

            ListView lstView = view.FindViewById<ListView>(Resource.Id.lstItems);

            PLUSRangerThreads thread = new PLUSRangerThreads();
            thread.OnGetCategory += (ServiceResult result) =>
            {

                if (result.intStatus == 1)
                {
                    prbLoading.Visibility = ViewStates.Gone;
                    List<TblCategory> lst = result.Data as List<TblCategory>;
                    adapter = new PlusRangerCategoryAdapter(mContext, lst);
                    lstView.Adapter = adapter;
                }
                else
                {
                    Toast.MakeText(this.Activity, result.strMess, ToastLength.Short).Show();
                }
            };
            thread.GetCategory();
            prbLoading.Visibility = ViewStates.Visible;


            return view;
        }

        private void LstView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

        }
    }
}