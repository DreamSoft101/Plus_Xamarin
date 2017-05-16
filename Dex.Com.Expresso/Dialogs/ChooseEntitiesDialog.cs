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
using EXPRESSO.Models;
using Dex.Com.Expresso.Activities;
using Dex.Com.Expresso.Adapters.Listview;

namespace Dex.Com.Expresso.Dialogs
{
    public class ChooseEntitiesDialog : DialogFragment
    {
        private MyEntityAdapter adapter;
        private Context mContext;
        public delegate void onChangeEntity(MyEntity entity);
        public onChangeEntity OnChangeEntity;


        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return base.OnCreateDialog(savedInstanceState);
        }
       
            //RelativeLayout root = new RelativeLayout(getActivity());
            //root.setLayoutParams(new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
            //final Dialog dialog = new Dialog(getActivity());
            //dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
            //dialog.setContentView(root);
            //dialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.WHITE));
            //dialog.getWindow().setLayout(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT);

          


       
        public static ChooseEntitiesDialog NewInstance(Bundle bundle)
        {
            ChooseEntitiesDialog fragment = new ChooseEntitiesDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.SetTitle(Resource.String.title_choose_entity);
            Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.dialog_listentities, container, false);
            ListView lstView = view.FindViewById<ListView>(Resource.Id.listView1);
            List<MyEntity> lstEntity = ((BaseActivity)this.Activity).getMyEntity();
             adapter = new MyEntityAdapter(this.Activity, lstEntity);
            lstView.Adapter = adapter;

            lstView.ItemClick += LstView_ItemClick;

            return view;
        }

        private void LstView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = adapter.GetMyEntity(e.Position);
            if (OnChangeEntity != null)
            {
                OnChangeEntity(item);
                this.Dismiss();
            }
        }
    }
}