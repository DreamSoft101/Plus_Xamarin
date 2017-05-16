using Foundation;
using Loyalty.Models.ServiceOutput;
using Loyalty.Threads;
using Loyalty.Utils;
using Newtonsoft.Json;
using PLUS.iOS.Sources.PickerModels;
using System;
using System.Collections.Generic;
using ToastIOS;
using UIKit;

namespace PLUS.iOS
{
    public partial class LinkMasterAccountController : BaseViewController
    {


        ReferenceTypeModels model;


        public LinkMasterAccountController (IntPtr handle) : base (handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            model = new ReferenceTypeModels();
            spnReferenceType.Model = model;
            //PickerModel
        }

        partial void btnCancel_Click(UIButton sender)
        {
            //var root = NavigationController.TopViewController;
            //this.DismissViewController(true, null);
            UIStoryboard board = UIStoryboard.FromName("Main", null);
            var home = board.InstantiateViewController("HomeViewController");
            this.NavigationController.PushViewController(home, true);
        }

        partial void btnConfirm_Click(UIButton sender)
        {
            string refNo = this.txtReferenceNo.Text;
            int refType = model.SelectedItem.Type;//
            UserThreads thread = new UserThreads();
            this.showLoading();
            thread.OnResult += (ServiceResult result) =>
            {
                this.hideLoading();
                if (result.StatusCode == 1)
                {


                    UserThreads threadaccount = new UserThreads();
                    threadaccount.OnResult += (ServiceResult resultmember) =>
                    {
                        if (resultmember.StatusCode == 1)
                        {
                            MGetMemberProfile profile = resultmember.Data as MGetMemberProfile;

                            Cons.mMemberCredentials.MemberProfile = profile;

                            string data = JsonConvert.SerializeObject(Cons.mMemberCredentials);
                           // setCacheString(MyAuth, data);
                            //Finish();
                        }
                        else
                        {
                           // mLnlLoading.Visibility = ViewStates.Gone;
                            Toast.MakeText(resultmember.Mess).Show();
                        }
                    };
                    this.showLoading();
                    threadaccount.GetMemberProfile(Cons.mMemberCredentials.LoginParams.authSessionId, Cons.mMemberCredentials.LoginParams.strLoginId, false);


                }
                else
                {
                    Toast.MakeText(result.Mess);
                }
            };
            thread.LinkToMasterAccount(refNo, refType);
        }
    }
}