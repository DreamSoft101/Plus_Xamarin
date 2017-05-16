using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MGetRewardAccountSummary : ResponseBase
    {
        private long mIntMasterAccount;
        public long idMasterAccount
        {
            get { return this.mIntMasterAccount; }
            set { this.mIntMasterAccount = value; }
        }
        private string mStrCardNumber;
        public string CardNumber
        {
            get { return this.mStrCardNumber; }
            set { this.mStrCardNumber = value; }
        }
        private DateTime mDteStartBalanceDate;
        public DateTime StartBalanceDate
        {
            get { return this.mDteStartBalanceDate; }
            set { this.mDteStartBalanceDate = value; }
        }
        private int mIntOpenBalancePoints;
        public int OpenBalancePoints
        {
            get { return this.mIntOpenBalancePoints; }
            set { this.mIntOpenBalancePoints = value; }
        }
        private int mIntEndBalancePoints;
        public int EndBalancePoints
        {
            get { return this.mIntEndBalancePoints; }
            set { this.mIntEndBalancePoints = value; }
        }
        private decimal mDecOpenBalanceRebate;
        public decimal OpenBalanceRebate
        {
            get { return this.mDecOpenBalanceRebate; }
            set { this.mDecOpenBalanceRebate = value; }
        }
        private decimal mDecEndBalanceRebate;
        public decimal EndBalanceRebate
        {
            get { return this.mDecEndBalanceRebate; }
            set { this.mDecEndBalanceRebate = value; }
        }

        private MRewardDetails[] mArrRewardDetails;
        public MRewardDetails[] RewardDetails
        {
            get { return this.mArrRewardDetails; }
            set { this.mArrRewardDetails = value; }
        }

        public MGetRewardAccountSummary()
        {

        }
        public MGetRewardAccountSummary(ResponseBase p)
        {
            this.CopyFromBase(p);
        }

        public int? PointsEarned { get; set; }
        public decimal? RebateEarned { get; set; }
        public int? PointsRedeem { get; set; }
        public decimal? RebateRedeem { get; set; }
        public int? PointsTransfer { get; set; }
        public decimal? RebateTransfer { get; set; }
        public int? PointsAdjusted { get; set; }
        public decimal? RebateAdjusted { get; set; }
        public int? PointsExpired { get; set; }
    }
}