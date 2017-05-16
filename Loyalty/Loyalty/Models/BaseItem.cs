
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Models
{
    public class BaseItem
    {
        public enum TagName
        {
            Item_Size = 1,
        }
        public object Item;

        public Dictionary<int, object> Tag = new Dictionary<int, object>();

        public void setTag(TagName tag, bool value)
        {
            if (Tag.ContainsKey((int)tag))
            {
                Tag[(int)tag] = value;
            }
            else
            {
                Tag.Add((int)tag, value);
            }
            
        }
        public void setTag(TagName tag, int value)
        {
            if (Tag.ContainsKey((int)tag))
            {
                Tag[(int)tag] = value;
            }
            else
            {
                Tag.Add((int)tag, value);
            }
        }
        public void setTag(TagName tag, string value)
        {
            if (Tag.ContainsKey((int)tag))
            {
                Tag[(int)tag] = value;
            }
            else
            {
                Tag.Add((int)tag, value);
            }
        }
        public void setTag(TagName tag, double value)
        {
            if (Tag.ContainsKey((int)tag))
            {
                Tag[(int)tag] = value;
            }
            else
            {
                Tag.Add((int)tag, value);
            }
        }
        public void setTag(TagName tag, float value)
        {
            if (Tag.ContainsKey((int)tag))
            {
                Tag[(int)tag] = value;
            }
            else
            {
                Tag.Add((int)tag, value);
            }
        }

        public void setTag(TagName tag, MerchantProductItem.Size size)
        {
            Tag.Add((int)tag, (int)size);
        }

        public object getTag(TagName tag)
        {
            object value = Tag[(int)tag];
            return value;
        }

        public MerchantProductItem.Size getSize(TagName tag)
        {

            MerchantProductItem.Size value = (MerchantProductItem.Size)(int.Parse(Tag[((int)tag)].ToString()));
            return value;

        }


    }
}
