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
    [Register ("ListAccountController")]
    partial class ListAccountController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView lstItems { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView prbLoading { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lstItems != null) {
                lstItems.Dispose ();
                lstItems = null;
            }

            if (prbLoading != null) {
                prbLoading.Dispose ();
                prbLoading = null;
            }
        }
    }
}