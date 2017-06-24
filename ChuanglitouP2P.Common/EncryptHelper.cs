using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.Common
{
    /// <summary>
    /// DES加密/解密类。
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// DES加密/解密类。
        /// </summary>
        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, Settings.Instance.EncryptKey);
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中     

            byte[] inputByteArray = Encoding.Default.GetBytes(text);

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        #endregion


        #region ========解密========


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            return Decrypt(text, Settings.Instance.EncryptKey);
        }

        /// <summary>
        /// //解密方法 
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        /// <summary>
        /// MD5 16位加密 toLower=1 大写 0 小写
        /// </summary>
        /// <param name="convertString"></param>
        /// <param name="toLower"> </param>
        /// <returns></returns>
        public static string GetMd5Str16(string convertString, int toLower)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(convertString)), 4, 8);
            t2 = toLower == 0 ? t2.Replace("-", "").ToLower() : t2.Replace("-", "");
            return t2;
        }

        /// <summary>
        /// MD5　32位加密 X大写，x小写 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="toLower"> </param>
        /// <returns></returns>
        public static string GetMd5Str32(string str, string toLower)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString(toLower);
            }
            return pwd;
        }

        /// <summary>
        /// MD5加密，此加密算法不可逆
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Md5Encrypt(string plainText)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(plainText, "md5").ToUpper();
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                file.Dispose();
                file = null;

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                GC.Collect();
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }

}
