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
using EXPRESSO.Models.Database;
using EXPRESSO.Threads;
using Square.Picasso;
using EXPRESSO.Utils;
using Dex.Com.Expresso.Dialogs;

namespace Dex.Com.Expresso.Fragments
{
    public class Fragment_TollPlaza_TollFare : BaseFragment
    {
        private TblTollPlaza mTollPlaza;
        private Spinner mSpnTollPlaza, mSpnVechi;
        private Adapters.Spinner.TollPlazaAdapter adapterTollPlaza;
        private TextView mTxtSum;
        private LinearLayout mLnlResult;
        private RadioButton mRdbTo, mRdbFrom;
        private ImageView mImgCar;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment_TollPlaza_TollFare NewInstance(TblTollPlaza toll)
        {
            var frag1 = new Fragment_TollPlaza_TollFare { Arguments = new Bundle(), mTollPlaza = toll };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.exp_fragment_tollplaza_tollfare, null);
            mImgCar = view.FindViewById<ImageView>(Resource.Id.imgCar);
            mRdbTo = view.FindViewById<RadioButton>(Resource.Id.rdbTo);
            mRdbFrom = view.FindViewById<RadioButton>(Resource.Id.rdbFrom);
            mRdbTo.Checked = true;
            mLnlResult = view.FindViewById<LinearLayout>(Resource.Id.lnlResult);
            mRdbTo.CheckedChange += MRdbTo_CheckedChange;
            mRdbFrom.CheckedChange += MRdbFrom_CheckedChange;
            mRdbTo.Visibility = ViewStates.Gone;
            mRdbFrom.Visibility = ViewStates.Gone;

            mSpnTollPlaza = view.FindViewById<Spinner>(Resource.Id.spnTollPlaza);
            mSpnTollPlaza.Touch += MSpnTollPlaza_Touch;
            mSpnVechi = view.FindViewById<Spinner>(Resource.Id.spnVechicle);

            mSpnVechi.ItemSelected += MSpnVechi_ItemSelected;
            mSpnTollPlaza.ItemSelected += MSpnTollPlaza_ItemSelected;

            mTxtSum = view.FindViewById<TextView>(Resource.Id.txtSumMoney);
            TollFareThread thread = new TollFareThread();
            thread.OnLoadClassesResult += (lstClasses, lstTollPlaza) =>
            {
                Adapters.Spinner.VehicleClassesAdapter adapter = new Adapters.Spinner.VehicleClassesAdapter(getActivity(), lstClasses);
                mSpnVechi.Adapter = adapter;
                adapterTollPlaza = new Adapters.Spinner.TollPlazaAdapter(getActivity(), lstTollPlaza);
                mSpnTollPlaza.Adapter = adapterTollPlaza;


            };
            thread.loadTollFareHomePage();


            if (mTollPlaza.strType == "0")
            {
                //Open
                mSpnTollPlaza.Visibility = ViewStates.Gone;
            }
            else
            {
                mSpnTollPlaza.Visibility = ViewStates.Visible;
                mRdbTo.Visibility = ViewStates.Visible;
                mRdbFrom.Visibility = ViewStates.Visible;
            }

            return view;
        }

