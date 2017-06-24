using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.SmsEmail
{
    public class SmsEmailEntity
    {
        #region Model
        private int _smsemailid;
        private string _smsename;
        private string _secontext;
        private int? _setype = 0;
        /// <summary>
        /// 
        /// </summary>
        public int SmsEmailId
        {
            set { _smsemailid = value; }
            get { return _smsemailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SmsEname
        {
            set { _smsename = value; }
            get { return _smsename; }
        }
        /// <summary>
        /// 0 是邮件类别 1 短信类别
        /// </summary>
        public string SEContext
        {
            set { _secontext = value; }
            get { return _secontext; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SEtype
        {
            set { _setype = value; }
            get { return _setype; }
        }
        #endregion Model

    }
}
