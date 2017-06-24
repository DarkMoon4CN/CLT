using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Bank;
using ChuangLiTou.Core.Entities.Response.Bonus;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuanglitouP2P.DAL.Api;
using ChuanglitouP2P.Model;
using ChuangLitouP2P.Models;

namespace ChuanglitouP2P.BLL.Api
{
    public class MemberLogic
    {
        private readonly MemberDal _dal = new MemberDal();

        /// <summary>
        /// 验证手机号是否被占用
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool CheckMobile(string mobile)
        {
            return _dal.CheckMobile(mobile);
        }
        /// <summary>
        /// 验证手机号是否被占用
        /// </summary>
        /// <param name="mobile">The mobile.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-25 15:25:31
        public bool CheckMobileForBindMobile(string mobile, int userId)
        {
            return _dal.CheckMobileForBindMobile(mobile, userId);
        }

        public List<BankEntity> SelectBankList()
        {
            return _dal.SelectBankList();
        }
        /// <summary>
        /// 获取用户绑卡数据
        /// </summary>
        /// <param name="usrBindCardId"></param>
        /// <returns></returns>
        public List<MemberBankEntity> SelectUserBindCards(int usrBindCardId)
        {
            return _dal.SelectUserBindCards(usrBindCardId);
        }

        /// <summary>
        /// 获取绑定的卡片是否支持特定的提现类型
        /// </summary>
        /// <param name="usrBindCardId">用户绑卡ID</param>
        /// <param name="withdrawalType">提现方式<remark>提现方式.0:普通提现;1:快速提现；2:即时提现.默认值为0</remark></param>
        /// <returns></returns>
        public bool IsAllowWithdrawalCash(int usrBindCardId, int withdrawalType)
        {
            if (withdrawalType != 2)//只有即时提现才需要判断银行是否支持
                return true;
            return _dal.IsAllowWithdrawalCash(usrBindCardId, withdrawalType);
        }

        /// <summary>
        /// 根据获取用户的绑卡 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<BankEntity> SelectBankListByUserID(int userId)
        {
            return _dal.SelectBankListByUserID(userId);
        }

        /// <summary>
        /// 会员注册.
        /// </summary>
        /// <param name="ent">用户实体</param>
        /// <param name="code">验证码</param>
        /// <returns>string.</returns>
        public string Regist(MemberEntity ent, string code, int? from = 0)
        {
            return _dal.Regist(ent, code, from);
        }

        /// <summary>
        /// 获取信息实体
        /// </summary>
        /// <param name="type">	0 是邮件类别 1 短信类别</param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public SmsEmailEntity GetSmsEmailEntity(int type, int messageId)
        {
            return _dal.GetSmsEmailEntity(type, messageId);
        }

        /// <summary>
        /// 获取用户银行卡接口--解志辉
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MemberBankEntity> SelectUserBankList(int userId)
        {
            return _dal.SelectUserBankList(userId);
        }

        /// <summary>
        /// 通过用户名获取用户实体.
        /// </summary>
        /// <param name="un">The un.</param>
        /// <param name="up">Up.</param>
        /// <returns>MemberEntity.</returns>
        public MemberEntity SelectMemberEntityByName(string un)
        {
            return _dal.SelectMemberEntityByName(un);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="mobile">手机号.</param>
        /// <param name="pwd">密码明文.</param>
        public void ModifyPassword(string mobile, string pwd)
        {
            _dal.ModifyPassword(mobile, pwd);
        }

        /// <summary>
        /// 修改用户图片
        /// </summary>
        /// <param name="avatar">图片路径</param>
        /// <param name="registerid">用户ID</param>
        public void UpdateMemberImg(string avatar, int registerid)
        {
            _dal.UpdateMemberImg(avatar, registerid);
        }

        /// <summary>
        /// 通过id查询用户信息.
        /// </summary>
        /// <param name="userId">用户id.</param>
        /// <returns>MemberEntity.</returns>
        public MemberEntity SelectMemberByUserId(int userId)
        {
            return _dal.SelectMemberByUserId(userId);
        }

        /// <summary>
        /// 通过id查询用户信息.
        /// </summary>
        /// <param name="userId">用户id.</param>
        /// <returns>MemberEntity.</returns>
        public MemberWithRedpacketEntity SelectMemberWithRedpacketByUserId(int userId)
        {
            return _dal.SelectMemberWithRedpacketByUserId(userId);
        }

        /// <summary>
        /// 获取用户手机号
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.String.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-25 14:12:44
        public string SelectUserMobileByUserId(int userId)
        {
            try
            {
                return _dal.SelectMemberByUserId(userId).mobile;

            }
            catch (Exception)
            {
                return "x";
            }
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
        ///  创建日期：2016-05-26 15:45:27
        public int SaveAddress(int userId, int provId, int cityId, int contId, string det, string userName, string mobile, string zipCode)
        {
            return _dal.SaveAddress(userId, provId, cityId, contId, det, userName, mobile, zipCode);
        }


        /// <summary>
        /// 实名认证修改用户商户号与真实姓名
        /// </summary>
        public bool UpdateUserRealAuth(MemberEntity model)
        {
            return _dal.UpdateUserRealAuth(model);
        }
        /// <summary>
        /// 保存用户登录信息
        /// </summary>
        /// <param name="usrmode"></param>
        public void SaveLoginInfo(hx_td_usrlogininfo usrmode)
        {
            _dal.SaveLoginInfo(usrmode);
        }

        public bool SelectVUserCashBank(string openAcctIds, int ordIdState)
        {
            return _dal.SelectVUserCashBank(openAcctIds, ordIdState);
        }
    }
}
