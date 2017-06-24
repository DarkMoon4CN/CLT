using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ChuangLiTou.Core.Entities;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Bank;
using ChuangLiTou.Core.Entities.Response.Bonus;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuanglitouP2P.Model.Invest;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using ChuangLitouP2P.Models;
namespace ChuanglitouP2P.DAL.Api
{
    /// <summary>
    /// Class MemberDal.
    /// </summary>
    public class MemberDal : ImplBase
    {
        /// <summary>
        /// 验证手机号是否被占用
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool CheckMobile(string mobile)
        {
            var sql =
               "select count(registerid) from hx_member_table where mobile=@mobile or username=@mobile";
            SqlParameter[] parameters = {
                    new SqlParameter("@mobile", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = mobile;

            return DbHelper.Exists(sql, parameters);
        }

        /// <summary>
        /// 验证手机号是否被占用
        /// </summary>
        /// <param name="mobile">The mobile.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-25 16:00:01
        /// 创 建 者：解志辉
        ///  创建日期：2016-05-25 15:25:14
        public bool CheckMobileForBindMobile(string mobile, int userId)
        {
            var sql = "select count(registerid) from hx_member_table where (mobile=@mobile or username=@mobile) AND registerid<> @userId ";

            SqlParameter[] parameters = {
                    new SqlParameter("@mobile", SqlDbType.NVarChar,50),
                    new SqlParameter("@userId", SqlDbType.Int,4)
            };
            parameters[0].Value = mobile;
            parameters[1].Value = userId;

            return DbHelper.Exists(sql, parameters);
        }

        /// <summary>
        /// 保存用户地址
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="provId">The prov identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="contId">The cont identifier.</param>
        /// <param name="det">The det.</param>
        /// <param name="userName">用户姓名</param>
        /// <param name="mobile">手机号</param>
        /// <param name="zipCode">邮政编码</param>
        /// <returns>System.Int32.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 15:54:00
        public int SaveAddress(int userId, int provId, int cityId, int contId, string det, string userName, string mobile, string zipCode)
        {
            const string proc = @"prSaveMemberAddress";

            IDataParameter[] parameters = {
                    new SqlParameter("@userId", SqlDbType.Int,4),
                    new SqlParameter("@provId", SqlDbType.Int,4),
                    new SqlParameter("@cityId", SqlDbType.Int,4),
                    new SqlParameter("@counId", SqlDbType.Int,4),
                    new SqlParameter("@detAdds", SqlDbType.NVarChar,500),
                    new SqlParameter("@userName", SqlDbType.NVarChar,50),
                    new SqlParameter("@mobile", SqlDbType.NVarChar,20),
                    new SqlParameter("@zipCode", SqlDbType.NVarChar,10)
                                          };
            parameters[0].Value = userId;
            parameters[1].Value = provId;
            parameters[2].Value = cityId;
            parameters[3].Value = contId;
            parameters[4].Value = det;
            parameters[5].Value = userName;
            parameters[6].Value = mobile;
            parameters[7].Value = zipCode;

            return DbHelper.RunProcedure(proc, parameters);
        }

        /// <summary>
        /// 会员注册.
        /// </summary> 
        /// <param name="ent">用户实体</param>
        /// <param name="code">验证码</param>
        /// <returns>string.</returns>
        public string Regist(MemberEntity ent, string code, int? from = 0)
        {
            var returnCode = "";
            // returnCode = CheckSmsCode(ent.mobile, code);
            returnCode = "1";
            #region check code

            if (returnCode == "1")
            {

                if (!CheckMobile(ent.mobile))
                {
                    #region 注册用户
                    ent.email = "";
                    ent.usertypes = 0;
                    ent.ismobile = 1;
                    ent.Tid = "";
                    if (from == 123456)
                    {
                        ent.Channelsource = 4;//ios
                    }
                    else if (from == 654321)
                    {
                        ent.Channelsource = 5;//安卓
                    }
                    else
                    {
                        ent.Channelsource = 0;
                    }
                    ent.realname = "";
                    ent.userstate = 0;
                    ent.available_balance = 0;
                    ent.collect_total_amount = 0;
                    ent.frozen_sum = 0;
                    ent.account_total_assets = 0;
                    ent.invitedcode = Getinvitedcode();

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into hx_member_table(");
                    strSql.Append("username,password,mobile,email,realname,iD_number,transactionpassword,istransactionpassword,ismobile,isrealname,isbankcard,isemail,userstate,account_total_assets,available_balance,collect_total_amount,frozen_sum,open_tonto_account,tonto_account_user,usertypes,invitedcode,Channelsource,Tid)");
                    strSql.Append(" values (");
                    strSql.Append("@username,@password,@mobile,@email,@realname,@iD_number,@transactionpassword,@istransactionpassword,@ismobile,@isrealname,@isbankcard,@isemail,@userstate,@account_total_assets,@available_balance,@collect_total_amount,@frozen_sum,@open_tonto_account,@tonto_account_user,@usertypes,@invitedcode,@Channelsource,@Tid)");
                    strSql.Append(";select @@IDENTITY");
                    SqlParameter[] parameters = {
                    new SqlParameter("@username", SqlDbType.NVarChar,50),
                    new SqlParameter("@password", SqlDbType.NVarChar,50),
                    new SqlParameter("@mobile", SqlDbType.VarChar,20),
                    new SqlParameter("@email", SqlDbType.NVarChar,50),
                    new SqlParameter("@realname", SqlDbType.NVarChar,30),
                    new SqlParameter("@iD_number", SqlDbType.VarChar,30),
                    new SqlParameter("@transactionpassword", SqlDbType.NVarChar,50),
                    new SqlParameter("@istransactionpassword", SqlDbType.Int,4),
                    new SqlParameter("@ismobile", SqlDbType.Int,4),
                    new SqlParameter("@isrealname", SqlDbType.Int,4),
                    new SqlParameter("@isbankcard", SqlDbType.Int,4),
                    new SqlParameter("@isemail", SqlDbType.Int,4),
                    new SqlParameter("@userstate", SqlDbType.Int,4),
                    new SqlParameter("@account_total_assets", SqlDbType.Decimal,17),
                    new SqlParameter("@available_balance", SqlDbType.Decimal,17),
                    new SqlParameter("@collect_total_amount", SqlDbType.Decimal,17),
                    new SqlParameter("@frozen_sum", SqlDbType.Decimal,17),
                    new SqlParameter("@open_tonto_account", SqlDbType.Int,4),
                    new SqlParameter("@tonto_account_user", SqlDbType.VarChar,30),
                    new SqlParameter("@usertypes",SqlDbType.Int,4),
                    new SqlParameter("@invitedcode", SqlDbType.VarChar,30),
                    new SqlParameter("@Channelsource", SqlDbType.Int,4),
                    new SqlParameter("@Tid", SqlDbType.VarChar,300) };

                    parameters[0].Value = ent.username;
                    parameters[1].Value = ent.password;
                    parameters[2].Value = ent.mobile;
                    parameters[3].Value = ent.email;
                    parameters[4].Value = ent.realname;
                    parameters[5].Value = ent.iD_number;
                    parameters[6].Value = ent.transactionpassword;
                    parameters[7].Value = ent.istransactionpassword;
                    parameters[8].Value = ent.ismobile;
                    parameters[9].Value = ent.isrealname;
                    parameters[10].Value = ent.isbankcard;
                    parameters[11].Value = ent.isemail;
                    parameters[12].Value = ent.userstate;
                    parameters[13].Value = ent.account_total_assets;
                    parameters[14].Value = ent.available_balance;
                    parameters[15].Value = ent.collect_total_amount;
                    parameters[16].Value = ent.frozen_sum;
                    parameters[17].Value = ent.open_tonto_account;
                    parameters[18].Value = ent.tonto_account_user;
                    parameters[19].Value = ent.usertypes;
                    parameters[20].Value = ent.invitedcode;
                    parameters[21].Value = ent.Channelsource;
                    parameters[22].Value = ent.Tid;

                    var res = DbHelper.GetSingle(strSql.ToString(), parameters);
                    if (res != null && !string.IsNullOrEmpty(res.ToString()))
                    {
                        LoggerHelper.Info(res);
                        return res.ToString();
                    }
                    return "100000000.0";

                    #endregion
                }
                return "1000000002";
            }
            #endregion
            return returnCode;
        }

        public SmsEmailEntity GetSmsEmailEntity(int type, int messageId)
        {
            return SelectSmsEmailEntity(type, messageId);
        }

        /// <summary>
        /// 通过用户名获取用户实体.
        /// </summary>
        /// <param name="un">The un.</param>
        /// <param name="up">Up.</param>
        /// <returns>MemberEntity.</returns>
        public MemberEntity SelectMemberEntityByName(string un)
        {
            var sql = "select registerid ,username,password,userstate  from hx_member_table where  username =@userName or mobile=@mobile";

            SqlParameter[] parameters = {
                    new SqlParameter("@userName", SqlDbType.NVarChar,50),
                    new SqlParameter("@mobile", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = un;
            parameters[1].Value = un;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                var ent = InitMemberEntity(ds.Tables[0]);
                sql = "update hx_member_table set lastlogintime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',lastloginIP='" + Settings.Instance.ClientIp + "' where registerid=" + ent.registerid;
                DbHelper.ExecuteSql(sql);

                return ent;
            }
            return null;
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="mobile">手机号.</param>
        /// <param name="pwd">密码明文.</param>
        public void ModifyPassword(string mobile, string pwd)
        {

            var sql = string.Format("UPDATE dbo.hx_member_table SET [password]='{0}' WHERE ( mobile='{1}' OR username='{1}') ",
                EncryptHelper.Encrypt(pwd), mobile
                );
            DbHelper.ExecuteSql(sql);

        }
        /// <summary>
        /// 修改用户图片
        /// </summary>
        /// <param name="avatar">图片路径</param>
        /// <param name="registerid">用户ID</param>
        public void UpdateMemberImg(string avatar, int registerid)
        {

            var sql = string.Format("UPDATE dbo.hx_member_table SET [avatar]='{0}' WHERE registerid={1} ", avatar, registerid);
            DbHelper.ExecuteSql(sql);

        }
        public MemberEntity SelectMemberByUserId(int userId)
        {

            var sql = @"select registerid,password,username,mobile,realname,id_number,isrealname,userstate,avatar as userPhotoUrl,account_total_assets,available_balance,UsrCustId,frozen_sum,usertypes,invitedcode,lastlogintime from hx_member_table  WHERE  registerid =@registerid";

            SqlParameter[] parameters = {
                    new SqlParameter("@registerid", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = userId;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                var ent = InitMemberEntity(ds.Tables[0]);
                CommonDal.FillMemberEntityAccountTotalAssets(ref ent, userId);
                ent.totalGains = GetTotalGains(userId);//累计赚取总额
                return ent;
            }
            return null;
        }
        public MemberWithRedpacketEntity SelectMemberWithRedpacketByUserId(int userId)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            var sql = @"select registerid,password,username,mobile,realname,id_number,isrealname,userstate,userPhotoUrl,account_total_assets,available_balance,UsrCustId,frozen_sum,usertypes,invitedcode,lastlogintime,
                        ISNULL(VoucherCount,0) VoucherCount,ISNULL(RedpacketMoney,0) RedpacketMoney
                        from
                        (
                        select registerid,password,username,mobile,realname,id_number,isrealname,userstate,avatar as userPhotoUrl,account_total_assets,available_balance,UsrCustId,frozen_sum,usertypes,invitedcode,lastlogintime from hx_member_table  
                        ) c
                        left join
                        (
                        SELECT count(*) VoucherCount,[registerid] RID FROM  hx_UserAct where UseState = 0 and AmtEndtime >= '" + today + "' and RewTypeID in (2,3) group by [registerid]) d on c.registerid = d.RID left join(SELECT sum(amt) RedpacketMoney,[registerid] RID FROM  hx_UserAct where UseState = 0 and AmtEndtime >= '" + today + "' and RewTypeID = 2 group by [registerid]) e on c.registerid = e.RID WHERE  registerid =@registerid";

            SqlParameter[] parameters = {
                    new SqlParameter("@registerid", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = userId;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                var ent = DataHelper.GetEntity<MemberWithRedpacketEntity>(ds.Tables[0]);
                //var ent = InitMemberEntity(ds.Tables[0]);
                ent.account_total_assets = CommonDal.GetAccountTotalAssets(ent.available_balance.Value, ent.frozen_sum.Value, userId);
                ent.totalGains = GetTotalGains2(userId);//累计赚取总额
                return ent;
            }
            return null;
        }


        /// <summary>
        /// 资金总计接口.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns>MemberEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-18 17:23:55
        public MemberEntity TotalCapital(int userId)
        {
            string sql = "select  available_balance  from  hx_member_table where registerid=@userId";
            SqlParameter[] parameters = {
                    new SqlParameter("@userId", SqlDbType.Int,4)
            };
            parameters[0].Value = userId;
            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                var ent = InitMemberEntity(ds.Tables[0]);
                return ent;
            }
            return null;
        }



        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns> 
        ///<Error ErrorCode="1000000003" ErrorInfo="手机验证码不正确"/>
        ///<Error ErrorCode="1000000004" ErrorInfo="手机验证码不存在"/> 
        /// </returns>
        public string CheckSmsCode(string mobile, string code)
        {

            string str = "";

            int w = (int)System.Enum.Parse(typeof(SmsType), SmsType.短信验证码.ToString());

            int v = (int)System.Enum.Parse(typeof(SmsType), SmsType.语音短信验证码.ToString());

            string sql = "select vcode from hx_td_SMS_record where ( smstype=" + w + "  or  smstype=" + v + " ) and phone_number='" + mobile + "' and  DATEDIFF(MINUTE,sendtime,getDate())<3  order by sms_record_id desc";

            var eCode = DbHelper.GetSingle(sql);
            if (eCode != null && !string.IsNullOrEmpty(eCode.ToString()))
            {
                str = eCode.ToString() == code ? "1" : "1000000003";

            }
            else
            {
                str = "1000000004";
            }
            return str;
        }
        /// <summary>
		/// 实名认证修改用户商户号与真实姓名
		/// </summary>
		public bool UpdateUserRealAuth(MemberEntity m)
        {
            string sql = " update hx_member_table  set  UsrCustId ='" + m.UsrCustId + "', realname='" + m.realname + "',iD_number='" + m.iD_number + "',UsrId='" + m.UsrId + "',isrealname=1 where username='" +
                PageHelper.GetUserSplit(m.UsrId) + "'";
            int rows = DbHelper.ExecuteSql(sql);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取银行列表
        /// </summary>
        /// <returns></returns>
        public List<BankEntity> SelectBankList()
        {
            var sql = " SELECT BankName,OpenBankId,SingleTransQuota,CardDailyTransQuota FROM [dbo].[hx_td_Bank] where Isquick=1";
            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                return InitBankEntityList(ds.Tables[0]);
            }
            return null;
        }

        public List<MemberBankEntity> SelectUserBindCards(int usrBindCardId)
        {
            var sql = " SELECT  *  FROM  [dbo].[hx_UsrBindCardC] where UsrBindCardID= " + usrBindCardId;


            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                return InitUsrBindCardEntityList(ds.Tables[0]);

            }
            return null;
        }

        /// <summary>
        /// 判定用户绑定的银行卡是否允许即时提现
        /// </summary>
        /// <param name="usrBindCardId"></param>
        /// <param name="withdrawalType"></param>
        /// <returns></returns>
        public bool IsAllowWithdrawalCash(int usrBindCardId, int withdrawalType)
        {
            var sql = " select bank.isGren from hx_td_Bank bank left join hx_UsrBindCardC bind on bank.OpenBankId = bind.OpenBankId where isGren=1 and UsrBindCardID = " + usrBindCardId.ToString();
            var ds = DbHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return true;
            return false;
        }

        public List<BankEntity> SelectBankListByUserID(int userId)
        {
            var sql = "SELECT  TOP 1 ubc.*,Bank.BankName,Bank.[SingleTransQuota],Bank.[CardDailyTransQuota]  FROM  [dbo].[hx_UsrBindCardC] AS ubc  LEFT JOIN hx_member_table  AS u ON ubc.UsrCustId=u.UsrCustId left join hx_td_Bank Bank on ubc.OpenBankId=Bank.OpenBankId ";
            sql += " WHERE u.registerid='{0}'  AND ubc.BindCardType='1' AND ubc.defCard='1' ";
            sql = string.Format(sql, userId);
            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                //var list = GetBankModel().Where(p=>p.OpenBankId==ds.Tables[0].Rows[0]["OpenBankId"].ToString()).ToList();
                //return list;
                return RenameBankName(InitBankEntityList(ds.Tables[0]));
            }
            return null;
        }

        private List<BankEntity> RenameBankName(List<BankEntity> source)
        {
            var result = source;
            var list = new List<BankEntity> {
                new BankEntity() {Bankid=2,BankName="中国农业银行",OpenBankId="ABC"},
                new BankEntity() {Bankid=1,BankName="中国工商银行",OpenBankId="ICBC"},
                new BankEntity() {Bankid=7,BankName="中国银行",OpenBankId="BOC"},
                new BankEntity() {Bankid=4,BankName="中国建设银行",OpenBankId="CCB"},
                new BankEntity() {Bankid=22,BankName="中国邮政储蓄银行",OpenBankId="PSBC"},
                new BankEntity() {Bankid=24,BankName="浦发银行",OpenBankId="SPDB"},
                new BankEntity() {Bankid=12,BankName="中国光大银行",OpenBankId="CEB"},
                new BankEntity() {Bankid=14,BankName="中信银行",OpenBankId="CITIC"},
                new BankEntity() {Bankid=13,BankName="兴业银行",OpenBankId="CIB"},
                new BankEntity() {Bankid=21,BankName="平安银行",OpenBankId="PINGAN"},
                new BankEntity() {Bankid=10,BankName="上海银行",OpenBankId="BOS"},
                new BankEntity() {Bankid=11,BankName="渤海银行",OpenBankId="CBHB"},
                new BankEntity() {Bankid=3,BankName="招商银行",OpenBankId="CMB"}
            };
            foreach (var bank in result)
            {
                var item = list.Where(d => d.OpenBankId == bank.OpenBankId).FirstOrDefault();
                if (item != null)
                    bank.BankName = item.BankName;
            }

            return result;
        }


        public List<MemberBankEntity> SelectUserBankList(int userId)
        {
            var sql = string.Format("select BC.*,MT.realname AS RealName,tb.BankName ,tb.CardImage,tb.isGren as CanImmediateWithdrawal FROM hx_UsrBindCardC BC LEFT JOIN dbo.hx_member_table MT ON BC.UsrCustId = MT.UsrCustId LEFT JOIN dbo.hx_td_Bank tb ON bc.OpenBankId = tb.OpenBankId where MT.registerId={0} ", userId);
            //--and Isquick = 1


            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                return InitUsrBindCardEntityList(ds.Tables[0]);
            }
            return null;
        }

        public void SaveLoginInfo(hx_td_usrlogininfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hx_td_usrlogininfo(");
            strSql.Append("registerid,logintime,Loginusrname,loginusrpass,loginstate,loginIP,logincity,loginsource)");
            strSql.Append(" values (");
            strSql.Append("@registerid,@logintime,@Loginusrname,@loginusrpass,@loginstate,@loginIP,@logincity,@loginsource)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@registerid", SqlDbType.Int,4),
                    new SqlParameter("@logintime", SqlDbType.DateTime),
                    new SqlParameter("@Loginusrname", SqlDbType.VarChar,200),
                    new SqlParameter("@loginusrpass", SqlDbType.VarChar,200),
                    new SqlParameter("@loginstate", SqlDbType.Int,4),
                    new SqlParameter("@loginIP", SqlDbType.VarChar,20),
                    new SqlParameter("@logincity", SqlDbType.VarChar,200),
                    new SqlParameter("@loginsource", SqlDbType.Int,4)};
            parameters[0].Value = model.registerid;
            parameters[1].Value = model.logintime;
            parameters[2].Value = model.Loginusrname;
            parameters[3].Value = model.loginusrpass;
            parameters[4].Value = model.loginstate;
            parameters[5].Value = model.loginIP;
            parameters[6].Value = model.logincity;
            parameters[7].Value = model.loginsource;

            object obj = DbHelper.GetSingle(strSql.ToString(), parameters);
        }


        public bool SelectVUserCashBank(string openAcctIds, int ordIdState)
        {
            var sql = " SELECT COUNT(*) FROM V_UserCash_Bank  WHERE  OpenAcctId IN({0})  AND ordIdState={1} ";
            sql = string.Format(sql, openAcctIds, ordIdState);
            int result = DbHelper.GetSingle(sql).ToInt();

            return result > 0 ? true : false;
        }
    }
}
