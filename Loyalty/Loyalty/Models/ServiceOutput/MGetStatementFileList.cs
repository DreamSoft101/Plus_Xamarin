using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.ServiceOutput
{
    public class MGetStatementFileList : ResponseBase
    {

        public List<MStatementFile> Files { get; set; }
        public MGetStatementFileList()
        {

        }
        public MGetStatementFileList(ResponseBase p)
        {
            this.CopyFromBase(p);
        }
    }
    
}