using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class CheckMasterAccountExistResult : ResponseBase
    {
        public long idMasterAccount { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int RefType { get; set; }
        public string RefNo { get; set; }

        public CheckMasterAccountExistResult()
        {

        }

        public CheckMasterAccountExistResult(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}