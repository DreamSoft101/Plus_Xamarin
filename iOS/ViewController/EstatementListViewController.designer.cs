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
    [Register ("EstatementListViewController")]
    partial class EstatementListViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbCardNo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblEstatement { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lbCardNo != null) {
                lbCardNo.Dispose ();
                lbCardNo = null;
            }

            if (tblEstatement != null) {
                tblEstatement.Dispose ();
                tblEstatement = null;
            }
        }
    }
}