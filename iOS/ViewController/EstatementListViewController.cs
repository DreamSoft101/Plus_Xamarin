using Foundation;
using Loyalty.Models.ServiceOutput;
using Loyalty.Threads;
using PLUS.iOS.ViewController;
using System;
using UIKit;

namespace PLUS.iOS
{
    public partial class EstatementListViewController : BaseViewController
    {
        public static MIMX_AccountDetails Member { get; set; }
        public EstatementListViewController (IntPtr handle) : base (handle)
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
                    MGetStatementFileList data = resultEntity.Data as MGetStatementFileList;
                    lbCardNo.Text = Member.strCardNumber;
                    tblEstatement.Source = new Loy_ListEstatementResource(this, data.Files.ToArray());
                }
                else
                {
                }

            };
            thread.GetStatementFileList(Member.idMember);
        }
    }
}