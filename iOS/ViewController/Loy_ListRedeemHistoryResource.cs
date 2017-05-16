using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace PLUS.iOS.ViewController
{
    public class Loy_ListRedeemHistoryResource : UITableViewSource
    {
        
        Loyalty.Models.ServiceOutput.RedemptionHistory[] mListItems;
        PLUS.iOS.BaseViewController mrootView;
        public Loy_ListRedeemHistoryResource(PLUS.iOS.BaseViewController mroot, Loyalty.Models.ServiceOutput.RedemptionHistory[] listItems)
        {
            this.mrootView = mroot;
            this.mListItems = listItems;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("redeemHistorycv", indexPath);
            UILabel dateLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(1);
            dateLabel.Text = mListItems[indexPath.Row].RedeemDate.ToShortDateString();

            var test = cell.ContentView.ViewWithTag(2);
            UILabel noLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(2);
            noLabel.Text = mListItems[indexPath.Row].strRedemptionNo;

            UILabel statusLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(3);
            statusLabel.Text = mListItems[indexPath.Row].RedemptionStatus==0? "Pending":"Closed";
            return cell;
        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var redemption = mListItems[indexPath.Row];
            RedemptionDetailsViewController.RedeemHistory = redemption;
            RedemptionDetailsViewController detailController = mrootView.Storyboard.InstantiateViewController("RedemptionDetailsViewController") as RedemptionDetailsViewController;
            mrootView.NavigationController.PushViewController(detailController, true);
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return mListItems.Length;
        }
    }
}
