using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MGetFAQ : ResponseBase
    {
        private MFAQ[] mArrFAQs;
        public MFAQ[] FAQ
        {
            get { return this.mArrFAQs; }
            set { this.mArrFAQs = value; }
        }

        public MGetFAQ()
        {

        }
        public MGetFAQ(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }

    public class MFAQ : ResponseBase
    {
        private Guid mGuidIdFAQ;
        public Guid idFAQ
        {
            get { return this.mGuidIdFAQ; }
            set { this.mGuidIdFAQ = value; }
        }
        private string mStrTitle;
        public string Question
        {
            get { return this.mStrTitle; }
            set { this.mStrTitle = value; }
        }
        private string mStrDescription;
        public string Answer
        {
            get { return this.mStrDescription; }
            set { this.mStrDescription = value; }
        }
        private string mStrImageURL;

        private string mStrSequenceofQuestion;
        public string SequenceOfQuestion
        {
            get { return this.mStrSequenceofQuestion; }
            set { this.mStrSequenceofQuestion = value; }
        }

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


    }
}