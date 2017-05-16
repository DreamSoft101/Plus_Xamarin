using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Loyalty.Utils
{
    public static class EncryptUtils
    {

        public static string getHash(string data)
        {
            /*
            byte[] bytData = new byte[data.Length];
            for (int i=0; i < data.Length; i ++)
            {
                bytData[i] = (byte)data[i];
            }
            bytData = ComputeHash(bytData);
            string hex = "";
            foreach (byte x in bytData)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;*/

            var encData = Encoding.UTF8.GetBytes(data);
            Org.BouncyCastle.Crypto.Digests.Sha256Digest myHash = new Org.BouncyCastle.Crypto.Digests.Sha256Digest();
            myHash.BlockUpdate(encData, 0, encData.Length);
            byte[] compArr = new byte[myHash.GetDigestSize()];
            myHash.DoFinal(compArr, 0);
            string hex = "";
            foreach (byte x in compArr)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

    }
}
