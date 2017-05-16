using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models.Database
{
    public class Document
    {
        public Document()
        {

        }

        [PrimaryKey]
        public Guid ID { get; set; }

        public string FileName { get; set; }
    }
}
