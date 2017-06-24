var tot = parseFloat("0.0");
var Rewardsid = [];//定义一个数组   优惠券id
var df;
var dsid = null; //存储已经用优惠券或加息券
var investmentamountd1 = parseFloat("0.0"); //当前优惠券的投资金额
var tmpinvest = parseFloat("0.0"); //临时投资变量
var invat = parseFloat("0.0");

var invtemp = parseFloat("0.0");
var todt;
var tmpamt = parseFloat("0.0");
var tmpCurAmt = parseFloat("0.0");
var tmpccamt = parseFloat("0.0");
var fxtype = 0;//0未选中； 1=1%；  2=2%
$(function () {
    df = $('#Rewardsid'); //优惠券id对象   
    var invst = $('#investamount'); //当前投资金额
    tmpinvest = parseFloat(invst.val());
    var todt = $('#Rewards'); //使用使用奖励金额    
    var Isinterest = $('#Isinterest').val();
    invat = $('#investamount').val();
    invtemp = invat;
    var num = parseFloat("0");


    if (df.val().length > 0) {  //这行暂时不管，是选中已经用的优惠券
        dsid = df.val().split(",");// 
        $("tbody>tr:even").addClass("even");  /*给奇数行添加样式*/
        $("tbody>tr:odd").addClass("odd");  /*偶数行添加样式*/
        $('#list_tb tbody tr').slice(1).each(function () {



        });


    }


    $("body").delegate('tbody>tr', "click", function () {


        investmentamountd1 = parseFloat(invst.val()); //当前优惠券的投资金额
        var xianer = parseFloat($(this).find("td").eq(3).text()); //限额
        var CurAmt = parseFloat($(this).find("td").eq(2).text().replace('%', ''));//当前优惠券金额


        tmpCurAmt = CurAmt
        var tid = $(this).find(":checkbox").val(); //当前活动id
        if (parseInt(tid) > 0) {
            var AmtUses = $('#u' + tid).val(); //这里使用要求

            if ($(this).hasClass("selected")) {  //判断是否选中
                // alert(tmpCurAmt);

                debugger;
                //alert("取消");           
                var RewLen = Rewardsid.indexOf(tid);
                if (RewLen > -1) {  //如果存在   

                    if (tmpinvest < investmentamountd1) {  //这里
                        var tmptot = tmpinvest + xianer;
                        if (tmptot <= investmentamountd1) {
                            tmpinvest = tmptot;
                        }
                    }



                    var rews = $('#Rewards').text();
                    if (rews == "") {
                        rews = 0.00;
                    }
                    else {
                        rews = parseFloat(rews);
                    }
                    var amtc = 0;
                    if (rews >= CurAmt) {
                        amtc = rews - CurAmt;
                        if (amtc > 0) {
                            var Isinterest = $('#Isinterest').val();
                            if (Isinterest == 0) {
                                $('#Rewards').text(amtc);
                            }
                            else if (Isinterest == 1) {
                                $('#Rewards').text(amtc + "%");
                            }

                            CurAmt = amtc;
                        } else {
                            $('#Rewards').text("0");
                            CurAmt = 0;
                        }
                    }
                    else {
                        $('#Rewards').text("0");
                    }

                    Rewardsid.remove(tid);

                    invtemp = parseFloat(invtemp) + parseFloat(tmpCurAmt);


                    tmpamt = invtemp;

                    // alert(tmpamt);

                    setText(amtc, 1, tid, tmpamt);

                    var Isinterest = $('#Isinterest').val();
                    if (Isinterest == 1) {
                        var obj = $("#investamount").val();
                        var numamt = parseInt(obj);
                        //showEarnByAmt(numamt);
                    }
                    df.val(Rewardsid);
                    $(this).removeClass("selected")
                    .find(":checkbox").prop("checked", false);
                    if (Isinterest == 0) {
                        if (investmentamountd1 == tmpinvest) {
                            fxtype = 0;
                        }
                    }
                    layer.tips('奖励取消使用!', this, { guide: 0, time: 3, style: ['background-color:#33CC00; color:#FFFFFF;', '#33CC00'] });
                    num = num - 1;
                    if ($('#jiaxiquan_txt').val() == "0%") { $('#jiaxiquan_txt').val("") }
                }



            }
            else {


           


                //alert("选中");
                var RewLen = Rewardsid.indexOf(tid);
                if (RewLen > -1) {  //如果存在
                    //if (AmtUses == 1) {
                    //    //这里需要业务处理
                    //    layer.msg("此优惠券或加息券只能单独使用！", 2, 5);
                    //    return false;
                    //}



                    if (tmpinvest < investmentamountd1) {  //这里
                        var tmptot = tmpinvest + xianer;
                        if (tmptot <= investmentamountd1) {
                            tmpinvest = tmptot;
                        }
                    }



                    var rews = $('#Rewards').text();
                    if (rews == "") {
                        rews = 0.00;
                    }
                    else {
                        rews = parseFloat(rews);
                    }
                    var amtc = 0;
                    if (rews >= CurAmt) {
                        amtc = rews - CurAmt;
                        if (amtc > 0) {
                            var Isinterest = $('#Isinterest').val();
                            if (Isinterest == 0) {
                                $('#Rewards').text(amtc);
                            }
                            else if (Isinterest == 1) {
                                $('#Rewards').text(amtc + "%");
                            }

                            CurAmt = amtc;
                        } else {
                            $('#Rewards').text("0");
                            CurAmt = 0;
                        }
                    }
                    else {
                        $('#Rewards').text("0");
                    }

                    Rewardsid.remove(tid);

                    invtemp = parseFloat(invtemp) + parseFloat(tmpCurAmt);


                    tmpamt = invtemp;

                    // alert(tmpamt);

                    setText(amtc, 1, tid, tmpamt);

                    var Isinterest = $('#Isinterest').val();
                    if (Isinterest == 1) {
                        var obj = $("#investamount").val();
                        var numamt = parseInt(obj);
                        //showEarnByAmt(numamt);
                    }
                    df.val(Rewardsid);
                    $(this).removeClass("selected")
                    .find(":checkbox").prop("checked", false);
                    if (Isinterest == 0) {
                        if (investmentamountd1 == tmpinvest) {
                            fxtype = 0;
                        }
                    }
                    layer.tips('奖励取消使用!', this, { guide: 0, time: 3, style: ['background-color:#33CC00; color:#FFFFFF;', '#33CC00'] });
                    num = num - 1;
                    if ($('#jiaxiquan_txt').val() == "0%") { $('#jiaxiquan_txt').val("") }
                }
                else {

                    var Isinterest = $('#Isinterest').val();
                    if (Isinterest == 1) {

                        if (num >= 1) {
                            layer.msg("亲!加息券单笔投资只能使用一张", 2, 5);
                            return false;
                        }

                    }
                    //抵扣券分档判断
                    //var bl = (CurAmt / xianer) * 100;
                    //if (Isinterest == 0) {


                    //    if (fxtype == 1) {
                    //        if (bl == 2) {
                    //            layer.msg("亲!8元，10元不能与20元，50元，200元，500元同时使用哦！", 2, 5);
                    //            return false;
                    //        }
                    //    } else if (fxtype == 2) {
                    //        if (bl == 1) {

                    //            layer.msg("亲!20元，50元，200元，500元不能与8元，10元同时使用哦！", 2, 5);
                    //            return false;
                    //        }
                    //    } else {

                    //    }
                    //}

                    var Diff = parseFloat($("#Diff").val());  //这个位置有些问题

                    if (investmentamountd1 > Diff) {
                        layer.msg("投资金额超出最大可投资金额！", 2, 5);
                        return false;
                    }
                    else {
                        var RewLen = Rewardsid.indexOf(tid);

                        if (tmpinvest < CurAmt) {
                            layer.msg("亲!您的奖励使用金额超出投资金额哦！", 2, 5);
                            return false;
                        }
                        if (tmpinvest < xianer) {
                            layer.msg("亲!此奖励使用超出使用限额了哦", 2, 5);
                            return false;
                        }


                        $(this).addClass("selected")
                        .find(":checkbox").prop("checked", true);
                        if (RewLen < 0) {  //如果不存在就记录
                            Rewardsid.push(tid);
                            if (tmpinvest >= xianer) {
                                tmpinvest = tmpinvest - xianer;
                            }
                            var rews = $('#Rewards').text();
                            if (rews == "") {
                                rews = 0.00;
                            }
                            else {
                                rews = parseFloat(rews);
                            }


                            invtemp = parseFloat(invtemp) - parseFloat(tmpCurAmt);

                            tmpamt = invtemp;
                            $('#Rewards').text(rews + CurAmt);
                            setText(CurAmt, 0, tid, tmpamt);

                            var Isinterest = $('#Isinterest').val();
                            if (Isinterest == 1) {
                                var obj = $("#investamount").val();
                                var numamt = parseInt(obj);
                                // showEarnByAmt(numamt);
                            }

                            df.val(Rewardsid);
                            if (Isinterest == 0) {
                                if (bl == 2) {
                                    fxtype = 2;
                                } else if (bl == 1) {
                                    fxtype = 1;
                                } else {
                                    fxtype = 0;
                                }
                            }
                            layer.tips('奖励使用成功!', this, { guide: 0, time: 3, style: ['background-color:#33CC00; color:#FFFFFF;', '#33CC00'] });
                            num = num + 1;
                        }
                    }
                }
            }

        } else {
            layer.msg("亲!此奖励不能使用哦", 2, 5);
            return false;
        }

    });



});

function setText(amt, t, tid, tmpamt) {
    //alert('amt:'+amt + 't:'+t + 'tid:'+tid);

    var Isinterest = $('#Isinterest').val();

    if (Isinterest == 0) {

        if (t == 0) {
            $('#hidResult').val($('#Rewards').text() + '元');
            //  $('#dispamt').text(tmpamt + '元');
        } else {
            $('#hidResult').val($('#Rewards').text() + '元');
            // $('#dispamt').text(tmpamt + '元');
        }
    }
    else if (Isinterest == 1) {
        if (t == 0) {
            $('#hidResult').val(amt + '%');
        } else {
            $('#hidResult').val(amt + '%');
        }
    }

}

Array.prototype.indexOf = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) return i;
    }
    return -1;
};
Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) { this.splice(index, 1); }
};