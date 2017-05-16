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
using Dex.Com.Expresso.Adapters.Spinner;
using FR.Ganfra.Materialspinner;
using EXPRESSO.Models.Database;
using EXPRESSO.Models;
using Dex.Com.Expresso.Activities;
using EXPRESSO.Utils;
using Square.Picasso;
using Dex.Com.Expresso.Dialogs;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_TollFare : BaseFragment 
    {
        private string TAG = "Fragment_TollFare";
        private MaterialSpinner spnClasses;
        private MaterialSpinner spnFrom;
        private MaterialSpinner spnTo;
        private Button btnSearch;
        private VehicleClassesAdapter adapter;
        private TollPlazaAdapter adapterHighway;
        private TextView mTxtVehicleClass;
        private TextView mTxtAmount, mTxtView;
        private LinearLayout mLnlResult, lnlToFrom;
        private ImageView imgCar;
        public RadioButton mRdbTo, mRdbFrom;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment_TollFare NewInstance()
        {
            var frag1 = new Fragment_TollFare { Arguments = new Bundle() };
            return frag1;
        }

        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_tollfare, null);
            spnClasses = view.FindViewById<MaterialSpinner>(Resource.Id.spnClass);
            spnFrom = view.FindViewById<MaterialSpinner>(Resource.Id.spnForm);
            spnTo = view.FindViewById<MaterialSpinner>(Resource.Id.spnTo);
            btnSearch = view.FindViewById<Button>(Resource.Id.btnSearch);
            mTxtAmount = view.FindViewById<TextView>(Resource.Id.txtAmount);
            mTxtView = view.FindViewById<TextView>(Resource.Id.txtView);
            mLnlResult = view.FindViewById<LinearLayout>(Resource.Id.lnlResult);
            imgCar = view.FindViewById<ImageView>(Resource.Id.imgCar);
            mTxtView.Visibility = ViewStates.Gone;
            mLnlResult.Visibility = ViewStates.Gone;
            mRdbTo = view.FindViewById<RadioButton>(Resource.Id.rdbTo);
            mRdbFrom = view.FindViewById<RadioButton>(Resource.Id.rdbFrom);
            lnlToFrom = view.FindViewById<LinearLayout>(Resource.Id.lnlToFrom);
            spnFrom.ItemSelected += SpnFrom_ItemSelected;

            lnlToFrom.Visibility = ViewStates.Gone;
            spnTo.Visibility = ViewStates.Gone;
            mRdbTo.Checked = true;
            mRdbTo.CheckedChange += MRdbTo_CheckedChange;
            mRdbFrom.CheckedChange += MRdbFrom_CheckedChange;
            this.mTxtVehicleClass = view.FindViewById<TextView>(Resource.Id.txtVehicleClass);
            this.mTxtVehicleClass.Click += delegate
            {
                Intent intent = new Intent(getActivity(), typeof(VehicleClassesActivity));
                StartActivity(intent);
            };


            TollFareThread thread = new TollFareThread();
            thread.OnLoadClassesResult += (lstClasses,lstTollPlaza) =>
            {
                adapter = new VehicleClassesAdapter(getActivity(), lstClasses);
                spnClasses.Adapter = adapter;
                adapterHighway = new TollPlazaAdapter(getActivity(), lstTollPlaza);
                spnFrom.Adapter = adapterHighway;
                spnTo.Adapter = adapterHighway;
            };
            thread.loadTollFareHomePage();

            btnSearch.Click += delegate {
                if (spnFrom.SelectedItemPosition == 0)
                {
                    Toast.MakeText(getActivity(), Resource.String.toll_mess_select_all, ToastLength.Short).Show();
                    return;
                }
              
                if (spnClasses.SelectedItemPosition == 0)
                {
                    Toast.MakeText(getActivity(), Resource.String.toll_mess_select_all, ToastLength.Short).Show();
                    return;
                }

                if (spnFrom.SelectedItemPosition == spnTo.SelectedItemPosition)
                {
                    Toast.MakeText(getActivity(), Resource.String.toll_mess_select_different, ToastLength.Short).Show();
                    return;
                }
                TblTollPlaza from = adapterHighway.GetTollPlaza(spnFrom.SelectedItemPosition - 1);
                VehicleClasses Class = adapter.GetVehicleClasses(spnClasses.SelectedItemPosition - 1);

                Settings set = Cons.myEntity.mSettings;

                switch (Class.intValue)
                {
                    case 1:
                        {
                            if (!string.IsNullOrEmpty(set.vehicle_class_1_icon))
                            {
                                Picasso.With(this.getActivity()).Load(set.vehicle_class_1_icon).Error(Resource.Drawable.car_class1).Into(imgCar);
                            }
                            else
                            {
                                Picasso.With(this.getActivity()).Load(Resource.Drawable.car_class1).Into(imgCar);
                            }
                            break;
                        }
                    case 2:
                        {
                            if (!string.IsNullOrEmpty(set.vehicle_class_2_icon))
                            {
                                Picasso.With(this.getActivity()).Load(set.vehicle_class_2_icon).Error(Resource.Drawable.car_class2).Into(imgCar);
                            }
                            else
                            {
                                Picasso.With(this.getActivity()).Load(Resource.Drawable.car_class2).Into(imgCar);
                            }
                            break;
                        }
                    case 3:
                        {
                            if (!string.IsNullOrEmpty(set.vehicle_class_3_icon))
                            {
                                Picasso.With(this.getActivity()).Load(set.vehicle_class_3_icon).Error(Resource.Drawable.car_class3).Into(imgCar);
                            }
                            else
                            {
                                Picasso.With(this.getActivity()).Load(Resource.Drawable.car_class3).Into(imgCar);
                            }
                            break;
                        }
                    case 4:
                        {
                            if (!string.IsNullOrEmpty(set.vehicle_class_4_icon))
                            {
                                Picasso.With(this.getActivity()).Load(set.vehicle_class_4_icon).Error(Resource.Drawable.car_class4).Into(imgCar);
                            }
                            else
                            {
                                Picasso.With(this.getActivity()).Load(Resource.Drawable.car_class4).Into(imgCar);
                            }
                            break;
                        }
                    case 5:
                        {
                            if (!string.IsNullOrEmpty(set.vehicle_class_5_icon))
                            {
                                Picasso.With(this.getActivity()).Load(set.vehicle_class_5_icon).Error(Resource.Drawable.car_class5).Into(imgCar);
                            }
                            else
                            {
                                Picasso.With(this.getActivity()).Load(Resource.Drawable.car_class5).Into(imgCar);
                            }
                            break;
                        }
                }
                if (from.strType == "1")
                {
                    if (spnTo.SelectedItemPosition == 0 && from.strType == "1")
                    {
                        Toast.MakeText(getActivity(), Resource.String.toll_mess_select_all, ToastLength.Short).Show();
                        return;
                    }
                    
                    //set.vehicle_class_1_icon 


                    TblTollPlaza To = adapterHighway.GetTollPlaza(spnTo.SelectedItemPosition - 1);
                  
                    EXPRESSO.Utils.LogUtils.WriteLog(TAG, String.Format("Search / From: {0} / To: {1} / Class: {2}", from.idTollPlaza, To.idTollPlaza, Class.strName));
                    TollFareThread tollSearchThread = new TollFareThread();
                    tollSearchThread.OnGetAmount += (decimal result) =>
                    {
                        if (result != -1)
                        {
                            mLnlResult.Visibility = ViewStates.Visible;
                            mTxtView.Visibility = ViewStates.Visible;
                            mTxtAmount.Text = "RM " + result;// string.Format(this.getActivity().GetString(Resource.String.money))
                        }
                        else
                        {
                            mLnlResult.Visibility = ViewStates.Gone;
                            mTxtView.Visibility = ViewStates.Gone;
                            Toast.MakeText(getActivity(), "", ToastLength.Short).Show();
                        }
                    };
                    if (mRdbTo.Checked)
                    {
                        tollSearchThread.GetAmountTollFare(from, To, Class);
                    }
                    else
                    {
                        tollSearchThread.GetAmountTollFare(To, from, Class);
                    }
                    
                }
                else
                {
                    TollFareThread tollSearchThread = new TollFareThread();
                    tollSearchThread.OnGetAmount += (decimal result) =>
                    {
                        if (result != -1)
                        {
                            mLnlResult.Visibility = ViewStates.Visible;
                            mTxtView.Visibility = ViewStates.Gone;
                            mTxtAmount.Text = "RM " + result;// string.Format(this.getActivity().GetString(Resource.String.money))
                        }
                        else
                        {
                            mTxtView.Visibility = ViewStates.Gone;
                            mLnlResult.Visibility = ViewStates.Gone;
                            Toast.MakeText(getActivity(), "", ToastLength.Short).Show();
                        }
                    };
                    tollSearchThread.GetAmountTollFare(from, null, Class);
                }
            };

            spnFrom.Touch += SpnFrom_Touch;
            spnTo.Touch += SpnTo_Touch;
            return view;
        }

        private void SpnTo_Touch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up)
            {
                FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
                Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog_favorite");
                if (prev != null)
                {
                    ft.Remove(prev);
                }
                ft.AddToBackStack(null);
                TollPlazaDialog newFragment = TollPlazaDialog.NewInstance(null);
                newFragment.OnChoose += (TblTollPlaza toll) =>
                {
                    for (int i = 0; i < adapterHighway.Count; i++)
                    {
                        var item = adapterHighway.GetTollPlaza(i);
                        if (item.idTollPlaza == toll.idTollPlaza)
                        {
                            spnTo.SetSelection(i + 1);
                            break;
                        }
                    }
                };
                newFragment.Show(ft, "dialog_favorite");
            }
           
        }

        private void SpnFrom_Touch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up)
            {
                FragmentTransaction ft = getActivity().FragmentManager.BeginTransaction();
                Fragment prev = getActivity().FragmentManager.FindFragmentByTag("dialog_favorite");
                if (prev != null)
                {
                    ft.Remove(prev);
                }
                ft.AddToBackStack(null);
                TollPlazaDialog newFragment = TollPlazaDialog.NewInstance(null);
                newFragment.OnChoose += (TblTollPlaza toll) =>
                {
                    for (int i = 0; i < adapterHighway.Count; i++)
                    {
                        var item = adapterHighway.GetTollPlaza(i);
                        if (item.idTollPlaza == toll.idTollPlaza)
                        {
                            spnFrom.SetSelection(i + 1);
                            break;
                        }
                    }
                };
                newFragment.Show(ft, "dialog_favorite");
            }
            //throw new NotImplementedException();
           
            
        }

        private void MRdbFrom_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                mRdbTo.Checked = false;
            }
            
        }

        private void MRdbTo_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                mRdbFrom.Checked = false;
            }
            //throw new NotImplementedException();
            
        }

        private void SpnFrom_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (spnFrom.SelectedItemPosition > 0)
            {
                TblTollPlaza from = adapterHighway.GetTollPlaza(spnFrom.SelectedItemPosition - 1);
                if (from.strType == "0")
                {
                    spnTo.Visibility = ViewStates.Gone;
                    lnlToFrom.Visibility = ViewStates.Gone;
                }
                else
                {
                    spnTo.Visibility = ViewStates.Visible;
                    lnlToFrom.Visibility = ViewStates.Visible;
                }
            }
          
        }

        public static string IDROUND = "IDROUND";
        public static string INTSTARTSEQ = "INTSTARTSEQ";
        public static string INTENDSEQ = "INTENDSEQ";
        public static string DECRATE = "DECRATE";
        public static string INTVHECLASS = "INTVHECLASS";
    }
}