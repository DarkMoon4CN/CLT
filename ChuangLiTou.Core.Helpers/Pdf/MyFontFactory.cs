using System;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ChuangLiTou.Core.Helpers.Pdf
{
    public class MyFontFactory : IFontProvider
    {

        public Font GetFont(String fontname, String encoding, Boolean embedded, float size, int style, BaseColor color)
        {
            /*
            if (fontname == "微软雅黑")
            {
                string fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\LCPI\\Fonts\\MSYH.ttf";
                BaseFont bf3 = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font fontContent = new Font(bf3, size, style, color);
                return fontContent;
            }
            else
            {
                Font fontContent = FontFactory.GetFont(fontname, size, style, color);
                return fontContent;
            }*/

            //BaseFont _bfSun = BaseFont.CreateFont(@"c:\Windows\fonts\SIMSUN.TTC,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            //return _bfSun;

            string fontpath = @"c:\Windows\fonts\SIMSUN.TTC,1";
            BaseFont bf3 = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font fontContent = new Font(bf3, size, style, color);
            return fontContent;
        }

        public Boolean IsRegistered(String fontname)
        {
            return false;
        }


    }
}
