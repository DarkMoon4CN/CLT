using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model.chinapnr.QueryTransStat;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL.EF
{
    /// <summary>
    /// 交易状态查询
    /// </summary>
    public  class TransStat
    {



        #region 交询状态查询
        /// <summary>
        /// 交询状态查询
        /// </summary>
        /// <param name="OrdId"></param>
        /// <param name="OrdDate">格式为 YYYYMMDD，例如：20130307</param>
        /// <param name="QueryTransType"></param>
        /// <returns></returns>
        public bool checktrans(string OrdId, string OrdDate, string QueryTransType= "TENDER")
        {
            bool t = false;

            M_QueryTransStat m = new M_QueryTransStat();
            m.Version = "10";
            m.CmdId = "QueryTransStat";
            m.MerCustId = Utils.GetMerCustID();
            m.OrdId = OrdId;
            m.OrdDate = OrdDate;          
            m.QueryTransType = QueryTransType;
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.OrdDate);
            chkVal.Append(m.QueryTransType);
            string chkv = chkVal.ToString();
            LogInfo.WriteLog("行为:"+ QueryTransType + "加签chkv字符:" + chkv);

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

            LogInfo.WriteLog("行为:" + QueryTransType + "加签字符:" + str.ToString());

            m.ChkValue = sbChkValue.ToString();

            LogInfo.WriteLog("行为:" + QueryTransType + "提交信息：" + FastJSON.toJOSN(m));
            LogInfo.WriteLog("行为:" + QueryTransType + "ChkValue:" + m.ChkValue);

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values.Add("Version", m.Version);
                values.Add("CmdId", m.CmdId);
                values.Add("MerCustId", m.MerCustId);
                values.Add("OrdId", m.OrdId);
                values.Add("OrdDate", m.OrdDate);
                values.Add("QueryTransType", m.QueryTransType);
                values.Add("ChkValue", m.ChkValue);
                string url = Utils.GetChinapnrUrl();
                //同步发送form表单请求
                byte[] result = client.UploadValues(url, "POST", values);
                var retStr = Encoding.UTF8.GetString(result);
                // Response.Write(retStr);

                LogInfo.WriteLog("行为:" + QueryTransType + "交易状态返回报文" + retStr);

                ReQueryTransStat reg = new ReQueryTransStat();

                var retloan = (ReQueryTransStat)FastJSON.ToObject(retStr, reg);

                StringBuilder builder = new StringBuilder();
                builder.Append(retloan.CmdId);
                builder.Append(retloan.RespCode);
                builder.Append(retloan.MerCustId);
                builder.Append(retloan.OrdId);
                builder.Append(retloan.OrdDate);
                builder.Append(retloan.QueryTransType);
                builder.Append(retloan.TransStat);

                var msg = builder.ToString();

                LogInfo.WriteLog("行为:" + QueryTransType + "返回参数:" + msg);
                //验签
                string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);

                LogInfo.WriteLog("行为:" + QueryTransType + "验签ret:" + ret.ToString());
                if (ret == 0)
                {
                    if (retloan.RespCode == "000")
                    {
                        if(QueryTransType == "TENDER") //投标查询结果
                        {
                            /*投标
                            N--成功
                            C-失败
                            */
                            if (retloan.TransStat == "N")
                            {
                                t = true;
                            }
                        }
                        else if(QueryTransType == "REPAYMENT" || QueryTransType == "LOANS")//还款
                        {
                            /*放款，还款
                            I--初始
                            P--部分成功
                            */
                            if (retloan.TransStat == "P")  
                            {
                                t = true;
                            }
                        }
                        else if (QueryTransType == "CASH" )//取现
                        {
                            /*S--成功
                            F--失败
                            H--经办
                            R--拒绝
                            */
                            if (retloan.TransStat == "S")
                            {
                                t = true;
                            }
                        }
                        else if (QueryTransType == "FREEZE")//冻结解冻交易查询
                        {
                            /*F – 冻结
                                U – 已解冻
                            */
                            if (retloan.TransStat == "U")
                            {
                                t = true;
                            }
                        }

                    }
                    else
                    {
                        t = false;
                    }
                }

            }





            return t;
        }

        #endregion




        #region 交询冻结状态返回冻结号
        /// <summary>
        /// 交询状态查询
        /// </summary>
        /// <param name="OrdId"></param>
        /// <param name="OrdDate">格式为 YYYYMMDD，例如：20130307</param>
        /// <param name="QueryTransType"></param>
        /// <returns></returns>
        public bool ReturntransTrxId(string OrdId, string OrdDate, string QueryTransType = "FREEZE")
        {
            bool t = false;

            M_QueryTransStat m = new M_QueryTransStat();
            m.Version = "10";
            m.CmdId = "QueryTransStat";
            m.MerCustId = Utils.GetMerCustID();
            m.OrdId = OrdId;
            m.OrdDate = OrdDate;
            m.QueryTransType = QueryTransType;
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.OrdDate);
            chkVal.Append(m.QueryTransType);
            string chkv = chkVal.ToString();
            LogInfo.WriteLog("冻结行为:" + QueryTransType + "加签chkv字符:" + chkv);

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

            LogInfo.WriteLog("冻结行为:" + QueryTransType + "加签字符:" + str.ToString());

            m.ChkValue = sbChkValue.ToString();

            LogInfo.WriteLog("冻结行为:" + QueryTransType + "提交信息：" + FastJSON.toJOSN(m));
            LogInfo.WriteLog("冻结行为:" + QueryTransType + "ChkValue:" + m.ChkValue);

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values.Add("Version", m.Version);
                values.Add("CmdId", m.CmdId);
                values.Add("MerCustId", m.MerCustId);
                values.Add("OrdId", m.OrdId);
                values.Add("OrdDate", m.OrdDate);
                values.Add("QueryTransType", m.QueryTransType);
                values.Add("ChkValue", m.ChkValue);
                string url = Utils.GetChinapnrUrl();
                //同步发送form表单请求
                byte[] result = client.UploadValues(url, "POST", values);
                var retStr = Encoding.UTF8.GetString(result);
                // Response.Write(retStr);

                LogInfo.WriteLog("冻结行为:" + QueryTransType + "交易状态返回报文" + retStr);

                ReQueryTransStat reg = new ReQueryTransStat();

                var retloan = (ReQueryTransStat)FastJSON.ToObject(retStr, reg);

                StringBuilder builder = new StringBuilder();
                builder.Append(retloan.CmdId);
                builder.Append(retloan.RespCode);
                builder.Append(retloan.MerCustId);
                builder.Append(retloan.OrdId);
                builder.Append(retloan.OrdDate);
                builder.Append(retloan.QueryTransType);
                builder.Append(retloan.TransStat);

                var msg = builder.ToString();

                LogInfo.WriteLog("冻结行为:" + QueryTransType + "返回参数:" + msg);
                //验签
                string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);

                LogInfo.WriteLog("行为:" + QueryTransType + "验签ret:" + ret.ToString());
                if (ret == 0)
                {
                    if (retloan.RespCode == "000")
                    {
                        if (retloan.TransStat == "N")
                        {
                            t = true;
                        }



                    }
                    else
                    {
                        t = false;
                    }
                }

            }
            return t;
        }

        #endregion

    }
}
