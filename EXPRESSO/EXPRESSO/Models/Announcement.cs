using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class Announcement
    {
        public string idAnnouncement { get; set; }
        public string strTitle { get; set; }
        public string strDescription { get; set; }
        public List<string> lstImages { get; set; }
        public DateTime dtStart { get; set; }

        public int largerPhotoW { get; set; }
        public int largerPhotoH { get; set; }
        public string largerPhotoFit { get; set; }
        public int thumbPhotoW { get; set; }
        public int thumbPhotoH { get; set; }
        public string thumbPhotoFit { get; set; }

        public List<string> images;

    }
}
