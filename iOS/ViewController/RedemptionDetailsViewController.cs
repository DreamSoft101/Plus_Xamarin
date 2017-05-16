using Foundation;
using Loyalty.Models.ServiceOutput;
using Loyalty.Threads;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class RedemptionDetailsViewController : UIViewController
    {
        public static RedemptionHistory RedeemHistory { get; set; }
        public RedemptionDetailsViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Loyalty.Threads.RedemptionThreads thread = new RedemptionThreads();
            thread.OnResult += (ServiceResult resultEntity) =>
            {
                if (resultEntity.StatusCode == 1)
                {
                    MGetRedemptionDetails data = resultEntity.Data as MGetRedemptionDetails;
                    lbPoint.Text = data.PointsRedeem.ToString();
                    lbRedeemDate.Text = data.RedeemDate.ToShortTimeString();
                    lbRefNo.Text = data.RedemptionNumber;
                    lbStatus.Text = data.RedemptionStatus == 0 ? "Pending" : "Closed";

                    //tblRedemptionDetail.Source = new Loy_ListRedemptionDetailResource(this, data.RedeemProduct.ToArray());
                }
                else
                {
                }

            };
            thread.RedemptionDetail(RedeemHistory.MemberRedemptionID);
        }
    }
}