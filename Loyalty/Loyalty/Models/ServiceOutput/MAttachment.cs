using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MAttachment
    {
        private string mStrAttachmentName;
        public string AttachmentName
        {
            get { return this.mStrAttachmentName; }
            set { this.mStrAttachmentName = value; }
        }
        private string mStrAttachmentURL;
        public string AttachmentURL
        {
            get { return this.mStrAttachmentURL; }
            set { this.mStrAttachmentURL = value; }
        }
    }
}
