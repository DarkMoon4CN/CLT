/// <reference path="jquery-1.10.2.js" />

function getIosDate(strDate) {
    var arr = strDate.split(/[- :]/);
    return new Date(arr[0], arr[1] - 1, arr[2], arr[3], arr[4], arr[5]);
}
function investlist() { };

investlist.pages = 0;

investlist.initpage = function () {
    investlist.getSelectedValue(1);
    $("#table_search tr td a").each(function () {
        $(this).bind("click", function () {
            var parent_td = $(this).parent();
            var parent_tr = parent_td.parent();
            parent_tr.children().each(function () {
                $(this).removeClass("hsw_td2");
            });
            parent_td.addClass("hsw_td2");

            investlist.getSelectedValue(1);
        });
    });
}

investlist.getSelectedValue = function (page) {
    if (page == undefined) {
        page = 1;
    }
    var para = "page=" + page;
    $("#table_search .hsw_td2 a").each(function () {
        var key = $(this).attr("skey");
        var name = $(this).attr("sname");
        if (key != undefined && name != undefined) {

            if (para != "" && para.length > 1) {
                para = para + "&";
            }
            para = para + name + "=" + key;
        }
    });
    var url = "/Investlist/GetInvestList?" + para;
    $.getJSON(url, {}, function (data) {
        var html = [];
        investlist.pages = data.pagecount;
        for (var i = 0; i < data.data.length; i++) {
            var json = data.data[i];
            html.push(investlist.getHtml(json));
        }
        html.push('<div class="hsw_con3_fy" id="div_page">');
        html.push('</div>');

        $("#div_hsw_con3").empty();
        $("#div_hsw_con3").html(html.join(""));


        //调用分页
        laypage({
            cont: 'div_page',
            pages: investlist.pages,
            curr: page,
            jump: function (obj, first) { //触发分页后的回调(单击页码后)
                if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                    investlist.getSelectedValue(obj.curr); // (被单击的页码)
                }
            }
        })

    });

};

investlist.getHtml = function (json) {
    var arr = [];
    if (json != undefined) {
        var tender_state = json.tender_state;
        var IsUse = json.IsUse;
        arr.push('<div class="hsw_con3_common" >');
        //var sTime = getIosDate($("#hidFXSTime").val());
        //var eTime = getIosDate($("#hidFXETime").val());
        //var sysTime = getIosDate(json.systime);
        //if (sysTime > sTime && sysTime < eTime) {
        if (json.isShowJiaoBiao=="") {
            if (json.jkqx == 6) {
                arr.push('<span class="jiaobiao">2% 返现</span>');
            }
            else if (json.jkqx == 3) {
                arr.push('<span class="jiaobiao">1% 返现</span>');
            }
        }
        arr.push('<i><a href="invest_borrow_' + json.targetid + '.html">' + json.borrowing_title + '</a></i>');
        if (tender_state > 2) {
            arr.push('<em></em>');
        } else {
            if (IsUse == 0) {
                arr.push("<em>担保措施：个人连带</em>");
            } else {
                arr.push('<em>担保公司：<a href="/Guarantee/Agency/' + json.companyid + '" title="' + json.company_name + '">' + json.company_name + '</a></em>');
            }
        }
        arr.push('<p class="nianhuazhi" style="margin:0;">');
        arr.push('<span>' + json.annual_interest_rate + '</span>%');
        //arr.push('<small>预期年化</small>');
        arr.push('</p>');
        if (json.operState == 12) {
            arr.push('<div class="div2_div" style="padding: 0 0 12px 0;">');
            arr.push('<p style="height: 14px;">');
            arr.push('锁定期');
            arr.push('<span><small>' + json.jkqx + '</small>' + json.jkday + '</span>');
            arr.push('</p>');
            arr.push('<p>')
            arr.push('剩余可投<span><small>0.00</small> 元</span>');
            arr.push('</p>');
            arr.push('</div>');
            arr.push('<p>');

            arr.push(' <i><span class="speed_pro_bg"><span style="width:100%"></span></span></i>');
            arr.push('<strong>100%</strong>');
            arr.push('</p>');
        } else {
            arr.push('<div class="div2_div" style="padding: 0 0 12px 0;">');
            arr.push('<p style="height: 14px;">');
            arr.push('锁定期');
            arr.push('<span><small>' + json.jkqx + '</small>' + json.jkday + '</span>');
            arr.push('</p>');
            arr.push('<p>')
            arr.push('剩余可投<span><small>' + (json.needMoney < 0 ? 0 : json.needMoney) + '</small> 元</span>');
            arr.push('</p>');
            arr.push('</div>');
            arr.push('<p>');
            arr.push(' <i><span class="speed_pro_bg"><span style="width:' + (json.Percentage > 100 ? 100 : json.Percentage) + '%"></span></span></i>');
            arr.push('<strong>' + (json.Percentage > 100 ? 100 : json.Percentage) + '%</strong>');
            arr.push('</p>');
        }


        //arr.push('<span class="hsw_span1">项目总额' + json.borrowing_balance + '/还需 ' + json.needMoney + ' 元</span>');
        //arr.push('<div class="hsw_intro">');
        //arr.push('<b><span class="hsw_span2">' + json.annual_interest_rate + '%</span><br><span class="hsw_span3">预期年化收益</span></b>');
        //arr.push('<b class="hsw_intro_b1"><span class="hsw_span2">' + json.jkqx + '</span><br><span class="hsw_span3">投资期限</span></b>');
        if (json.operState == 11) {
            arr.push('<a href="javascript:void(0);" class="hsw_intro aSubmit">未开始</a>');
        } else if (json.operState == 12) {
            arr.push('<a href="javascript:void(0);" class="hsw_intro aSubmit">项目已结束</a>');
        } else if (json.operState == 13 || (json.operState == 14 && json.needMoney < 0)) {
            arr.push('<a href="javascript:void(0);" class="hsw_intro aSubmit">满标</a>');
        } else if (json.operState == 14) {
            arr.push('<a href="invest_borrow_' + json.targetid + '.html" title="' + json.borrowing_title + '" style="background: #38C4FF;" class="aSubmit">立即投资</a>');
        } else if (json.operState == 3) {
            arr.push('<a href="invest_borrow_' + json.targetid + '.html" title="' + json.borrowing_title + '" class="hsw_intro aSubmit">满标</a>"');
        } else if (json.operState == 4) {
            arr.push('<a href="invest_borrow_' + json.targetid + '.html" title="' + json.borrowing_title + '" class="hsw_intro aSubmit">还款中</a>');
        } else if (json.operState == 5) {
            arr.push('<a href="invest_borrow_' + json.targetid + '.html" title="' + json.borrowing_title + '" class="hsw_intro aSubmit">已还清</a>');
        }

        arr.push('</div>');
        arr.push('</div>');
    }

    return arr.join("");
};
