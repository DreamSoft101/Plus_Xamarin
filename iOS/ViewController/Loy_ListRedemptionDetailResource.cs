using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace PLUS.iOS.ViewController
{
    public class Loy_ListRedemptionDetailResource : UITableViewSource
    {
        
        Loyalty.Models.ServiceOutput.MRedemptionDetail[] mListItems;
        PLUS.iOS.BaseViewController mrootView;
        public Loy_ListRedemptionDetailResource(PLUS.iOS.BaseViewController mroot, Loyalty.Models.ServiceOutput.MRedemptionDetail[] listItems)
        {
            this.mrootView = mroot;
            this.mListItems = listItems;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var rawProduct = mListItems[indexPath.Row];
            if (rawProduct.intProductType == 5)
            {
                UITableViewCell cell = tableView.DequeueReusableCell("cvRedemptionDetailProduct", indexPath);
                var product = (Loyalty.Models.ServiceOutput.MRedemptionDetailVoucher)rawProduct;
                //productName
                UILabel prodNameLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(1);
                prodNameLabel.Text = product.ProductName;

                //status
                UILabel sttLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(2);
                string status = "";
                if (product.Status == 0)
                {
                    status = "Pending";
                }
                else if (product.Status == 1)
                {
                    status = "Closed";
                }
                else if (product.Status == 2)
                {
                    status = "Canceled";
                }
                sttLabel.Text = status;

                //evoucher No
                UILabel evoucherNoLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(3);
                evoucherNoLabel.Text = product.strVoucherNo;

                //amount
                UILabel amountLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(4);
                amountLabel.Text = product.RedeemPoints.ToString();

                return cell;
            }else
            {
                UITableViewCell cell = tableView.DequeueReusableCell("cvRedemptionDetailProductPhysic", indexPath);
                var product = (Loyalty.Models.ServiceOutput.MRedemptionDetailPhysic)rawProduct;
                //productName
                UILabel prodNameLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(1);
                prodNameLabel.Text = product.ProductName;

                //status
                UILabel sttLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(2);
                string status = "";
                if (product.Status == 0)
                {
                    status = "Pending";
                }
                else if (product.Status == 1)
                {
                    status = "Closed";
                }
                else if (product.Status == 2)
                {
                    status = "Canceled";
                }
                sttLabel.Text = status;

                //evoucher No
                UILabel amountLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(3);
                amountLabel.Text = product.RedeemAmount.ToString();

                //amount
                UILabel pointLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(4);
                pointLabel.Text = product.RedeemPoints.ToString();

                return cell;
            }
            
        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            /*RedemptionDetailsViewController detailController = mrootView.Storyboard.InstantiateViewController("RedemptionDetailsViewController") as RedemptionDetailsViewController;
            mrootView.NavigationController.PushViewController(detailController, true);*/
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return mListItems.Length;
        }
    }
}
