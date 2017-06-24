using System.Web;
using System.Web.Optimization;
using ChuanglitouP2P.Common;

namespace ChuangLiTouOpenApi
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/h52App.js").Include("~/Static/js/h52App.js"));
            bundles.Add(new ScriptBundle("~/helpCenter.js").Include("~/Static/js/helpCenter.js"));
            bundles.Add(new ScriptBundle("~/jquery-1.11.1.min.js").Include("~/Static/js/jquery-1.11.1.min.js"));
            bundles.Add(new ScriptBundle("~/jquery.touchSwipe.min.js").Include("~/Static/js/jquery.touchSwipe.min.js"));
            bundles.Add(new ScriptBundle("~/layer.min.js").Include("~/Static/js/layer.min.js"));

            bundles.Add(new StyleBundle("~/about.css").Include("~/Static/css/about.css"));
            bundles.Add(new StyleBundle("~/anbz.css").Include("~/Static/css/anbz.css"));
            bundles.Add(new StyleBundle("~/css.css").Include("~/Static/css/css.css"));
        }
    }
}