using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class GetFeedback
    {
        public int idFeedback { get; set; }
        public string strCreatedBy { get; set; }
        public string strContent { get; set; }
        public string dtCreatedDate { get; set; }
        public List<string> images { get; set; }
    }
}
