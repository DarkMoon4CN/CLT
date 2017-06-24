<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ChuanglitouP2P.topic._20160910.Index" %>

<%@ Register Src="/WebUserControl/head.ascx" TagName="head" TagPrefix="uc1" %>
<%@ Register Src="/WebUserControl/bottom.ascx" TagName="bottom" TagPrefix="uc2" %>
<%@ Import Namespace="ChuanglitouP2P.Model" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>一元抢iPhone</title>
    <link href="/topic/20160910/Content/css.css" rel="stylesheet" type="text/css" />
    <script src="/topic/20160910/Scripts/jquery-1.9.1.min.js"></script>
    <script src="/topic/20160910/Scripts/jquery.flexslider.js"></script>
    <script src="/topic/20160910/Scripts/index.js"></script>
    <script src="/topic/20160910/Scripts/clock_time.js"></script>
    <script type="text/javascript" src="/topic/20160910/Scripts/Validform_v5.3.2_min.js"></script>
    <script src="/topic/20160910/Scripts/layer.min.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        $(function () {

            //滚动效果

            var scrollingBox = document.getElementById("xstCont");

            var reachedBottom = false;
            var bottom;
            //var timer = null;
            function scrolling() {
                var origin = scrollingBox.scrollTop++;
                if (origin == scrollingBox.scrollTop && scrollingBox.scrollTop != 0) {
                    if (!reachedBottom) {
                        scrollingBox.innerHTML += scrollingBox.innerHTML;
                        reachedBottom = true;
                        bottom = origin;
                    } else {
                        scrollingBox.scrollTop = bottom;
                    }
                }

                scrollingBox.style.overflow = "hidden";
            }
            var timer = setInterval(scrolling, 120);
            scrollingBox.onmouseover = function () { clearInterval(timer); }
            scrollingBox.onmouseout = function () { timer = setInterval(scrolling, 120) }
        });
    </script>
