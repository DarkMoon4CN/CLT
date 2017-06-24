using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model
{
    public class M_LLPay
    {


        private string acct_name;

        public string Acct_name
        {
            get { return acct_name; }
            set { acct_name = value; }
        }

        private string card_no;

        public string Card_no
        {
            get { return card_no; }
            set { card_no = value; }
        }

        private string no_order;

        public string No_order
        {
            get { return no_order; }
            set { no_order = value; }
        }

        private string dt_order;

        public string Dt_order
        {
            get { return dt_order; }
            set { dt_order = value; }
        }

        private string money_order;

        public string Money_order
        {
            get { return money_order; }
            set { money_order = value; }
        }

        private string city_code;

        public string City_code
        {
            get { return city_code; }
            set { city_code = value; }
        }

        private string bank_code;

        public string Bank_code
        {
            get { return bank_code; }
            set { bank_code = value; }
        }

        private string brabank_name;

        public string Brabank_name
        {
            get { return brabank_name; }
            set { brabank_name = value; }
        }



    }
}
