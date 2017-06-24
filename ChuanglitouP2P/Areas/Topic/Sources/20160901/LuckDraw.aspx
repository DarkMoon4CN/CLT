<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LuckDraw.aspx.cs" Inherits="ChuanglitouP2P.topic._20160901.LuckDraw" %>

<%@ Register Src="/WebUserControl/head.ascx" TagName="head" TagPrefix="uc1" %>
<%@ Register Src="/WebUserControl/bottom.ascx" TagName="bottom" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>幸运大转盘每日抽奖，回馈老用户投资返现-创利投</title>
    <meta name="keywords" content="投资返现，创利投" />
    <meta name="description" content="新用户注册登陆即送抽奖机会，每日登陆日日抽奖，老用户投资奖励无上限，投资越多，返现越多！" />
    <link href="/topic/20160901/Content/css.css" rel="stylesheet" />
    <script src="/topic/20160901/Scripts/jquery-1.9.1.min.js"></script>
    <script src="/topic/20160901/Scripts/awardRotate.js"></script>
    <script src="/topic/20160901/Scripts/scroll_js.js"></script>
    <script type="text/javascript">
        //$(function () {
        //    var allFun = {};
        //    allFun.shortAjax = function (destStr, dataAll, succFun, errFun) {//Ajax简化
        //        $.ajax({
        //            url: destStr,
        //            type: "Post",
        //            data: dataAll,
        //            contentType: "application/json;charset=utf-8",
        //            dataType: "json",
        //            success: function (msg) {
        //                succFun(msg);
        //            },
        //            error: function (err) {
        //                if (errFun != undefined) {
        //                    errFun();
        //                }
        //                alert("通讯失败，请稍后重试！");
        //            }
        //        });
        //    }
        //    function luckDrawAward(msg) {
        //        var res = $.parseJSON(msg.d);
        //        if (res.code == "0") {
        //            switch (res.data) {
        //                case "0":
        //                    alert('1%加息券');
        //                    break;
        //                case "1":
        //                    alert('谢谢参与');
        //                    break;
        //                case "2":
        //                    alert('2%加息券');
        //                    break;
        //                case "3":
        //                    alert('50元现金');
        //                    break;
        //                case "4":
        //                    alert('20元代金券');
        //                    break;
        //                case "5":
        //                    alert('50元现金');
        //                    break;
        //                case "6":
        //                    alert('10元代金券');
        //                    break;
        //                case "7":
        //                    alert('50元代金券');
        //                    break;
        //            }
        //        }
        //        else {
        //            //$('.tc_01').fadeIn();
        //            //$('.c_bg').fadeIn();
        //            //$('.tc_01 dt').html(res.data);
        //            if (res.code == "1") {
        //                alert("您还没有登录");
        //                return;
        //            }
        //            if (res.code == "2") {
        //                $('.c_bg').fadeIn();
        //                $(".tc_03").fadeIn();
        //                return;
        //            }
        //            alert(res.data);
        //            return;
        //        }
        //    }
        //    $("#btnTest50Cash").click(function () {
        //        allFun.shortAjax("/topic/20160901/LuckDraw.aspx/Test50Cash", "{}", luckDrawAward);
        //    });

        //});
    </script>
