using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class Emagazine
    {
        public string FileName { get; set; }
        public string Subtitle { get; set; }
        public string Title { get; set; }
        public long DownloaID { get; set; }
        public DateTime LastDown { get; set; }
    }
}
