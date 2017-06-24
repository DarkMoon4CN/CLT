using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Helpers.Pdf;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using ChuanglitouP2P.Common;
namespace ChuangLiTou.Core.Helpers
{
    public class PdfHelper
    {



        public static Boolean HTMLToPDF(string html, String fileName,string createTime="")
        {
            if (createTime == "")
            {
                createTime=DateTime.Now.ToString("yyyy-MM-dd");
            }
            Boolean isOK = false;
            try
            {
                //  FontFactory.RegisterFamily("宋体", "simsun", @"c:\windows\fonts\SIMSUN.TTC,0");
                TextReader reader = new StringReader(html);
                // step 1: creation of a document-object
                //  Document document = new Document(PageSize.A4.Rotate(), 30, 30, 30, 30);
                Document document = new Document(PageSize.A4, 30, 30, 36, 36);//左右上下
                // step 2:
                // we create a writer that listens to the document
                // and directs a XML-stream to a file
                fileName = Settings.Instance.PdfRoot+ "\\PDF\\" + fileName + ".pdf";
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                HTMLWorker worker = new HTMLWorker(document);
                document.Open();

                document.AddTitle("创利投网站金融平台");
                document.AddAuthor("创利投");
                document.AddCreationDate();
                document.AddHeader("p2p合同", "p2p合同");
                document.AddCreator("创利投科技发展有限公司");
                document.AddKeywords("P2B合同");
                document.AddSubject("创利投四方合同");
                document.AddProducer();


                writer.PageEvent = new HeaderAndFooterEvent();

                HeaderAndFooterEvent.PAGE_NUMBER = true;//不实现页眉跟页脚  
                First("创利投金融平台网站合同", createTime, document, writer);//封面页  



                worker.StartDocument();
                StyleSheet css = new StyleSheet();



                Dictionary<String, Object> font = new Dictionary<string, object>();
                font.Add(HTMLWorker.FONT_PROVIDER, new MyFontFactory());



                Dictionary<String, String> dict = new Dictionary<string, string>();
                dict.Add(HtmlTags.BGCOLOR, "#01366C");
                dict.Add(HtmlTags.COLOR, "#00ff00");
                dict.Add(HtmlTags.SIZE, "25");
                css.LoadStyle("css", dict);



                List<IElement> p = HTMLWorker.ParseToList(reader, css, font);
                // List<IElement> p = HTMLWorker.ParseToList(reader, css);




                for (int k = 0; k < p.Count; k++)
                {
                    document.Add((IElement)p[k]);


                }

                worker.EndDocument();

                writer.Flush();
                writer.CloseStream = true;
                worker.Close();
                document.Close();
                reader.Close();
                isOK = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                isOK = false;
            }
            finally
            {

            }
            return isOK;
        }


        private static void First(string title, string time, Document doc, PdfWriter writer)
        {
            string tmp = title;
            doc.Add(HeaderAndFooterEvent.InsertTitleContent(tmp));

            //tmp = "(正文     页,附件 0 页)";
            tmp = "(时间: " + time + ")";
            doc.Add(HeaderAndFooterEvent.InsertTitleContent(tmp));

            //模版 显示总共页数  
            HeaderAndFooterEvent.tpl = writer.DirectContent.CreateTemplate(100, 100); //模版的宽度和高度  
            PdfContentByte cb = writer.DirectContent;
            cb.AddTemplate(HeaderAndFooterEvent.tpl, 266, 914);//调节模版显示的位置  
        }

    }
}
