using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReUse.BllUtility
{
    public static class MD5AndSHA1
    {

        #region MD5Encode
        /// <summary>
        /// 生成指定字符串的MD5散列值，返回大写串。
        /// </summary>
        /// <param name="SrcValue">源字符串</param>
        /// <param name="EncodeType">type类型：16位还是32位</param>
        /// <returns>MD5值</returns>
        public static string MD5Encode(string SrcValue, string EncodeType)
        {
            byte[] result = Encoding.Default.GetBytes(SrcValue);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            if (EncodeType == "16")
                return BitConverter.ToString(output).Replace("-", "").ToUpper().Substring(8, 16);
            else
                return BitConverter.ToString(output).Replace("-", "").ToUpper();
        }
        #endregion

        #region SHA1Encode
        /// <summary>
        /// 生成指定字符串的SHA1散列值，返回大写串。
        /// </summary>
        /// <param name="SrcValue">源字符串字符串</param>
        /// <returns>SHA1值</returns>
        public static string SHA1Encode(string SrcValue)
        {
            byte[] StrRes = Encoding.Default.GetBytes(SrcValue);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString().ToUpper();
        }
        #endregion

    }
}
