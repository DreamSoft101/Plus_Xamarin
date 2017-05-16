using Loyalty.Threads;
using Foundation;
using System;
using UIKit;
using Loyalty.Models.ServiceOutput;
using PLUS.iOS.ViewController;

namespace PLUS.iOS
{
    public partial class RedeemHistoryController : BaseViewController
    {
        
        public RedeemHistoryController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            UserThreads thread = new UserThreads();
            thread.OnResult += (ServiceResult resultEntity) =>
            {
                if (resultEntity.StatusCode == 1)
                {
                    MGetRedemptionHistory data = resultEntity.Data as MGetRedemptionHistory;
                    redeemHistoryTable.Source = new Loy_ListRedeemHistoryResource(this, data.histories.ToArray());
                    redeemHistoryTable.ReloadData();
                }
                else
                {
                }

            };
            thread.GetMemberRdemptionHistory();
        }
    }
}