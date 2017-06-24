using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace ChuanglitouP2P.DBUtility
{
	/// <summary>
	/// DES加密/解密类。
	/// </summary>
	public class DESEncrypt
	{
		public DESEncrypt()
		{			
		}

        /*

		#region ========加密======== 
 

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
		public static string Encrypt(string Text) 
		{
            return Encrypt(Text, "litianping");
		}
		/// <summary> 
		/// 加密数据 
		/// </summary> 
		/// <param name="Text"></param> 
		/// <param name="sKey"></param> 
		/// <returns></returns> 
		public static string Encrypt(string Text,string sKey) 
		{ 
			DESCryptoServiceProvider des = new DESCryptoServiceProvider(); 
			byte[] inputByteArray; 
			inputByteArray=Encoding.Default.GetBytes(Text); 
			des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8)); 
			des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8)); 
			System.IO.MemoryStream ms=new System.IO.MemoryStream(); 
			CryptoStream cs=new CryptoStream(ms,des.CreateEncryptor(),CryptoStreamMode.Write); 
			cs.Write(inputByteArray,0,inputByteArray.Length); 
			cs.FlushFinalBlock(); 
			StringBuilder ret=new StringBuilder(); 
			foreach( byte b in ms.ToArray()) 
			{ 
				ret.AppendFormat("{0:X2}",b); 
			} 
			return ret.ToString(); 
		} 

		#endregion
		
		#region ========解密======== 
   
 
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
		public static string Decrypt(string Text) 
		{
            return Decrypt(Text, "litianping");
		}
		/// <summary> 
		/// 解密数据 
		/// </summary> 
		/// <param name="Text"></param> 
		/// <param name="sKey"></param> 
		/// <returns></returns> 
		public static string Decrypt(string Text,string sKey) 
		{ 
			DESCryptoServiceProvider des = new DESCryptoServiceProvider(); 
			int len; 
			len=Text.Length/2; 
			byte[] inputByteArray = new byte[len]; 
			int x,i; 
			for(x=0;x<len;x++) 
			{ 
				i = Convert.ToInt32(Text.Substring(x * 2, 2), 16); 
				inputByteArray[x]=(byte)i; 
			} 
			des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8)); 
			des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8)); 
			System.IO.MemoryStream ms=new System.IO.MemoryStream(); 
			CryptoStream cs=new CryptoStream(ms,des.CreateDecryptor(),CryptoStreamMode.Write); 
			cs.Write(inputByteArray,0,inputByteArray.Length); 
			cs.FlushFinalBlock(); 
			return Encoding.Default.GetString(ms.ToArray()); 
		} 
 
		#endregion 

        */

         /// <summary>
        /// //加密方法  
        ///  //注意：sKey输入密码的时候，必须使用英文字符，区分大小写，且字符数量是8个，不能多也不能少，否则出错。
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中     
            //原来使用的UTF8编码，我改成Unicode编码了，不行     
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[]   inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);     

            //建立加密对象的密钥和偏移量     
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法     
            //使得输入密码必须输入英文文本     
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write   the   byte   array   into   the   crypto   stream     
            //(It   will   end   up   in   the   memory   stream)     
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get   the   data   back   from   the   memory   stream,   and   into   a   string     
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format   as   hex     
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
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

            //Put   the   input   string   into   the   byte   array     
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改     
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush   the   data   through   the   crypto   stream   into   the   memory   stream     
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //Get   the   decrypted   data   back   from   the   memory   stream     
            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象     
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

	}
}
