using Foundation;
using Loyalty.Models.ServiceOutput;
using Loyalty.Threads;
using Loyalty.Utils;
using PLUS.iOS.Sources.TableViews;
using System;
using ToastIOS;
using UIKit;

namespace PLUS.iOS
{
    public partial class ListAccountController : BaseViewController
    {
        // private UITableView mLstItems;
        public ListAccountController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "ACCOUNT SUMMARY";
            LoadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            lstItems.EstimatedRowHeight = 225;
            lstItems.RowHeight = UITableView.AutomaticDimension;
        }

        private void LoadData()
        {
            prbLoading.Hidden = false;
            lstItems.Hidden = true;
            this.showLoading();
            UserThreads thread = new UserThreads();
            thread.OnResult += (ServiceResult result) =>
            {
                this.hideLoading();
                if (result.StatusCode == 1)
                {
                    prbLoading.Hidden = true;
                    lstItems.Hidden = false;
                    var data = result.Data as MGetMemberProfile;
                    var lstData = data.AccountDetails;
                    Loy_ListMemberSource source = new Loy_ListMemberSource(this, lstData);
                    lstItems.Source = source;
                    lstItems.EstimatedRowHeight = 225;
                    lstItems.RowHeight = UITableView.AutomaticDimension;
                    lstItems.ReloadData();

                }
                else
                {
                    Toast.MakeText(result.Mess).Show();
                }
            };
            thread.GetMemberProfile(Cons.mMemberCredentials.LoginParams.authSessionId, Cons.mMemberCredentials.LoginParams.strLoginId, true);
        }
    }
}