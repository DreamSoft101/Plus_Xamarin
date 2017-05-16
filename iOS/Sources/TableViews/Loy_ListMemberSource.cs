using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;

namespace PLUS.iOS.Sources.TableViews
{
    public class Loy_ListMemberSource : UITableViewSource
    {
        BaseViewController mRootView;
        Loyalty.Models.ServiceOutput.MIMX_AccountDetails[] mLstItems;

        //itemMember
        public Loy_ListMemberSource(BaseViewController mroot, Loyalty.Models.ServiceOutput.MIMX_AccountDetails[] lstItems)
        {
            this.mRootView = mroot;
            this.mLstItems = lstItems;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return mLstItems.Length;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("itemMember", indexPath);            //string item = TableItems[indexPath.Row];

            //if (cell == null)
            //{
            //    cell = new UITableViewCell("itemMember", this);
            //}

            UILabel lbeCardNo = (UIKit.UILabel)cell.ContentView.ViewWithTag(1);
            lbeCardNo.Text = mLstItems[indexPath.Row].strCardNumber;

            UILabel lbeStatus = (UIKit.UILabel)cell.ContentView.ViewWithTag(2);
            lbeStatus.Text = mLstItems[indexPath.Row].intStatus == 1 ? "Active" : "Inactive" ;

            UILabel lbeExpiryDate = (UIKit.UILabel)cell.ContentView.ViewWithTag(3);
            lbeExpiryDate.Text = mLstItems[indexPath.Row].dteExpiryDate;

            UILabel lbeOpenPoints = (UIKit.UILabel)cell.ContentView.ViewWithTag(4);
            lbeOpenPoints.Text = mLstItems[indexPath.Row].Summary.OpenBalancePoints + "";

            UILabel lbeEarnPoints = (UIKit.UILabel)cell.ContentView.ViewWithTag(5);
            lbeEarnPoints.Text = mLstItems[indexPath.Row].Summary.PointsEarned + "";

            UILabel lbeRedeemPoints = (UIKit.UILabel)cell.ContentView.ViewWithTag(6);
            lbeRedeemPoints.Text = mLstItems[indexPath.Row].Summary.PointsRedeem + "";

            UILabel lbeTransferPoint = (UIKit.UILabel)cell.ContentView.ViewWithTag(7);
            lbeTransferPoint.Text = mLstItems[indexPath.Row].Summary.PointsTransfer + "";

            UILabel lbeExpired = (UIKit.UILabel)cell.ContentView.ViewWithTag(8);
            lbeExpired.Text = mLstItems[indexPath.Row].Summary.PointsExpired + "";

            UILabel lbeTotalPoints = (UIKit.UILabel)cell.ContentView.ViewWithTag(9);
            lbeTotalPoints.Text = (mLstItems[indexPath.Row].Summary.OpenBalancePoints - mLstItems[indexPath.Row].Summary.PointsRedeem - mLstItems[indexPath.Row].Summary.PointsExpired + mLstItems[indexPath.Row].Summary.PointsEarned).ToString(); 

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 240;
        }

    }
}