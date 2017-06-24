using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.wdzg
{
    /// <summary>
    /// 标信息
    /// </summary>
    public class TargetDetailEntity
    {
        #region Model
        private string _url;
        private int _id;
        private string _title;
        private decimal? _account;
        private decimal? _apr;
        private int? _limit;
        private int? _limit_type;
        private int? _way;
        private decimal? _account_yes;
        private decimal? _account_min;
        private decimal? _account_max;
        private string _username;
        private int? _add_time;
        private string _verify_time;
        private string _success_time;
        private string _type;
        private int? _status;
        private string _reward;
        private int _is_diya;
        private int _is_danbao;
        private int _is_mima;
        private int _is_miao;
        private int _is_zhuan;
        /// <summary>
        /// 标详情URL
        /// </summary>
        public string url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 标ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 标名称
        /// </summary>
        public string title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal? account
        {
            set { _account = value; }
            get { return _account; }
        }
        /// <summary>
        /// 年利率
        /// </summary>
        public decimal? apr
        {
            set { _apr = value; }
            get { return _apr; }
        }
        /// <summary>
        /// 借款期限
        /// </summary>
        public int? limit
        {
            set { _limit = value; }
            get { return _limit; }
        }
        /// <summary>
        /// 借款期限类型
        /// </summary>
        public int? limit_type
        {
            set { _limit_type = value; }
            get { return _limit_type; }
        }
        /// <summary>
        /// 还款方式
        /// </summary>
        public int? way
        {
            set { _way = value; }
            get { return _way; }
        }
        /// <summary>
        /// 已投金额
        /// </summary>
        public decimal? account_yes
        {
            set { _account_yes = value; }
            get { return _account_yes; }
        }
        /// <summary>
        /// 起投金额
        /// </summary>
        public decimal? account_min
        {
            set { _account_min = value; }
            get { return _account_min; }
        }
        /// <summary>
        /// 最大金额
        /// </summary>
        public decimal? account_max
        {
            set { _account_max = value; }
            get { return _account_max; }
        }
        /// <summary>
        /// 发标人用户名
        /// </summary>
        public string username
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public int? add_time
        {
            set { _add_time = value; }
            get { return _add_time; }
        }
        /// <summary>
        /// 初审时间
        /// </summary>
        public string verify_time
        {
            set { _verify_time = value; }
            get { return _verify_time; }
        }
        /// <summary>
        /// 复审通过时间
        /// </summary>
        public string success_time
        {
            set { _success_time = value; }
            get { return _success_time; }
        }
        /// <summary>
        /// 标类型
        /// </summary>
        public string type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 标状态
        /// </summary>
        public int? status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 奖励
        /// </summary>
        public string reward
        {
            set { _reward = value; }
            get { return _reward; }
        }
        /// <summary>
        /// 是否抵押
        /// </summary>
        public int is_diya
        {
            set { _is_diya = value; }
            get { return _is_diya; }
        }
        /// <summary>
        /// 是否担保
        /// </summary>
        public int is_danbao
        {
            set { _is_danbao = value; }
            get { return _is_danbao; }
        }
        /// <summary>
        /// 是否密码
        /// </summary>
        public int is_mima
        {
            set { _is_mima = value; }
            get { return _is_mima; }
        }
        /// <summary>
        /// 是否秒标
        /// </summary>
        public int is_miao
        {
            set { _is_miao = value; }
            get { return _is_miao; }
        }
        /// <summary>
        /// 是否债权转让
        /// </summary>
        public int is_zhuan
        {
            set { _is_zhuan = value; }
            get { return _is_zhuan; }
        }
        #endregion Model



        public List<InvestRecordEntity> tender { get; set; }
    }
}
