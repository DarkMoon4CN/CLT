using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChuanglitouP2P.BLL.EF
{
    public class GetCacheNesList
    {
        chuangtouEntities ef = new chuangtouEntities();

        /// <summary>
        /// 获取新闻V_type_news 列表
        /// </summary>
        /// <param name="typenews">新闻类型</param>
        /// <param name="top">条数</param>
        /// <param name="isimg">是否只获取有图片的新闻 大于0的数是获取图片内容</param>
        /// <returns></returns>
        public List<V_type_news> GetNews(int typenews, int top = 3, int isimg = 0,int indexcomm=0)
        {
            List<V_type_news> vtn = new List<V_type_news>();

            string key = "NewsList" + typenews.ToString() + top.ToString() + isimg.ToString();

            if (HttpRuntime.Cache[key] == null)
            {
                if (indexcomm > 0)
                {
                    if (isimg > 0)
                    {
                        vtn = ef.V_type_news.Where(p => p.web_Type_menu_id == typenews && p.newimg != null && p.newimg != "" && p.comm==indexcomm).OrderByDescending(p => p.newid).Take(top).ToList();
                    }
                    else
                    {
                        vtn = ef.V_type_news.Where(p => p.web_Type_menu_id == typenews && p.comm == indexcomm).OrderByDescending(p => p.newid).Take(top).ToList();
                    }

                }
                else
                {
                    if (isimg > 0)
                    {
                        vtn = ef.V_type_news.Where(p => p.web_Type_menu_id == typenews && p.newimg != null && p.newimg != "").OrderByDescending(p => p.newid).Take(top).ToList();
                    }
                    else
                    {
                        vtn = ef.V_type_news.Where(p => p.web_Type_menu_id == typenews).OrderByDescending(p => p.newid).Take(top).ToList();
                    }
                }

               

                HttpRuntime.Cache.Add(key, vtn, null, DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                return vtn;

            }
            else
            {
                vtn = HttpRuntime.Cache[key] as List<V_type_news>;

            }
            return vtn;
        }
    }
}
