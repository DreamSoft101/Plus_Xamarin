using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models.Database
{
    public class TblMedia
    {
        [PrimaryKey]
        public string idMedia { get; set; }
        public string idopscomm { get; set; }
        public string strURL { get; set; }
        public string mStrComment { get; set; }
        public int intPosted { get; set; } //Already post or not yet
        //public byte[] Data { get; set; }

        public string RandomName { get; set; }

        public string Path { get; set; }
    }
}
