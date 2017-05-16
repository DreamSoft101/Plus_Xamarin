using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace PLUS.iOS.ViewController
{
    public class Loy_ListEstatementResource : UITableViewSource
    {
        
        Loyalty.Models.ServiceOutput.MStatementFile[] mListItems;
        PLUS.iOS.BaseViewController mrootView;
        public Loy_ListEstatementResource(PLUS.iOS.BaseViewController mroot, Loyalty.Models.ServiceOutput.MStatementFile[] listItems)
        {
            this.mrootView = mroot;
            this.mListItems = listItems;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("cvEstatementList", indexPath);
            UILabel nameLabel = (UIKit.UILabel)cell.ContentView.ViewWithTag(1);
            nameLabel.Text = mListItems[indexPath.Row].Name;

            return cell;
        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var redemption = mListItems[indexPath.Row];
            
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return mListItems.Length;
        }
    }
}
