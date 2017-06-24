using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model.VeryCodes.NetCreditAssistant.Model
{
    public enum EnumRepaymentMode
    { /*
        [Description("按月等额本息")]
        EqualPrincipalAndInterest = 1,

       
        [Description("按季等额本金")]
        EqualPrincipalByQuart = 2,*/
        [Description("每月还息，到期还本")]
        InterestByMonth = 3,
        [Description("一次性还本付息")]
        LumpSumPrincipalAndInterest = 4
    }
}
