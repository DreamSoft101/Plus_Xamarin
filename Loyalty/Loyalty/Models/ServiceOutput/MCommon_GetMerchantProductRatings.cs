using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MCommon_GetMerchantProductRatings : ResponseBase
    {
        public List<Merchant_Rating_Paging_Info> RatingInfos;
        public int TotalPage { get; set; }

        public Merchant_Rating_Info CurrentUserRating { get; set; }
    }
}