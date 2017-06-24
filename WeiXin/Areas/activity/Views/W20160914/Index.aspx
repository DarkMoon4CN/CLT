<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CWX.activity._20160914.Index" %>

<%@ Import Namespace="ChuanglitouP2P.Model" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>一元抢IPhone</title>
    <meta name="keywords" content="新手理财活动,投资理财活动,理财送现金，创利投活动，创利投" />
    <meta name="description" content="创利投推出新手理财送好礼，现金红包送不停活动，活动期间新用户注册就送188元，首次投资成功再送您888元，挂牌上市企业创利投等您来赚钱！" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <link rel="stylesheet" href="/Areas/activity/Sources/20160914/css/css.css" />
       <link href="/Areas/activity/Sources/20160914/css/common.css" type="text/css" rel="stylesheet" />
    <script src="/Areas/activity/Sources/20160914/js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="/Areas/activity/Sources/20160914/js/awardRotate.js"></script>

    <script type="text/javascript" src="/js/Validform_v5.3.2_min.js"></script>
    <script src="/js/layer/layer.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="/Areas/activity/Sources/20160910/js/scroll_js.js"></script>

    <script type="text/javascript">
        var phoneWidth = parseInt(window.screen.width);
        var phoneScale = phoneWidth / 640;
        var ua = navigator.userAgent;
        if (/Android (\d+\.\d+)/.test(ua)) {
            if (phoneWidth > 640) {
                document.write('<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">');
            }
        } else {
            document.write('<meta name="viewport" content="width=device-width, user-scalable=no, target-densitydpi=device-dpi">');
        }

    </script>
    <script>
        (function (doc, win) { var docEl = doc.documentElement, recalc = function () { var clientWidth = docEl.clientWidth; if (!clientWidth) { return } var w = 20 * (clientWidth / 320); if (w > 40) { w = 40 } docEl.style.fontSize = w + "px" }; if (!doc.addEventListener) { return } if ("orientationchange" in window) { win.addEventListener("orientationchange", recalc, false) } win.addEventListener("resize", recalc, false); win.addEventListener("load", recalc, false); doc.addEventListener("DOMContentLoaded", recalc, false); recalc() })(document, window);
    </script>

    <script type="text/javascript">
        var editwindow;
        $(function () {
            $(".registerform").Validform({
                tiptype: 4,
                ajaxPost: true,
                callback: function (data) {
                    if (data.rs == "y") {
                        tprm = "userid=" + data.uid.toString();
                        __ozfac2(tprm, "#regSuccess");
                        layer.msg("注册成功，页面正在转向...", 3, 1);
                        location.href = data.url;
                    }
                    else {
                        layer.msg("" + data.error + "", 1, 5);
                    }
                }
            });

            $.extend($.Datatype, {
                "z2-4": /^[\u4E00-\u9FA5\uf900-\ufa2d]{2,4}$/,
                "d": /^(\d{4})\-(\d{2})\-(\d{2})$/,
                "day1": /^[1-2]\d{0,1}$|^28$/,
                "mobile": /^0?(13[0-9]|15[012356789]|18[0123456789]|17[0123456789]|14[57])[0-9]{8}$/,
                "regex": /^[^\u4E00-\u9FA5]{3,20}$/,
                "regex1": /^[0-9A-Za-z]{6,25}$/

            });

            $(document).keyup(function (event) {
                if (event.keyCode == 13) {
                    $("#butreg").trigger("click");
                }
            });


            $("#vsmscode").click(function () {
                var mobile = $("#mobile").val();
                if (mobile == null || mobile.length <= 0) {
                    layer.alert("手机号不能为空");
                    return false;
                }
                else {
                    var re = /^0?(13[0-9]|15[012356789]|18[0123456789]|17[0-9]|14[57])[0-9]{8}$/;
                    if (re.test(mobile) == false) {
                        layer.alert("手机号格式不对!");
                        $("#mobile").val("");
                        return false;
                    }
                }
                $.post("/register/checkmobile", { "param": mobile }, function (msg) {
                    if (msg == "手机号已被注册!") {
                        layer.alert(msg);
                    }
                    else {
                        $.post("/register/vsmscode", { "mobile": mobile }, function (msg) {
                            var data = JSON.parse(msg);
                            if (data.rs == "y") {
                                settime1();
                                layer.msg("" + data.info + "", 5, 1);
                            }
                            else {
                                if (data.info == "v") {
                                    df("ad");
                                } else {
                                    layer.msg("" + data.info + "", 2, 5);
                                    $("#mobile").val("");
                                }
                            }
                        });
                    }
                });
            });

            $("#vcodec").click(function () {
                var mobilec = $("#mobile").val();
                var urlc = "/register/GetValidateCode?&mobile=" + mobilec;
                $("#vcodec").attr("ajaxurl", urlc);
            });


            $("#smscode").click(function () {
                var mobile = $("#mobile").val();

                if (mobile == null || mobile.length <= 0) {
                    layer.alert("手机号不能为空");
                    return false;
                }
                else {
                    var re = /^0?(13[0-9]|15[012356789]|18[0123456789]|17[0-9]|14[57])[0-9]{8}$/;
                    if (re.test(mobile) == false) {
                        layer.alert("手机号格式不对!");
                        $("#mobile").val("");
                        return false;
                    }
                }
                $.post("/register.html?action=mobile&method=post", { "param": mobile }, function (msg) {
                    if (msg == "手机号已被注册!") {
                        layer.alert(msg);
                    }
                    else {
                        $.post("/register/vsmscode", { "mobile": mobile }, function (msg) {
                            var data = JSON.parse(msg);
                            if (data.rs == "y") {
                                settime();
                                layer.msg("" + data.info + "", 2, 1);
                            }
                            else {
                                if (data.info == "v") {
                                    df("sms");
                                } else {
                                    layer.msg("" + data.info + "", 2, 5);
                                    $("#mobile").val("");
                                }
                            }
                        });
                    }
                }
                );
            });

        });

        function changeImg() {
            $("#image1").attr("src", "/Validate.html?a=" + Math.random());
        }

        function validatetip(n) {
            if (n == "1") {
                layer.alert("验证码过期！", 0, "系统提示");
            } else if (n == "2") {
                layer.alert("验证码不对！", 0, "系统提示");
            } else if (n == "3") {
                layer.alert("注册失败！", 0, "系统提示");
            }
        }

        var countdown = 60;
        function settime() {
            if (countdown == 1) {
                $("#smscode").removeAttr("disabled");
                $("#smscode").val("发送验证码");
                $("#yuyin_code").slideDown();
                countdown = 60;
            } else {
                $("#smscode").attr("disabled", true);
                $("#smscode").val("" + countdown + "秒后可重发");
                countdown--;
                if (countdown > 0) {
                    setTimeout(function () {
                        settime();
                    }, 1000);
                }
            }
        }
        var countdown1 = 60;
        function settime1() {
            if (countdown1 == 1) {
                $("#vsmscode").removeAttr("disabled");
                $("#vsmscode").text("获取语音验证码");
                $("#yuyin_code").slideUp();
                countdown1 = 60;

            } else {
                $("#vsmscode").attr("disabled", true);
                $("#vsmscode").text("" + countdown1 + "秒后可重获取语音验证码");
                countdown1--;

                if (countdown1 > 0) {
                    setTimeout(function () {
                        settime1();
                    }, 1000);
                }
            }
        }

        function df(sms) {
            var pageii = $.layer({
                type: 1,
                title: false,
                area: ['700px', '460px'],
                border: [0],
                closeBtn: [0, false],
                shift: 'top',
                bgcolor: '',
                page: {

                    html: '<div class="reg_pop_wrap" id="vcodet"> <div class="reg_pop_box" style=" width:14.25rem; height:9.425rem; background:#ffcc01; position:fixed; left:50%; top:50%; z-index:99; margin:-4.7rem 0 0 -7.125rem;border-radius:0.5rem;"> <div style="font-size:0.9rem;color:#333;text-align:center;padding-top:1.15rem;">填写验证码完成短信发送</div> <div style="width:100%; float:left; margin:1rem 0 1rem 0;"> <span style="float:left;height:2rem;line-height:2rem;color:#fff;border-radius:0.5rem;margin:0 .75rem 0 1rem;background:#e4b600;font-size:.75rem;padding-left:.6rem;">验证码：<input id="Validatecode" name="" type="text" style="width:4rem; background:none;line-height:1.5rem; border:0px; color:#fff; outline:none; text-indent:5px;" /></span><span style="float:left;"><img id="image1" src="/Validate.html?a=&lt;%=rndstr%&gt;" style="cursor:pointer;width:2.6rem;height:1.1rem;margin-top:0.5rem;border:hidden;" onclick="javascript:changeImg();" /></span></div> <div><input id="codebtn" name="" value="发送短信" type="button" style="width:5.5rem; height:1.75rem; font-size:0.75rem; color:#333; text-align:center; background:#ffdf4c; border:2px solid #816a09; cursor:pointer; margin:0 0 0 1rem;font-family:microsoft YaHei;border-radius:0.5rem;" /> <input id="pagebtn" name="" value="关闭窗口" type="button" style="width:5.5rem; height:1.75rem; font-size:0.75rem; color:#333; text-align:center; background:#ffdf4c; border:2px solid #816a09; cursor:pointer; margin:0 0 0 1rem;font-family:microsoft YaHei;border-radius:0.5rem;" /></div> </div></div>'
                }
            });
            $('#pagebtn').on('click', function () {
                layer.close(pageii);
            });
            $('#codebtn').on('click', function () {
                var validvcode = $('#Validatecode').val();

                var mobile = $("#mobile").val();

                if (validvcode == null || validvcode.length <= 0) {
                    layer.alert("验证码不能为空");
                    return false;
                }
                if (mobile == null || mobile.length <= 0) {
                    layer.alert("手机号不能为空");
                    return false;
                } else {
                    var re = /^0?(13[0-9]|15[012356789]|18[0123456789]|17[0-9]|14[57])[0-9]{8}$/;
                    if (re.test(mobile) == false) {
                        layer.alert("手机号格式不对!");
                        $("#mobile").val("");
                        return false;
                    }
                }
                $.post("/checkregister.html?action=mobile&method=post", { "param": mobile }, function (msg) {

                    if (msg == "手机号已被注册!") {
                        layer.alert(msg);
                    } else {
                        if (sms == "ad") {
                            $.post("/usercenter/usercenter.html?action=vsmscode&method=post", { "mobile": mobile, "vcode": validvcode }, function (msg) {
                                var data = JSON.parse(msg);
                                if (data.rs == "y") {
                                    settime1();
                                    layer.close(pageii);
                                    layer.msg("" + data.info + "", 5, 1);
                                } else {
                                    layer.msg("" + data.info + "", 2, 5);
                                }

                            });

                        } else {
                            $.post("/usercenter/usercenter.html?action=Regsmscode&method=post", { "mobile": mobile, "vcode": validvcode }, function (msg) {
                                var data = JSON.parse(msg);
                                if (data.rs == "y") {
                                    settime();
                                    layer.close(pageii);
                                    layer.msg("" + data.info + "", 2, 1);
                                } else {
                                    if (data.info == "v") {
                                        layer.close(pageii);
                                        df("sms");
                                    } else {
                                        layer.msg("" + data.info + "", 2, 5);
                                    }
                                }

                            });
                        }
                    }

                }
                );
            });

            changeImg();
        }


        function changeImg() {
            $("#image1").attr("src", "/Validate.html?a=" + Math.random());
        }

    </script>
