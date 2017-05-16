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
    [Register ("LeftMenuViewController")]
    partial class LeftMenuViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgAvatar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tableview { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgAvatar != null) {
                imgAvatar.Dispose ();
                imgAvatar = null;
            }

            if (tableview != null) {
                tableview.Dispose ();
                tableview = null;
            }
        }
    }
}