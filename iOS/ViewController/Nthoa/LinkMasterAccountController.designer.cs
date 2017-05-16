// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace PLUS.iOS
{
    [Register ("LinkMasterAccountController")]
    partial class LinkMasterAccountController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCancel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnConfirm { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView spnReferenceType { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtReferenceNo { get; set; }

        [Action ("btnCancel_Click:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnCancel_Click (UIKit.UIButton sender);

        [Action ("btnConfirm_Click:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnConfirm_Click (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnCancel != null) {
                btnCancel.Dispose ();
                btnCancel = null;
            }

            if (btnConfirm != null) {
                btnConfirm.Dispose ();
                btnConfirm = null;
            }

            if (spnReferenceType != null) {
                spnReferenceType.Dispose ();
                spnReferenceType = null;
            }

            if (txtReferenceNo != null) {
                txtReferenceNo.Dispose ();
                txtReferenceNo = null;
            }
        }
    }
}