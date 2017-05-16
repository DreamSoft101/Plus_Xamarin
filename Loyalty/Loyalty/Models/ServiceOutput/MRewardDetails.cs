using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MRewardDetails
    {
        private Guid mGuidMemberId;
        public Guid MemberId
        {
            get { return this.mGuidMemberId; }
            set { this.mGuidMemberId = value; }
        }

        private string mStrCardNumber;
        public string CardNumber
        {
            get { return this.mStrCardNumber; }
            set { this.mStrCardNumber = value; }
        }

        private int mIntPointsEarned;
        public int PointsEarned
        {
            get { return this.mIntPointsEarned; }
            set { this.mIntPointsEarned = value; }
        }

        private decimal mDecRebateEarned;

        public decimal RebateEarned
        {
            get { return mDecRebateEarned; }
            set { this.mDecRebateEarned = value; }
        }

        private int mIntPointsRedeem;
        public int PointsRedeem
        {
            get { return this.mIntPointsRedeem; }
            set { this.mIntPointsRedeem = value; }
        }
        private decimal mDecRebateRedeem;
        public decimal RebateRedeem
        {
            get { return this.mDecRebateRedeem; }
            set { this.mDecRebateRedeem = value; }
        }

        private int mIntPointsTransfer;
        public int PointsTransfer
        {
            get { return this.mIntPointsTransfer; }
            set { this.mIntPointsTransfer = value; }
        }


        private decimal mDecRebateTransfer;
        public decimal RebateTransfer
        {
            get { return this.mDecRebateTransfer; }
            set { this.mDecRebateTransfer = value; }
        }


        private int mIntPointsAdjusted;
        public int PointsAdjusted
        {
            get { return this.mIntPointsAdjusted; }
            set { this.mIntPointsAdjusted = value; }
        }

        private decimal mDecRebateAdjusted;
        public decimal RebateAdjusted
        {
            get { return this.mDecRebateAdjusted; }
            set { this.mDecRebateAdjusted = value; }
        }

        private decimal mDecRebateExpired;
        public decimal RebateExpired
        {
            get { return this.mDecRebateExpired; }
            set { this.mDecRebateExpired = value; }
        }

        private int mIntPointsExpired;
        public int PointsExpired
        {
            get { return this.mIntPointsExpired; }
            set { this.mIntPointsExpired = value; }
        }

        public int mIntStatus;
        public int Status
        {
            get { return this.mIntStatus; }
            set { this.mIntStatus = value; }
        }

        private int mIntPointBalance;

        public int PointBalance
        {
            get { return this.mIntPointBalance; }
            set { this.mIntPointBalance = value; }
        }

        private int mIntOpenPointBalance;

        public int OpenPointBalance
        {
            get { return this.mIntOpenPointBalance; }
            set { this.mIntOpenPointBalance = value; }
        }
        private int mMonth;
        public int Month
        {
            get { return this.mMonth; }
            set { this.mMonth = value; }
        }
        public int mYear;
        public int Year
        {
            get { return this.mYear; }
            set { this.mYear = value; }
        }
    }
}
