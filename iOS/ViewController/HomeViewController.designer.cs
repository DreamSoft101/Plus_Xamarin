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
    [Register ("HomeViewController")]
    partial class HomeViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAnnoucement { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnFacilities { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnFavourite { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLiveFeed { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLiveTraffic { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPlusMiles { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnRSALayby { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnTollPlaza { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView homeView { get; set; }

        [Action ("btnAnnoucementClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnAnnoucementClicked (UIKit.UIButton sender);

        [Action ("btnFacilitiesClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnFacilitiesClicked (UIKit.UIButton sender);

        [Action ("btnFavouriteClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnFavouriteClicked (UIKit.UIButton sender);

        [Action ("btnLiveFeedClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnLiveFeedClicked (UIKit.UIButton sender);

        [Action ("btnLiveTrafficClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnLiveTrafficClicked (UIKit.UIButton sender);

        [Action ("btnPlusMilesClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnPlusMilesClicked (UIKit.UIButton sender);

        [Action ("btnRSAClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnRSAClicked (UIKit.UIButton sender);

        [Action ("btnTollPlazaClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnTollPlazaClicked (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnAnnoucement != null) {
                btnAnnoucement.Dispose ();
                btnAnnoucement = null;
            }

            if (btnFacilities != null) {
                btnFacilities.Dispose ();
                btnFacilities = null;
            }

            if (btnFavourite != null) {
                btnFavourite.Dispose ();
                btnFavourite = null;
            }

            if (btnLiveFeed != null) {
                btnLiveFeed.Dispose ();
                btnLiveFeed = null;
            }

            if (btnLiveTraffic != null) {
                btnLiveTraffic.Dispose ();
                btnLiveTraffic = null;
            }

            if (btnPlusMiles != null) {
                btnPlusMiles.Dispose ();
                btnPlusMiles = null;
            }

            if (btnRSALayby != null) {
                btnRSALayby.Dispose ();
                btnRSALayby = null;
            }

            if (btnTollPlaza != null) {
                btnTollPlaza.Dispose ();
                btnTollPlaza = null;
            }

            if (homeView != null) {
                homeView.Dispose ();
                homeView = null;
            }
        }
    }
}