using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class PortalMenuDetail
    {
        public Guid? idPortalMenu { get; set; }
        public string strName { get; set; }
        public string PortalContentName { get; set; }
        public int idPortalContent { get; set; }
        public string strTitle { get; set; }
        public string strBody { get; set; }
        public string strCategory { get; set; }
        public string strDescription { get; set; }
    }
}