        private void MSpnTollPlaza_Touch(object sender, View.TouchEventArgs e)
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
                    for (int i = 0; i < adapterTollPlaza.Count; i++)
                    {
                        var item = adapterTollPlaza.GetTollPlaza(i);
                        if (item.idTollPlaza == toll.idTollPlaza)
                        {
                            mSpnTollPlaza.SetSelection(i);
                            break;
                        }
                    }
                };
                newFragment.Show(ft, "dialog_favorite");
            }
        }

        private void MRdbFrom_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.IsChecked)
            {
                mRdbTo.Checked = false;
            }
        }

        private void MRdbTo_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.IsChecked)
            {
                mRdbFrom.Checked = false;
            }
        }

        private void MSpnTollPlaza_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {

            CalcuLator();
        }

        private void MSpnVechi_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            EXPRESSO.Models.Settings set = Cons.myEntity.mSettings;

            switch (mSpnVechi.SelectedItemPosition)
            {
                case 0:
                    {
                        if (!string.IsNullOrEmpty(set.vehicle_class_1_icon))
                        {
                            Picasso.With(this.getActivity()).Load(set.vehicle_class_1_icon).Error(Resource.Drawable.car_class1).Into(mImgCar);
                        }
                        else
                        {
                            Picasso.With(this.getActivity()).Load(Resource.Drawable.car_class1).Into(mImgCar);
                        }
                        break;
                    }
                case 1:
                    {
                        if (!string.IsNullOrEmpty(set.vehicle_class_2_icon))
                        {
                            Picasso.With(this.getActivity()).Load(set.vehicle_class_2_icon).Error(Resource.Drawable.car_class2).Into(mImgCar);
                        }
                        else
                        {
                            Picasso.With(this.getActivity()).Load(Resource.Drawable.car_class2).Into(mImgCar);
                        }
                        break;
                    }
                case 2:
                    {
                        if (!string.IsNullOrEmpty(set.vehicle_class_3_icon))
                        {
                            Picasso.With(this.getActivity()).Load(set.vehicle_class_3_icon).Error(Resource.Drawable.car_class3).Into(mImgCar);
                        }
                        else
                        {
                            Picasso.With(this.getActivity()).Load(Resource.Drawable.car_class3).Into(mImgCar);
                        }
                        break;
                    }
                case 3:
                    {
                        if (!string.IsNullOrEmpty(set.vehicle_class_4_icon))
                        {
                            Picasso.With(this.getActivity()).Load(set.vehicle_class_4_icon).Error(Resource.Drawable.car_class4).Into(mImgCar);
                        }
                        else
                        {
                            Picasso.With(this.getActivity()).Load(Resource.Drawable.car_class4).Into(mImgCar);
                        }
                        break;
                    }
                case 4:
                    {
                        if (!string.IsNullOrEmpty(set.vehicle_class_5_icon))
                        {
                            Picasso.With(this.getActivity()).Load(set.vehicle_class_5_icon).Error(Resource.Drawable.car_class5).Into(mImgCar);
                        }
                        else
                        {
                            Picasso.With(this.getActivity()).Load(Resource.Drawable.car_class5).Into(mImgCar);
                        }
                        break;
                    }
            }
            CalcuLator();
        }

        private void CalcuLator()
        {
            mLnlResult.Visibility = ViewStates.Gone;
            if (mTollPlaza.strType == "0")
            {
                mTxtSum.Text = "RM 0";
                TollFareThread thread = new TollFareThread();
                thread.OnGetTollFare += (TblTollFare tollfare) =>
                {
                    if (tollfare != null)
                    {
                        switch (mSpnVechi.SelectedItemPosition)
                        {
                            case 0:
                                mTxtSum.Text = "RM " + tollfare.decTollAmt1;
                                break;
                            case 1:
                                mTxtSum.Text = "RM " + tollfare.decTollAmt2;
                                break;
                            case 2:
                                mTxtSum.Text = "RM " + tollfare.decTollAmt3;
                                break;
                            case 3:
                                mTxtSum.Text = "RM " + tollfare.decTollAmt4;
                                break;
                            case 4:
                                mTxtSum.Text = "RM " + tollfare.decTollAmt5;
                                break;
                        }
                        mLnlResult.Visibility = ViewStates.Visible;
                    }
                };
                thread.getTollFare(mTollPlaza.idTollPlaza, null);
            }
            else
            {
                mTxtSum.Text = "RM 0";
                var itemTo = adapterTollPlaza.GetTollPlaza(mSpnTollPlaza.SelectedItemPosition).idTollPlaza;
                TollFareThread thread = new TollFareThread();
                thread.OnGetTollFare += (TblTollFare tollfare) =>
                {
                    if (tollfare != null)
                    {
                        switch (mSpnVechi.SelectedItemPosition)
                        {
                            case 0:
                                mTxtSum.Text = "RM " + tollfare.decTollAmt1;
                                break;
                            case 1:
                                mTxtSum.Text = "RM " + tollfare.decTollAmt2;
                                break;
                            case 2:
                                mTxtSum.Text = "RM " + tollfare.decTollAmt3;
                                break;
                            case 3:
                                mTxtSum.Text = "RM " + tollfare.decTollAmt4;
                                break;
                            case 4:
                                mTxtSum.Text = "RM " + tollfare.decTollAmt5;
                                break;
                        }
                        mLnlResult.Visibility = ViewStates.Visible;
                    }
                };
                if (mRdbTo.Checked)
                {
                    thread.getTollFare(mTollPlaza.idTollPlaza, itemTo);
                }
                else
                {
                    thread.getTollFare(itemTo, mTollPlaza.idTollPlaza);
                }
                
            }
        }

        public void Share()
        {
            string uri = "";
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraSubject, mTollPlaza.strName);
            intent.PutExtra(Intent.ExtraTitle, mTollPlaza.strName);
            if (mSpnTollPlaza.Visibility == ViewStates.Visible)
            {
                intent.PutExtra(Intent.ExtraText, "From :"  + mTollPlaza.strName + "\r\nTo : " + adapterTollPlaza.GetTollPlaza(mSpnTollPlaza.SelectedItemPosition).strName + "\r\nAmmount: " + mTxtSum.Text);
            }
            else
            {
                intent.PutExtra(Intent.ExtraText, "From :" + mTollPlaza.strName +  "\r\nAmmount: " + mTxtSum.Text);
            }
            StartActivity(intent);
        }
    }
}