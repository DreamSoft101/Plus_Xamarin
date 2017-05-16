using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblTollPlaza
    {
        public TblTollPlaza()
        {

        }
        [PrimaryKey]
        public string idTollPlaza { get; set; }
        public string strType { get; set; }
        public string strName { get; set; }
        public string idHighway { get; set; }
        public string strSection { get; set; }
        public string strReloadTimeFrom { get; set; }
        public string strReloadTimeTo { get; set; }
        public string strDirection { get; set; }
        
        public int intDirection { get; set; }
        public double decLocation { get; set; }
        public double decLong { get; set; }
        public double decLat { get; set; }
        public int intSort { get; set; }
        public string strExit { get; set; }
        public int intStatus { get; set; }
        public string idEntity { get; set; }
        public string strPicture { get; set; }

        private List<TollPlazaCCTV> LstCCTV { get; set; }

        public void SetLstCCTV(List<TollPlazaCCTV> cctvs)
        {
            this.LstCCTV = cctvs;
        }

        public List<TollPlazaCCTV> getCCTV()
        {
            return this.LstCCTV == null ? new List<TollPlazaCCTV>() : LstCCTV;
        }
    }
}