</head>
<body>
    <uc1:head ID="head1" runat="server" />
    <center>
        <div class="zhaunpan_content">
            <div class="zhaunpan_content_1">
                <img src="/topic/20160901/images/zhuanpan_01.jpg" class="bg_1">
            </div>

            <div class="zhaunpan_content_2">
                <img src="/topic/20160901/images/zhuanpan_02.jpg" class="bg_2">
                <div class="dazhaunpan_main">
                    <div class="zp_cont">
                        <div class="zp_pointer pointer">
                            <img src="/topic/20160901/images/zhuanpan_jiang.png">
                        </div>
                        <div class="zp_rotate">
                            <img id="rotate" src="/topic/20160901/images/zhuanpan_go.png" alt="turntable">
                        </div>
                        <div class="deng">
                            <div class="dengUp">
                                <img src="/topic/20160901/images/zhuanpan_deng1.png">
                            </div>
                            <div class="dengDode">
                                <img src="/topic/20160901/images/zhuanpan_deng2.png">
                            </div>
                        </div>
                        <p class="jl_time">您当前还有<i id="count"><asp:Literal runat="server" ID="ltrCanUseTimes" ></asp:Literal></i>次抽奖机会</p>
                        <%--<div>
                            <label>测试</label>
                            <input type="button" value="金手指" id="btnTest50Cash" />
                        </div>--%>
                    </div>
                    <div class="jilu">
                        <h1>中奖榜单</h1>
                        <div class="scroll_cont">
                            <ul class="gd_cont" id="xstCont" style="overflow: hidden;">
                                <asp:Literal runat="server" ID="ltrLuckMan"></asp:Literal>
                            </ul>
                        </div>
                        <h2>已有<span><asp:Literal runat="server" ID="ltrLuckCount"></asp:Literal></span>人中奖</h2>
                    </div>
                </div>
                <div class="tanchuang_main">
                    <!--弹窗遮罩-->
                    <div class="c_bg"></div>

                    <!--中奖弹窗-->
                    <div class="tc_01 tc_cont">
                        <span class="btn_close"></span>
                        <div class="guanbi"></div>
                        <dl>
                            <dt>恭喜您获得100元现金！</dt>
                            <dd><a href="#" class="chakan">查看</a><a href="#" class="shiyong">立即使用</a></dd>
                        </dl>
                    </div>
                     <div class="tc_05 tc_cont">
                        <span class="btn_close"></span>
                        <div class="guanbi"></div>
                        <dl>
                            <dt>恭喜您获得100元现金！</dt>
                            <dd>平台将在3个工作日内返现到您的账户，您可以在‘<strong>我的账户-账户总览-可用余额</strong>’中查看</dd>
                        </dl>
                    </div>
                    <div class="tc_04 tc_cont" style="display:none;">
                        <span class="btn_close"></span>
                        <div class="guanbi"></div>
                        <dl>
                            <dt>您还没哟登陆哦！</dt>
                            <dd><span class="tc_close">知道啦</span></dd>
                        </dl>
                    </div>

                    <!--不满足抽奖条件-->
                    <div class="tc_02 tc_cont">
                        <span class="btn_close"></span>
                        <div class="guanbi"></div>
                    </div>
                    <dl>
                        <dt><!-- 您还不满足抽奖条件哦！ --></dt>
                        <dd><!-- <span class="tc_close">知道啦</span> --></dd>
                    </dl>
                </div>

                <!--三次抽奖已满-->
                <div class="tc_03 tc_cont">
                    <span class="btn_close"></span>
                    <div class="guanbi"></div>
                    <dl>
                        <dt><!-- 今日三次机会已用完，请明日再来哦! --></dt>
                        <dd><!-- <span class="tc_close">知道了</span> --></dd>
                    </dl>
                </div>
                <input type="hidden" id="userid" value="0">
            </div>
        </div>

        <div class="zhaunpan_content_3">
            <img src="/topic/20160901/images/zhuanpan_03.jpg" class="bg_3">
            <div class="zhaunpan_content_3_main">
                <div class="zhaunpan_content_3_main_1">
                    <dl>
                        <dt>抽奖规则：</dt>
                        <dd>1、新用户当天注册并实名认证即可获得一次抽奖机会，次日完成实名认证不累计抽奖次数，新老用户每天登录均有一次抽奖机<br/>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp会。（当日登陆获得的抽奖次数有效期截止当日23:59:59）；</dd>
                        <dd>2、本次奖品中的代金券及加息券，截止日期为2016年9月30日（请在有效期内使用）；</dd>
                        <dd>3、加息券及代金券直接发放至个人账户，加息券和代金券不可同时使用，可以和复投同用；</dd>
                        <dd>4、现金奖励会在三个工作日内发放到您的个人账户中；</dd>
                        <dd>5、抽奖过程中，如果发现违规行为（如恶意注册大量账号，使用作弊程序等），创利投将取消您的中奖资格。</dd>
                    </dl>
                </div>
            </div>
        </div>

        <div class="zhaunpan_content_4">
            <img src="/topic/20160901/images/zhuanpan_04.jpg" class="bg_4">
            <div class="zhaunpan_content_4_main">
                <div class="zhaunpan_content_4_main_1">
                    <dl>
                        <dt>抽奖规则：</dt>
                        <dd>1、该活动奖励可与加息券或代金券奖励同时享受；</dd>
                        <dd>2、该返现奖励会在活动结束后三个工作日内直接返现到个人账户中，可用于投资，也可直接提现；</dd>
                        <dd>3、2016年9月1日之前注册的用户均可参加该活动，活动期间（即2016年9月1日-9月30日）投资越多，返现越多；</dd>
                        <dd>4、活动期间累计投资对应标的满指定金额，即可获得相应奖励；</dd>
                        <dd>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp举例：张三活动期间累计投资9万元，其中1月标1万元、3月标3万元、6月标5万元，张三将获得总计返现<br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp49+299+999=1347元；</dd>
                        <dd class="dd_last">*本活动规则在法律范围内解释权归创利投所有，如有疑问请联系在线客服400-890-2226</dd>
                    </dl>
                </div>
            </div>
        </div>
        

        <style>
            /*导航我要投资下拉菜单--放在css.css里面的样式*/
            .w_logo1 li {
                display: inline-block;
                float: left;
            }

                .w_logo1 li.subnav-tz {
                    position: relative;
                }

                .w_logo1 li a {
                    float: none;
                    border-bottom: 3px solid transparent;
                }

            .menuBox {
                position: absolute;
                z-index: 1000;
                border: 1px solid #e6e6e6;
                top: 10px;
                left: 0px;
                width: 91px;
                background-color: #fff;
                display: inline-block;
                padding: 5px 0;
                top: 70px;
                left: 0px;
                display: none;
            }

            .w_logo1 li.subnav-tz a:hover {
                border-bottom: 3px solid transparent;
            }

            .menuBox .sanjiao {
                height: 0;
                width: 0;
                position: absolute;
                border: 15px solid transparent;
                border-bottom-color: #e6e6e6;
                top: -31px;
                left: 25px;
                margin: 0;
                padding: 0;
            }

            .menuBox .sanjiao2 {
                height: 0;
                width: 0;
                position: absolute;
                border: 15px solid transparent;
                border-bottom-color: #fff;
                top: -30px;
                left: 25px;
                margin: 0;
                padding: 0;
            }

            .menuBox ul li {
                display: block;
                padding-left: 16px;
                line-height: 30px;
                height: 30px;
                float: none;
                overflow: hidden;
                width: 60px;
            }

                .menuBox ul li a {
                    display: inline-block;
                    width: 60px;
                    height: 30px;
                    overflow: hidden;
                    color: #666;
                    line-height: 30px;
                    text-align: left;
                    float: none;
                    margin-right: 0;
                    font-size: 14px;
                }

                    .menuBox ul li a:hover {
                        border-bottom: none;
                    }
        </style>
        <script>
            var bool = true;
            function aq() {
                if (bool) {
                    if ($(".dengUp").show()) {
                        $(".dengDode").hide();
                    }
                } else {
                    if ($(".dengDode").show()) {
                        $(".dengUp").hide();
                    }
                }
                bool = !bool;
            }
            setInterval(aq, 700);
        </script>
    </center>

    <uc2:bottom ID="bottom1" runat="server" />
</body>
</html>
