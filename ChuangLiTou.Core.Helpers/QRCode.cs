using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace ChuangLiTou.Core.Helpers
{
    /// <summary>
    /// 生成带图片的二维码
    /// </summary>
    public class QRCode
    {
        #region 生成带图片的二维码
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static Image CreateImage(string link)
        {
            var encoder = new QRCodeEncoder();
            String encoding = "Byte";
            if (encoding == "Byte")
            {
                encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            }
            else if (encoding == "AlphaNumeric")
            {
                encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            }
            else if (encoding == "Numeric")
            {
                encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
            }
            try
            {
                int scale = Convert.ToInt16(4);
                encoder.QRCodeScale = scale;
            }
            catch (Exception ex)
            {
                //"大小参数错误!");
                return null;
            }
            try
            {
                int version = Convert.ToInt16(7);
                encoder.QRCodeVersion = version;
            }
            catch (Exception ex)
            {
                //"版本参数错误 !");
                return null;
            }

            string errorCorrect = "M";
            if (errorCorrect == "L")
                encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            else if (errorCorrect == "M")
                encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            else if (errorCorrect == "Q")
                encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
            else if (errorCorrect == "H")
                encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;

            String data = link;

            Image image = encoder.Encode(data);
            return image;
        }
        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="imgBack"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Image CombinImage(Image imgBack, Image img)
        {
            if (img.Height != 50 || img.Width != 50)
            {
                img = ResizeImage(img, 50, 50, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);   
            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框  
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);  
            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }

        /// <summary>
        /// 调用此函数后使此两种图片合并，类似相册，有个
        /// 背景图，中间贴自己的目标图片
        /// </summary>
        /// <param name="imgBack">粘贴的源图片</param>
        /// <param name="destImg">粘贴的目标图片</param>
        public static Image CombinImage(Image imgBack, string destImg)
        {
            Image img = Image.FromFile(destImg);
            return CombinImage(imgBack, img);
        }

        /// <summary>
        /// Resize图片
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <param name="mode">保留着，暂时未用</param>
        /// <returns>处理以后的图片</returns>
        public static Image ResizeImage(Image bmp, int newW, int newH, int mode)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量  
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height),
                            GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
