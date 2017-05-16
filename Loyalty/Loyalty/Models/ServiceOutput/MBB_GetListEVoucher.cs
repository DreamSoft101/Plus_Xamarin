using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loyalty.Models.ServiceOutput
{
    public class MBB_GetListEVoucher : ResponseBase
    {
        public MBB_GetListEVoucher()
        {

        }
        public MBB_GetListEVoucher(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
        public List<MemberVoucherInfo> ListVoucher { get; set; }
        public int Total { get; set; }
    }
    public class MemberVoucherInfo
    {
        public long idVoucher { get; set; }
        public Nullable<System.Guid> idMember { get; set; }
        public Nullable<System.Guid> RedemptionProductId { get; set; }
        public string strVoucherNo { get; set; }
        public Nullable<byte> intChannel { get; set; }
        public Nullable<System.DateTime> dtRedeem { get; set; }
        public Nullable<System.DateTime> dtExpiry { get; set; }
        public Nullable<System.DateTime> dtUse { get; set; }
        public Nullable<System.DateTime> dtDismiss { get; set; }
        public string C2DBarcodeMessage { get; set; }
        public string strExternalRefNo { get; set; }
        public Nullable<byte> intStatus { get; set; }
        public Nullable<System.DateTime> dtCreateDate { get; set; }
        public Nullable<System.DateTime> dtLastUpdateDate { get; set; }
        public Nullable<System.Guid> RedemptionID { get; set; }
        public Nullable<System.Guid> idRedemptionProductDetail { get; set; }
        public Nullable<int> intPoint { get; set; }

        public string strImgPicture { get; set; }
        public string strName { get; set; }
    }
}
