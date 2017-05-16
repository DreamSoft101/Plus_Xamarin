using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class ServiceResult
    {
        public int StatusCode { get; set; }
        public string Mess { get; set; }
        public Object Data { get; set; }
    }
}
