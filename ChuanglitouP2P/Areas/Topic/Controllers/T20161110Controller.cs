using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Topic.Controllers
{
    public class T20161110Controller : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        private Dictionary<int, string> dict = new Dictionary<int, string>();

        public T20161110Controller()
        {
            #region 过期代码
            dict.Add(0, "努力投资吧,您离 iPhone 7 Plus 玫瑰金 128G 又近了一步");
            dict.Add(1, "3M 9501V KN95防护口罩3只/包");
            dict.Add(2, "罗技（Logitech） B175 商用无线鼠标 黑色");
            dict.Add(3, "飞利浦挂烫机GC502/28");
            dict.Add(4, "东菱（Donlim） 面包机");
            dict.Add(5, "夏普空气净化器 KC-WE20-W");
            dict.Add(6, "Beats Studio Wireless 录音师无线蓝牙版头戴式耳机 HI-FI降噪带麦 黑色");
            dict.Add(7, "华为 P9 全网通 4GB+64GB版 托帕蓝");
            dict.Add(8, "苹果iPhone 7 Plus 玫瑰金 128G 标配");
            #endregion
        }
        // GET: Topic/T20161018
        public ActionResult Index()
        {

            int userid = Utils.checkloginsessiontop();
            if (userid > 0)
            {
                string userUrl = string.Empty;
                hx_member_table hUser = null;
                if (userid == 0)
                {
                    userUrl = "/login.html";
                }
                else
                {
                    hUser = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();
                }
                var uid = userid;
                var src = userUrl != string.Empty ? userUrl : Utils.GetRe_url("Invitation/" + hUser.invitedcode + ".html");
                var url = Server.UrlEncode(src);
                var desc = Server.UrlEncode(string.Format("邀请好友来投资，双方都有奖励奖励无上限！送3重豪礼不限量 iPhone7 plus等你拿！复制您的“邀请链接”,通过QQ,微信,微博等方式发给好友,好友通过“邀请链接”注册！{0}", src));
                var title = Server.UrlEncode("邀请好友来投资,双方都有奖励奖励无上限！送3重豪礼不限量 iPhone7 plus等你拿！！");
                var img = Server.UrlEncode(Utils.GetRe_url("images/logo.jpg"));
                var site = Server.UrlEncode("创利投");
                var qzone = userUrl != string.Empty ? userUrl : string.Format("http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url={0}&desc={1}&title={2}&pics={3}&site={4}", url, desc, desc, img, site);
                var weibo = userUrl != string.Empty ? userUrl : string.Format("http://v.t.sina.com.cn/share/share.php?appkey=&&source=&content={0}&url={1}&title={2}&pic={3}&site={4}", desc, url, desc, img, site);
                var QQ = userUrl != string.Empty ? userUrl : string.Format("http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?to=pengyou&url={0}&desc={1}&title={2}&pics={3}&site={4} title = '分享到朋友社区'", url, desc, desc, img, site);
                QQ = userUrl != string.Empty ? userUrl : string.Format("http://connect.qq.com/widget/shareqq/index.html?url={0}&title={1}&summary={2}&site={3}&pics={4}", url, desc, desc, site, img);
                var doubai = userUrl != string.Empty ? userUrl : string.Format("http://www.douban.com/recommend/?url={0}&desc={1}&title={2}&pics={3}&site={4}", url, desc, desc, img, site);
                var QQweibo = userUrl != string.Empty ? userUrl : string.Format("http://v.t.qq.com/share/share.php?title={0}&url={1}&appkey={2}&site={3}&pic={4}", desc, url, "125522", site, img);
                var newWxUrl = string.Empty;
                if (Utils.GetAppSetting("DeBug") == "1")
                {
                    newWxUrl = userUrl != string.Empty ? userUrl : Utils.GetAppSetting("MDeBugURL") + "register/index?invitedcode=" + hUser.invitedcode;
                }
                else
                {
                    newWxUrl = userUrl != string.Empty ? userUrl : Utils.GetAppSetting("MReleaseURL") + "register/index?invitedcode=" + hUser.invitedcode;
                }
                ViewBag.NewWxUrl = newWxUrl;
                ViewBag.QQweibo = QQweibo;
                ViewBag.doubai = doubai;
                ViewBag.QQ = QQ;

                ViewBag.qzone = qzone;
                ViewBag.weibo = weibo;
                ViewBag.UserUrl = userUrl;
                ViewBag.codes = userUrl != string.Empty ? string.Empty : Utils.GetRe_url("Invitation/" + hUser.invitedcode + ".html");
                ViewBag.HUsr = hUser;
            }
            else
            {

                string userUrl = string.Empty;
                var src = userUrl != string.Empty ? userUrl : Utils.GetRe_url("20161110.html");
                var url = Server.UrlEncode(src);
                var desc = Server.UrlEncode(string.Format("邀请好友来投资，双方都有奖励奖励无上限！送3重豪礼不限量 iPhone7 plus等你拿！复制您的“邀请链接”,通过QQ,微信,微博等方式发给好友,好友通过“邀请链接”注册！{0}", src));
                var title = Server.UrlEncode("邀请好友来投资,双方都有奖励奖励无上限！送3重豪礼不限量 iPhone7 plus等你拿！！");
                var img = Server.UrlEncode(Utils.GetRe_url("images/logo.jpg"));
                var site = Server.UrlEncode("创利投");
                var qzone = userUrl != string.Empty ? userUrl : string.Format("http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url={0}&desc={1}&title={2}&pics={3}&site={4}", url, desc, desc, img, site);
                var weibo = userUrl != string.Empty ? userUrl : string.Format("http://v.t.sina.com.cn/share/share.php?appkey=&&source=&content={0}&url={1}&title={2}&pic={3}&site={4}", desc, url, desc, img, site);
                var QQ = userUrl != string.Empty ? userUrl : string.Format("http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?to=pengyou&url={0}&desc={1}&title={2}&pics={3}&site={4} title = '分享到朋友社区'", url, desc, desc, img, site);
                QQ = userUrl != string.Empty ? userUrl : string.Format("http://connect.qq.com/widget/shareqq/index.html?url={0}&title={1}&summary={2}&site={3}&pics={4}", url, desc, desc, site, img);
                var doubai = userUrl != string.Empty ? userUrl : string.Format("http://www.douban.com/recommend/?url={0}&desc={1}&title={2}&pics={3}&site={4}", url, desc, desc, img, site);
                var QQweibo = userUrl != string.Empty ? userUrl : string.Format("http://v.t.qq.com/share/share.php?title={0}&url={1}&appkey={2}&site={3}&pic={4}", desc, url, "125522", site, img);
                var newWxUrl = string.Empty;
                if (Utils.GetAppSetting("DeBug") == "1")
                {
                    newWxUrl = userUrl != string.Empty ? userUrl : Utils.GetAppSetting("MDeBugURL") + "register/index";
                }
                else
                {
                    newWxUrl = userUrl != string.Empty ? userUrl : Utils.GetAppSetting("MReleaseURL") + "register/index";
                }
                ViewBag.NewWxUrl = newWxUrl;
                ViewBag.QQweibo = QQweibo;
                ViewBag.doubai = doubai;
                ViewBag.QQ = QQ;

                ViewBag.qzone = qzone;
                ViewBag.weibo = weibo;
                ViewBag.UserUrl = userUrl;
                ViewBag.codes = userUrl != string.Empty ? string.Empty : Utils.GetRe_url("");

            }
            return View();
        }

        #region 过期代码
        private string ActivityReward(int key)
        {
            return dict[key];
        }
        private int AmountRange(decimal amount = 0)
        {

            if (amount >= 5000 && amount <= 9999)
            {
                return 1;
            }
            else if (amount >= 10000 && amount <= 29999)
            {
                return 2;
            }
            else if (amount >= 30000 && amount <= 49999)
            {
                return 3;
            }
            else if (amount >= 50000 && amount <= 99999)
            {
                return 4;
            }
            else if (amount >= 100000 && amount <= 199999)
            {
                return 5;
            }
            else if (amount >= 200000 && amount <= 299999)
            {
                return 6;
            }
            else if (amount >= 300000 && amount <= 599999)
            {
                return 7;
            }
            else if (amount >= 600000)
            {
                return 8;
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}