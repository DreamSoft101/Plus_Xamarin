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
    public class ChooseTimeDialog : DialogFragment
    {
        public delegate void onChangeValue(int hour, int miunite);
        public onChangeValue OnChangeValue;

        public int hour = DateTime.Now.TimeOfDay.Hours;
        public int miunites = DateTime.Now.Minute;

        public ChooseTimeDialog(int hour, int mi)
        {
            this.hour = hour;
            this.miunites = mi;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return new TimePickerDialog(Activity, (object sender, TimePickerDialog.TimeSetEventArgs e) =>
            {
                if (OnChangeValue != null)
                {
                    OnChangeValue(e.HourOfDay, e.Minute);
                }
                //args contains new time
            }, hour, miunites, true);
        }
    }
}