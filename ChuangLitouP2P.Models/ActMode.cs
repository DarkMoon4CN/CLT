using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLitouP2P.Models
{
    public class ActMode
    {

        public int UserAct { set; get; }

        public Decimal? investment_amount { set; get; }

        public string realname { set; get; }

        public string mobile { set; get; }

        public int? useridentity { set; get; }

        public int? RewTypeID { set; get; }

        public decimal? Amt { set; get; }

        public Decimal? Uselower { set; get; }
        public Decimal? Usehight { set; get; }
        public DateTime? AmtEndtime { set; get; }
        public int? UseState { set; get; }
        public int? AmtProid { set; get; }
        public DateTime? UseTime { set; get; }
        public int? ISSmsOne { set; get; }
        public int? IsSmsThree { set; get; }
        public int? IsSmsSeven { set; get; }
        public int? isSmsFifteen { set; get; }
        public int? isSmsSixteen { set; get; }
        public string OrderID { set; get; }
        public DateTime? Createtime { set; get; }
        public DateTime? ActStarttime { set; get; }
        public DateTime? ActEndtime { set; get; }
        public int? ActState { set; get; }
        public string ActName { set; get; }
        public string ActName1 { set; get; }
        public string username { set; get; }
        public int? Channelsource { set; get; }

        public DateTime? registration_time { set; get; }


        public int? ActID { set; get; }




    }

    public class ActDetailMode
    {
        public int? UserAct { set; get; }
        public string hbtype { set; get; }
        public int? bid_records_id { set; get; }
        public Decimal? jxq { set; get; }
        public Decimal? hdhbje { set; get; }
        public Decimal? xjjl { set; get; }
        public int? registerid { set; get; }
        public string realname { set; get; }
        public int? useridentity { set; get; }
        public Decimal? BonusAmt { set; get; }
        public int? RewTypeID { set; get; }
        public DateTime? registration_time { set; get; }
        public int? ActID { set; get; }
        public string borrowingTitle { set; get; }

        public Decimal? investment_amount { set; get; }
        public DateTime? invest_time { set; get; }

    }
}
