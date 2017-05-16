using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MAnnouncement
    {
        private Guid mGuidIdAnnouncement;
        public Guid idAnnouncement
        {
            get { return this.mGuidIdAnnouncement; }
            set { this.mGuidIdAnnouncement = value; }
        }
        private string mStrTitle;
        public string Title
        {
            get { return this.mStrTitle; }
            set { this.mStrTitle = value; }
        }
        private string mStrDescription;
        public string Description
        {
            get { return this.mStrDescription; }
            set { this.mStrDescription = value; }
        }

        private string mStrImageURL;
        public string ImageURL
        {
            get { return this.mStrImageURL; }
            set { this.mStrImageURL = value; }
        }

        private string mStrThumbnailURL;
        public string ThumbnailURL
        {
            get { return this.mStrThumbnailURL; }
            set { this.mStrThumbnailURL = value; }
        }

        private MAttachment[] mArrAttachments;
        public MAttachment[] Attachments
        {
            get { return this.mArrAttachments; }
            set { this.mArrAttachments = value; }
        }

        private byte mIntStatus;
        public byte Status
        {
            get { return this.mIntStatus; }
            set { this.mIntStatus = value; }
        }

        private string mStrCommencementDate;
        public string CommencementDate
        {
            get { return this.mStrCommencementDate; }
            set { this.mStrCommencementDate = value; }
        }
    }
}
