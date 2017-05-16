using Foundation;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class LeftMenuViewController : UIViewController
    {
        public string[] arrMenuItems;
        public string[] arrMenuIcons;
        public RootViewController rootVC;
        public LeftMenuViewController(IntPtr handle) : base(handle)
        {

        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            arrMenuItems = new string[] { "My Account", "Add card", "My Vouchers", "Promotions", "Redemption", "Redemption Cart", "About PLUSMiles", "My Report" };
            arrMenuIcons = new string[] { "My Account", "Add card", "My Vouchers", "Promotions", "Redemption", "Redemption Cart", "About PLUSMiles", "My Report" };

            tableview.DataSource = new LeftMenuTableViewDataSource(this);
            tableview.Delegate = new LeftMenuTableViewDelegate(this);

            imgAvatar.Layer.CornerRadius = imgAvatar.Frame.Width / 2;
            imgAvatar.Layer.MasksToBounds = true;
        }

    }

    public class LeftMenuTableViewDataSource : UITableViewDataSource
    {
        LeftMenuViewController rootVC;
        public LeftMenuTableViewDataSource(LeftMenuViewController vc)
        {
            rootVC = vc;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("Cell", indexPath);            //string item = TableItems[indexPath.Row];

            UILabel lblText = (UIKit.UILabel)cell.ContentView.ViewWithTag(2);
            lblText.Text = rootVC.arrMenuItems[indexPath.Row];

            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return rootVC.arrMenuItems.Length;
        }
    }

    public class LeftMenuTableViewDelegate : UITableViewDelegate
    {
        LeftMenuViewController rootVC;
        public LeftMenuTableViewDelegate(LeftMenuViewController vc)
        {
            rootVC = vc;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected(tableView, indexPath);
            rootVC.rootVC.selectLeftMenuItem(indexPath.Row);
        }

    }
}