using CoreGraphics;
using Foundation;
using SidebarNavigation;
using System;
using UIKit;

namespace PLUS.iOS
{
	public partial class HomeViewController : BaseViewController
    {

        UIStoryboard board = UIStoryboard.FromName("Main", null);
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.Title = "Home";
            this.renderUI();
        }

        private void renderUI() {
            var width = (nfloat)UIScreen.MainScreen.Bounds.Width;
            var height = (nfloat)UIScreen.MainScreen.Bounds.Height;
            var statusBarHeight = this.NavigationController.NavigationBar.Bounds.Height + UIApplication.SharedApplication.StatusBarFrame.Height;

            // 2 column, 3 row
            var iconW = (width - (40 * 3)) / 2;
            var iconH = (height - statusBarHeight - (40 * 5)) / 4;
            
            if(iconW < iconH)
            {
                iconH = iconW;
            }
            else
            {
                iconW = iconH;
            }
            var spacingW = (width - (iconW * 2)) / 3;
            var spacingH = (height - statusBarHeight - (iconH * 4)) / 5;

            btnTollPlaza.Frame = new CGRect(spacingW, statusBarHeight+ spacingH, iconW, iconH);
            btnRSALayby.Frame = new CGRect(spacingW *2+ iconW, statusBarHeight + spacingH, iconW, iconH);

            btnLiveTraffic.Frame = new CGRect(spacingW, statusBarHeight + spacingH * 2 + iconH, iconW, iconH);
            btnFacilities.Frame = new CGRect(spacingW * 2 + iconW, statusBarHeight + spacingH * 2 + iconH , iconW, iconH);

            btnLiveFeed.Frame = new CGRect(spacingW, statusBarHeight + spacingH * 3 + iconH*2, iconW, iconH);
            btnAnnoucement.Frame = new CGRect(spacingW * 2 + iconW, statusBarHeight + spacingH * 3 + iconH*2, iconW, iconH);

            btnFavourite.Frame = new CGRect(spacingW, statusBarHeight + spacingH * 4 + iconH * 3, iconW, iconH);
            btnPlusMiles.Frame = new CGRect(spacingW * 2 + iconW, statusBarHeight + spacingH * 4 + iconH*3, iconW, iconH);

            UIImage backgroundImage = null;
            if (UIScreen.MainScreen.Bounds.Height == 568)
                backgroundImage = UIImage.FromFile("images/Default-568h@2x.jpg");
            else if (UIScreen.MainScreen.Bounds.Height == 667)
                backgroundImage = UIImage.FromFile("images/Default-667h@2x.jpg");
            else
                backgroundImage = UIImage.FromBundle("BackgroundImage");

            View.BackgroundColor = UIColor.FromPatternImage(backgroundImage);

        }

        partial void btnPlusMilesClicked(UIButton sender)
		{
			throw new NotImplementedException();
		}

		partial void btnFavouriteClicked(UIButton sender)
		{
			FavouriteViewController vc = (FavouriteViewController)board.InstantiateViewController("FavouriteViewController");
			this.NavigationController.PushViewController(vc, true);
		}

		partial void btnAnnoucementClicked(UIButton sender)
		{
			AnnoucementViewController vc = (AnnoucementViewController)board.InstantiateViewController("AnnoucementViewController");
			this.NavigationController.PushViewController(vc, true);
		}

		partial void btnLiveFeedClicked(UIButton sender)
		{
			LiveFeedViewController vc = (LiveFeedViewController)board.InstantiateViewController("LiveFeedViewController");
			this.NavigationController.PushViewController(vc, true);
		}

		partial void btnFacilitiesClicked(UIButton sender)
		{
			FacilitiesViewController vc = (FacilitiesViewController)board.InstantiateViewController("FacilitiesViewController");
			this.NavigationController.PushViewController(vc, true);
		}

		partial void btnLiveTrafficClicked(UIButton sender)
		{
			LiveTrafficViewController vc = (LiveTrafficViewController)board.InstantiateViewController("LiveTrafficViewController");
			this.NavigationController.PushViewController(vc, true);
		}

		partial void btnTollPlazaClicked(UIButton sender)
		{
			TollPlazaViewController vc = (TollPlazaViewController)board.InstantiateViewController("TollPlazaViewController");
			this.NavigationController.PushViewController(vc, true);
		}

		partial void btnRSAClicked(UIButton sender)
		{
			RSAViewController vc = (RSAViewController)board.InstantiateViewController("RSAViewController");
			this.NavigationController.PushViewController(vc, true);
		}

		public HomeViewController(IntPtr handle) : base(handle)
		{
			
		}

	}
}