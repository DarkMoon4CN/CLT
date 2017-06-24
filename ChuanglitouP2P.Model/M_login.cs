using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model
{
    public class M_login
    {

        private int _userid;
        private string _username;
        private string _codeno;
        private string _UsrName;
       // private string _UsrCustId;

        /// <summary>
        /// 用户id
        /// </summary>
        public int userid
		{
            set { _userid = value; }
            get { return _userid; }
		}


        /// <summary>
        /// 用户名
        /// </summary>
        public string username
        {
            set { _username = value; }
            get { return _username; }
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string UsrName
        {
            set { _UsrName = value; }
            get { return _UsrName; }
        }

        ///// <summary>
        ///// 用户客户id
        ///// </summary>
        //public string UsrCustId
        //{
        //    set { _UsrCustId = value; }
        //    get { return _UsrCustId; }
        //}

        /// <summary>
        /// 登录随机码
        /// </summary>
        public string codeno
        {
            set { _codeno = value; }
            get { return _codeno; }
        }




    }
}
