using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xfrog.Net;
using System.IO;
using System.Net;
using System.Web;
namespace ChuanglitouP2P.Common
{
    public class CalendarService : IDisposable
    {
        /// <summary>
        /// 获取休假列表(已排除调休周末)
        /// </summary>
        /// <param name="year">公历年</param>
        /// <returns></returns>
        public List<HolidayEntity> GetHolidayList(string year)
        {
            List<HolidayEntity> result = new List<HolidayEntity>();
            var lawyerHoliday = GetChinaFestivalList(year);
            var weekend = GetWeekDayList(year);
            foreach (var item in lawyerHoliday)
            {
                if (item.Status == 2)
                {
                    var weekEndItem = weekend.Where(q => q.From == item.From || q.To == item.To).FirstOrDefault();
                    if (weekEndItem != null)
                    {
                        weekEndItem.Status = 2;
                    }
                }
                else
                {
                    result.Add(item);
                }
            }
            foreach (var item in weekend)
            {
                if (item.Status != 2)
                    result.Add(item);
            }
            return result.OrderBy(o => o.From).ToList();
        }

        /// <summary>
        /// 获取中国节日列表
        /// </summary>
        /// <param name="calenderYear">公历年</param>
        /// <returns></returns>
        public List<HolidayEntity> GetChinaFestivalList(string calenderYear)
        {
            string appkey = Settings.Instance.GetCalendarAppKey;

            List<HolidayEntity> result = new List<HolidayEntity>();
            for (int i = 1; i < 13; i++)
            {
                string url2 = "http://japi.juhe.cn/calendar/month";
                var parameters2 = new Dictionary<string, string>();
                parameters2.Add("key", appkey);//你申请的key
                parameters2.Add("year-month", calenderYear + "-" + i.ToString()); //指定月份,格式为YYYY-MM,如月份和日期小于10,则取个位,如:2012-1
                string result2 = sendPost(url2, parameters2, "get");
                JsonObject newObj2 = new JsonObject(result2);
                String errorCode2 = newObj2["error_code"].Value;
                if (errorCode2 == "0")
                {
                    var holidayObject = new JsonObject("{holiday:" + newObj2["result"].Object["holiday"].Value + "}");
                    var itemsList = holidayObject["holiday"].Items;
                    foreach (var jsonProperty in itemsList)
                    {
                        var name = jsonProperty.Object["name"].Value;
                        if (result.Where(q => q.Name == name).ToList().Count > 0)
                            continue;
                        var desc = jsonProperty.Object["desc"].Value;
                        var year = newObj2["result"].Object["year"].Value;
                        var daysList = jsonProperty.Object["list"].Items;

                        string refDateFrom = string.Empty;
                        string refDateTo = string.Empty;
                        string refStatus = string.Empty;

                        for (int j = 0; j < daysList.Count; j++)
                        {
                            var day = daysList[j];
                            string tempDate = day.Object["date"].Value;
                            string tempStatus = day.Object["status"].Value;
                            if (!string.IsNullOrWhiteSpace(refDateFrom))
                            {
                                if (refStatus != tempStatus)
                                {
                                    result.Add(new HolidayEntity { Description = desc, From = Convert.ToDateTime(refDateFrom), To = Convert.ToDateTime(refDateTo), Name = name, Status = Convert.ToInt32(refStatus) });
                                    refDateFrom = tempDate;
                                    refStatus = tempStatus;
                                }
                            }
                            else if (string.IsNullOrWhiteSpace(refDateFrom))
                            {
                                refDateTo = refDateFrom = tempDate;
                                refStatus = tempStatus;
                            }
                            if (j == daysList.Count - 1)
                            {
                                if (tempDate == refDateTo && tempStatus == refStatus && !string.IsNullOrWhiteSpace(refStatus))
                                {
                                    refDateTo = tempDate;
                                    result.Add(new HolidayEntity { Description = desc, From = Convert.ToDateTime(refDateFrom), To = Convert.ToDateTime(refDateTo), Name = name, Status = Convert.ToInt32(refStatus) });
                                }
                                else if (tempDate != refDateTo && !string.IsNullOrWhiteSpace(tempDate) && !string.IsNullOrWhiteSpace(refDateTo) && refStatus != tempStatus)
                                {
                                    result.Add(new HolidayEntity { Description = desc, From = Convert.ToDateTime(refDateFrom), To = Convert.ToDateTime(refDateTo), Name = name, Status = Convert.ToInt32(refStatus) });
                                    result.Add(new HolidayEntity { Description = desc, From = Convert.ToDateTime(tempDate), To = Convert.ToDateTime(tempDate), Name = name, Status = Convert.ToInt32(refStatus) });
                                }
                                else if (tempDate != refDateTo && !string.IsNullOrWhiteSpace(tempDate) && !string.IsNullOrWhiteSpace(refDateTo) && refStatus == tempStatus)
                                {
                                    refDateTo = tempDate;
                                    result.Add(new HolidayEntity { Description = desc, From = Convert.ToDateTime(refDateFrom), To = Convert.ToDateTime(refDateTo), Name = name, Status = Convert.ToInt32(refStatus) });
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Debug.WriteLine("失败");
                    LoggerHelper.Error("ErrorCode:" + newObj2["error_code"].Value + " ErrorReason:" + newObj2["reason"].Value + " URL:http://japi.juhe.cn/calendar/month" + " year-month:" + calenderYear + " - " + i.ToString());
                    //Debug.WriteLine(newObj2["error_code"].Value + ":" + newObj2["reason"].Value);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取公历周末(未排除节假日和调休)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<HolidayEntity> GetWeekDayList(string year)
        {
            List<HolidayEntity> result = new List<HolidayEntity>();
            DateTime refferTime = new DateTime(Convert.ToInt32(year), 1, 1);
            DateTime startTime = new DateTime(Convert.ToInt32(year), 1, 1);
            DateTime endTime = new DateTime(Convert.ToInt32(year) + 1, 1, 1);
            while (refferTime < endTime)
            {
                if (refferTime.DayOfWeek == DayOfWeek.Saturday)
                {
                    HolidayEntity item = new HolidayEntity() { Name = "周末", Status = 1, Years = year, From = refferTime, To = refferTime.AddDays(1) };
                    result.Add(item);
                    refferTime = refferTime.AddDays(7);
                }
                else if (refferTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    HolidayEntity item = new HolidayEntity() { Name = "周末", Status = 1, Years = year, From = refferTime, To = refferTime };
                    result.Add(item);
                    refferTime = refferTime.AddDays(6);
                }
                else
                {
                    while (refferTime.DayOfWeek != DayOfWeek.Saturday)
                    {
                        refferTime = refferTime.AddDays(1);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Http (GET/POST)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>响应内容</returns>
        private string sendPost(string url, IDictionary<string, string> parameters, string method)
        {
            if (method.ToLower() == "post")
            {
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = method;
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse)req.GetResponse();
                    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (reqStream != null) reqStream.Close();
                    if (rsp != null) rsp.Close();
                }
            }
            else
            {
                //创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

                //GET请求
                request.Method = "GET";
                request.ReadWriteTimeout = 5000;
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                //返回内容
                string retString = myStreamReader.ReadToEnd();
                return retString;
            }
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        private string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        private string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.Collect();
            }
            else
            {
                GC.SuppressFinalize(this);
            }
        }
        ~CalendarService()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
