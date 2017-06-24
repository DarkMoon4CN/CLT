using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;

namespace HX.QRCode
{
    public  class QRCode
    {

        /*
              程序调用实例

             string str = TextBox1.Text;  //二维码生成的内容
             string path = System.Web.HttpContext.Current.Server.MapPath("images");
             string Logopath = System.Web.HttpContext.Current.Server.MapPath("images/logo.jpg");  //中间logo
             string file = path + "\\code.png";
             string BGColorValue = BG_ColorValue.Value; //背景颜色  #ffffff
             string FiColorValue = Fi_ColorValue.Value; //前景颜色  #293955

             if (ToQRCode(str, file, BGColorValue, FiColorValue, Logopath))
             {
                 Image1.ImageUrl = "images/code.png";
             }

        */




        #region 生成普通二维码
        /// <summary>
        /// 生成普通二维码
        /// </summary>
        /// <param name="str">信息字符串</param>
        /// <param name="filename">二维图标保存完整路径</param>
        /// <param name="BGColorValue">背景色</param>
        /// <param name="FiColorValue">前景色</param>
        /// <returns></returns>
        public bool ToQRCode(string str, string filename, string BGColorValue, string FiColorValue,int scale=8,int version=8)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方法
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//纠错级别
            qrCodeEncoder.QRCodeVersion = version;//大小
            qrCodeEncoder.QRCodeScale = scale;//版本
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.ColorTranslator.FromHtml(FiColorValue);
            qrCodeEncoder.QRCodeBackgroundColor = System.Drawing.ColorTranslator.FromHtml(BGColorValue);
            //生成图像
            Bitmap QRCodeimage = qrCodeEncoder.Encode(str, Encoding.UTF8);
            QRCodeimage.Save(filename);
            QRCodeimage.Dispose();
            return true;
        }
        #endregion





        #region 生成中间有logo二维码
        /// <summary>
        /// 生成中间有logo二维码
        /// </summary>
        /// <param name="str">信息字符串</param>
        /// <param name="filename">二维图标保存完整路径</param>
        /// <param name="BGColorValue">背景色</param>
        /// <param name="FiColorValue">前景色</param>
        /// <param name="centerlogo">中心图标完整路径</param>
        /// <returns></returns>
        public bool ToQRCode(string str, string filename, string BGColorValue, string FiColorValue, string centerlogo)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方法
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//纠错级别
            qrCodeEncoder.QRCodeVersion = 8;//大小
            qrCodeEncoder.QRCodeScale = 8;//版本
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.ColorTranslator.FromHtml(FiColorValue);
            qrCodeEncoder.QRCodeBackgroundColor = System.Drawing.ColorTranslator.FromHtml(BGColorValue);
            //生成图像
            Bitmap QRCodeimage = qrCodeEncoder.Encode(str, Encoding.UTF8);

            //将image保存到filename
            //QRCodeimage.Save(filename);
            QRCodeimage = CombinImage(QRCodeimage, centerlogo);          //加logo
            QRCodeimage.Save(filename);
            QRCodeimage.Dispose();
            return true;
        }
        #endregion



        #region 调用此函数后使此两种图片合并，类似相册，有个背景图，中间贴自己的目标图片
        /// <summary>  
        /// 调用此函数后使此两种图片合并，类似相册，有个  
        /// 背景图，中间贴自己的目标图片  
        /// </summary>  
        /// <param name="imgBack">粘贴的源图片  
        /// <param name="destImg">粘贴的目标图片路径
        public System.Drawing.Bitmap CombinImage(System.Drawing.Bitmap imgBack, string destImg)
        {
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(destImg);        //照片图片    
            if (img.Height != 65 || img.Width != 65)
            {
                img = KiResizeImage(img, 65, 65);
            }
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);      //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);   

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框  

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);  

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }
        #endregion




        #region Resize图片  
        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap  
        /// <param name="newW">新的宽度  
        /// <param name="newH">新的高度  
        /// <returns>处理以后的图片</returns>  
        public System.Drawing.Bitmap KiResizeImage(System.Drawing.Bitmap bmp, int newW, int newH)
        {
            try
            {
                System.Drawing.Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量  
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
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
