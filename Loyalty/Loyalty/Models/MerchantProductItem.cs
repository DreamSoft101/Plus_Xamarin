using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models
{
    public class MerchantProductItem
    {
        public enum Size
        {
            Size_1x1 = 1,
            Size_1x2 = 2,
            Size_2x1 = 3,
            Size_2x2 = 4
        }
        //public Size mSize;
        public int NeedItem = -1;
        public List<BaseItem> LstItem;


    }
}
