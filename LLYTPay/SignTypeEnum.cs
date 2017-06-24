using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLYTPay
{

    public enum SignTypeEnum
    {
        RSA, MD5
    }

    public class SignTypeEnumClass
    {
        public static string getCode(SignTypeEnum x)
        {
            switch (x)
            {
                case SignTypeEnum.MD5:
                    return "MD5";
                case SignTypeEnum.RSA:
                    return "RSA";
                default:
                    return "";
            }
        }
    }
}
