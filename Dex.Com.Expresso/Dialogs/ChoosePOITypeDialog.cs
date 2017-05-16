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
using Dex.Com.Expresso.Adapters.Listview;
using EXPRESSO.Models;
using EXPRESSO.Models.Database;
using Dex.Com.Expresso.Activities;

namespace Dex.Com.Expresso.Dialogs
{
    public class ChoosePOITypeDialog : DialogFragment
    {
        private POITypeAdapter adapter;
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
            //int width = (int)(this.Activity.Resources.DisplayMetrics.WidthPixels * 0.75);
            //int height = (int)(this.Activity.Resources.DisplayMetrics.HeightPixels * 0.75);
            //this.Dialog.Window.SetLayout(width, WindowManagerLayoutParams.WrapContent);
        }

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



        public static ChoosePOITypeDialog NewInstance(Bundle bundle)
        {
            ChoosePOITypeDialog fragment = new ChoosePOITypeDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.SetTitle(Resource.String.exp_dialog_title);
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.exp_dialog_choose_poi_type, container, false);
            ListView lstView = view.FindViewById<ListView>(Resource.Id.lstItems);

            PointOfInterestThread thread = new PointOfInterestThread();
            List<POITypeSetting> mLstBaseItem = ((BaseActivity)this.Activity).getAllPOIType();
            
            thread.OnLoadNearbyCategory += (List<TblNearbyCatg> cate) =>
            {
                foreach (var item in cate)
                {
                    POITypeSetting fitem = new POITypeSetting();
                    fitem.intNearby = 1;
                    fitem.intID = item.idNearbyCatg;
                    fitem.strName = item.strNearbyCatgName;
                    fitem.strURL = item.strNearbyCatgImg;
                    mLstBaseItem.Add(fitem);
                }

                POITypeAdapter adapter = new POITypeAdapter(this.mContext, mLstBaseItem);
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