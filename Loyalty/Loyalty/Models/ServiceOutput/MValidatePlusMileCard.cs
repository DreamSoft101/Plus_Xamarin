using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MValidatePlusMileCard : ResponseBase
    {

        public string Name { get; set; }
        public string Mobile { get; set; }
        public int RefType { get; set; }
        public string RefNo { get; set; }
        public long idMasterAccount { get; set; }
        public Guid MemberID { get; set; }
        public int AddCardState
        {
            get;
            set;
        }
        public MValidatePlusMileCard()
        {

        }
        public MValidatePlusMileCard(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}