</head>
<body>
    <header class="header">
        <div class="logo"><a href="http://m.chuanglitou.cn/">
            <img src="./images/wx_logo.jpg" alt="创利投"></a></div>
        <div class="header_link">

            <a style="font-size: 12px;" href="http://m.chuanglitou.cn/register.html">注册</a>  |  <a href="http://m.chuanglitou.cn/login.html" style="font-size: 12px;">登录</a>




        </div>
    </header>
    <div class="wiPhonetuiguang">
        <div class="wiPhonetuiguang_01">
            <img src="/Areas/activity/Sources/20160914/images/wiPhonetuiguang_01.jpg" class="wiPhonetuiguang_bg1">
        </div>
        <div class="wiPhonetuiguang_02">
            <img src="/Areas/activity/Sources/20160914/images/wiPhonetuiguang_02.jpg" class="wiPhonetuiguang_bg2">
            <div class="wiPhonetuiguang_02_main">
                <div class="wiPhonetuiguang_02_main_top">
                    <p>当前加入<span><%=getLJJRCount() %></span>人</p>
                    <%--  <p>我的加入排名<span>12345</span>名</p>--%>
                </div>
                <div class="wiPhonetuiguang_02_main_bottom">
                    <div class="wiPhonescroll_cont">
                        <%
                            List<M_GrabIphone> GILists = getCYLIst();
                            if (GILists != null)
                            {
                        %>
                        <ul class="wiPhonegd_cont" id="xstCont" style="overflow: hidden;">
                            <%
                                foreach (M_GrabIphone item in GILists)
                                {
                                    M_member_table mt = getMemberInfo(item.RegrsterID);
                                    if (mt != null)
                                    {
                                        string name = "";
                                        if (mt.username.Length == 11)
                                        {
                                            string k = mt.username.Substring(0, 1);
                                            string d = mt.username.Substring(mt.username.Length - 1, 1);
                                            name = k + "***" + d;
                                        }
                            %>
                            <li>
                                <p><span><%=name %></span><span>投资</span><span><%=item.InvestmentAmount %>元</span></p>
                            </li>
                            <%
                                    }
                                }

                            %>
                        </ul>
                        <%	} %>
                    </div>
                </div>
            </div>
        </div>
        <div class="wiPhonetuiguang_03">
            <div class="zc_n_form">
                <form id="form2" class="registerform" action="/checkregister.html" runat="server" method="post">
                    <input type="hidden" id="register" name="action" value="register" />
                    <ul>
                        <li class="zc_form zc_form_01">
                            <div class="tticon_left">
                                <i></i>
                            </div>
                            <input name="mobile" id="mobile" class="form_input" type="tel" placeholder="请输入您的手机号" ajaxurl="/checkregister.html?action=mobile" datatype="mobile" nullmsg="请填写手机号！" errormsg="手机号格式不对！" />
                            <p>请填写手机号！</p>
                        </li>
                        <li class="zc_form zc_form_02">
                            <div class="tticon_left">
                                <i></i>
                            </div>
                            <input name="vcodec" id="vcodec" class="form_input" type="text" ajaxurl="/checkregister.html?action=vcodec" datatype="*" nullmsg="请填写验证码！" class="form_text" placeholder="请填写验证码" /><input name="smscode" id="smscode" class="zc_button" type="button" value="发送验证码" />
                            <p>请填写验证码！</p>
                        </li>
                        <li class="zc_form zc_form_03">
                            <div class="tticon_left">
                                <i></i>
                            </div>
                            <input name="userpassword" id="userpassword" runat="server" class="form_input" type="password" placeholder="请设置密码" datatype="*6-16" nullmsg="请设置密码！" errormsg="密码范围在6~16位之间！" />
                            <p>请设置密码！</p>
                        </li>
                        <li class="zc_form zc_form_04">
                            <div class="tticon_left">
                                <i></i>
                            </div>
                            <input name="reuserpassword" id="reuserpassword" runat="server" class="form_input" type="password" placeholder="请重复密码" datatype="*" recheck="userpassword" nullmsg="请再输入一次密码！" errormsg="您两次输入的账号密码不一致！" />
                            <p>请再输入一次密码！</p>
                        </li>
                        <li class="zc_form_07">
                            <!-- <input type="radio" id="dian"><label form="dian">我已阅读并接受<small>《创利投注册协议》</small></label> -->
                            <span>已有账号？<a href="/login.aspx">马上登录</a></span>
                        </li>
                        <li class="zc_form_btn">
                            <input type="submit" name="butreg" id="butreg" value="" /></li>
                    </ul>
                </form>
            </div>
        </div>
        <div class="wiPhonetuiguang_04">
            <img src="/Areas/activity/Sources/20160914/images/wiPhonetuiguang_04.jpg" class="wiPhonetuiguang_bg4">
            <img src="/Areas/activity/Sources/20160914/images/wiPhonetuiguang_06.png" class="wiPhonetuiguang_04_main_01">
            <img src="/Areas/activity/Sources/20160914/images/wiPhonetuiguang_07.png" class="wiPhonetuiguang_04_main_02">
        </div>
        <div class="wiPhonetuiguang_05">
            <img src="/Areas/activity/Sources/20160914/images/wiPhonetuiguang_05.jpg" class="wiPhonetuiguang_bg5">
            <div class="wiPhonetuiguang_05_main_top">
                <dl>
                    <dt>活动规则</dt>
                    <dd>1、本活动仅限新注册的用户，每个用户仅限有一次参与机会。</dd>
                    <dd>2、本活动投资仅限6月标参与。</dd>
                    <dd>3、同一用户多次投资只计算一次抽奖机会。</dd>
                    <dd>4、满5388人可进行一次抽奖，参与人数满5388后，开始进行随机抽奖，上轮抽奖结束后开始累计新一轮计算。</dd>
                    <dd>5、肾7新品于2016年9月7日发布，我平台将根据新品起售时间进行奖品发放， 发放详情会在苹果官网发售后与获奖者沟通发放，活动奖品将于单轮抽奖结束后的3个工作日内由创利投平台客服核对中奖信息。</dd>
                </dl>
            </div>
        </div>
        <div class="wiPhonetuiguang_05_main_bottom">
            <p>
                <img src="/Areas/activity/Sources/20160914/images/wiPhonetuiguang_08.png" />客服电话：010-53732056（工作时间:9:00-18:00）
            </p>
            <p>北京创利投网络科技有限公司  京ICP备14060741</p>
            <small>市场有风险，投资须谨慎</small>
        </div>
    </div>

    <div class="fixed_Bar">
        <div class="BarCentert">
            <a href="http://m.chuanglitou.cn/register.html">马上注册</a>
        </div>
    </div>
    <script type="text/javascript">
        var cnzz_protocol = (("https:" == document.location.protocol) ? " https://" : " http://"); document.write(unescape("%3Cspan id='cnzz_stat_icon_1254620195'%3E%3C/span%3E%3Cscript src='" + cnzz_protocol + "s95.cnzz.com/z_stat.php%3Fid%3D1254620195%26show%3Dpic' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <span id="cnzz_stat_icon_1254620195">
        <a href="http://www.cnzz.com/stat/website.php?web_id=1254620195" target="_blank" title="站长统计"></a>
    </span>
    <script type="text/html">
        function test(){
	
	
	$(".zc_form input").focus(function(){
				$(this).blur(function(){ 
					var a = $(this).val();		
					if(a){
						$(this).siblings('p').css("display","none")
					}else{
						$(this).siblings('p').css("display","block")
					}
				})
			})
}
test()
    </script>
    <script src="/Areas/activity/Sources/20160914/js/o_code.js"></script>
</body>
</html>
