using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class PostFeedback
    {
        public string idObject { get; set; }
        public int intType { get; set; }
        public string strContent { get; set; }
        public string dtCreatedDate { get; set; }
        public List<string> images { get; set; }
    }
}
