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
    [Register ("RedemptionDetailsViewController")]
    partial class RedemptionDetailsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbPoint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbRedeemDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbRefNo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblRedemptionDetail { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lbPoint != null) {
                lbPoint.Dispose ();
                lbPoint = null;
            }

            if (lbRedeemDate != null) {
                lbRedeemDate.Dispose ();
                lbRedeemDate = null;
            }

            if (lbRefNo != null) {
                lbRefNo.Dispose ();
                lbRefNo = null;
            }

            if (lbStatus != null) {
                lbStatus.Dispose ();
                lbStatus = null;
            }

            if (tblRedemptionDetail != null) {
                tblRedemptionDetail.Dispose ();
                tblRedemptionDetail = null;
            }
        }
    }
}