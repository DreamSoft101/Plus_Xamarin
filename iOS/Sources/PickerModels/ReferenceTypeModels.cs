using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace PLUS.iOS.Sources.PickerModels
{
    public class ReferenceTypeModels : UIPickerViewModel
    {
        private int selectedIndex;
        public class ReferenceType
        {
            public int Type;
            public string Name;
        }

        private List<ReferenceType> mLstItem;
        public ReferenceTypeModels()
        {
            this.mLstItem = new List<ReferenceType>();
            this.mLstItem.Add(new ReferenceType() { Type = 1, Name = "IC No" });
            this.mLstItem.Add(new ReferenceType() { Type = 2, Name = "Old IC No" });
            this.mLstItem.Add(new ReferenceType() { Type = 3, Name = "Passpord" });
            this.mLstItem.Add(new ReferenceType() { Type = 4, Name = "Police Card" });
            this.mLstItem.Add(new ReferenceType() { Type = 5, Name = "Army Card" });
            this.mLstItem.Add(new ReferenceType() { Type = 6, Name = "Others" });

        }

        public ReferenceType SelectedItem
        {
            get
            {
                return mLstItem[selectedIndex];
            }
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            selectedIndex = (int)row;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return this.mLstItem.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return this.mLstItem[Convert.ToInt16(row.ToString())].Name;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {

            return 1;
        }
    }
}
