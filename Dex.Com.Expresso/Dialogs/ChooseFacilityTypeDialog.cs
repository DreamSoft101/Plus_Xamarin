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
using EXPRESSO.Models;
using static EXPRESSO.Models.EnumType;
using EXPRESSO.Threads;
using EXPRESSO.Models.Database;

namespace Dex.Com.Expresso.Dialogs
{
    public class ChooseFacilityTypeDialog : DialogFragment
    {
        private FacilitiesTypeAdapter adapter;
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

            int width = (int)(this.Activity.Resources.DisplayMetrics.WidthPixels * 0.95);
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
            //var save = adapter.getSaveData();
            //string strSave = EXPRESSO.Utils.StringUtils.Object2String(save);
            //((BaseActivity)this.Activity).saveMySetting(strSave);
            if (OnDismess != null)
            {
                OnDismess();
            }
        }



        public static ChooseFacilityTypeDialog NewInstance(Bundle bundle)
        {
            ChooseFacilityTypeDialog fragment = new ChooseFacilityTypeDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.SetTitle(Resource.String.exp_dialog_title);
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.exp_dialog_choose_facilitiy_type, container, false);
            ListView lstView = view.FindViewById<ListView>(Resource.Id.lstItems);

            PointOfInterestThread thread = new PointOfInterestThread();
            List<FacilitiesSetting> mLstBaseItem = new List<FacilitiesSetting>();
            thread.OnLoadFacilityType += (List<TblFacilityType> type) =>
            {
                foreach (var item in type)
                {
                    FacilitiesSetting fitem = new FacilitiesSetting();
                    fitem.intType = 1;
                    fitem.intID = item.intFacilityType.ToString();
                    fitem.strURL = item.strPicture;
                    fitem.strName = item.strName;
                    mLstBaseItem.Add(fitem);
                }
            };
            thread.OnLoadNearbyCategory += (List<TblNearbyCatg> cate) =>
            {
                //foreach (var item in cate)
                //{
                //    FacilitiesSetting fitem = new FacilitiesSetting();
                //    fitem.intType = 1;
                //    fitem.intID = item.idNearbyCatg;
                //    fitem.strName = item.strNearbyCatgName;
                //    mLstBaseItem.Add(fitem);
                //}

                FacilitiesTypeAdapter adapter = new FacilitiesTypeAdapter(this.mContext, mLstBaseItem);
                lstView.Adapter = adapter;
            };
            thread.loadFavoriteType();
          
            return view;
        }

        private void LstView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

        }
    }
}