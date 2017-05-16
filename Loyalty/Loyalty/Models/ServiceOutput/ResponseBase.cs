using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class ResponseBase
    {
        private string mStrResponseMessage;
        private int mIntStatusCode;
        private string minTransactionRefNo;
        private Guid minAPIid;
        private string minHash;
        public int StatusCode
        {
            get
            {
                return mIntStatusCode;
            }
            set
            {
                this.mIntStatusCode = value;
            }
        }
        public string inTransactionRefNo
        {
            get
            {
                return minTransactionRefNo;
            }
            set
            {
                minTransactionRefNo = value;
            }
        }
        public Guid inAPIid
        {
            get
            {
                return minAPIid;
            }
            set
            {
                minAPIid = value;
            }
        }
        public string inHash
        {
            get
            {
                return minHash;
            }
            set
            {
                minHash = value;
            }
        }
        public string ResponseMessage
        {
            get
            {
                return mStrResponseMessage;
            }
            set
            {
                this.mStrResponseMessage = value;
            }
        }

        public void CopyFromBase(ResponseBase item)
        {

        }
    }
}