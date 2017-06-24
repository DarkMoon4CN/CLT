using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common
{
    /// <summary>
    /// 通过ip获取所在城市
    /// </summary>
    public class GetIpToCity
    {
        

        /// <summary>
        /// 返回地区字符串
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetAddressByIp(string ip)
        {
            string str = "";
            string PostUrl = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?ip=" + ip;
            string res = GetDataByPost(PostUrl);//该条请求返回的数据为：res=1\t115.193.210.0\t115.194.201.255\t中国\t浙江\t杭州\t电信
            string[] arr = getAreaInfoList(res);

            try
            {
                str = arr[0].ToString().Trim();

                str = str +","+arr[1].ToString().Trim();
            }
            catch
            {
                str = "未知";
            }

            return str;
        }





        #region 获取提交流数据
        /// <summary>
        /// 获取提交流数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDataByPost(string url)
        {
            string backstr = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                string s = "anything";
                byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(s);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = requestBytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
                backstr = sr.ReadToEnd();
                sr.Close();
                res.Close();
            }
            catch
            {

            }
            return backstr;
        }


        #endregion



        #region 获取省市
        /// <summary>
        /// 获取省市
        /// </summary>
        /// <param name="ipData"></param>
        /// <returns></returns>
        public static string[] getAreaInfoList(string ipData)
        {
            //1\t115.193.210.0\t115.194.201.255\t中国\t浙江\t杭州\t电信
            string[] areaArr = new string[10];
            string[] newAreaArr = new string[2];


            try
            {
                //取所要的数据，这里只取省市
                areaArr = ipData.Split('\t');
                newAreaArr[0] = areaArr[4];//省
                newAreaArr[1] = areaArr[5];//市
            }
            catch (Exception e)
            {
                // TODO: handle exception
            }
            return newAreaArr;
        } 
        
        #endregion

    }
}
