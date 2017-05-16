using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MGetAnnouncement : ResponseBase
    {
        private MAnnouncement[] mArrAnnouncements;
        public MAnnouncement[] Announcements
        {
            get { return this.mArrAnnouncements; }
            set { this.mArrAnnouncements = value; }
        }

        public MGetAnnouncement()
        {

        }
        public MGetAnnouncement(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
}