using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXPRESSO.Models.Database;
using static EXPRESSO.Models.EnumType;

namespace EXPRESSO.Models
{
    public class Journey
    {
        public TblRouteDetails mTblRouteDetail;
        public string mStrParentName;
        public FacilitiesType mParentType;
        public string mStrParentTypeName;
    }
}
