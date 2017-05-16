using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Models
{
    public class BaseItem
    {
        public  enum TagName
        {
            RSA_IsCCTV = 2,
            RSA_IsSign = 1,
            RSA_IsFood = 3,
            Facility_BrandID = 4,
            Facility_BrandName = 5,
            Facility_TypeName = 6,
            Facility_Brand = 7,
            Facility_Type = 8,
            IsFavorite = 99,
            Position = 98,
            Pertrol_BrandIMG = 9,
            Favorite_Startus = 10,
            IdHighway = 10,
            StrICon = 11,
        }
        public object Item;

        public System.Collections.Generic.Dictionary<int,object> Tag = new Dictionary<int, object>();

        public void setTag(TagName tag,bool value)
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

        public object getTag(TagName tag)
        {
            object value;
            bool result = Tag.TryGetValue((int)tag,out value);
            if (value == null)
            {
                if (tag == TagName.IsFavorite)
                    return false;
                if (tag == TagName.RSA_IsCCTV || tag == TagName.RSA_IsFood || tag == TagName.RSA_IsSign)
                    return false;
                if (tag == TagName.Facility_Brand || tag == TagName.Facility_BrandID || tag == TagName.Facility_BrandName || tag == TagName.Facility_Type || tag == TagName.Facility_TypeName)
                    return null;
            }
            return value;
        }
    }
}
