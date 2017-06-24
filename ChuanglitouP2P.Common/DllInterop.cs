using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ChuanglitouP2P.Common
{
    public class DllInterop
    {

        [DllImport("pnrpay.dll", EntryPoint = "SignMsg")]
        public static extern int SignMsg(string MerId, string MerPubFile, string MsgData, int MsgLen, StringBuilder chkVal);

        [DllImport("pnrpay.dll", EntryPoint = "VeriSignMsg")]
        public static extern int VeriSignMsg(string PgKeyFile, string MsgData, int MsgLen, string ChkValue);


    }
}