</head>
<body>
    <%--<form id="form1" runat="server">
        <div>
            <%
                double c = getLDCount();
                c = 300 * c;
            %>
            <div style="width: 300px; height: 15px; border-radius: 10px; border: 1px solid blue; box-shadow: 2px 2px 5px 1px blue;">
                <div style="width: <%=c %>px; height: 100%; background: #808080; border-radius: 10px;">
                </div>
            </div>



            已参与用户： <%=getLJJRCount() %>
            <%
                List<M_GrabIphone> GILists = getCYLIst();
                foreach (M_GrabIphone item in GILists)
                {
                    M_member_table mt = getMemberInfo(item.RegrsterID);
                    if (mt != null)
                    {
                        Hashtable hashtable = GetHashtable();

                        string name = mt.username;
            %>
            <div>用户：<%=name %>    已投资   金额：<%=item.InvestmentAmount %></div>
            <%

                    }
                }

            %>
        </div>
        <br />


        <%
            List<M_GrabIphone> zjLists = getZJLIst();
            if (zjLists != null)
            {
        %>
        已中奖用户：<%=getZJLIst().Count %>
        <%
            foreach (M_GrabIphone item in zjLists)
            {
                M_member_table mt = getMemberInfo(item.RegrsterID);
                if (mt != null)
                {

                    Hashtable hashtable = GetHashtable();
                    string mobile = mt.username;
                    string iD_number = mt.iD_number;
                    string name = "*";
                    string Province = "";
                    if (mt.realname.Length > 0)
                    {
                        name = mt.realname.Substring(0, 1);


                        if (iD_number.Length == 18)
                        {
                            if (int.Parse(iD_number.Substring(16, 1)) % 2 == 0)
                                name += "女士";
                            else
                                name += "先生";
                        }
                        else if (iD_number.Length == 15)
                        {
                            if (int.Parse(iD_number.Substring(14, 1)) % 2 == 0)
                                name += "女士";
                            else
                                name += "先生";

                        }
                        else
                        {
                            name += "";
                        }
                        if (iD_number.Length > 0)
                        {
                            Province = (string)hashtable[iD_number.Substring(0, 2)];
                        }

        %>
        <div>省：<%=Province %>    用户：<%=name %>(<%=mobile %>)  Iphone7   金额：<%=item.InvestmentAmount %></div>
        <%
                        }
                    }
                }
            }

        %>
    </form>--%>
    <uc1:head ID="head1" runat="server" />
    <center>
        <div class="qiang_content">
            <div class="qiang_c01">
                <img src="/topic/20160910/images/qiang_01.jpg" class="qiang_c01_bg">
            </div>
            <div class="qiang_c02">
                <img src="/topic/20160910/images/qiang_02.jpg" class="qiang_c02_bg">
                <!-- <div class="qiang_c02_main"> -->
                <div class="qiang_c02_main_1">
                    <div class="qiang_c02_main_1_jindubg">
                         <%
                             double c = getLDCount();
                             c = 100 * c;
                                 %>
                        <div class="qiang_c02_main_1_jingdu" style="width: <%=c%>%"></div>
                        <div class="qiang_c02_main_1_jindubg_zi">
                            
                            <p>活动已加入</p>
                            <span><asp:Literal runat="server" ID="ltrCurrentCount"> </asp:Literal> </span><small>位</small>
                        </div>
                    </div>
                </div>
                <div class="qiang_c02_main_2">
                    <div class="qiang_c02_main_2_jilu">
                        <h2>累计加入<span><%=getLJJRCount() %></span>人</h2>
                        <div class="scroll_cont">
                            <ul class="gd_cont" id="xstCont" style="overflow: hidden;">
                            <%
                                List<M_GrabIphone> GILists = getCYLIst();
                                foreach (M_GrabIphone item in GILists)
                                {
                                    M_member_table mt = getMemberInfo(item.RegrsterID);
                                    if (mt != null)
                                    {
                                        Hashtable hashtable = GetHashtable();

                                        string name = mt.username.Substring(0, 1) + "***" + mt.username.Substring(mt.username.Length - 1, 1);
                                        %>
                                
                                    <li style="font-size:1.2em;">
                                        <p><span><%=name %></span><label style="float:left;">投资</label><span><%=item.InvestmentAmount %>元</span></p>
                                    </li>
                                        <%

                                                }
                                            }

                            %>  
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="qiang_c02_main_3">
                    <h2><span>幸运大奖</span>花落谁家</h2>
                    <div class="qiang_c02_main_3_main">
                        <%--<div class="qiang_c02_main_3_main_1">
                            <i>1</i>
                            <p><span>北京市</span><span>王先生</span><span>获新一代iPhone7</span><span>32G</span></p>
                        </div>
                        <div class="qiang_c02_main_3_main_1">
                            <i>2</i>
                            <p><span>北京市</span><span>王先生</span><span>获新一代iPhone7</span><span>32G</span></p>
                        </div>
                        <div class="qiang_c02_main_3_main_1">
                            <i>3</i>
                            <p><span>北京市</span><span>王先生</span><span>获新一代iPhone7</span><span>32G</span></p>
                        </div>
                        <div class="qiang_c02_main_3_main_1">
                            <i>4</i>
                            <p><span>北京市</span><span>王先生</span><span>获新一代iPhone7</span><span>32G</span></p>
                        </div>
                        <div class="qiang_c02_main_3_main_1">
                            <i>5</i>
                            <p><span>北京市</span><span>王先生</span><span>获新一代iPhone7</span><span>32G</span></p>
                        </div>--%>
                         <%
                             List<M_GrabIphone> zjLists = getZJLIst();
                             if (zjLists != null)
                             {
        %>
        <%--已中奖用户：<%=getZJLIst().Count %>--%>
                     <%
                         int indexer = 1;
                         foreach (M_GrabIphone item in zjLists)
                         {
                             M_member_table mt = getMemberInfo(item.RegrsterID);
                             if (mt != null)
                             {

                                 Hashtable hashtable = GetHashtable();
                                 string mobile = mt.username;
                                 string iD_number = mt.iD_number;
                                 string name = "*";
                                 string Province = "";
                                 if (mt.realname.Length > 0)
                                 {
                                     name = mt.realname.Substring(0, 1);


                                     if (iD_number.Length == 18)
                                     {
                                         if (int.Parse(iD_number.Substring(16, 1)) % 2 == 0)
                                             name += "女士";
                                         else
                                             name += "先生";
                                     }
                                     else if (iD_number.Length == 15)
                                     {
                                         if (int.Parse(iD_number.Substring(14, 1)) % 2 == 0)
                                             name += "女士";
                                         else
                                             name += "先生";

                                     }
                                     else
                                     {
                                         name += "";
                                     }
                                     if (iD_number.Length > 0)
                                     {
                                         Province = (string)hashtable[iD_number.Substring(0, 2)];
                                     }

                                        %>
                                        <div class="qiang_c02_main_3_main_1">
                                            <i><%=indexer%></i>
                                            <p><span><%=Province %></span><span><%=name %></span><span>获新一代iPhone7</span><span>32G</span></p>
                                        </div>
                                       <%-- <div>省：<%=Province %>    用户：<%=name %>(<%=mobile %>)  Iphone7   金额：<%=item.InvestmentAmount %></div>--%>
                                        <%
                                                        }
                                                    }
                                                    indexer++;
                                                }
                                            }

        %>
                    </div>
                   
                </div>
                <!-- </div> -->
            </div>
            <div class="qiang_c03">
                <img src="/topic/20160910/images/qiang_03.jpg" class="qiang_c03_bg">
                <div class="qiang_c03_main">
                    <a href="/investlist.html"></a>
                </div>
            </div>
            <div class="qiang_c04">
                <img src="/topic/20160910/images/qiang_04.jpg" class="qiang_c04_bg">
                <div class="qiang_c04_main">
                    <div>
                        <dl>
                            <dt>活动规则</dt>
                            <dd>1、本活动仅限新注册的用户，每个用户仅限有一次参与机会。</dd>
                            <dd>2、本活动仅限投资6个月标。</dd>
                            <dd>3、同一用户多次投资只计算一次抽奖机会。</dd>
                            <dd>4、满5388人可进行一次抽奖，参与人数满5388后，开始进行随机抽奖，上轮抽奖结束后开始累计新一轮计算。</dd>
                            <dd>5、iPhone7新品于2016年9月7日发布，我平台将根据新品起售时间进行奖品发放， 发放详情会在苹果官网发售后与获奖者沟通发放，活动奖<br>&nbsp&nbsp&nbsp&nbsp&nbsp品将于单轮抽奖结束后的3个工作日内由创利投平台客服核对中奖信息，中奖商品不开具相应的发票，不可折现；如无质量问题，不支持退<br>&nbsp&nbsp&nbsp&nbsp&nbsp换货。</dd>
                            <dd>6、如平台工作人员连续七个工作日未能联系到您，视为自动放弃奖品。</dd>
                        </dl>
                    </div>
                </div>
            </div>
        </div>
       
        <!--返回顶部-->
        <div class="go_top" id="go_top">
            <a class="go_top_a1" href="javascript:void(0);">
                <img src="./images/go_topa1.png" alt="">
            </a>
            <a class="go_top_a2" href="javascript:void(0);" id="BizQQWPA1">
                <div class="go_topa2">
                    在线客服<br>9:00~18:00
                </div>
            </a>
            <a class="go_top_a3" href="javascript:AddFavorite()">
                <div class="go_topa2 go_topa3">
                    收藏本站
                </div>
            </a>
            <a class="go_top_a4" href="#">
                <div class="go_topa2 go_topa3">
                    返回顶部
                </div>
            </a>
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
    </center>
    <script src="/topic/20160910/Scripts/o_code.js"></script>
    <uc2:bottom ID="bottom1" runat="server" />
</body>
</html>
