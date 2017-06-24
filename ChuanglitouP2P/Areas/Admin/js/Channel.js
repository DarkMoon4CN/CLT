/// <reference path="Channel.js" />
///  FOR  /Admin/Channel
//start Channel ajax
var _defaultData;
var _isSuccess = true;
var editPasswordFlag=true;
function defaultAjax(url, data, callback) {
    $.ajax({
        url: url,
        type: 'POST',
        data: data,
        async: false,
        error: function () {
            _isSuccess = false;
            layer.alert('服务器访问失败，请查看网络是否畅通！');
        },
        success: defaultCallback
    });
}
function defaultCallback(data) {
    _defaultData = data;
}
//end Channel ajax
var Channel =
{
      listInit: function () { Channel.ListInit(); }
    , addInit: function () { Channel.AddInit(); }
    , editorInit: function () { Channel.EditorInit(); }
    , loginInit: function () { Channel.LoginInit(); }
    , userListInit: function () { Channel.UserListInit(); }
    , channelUserListInit: function () { Channel.ChannelUserListInit(); }
    , channelInvListInit: function () { Channel.ChannelInvListInit(); }
    , adminUserListInit: function () { Channel.AdminUserListInit(); }
}
Channel.ListInit=function()
{
    $("#btn_Add").on("click", function () {
       var url = "/Admin/Channel/Add";
       var addwindow = $.layer({
            type: 2,
            shadeClose: false,
            title: '新增渠道信息',
            shade: [0.7, '#999'],
            offset: ['', ''],
            area: ['989px', "500px"],
            iframe: { src: url, scrolling: false }
        });
    });
}
Channel.AddInit = function () {
   
    var url = "/Admin/Channel/GetInvitedcode";
    var data = {};
    _defaultData = null;
    defaultAjax(url, data, defaultCallback);
    $("#invitedcode").val(_defaultData.data);
    $("#invitedcodeURL").val("http://www.chuanglitou.cn/register/index?channel=" + _defaultData.data + "&type=1");
    $("#btnSave").on("click", function () {
        Channel.AddSubmitCheck();
    });
}
Channel.EditorInit = function ()
{
    $("#btnSave").on("click", function () {
        Channel.EditorSubmitCheck();
    });
    $("#adminUserPassword").on("focus", function () {

        if (editPasswordFlag == true)
        {
            $(this).val("");
            editPasswordFlag = false;
        }
    });
}
Channel.LoginInit = function ()
{
    $("#btn_submit").on("click", function () {
        var txtUserName = $("#txtUserName").val();
        var txtPassword = $("#txtPassword").val();
        var txtCheckCode = $("#txtCheckCode").val();
        if (txtUserName == "") {
                layer.msg("对不起，用户名不能为空！");
                $("#txtUserName").focus();
                return;
            }
        if (txtPassword == "") {
                layer.msg("对不起，密码不能为空！");
                $("#txtUserName").focus();
                return;
            }
        if (txtCheckCode == "") {
                layer.msg("对不起，验证码不能为空！");
                $("#txtUserName").focus();
                return;
        }
        var index = layer.msg("正在登录中...",50, 1);
        var url = "/Admin/Channel/DoLogin";
        var data = { txtUserName: txtUserName, txtPassword: txtPassword, txtCheckCode: txtCheckCode };
        _defaultData = null;
        defaultAjax(url, data, defaultCallback);
        layer.close(index);
        if (_defaultData.state == 0)
        {
            layer.msg(_defaultData.msg);
        }
        else
        {
            layer.msg(_defaultData.msg+" 正在跳转...", 3, 1, function () {
                window.location.href = "/Admin/Channel/Main";
            });
        }

    });

    $(document).keydown(function (event) {
        var keycode = event.keyCode ? event.keyCode : event.which;
        if (keycode == 13)
        {
            $("#btn_submit").click();
        }
    });
}
Channel.UserListInit = function ()
{
    $("#get_Invitedcode").on("click", function () {
        Channel.Copy();
    });

    $("#sel_channelType").on("change", function () {
        var select = $(this).val();
        var hidInvitedcode=$("#invitedcode").val();
        if (select == "1")
        {
            $("#invitedcodeURL").val("http://www.chuanglitou.cn/register/index?channel=" + hidInvitedcode + "&type=1");
        }
        else if (select == "2")
        {
            $("#invitedcodeURL").val("http://m.chuanglitou.cn/register/index?channel=" + hidInvitedcode + "&type=1");
        }
    });

     $("#toExcel").on("click", function () {
        var realname = $("#realname").val();
        var ordId = $("#ordId").val();
        var btitle = $("#btitle").val();
        var startTime = $("#startTime").val();
        var endTime = $("#endTime").val();
        var index = layer.msg("渠道用户列表Excel正在生成中...", 50, 1);
        var url = "/Admin/Channel/UListExcel";
        var data = { realname: realname, startTime: startTime, endTime: endTime, ordId: ordId, btitle: btitle };
        _defaultData = null;
        defaultAjax(url, data, defaultCallback);
        layer.close(index);
        if (_defaultData.state == 0) {
            layer.alert(_defaultData.msg);
            return;
        }
        window.location.href = _defaultData.data;
    });
}
Channel.ChannelUserListInit = function ()
{
    $("#toExcel").on("click", function () {
        var adminUserName = $("#adminUserName").val();
        var userName = $("#userName").val();
        var startTime = $("#startTime").val();
        var endTime = $("#endTime").val();
        var index = layer.msg("渠道用户列表Excel正在生成中...", 50, 1);
        var url = "/Admin/Channel/AUListExcel";
        var data = { adminUserName: adminUserName, userName: userName, startTime: startTime, endTime: endTime };
        _defaultData = null;
        defaultAjax(url, data, defaultCallback);
        layer.close(index);
        if (_defaultData.state == 0) {
            layer.alert(_defaultData.msg);
            return;
        }
        window.location.href = _defaultData.data;
    });
}
Channel.ChannelInvListInit = function ()
{
   
    $("#toExcel").on("click", function () {
        var adminUserName = $("#adminUserName").val();
        var channelName = $("#channelName").val();
        var ordId = $("#ordId").val();
        var btitle = $("#btitle").val();
        var startTime = $("#startTime").val();
        var endTime = $("#endTime").val();
        var index = layer.msg("投资列表Excel正在生成中...", 50, 1);
        var url = "/Admin/Channel/CIListExcel";
        var data = { adminUserName: adminUserName, channelName: channelName, startTime: startTime, endTime: endTime, ordId: ordId, btitle: btitle };
        _defaultData = null;
        defaultAjax(url, data, defaultCallback);
        layer.close(index);
        if (_defaultData.state == 0) {
            layer.alert(_defaultData.msg);
            return;
        }
        window.location.href = _defaultData.data;
    });
}
Channel.AdminUserListInit = function ()
{
    $("#toExcel").on("click", function () {
        var userName = $("#userName").val();
        var realName = $("#realName").val();
        var startTime = $("#startTime").val();
        var endTime = $("#endTime").val();
        var index = layer.msg("投资列表Excel正在生成中...", 50, 1);
        var url = "/Admin/Channel/AdminUserListExcel";
        var data = { userName: userName, startTime: startTime, endTime: endTime, realName: realName };
        _defaultData = null;
        defaultAjax(url, data, defaultCallback);
        layer.close(index);
        if (_defaultData.state == 0)
        {
            layer.alert(_defaultData.msg);
            return;
        }
        window.location.href = _defaultData.data;
    });
}

Channel.EditorSubmitCheck = function ()
{
    var channelName = $("#channelName").val();
    var channelType = $("#channelType").val();
    var status = $("#status").val();
    var adminUserName = $("#adminUserName").val();
    var adminUserPassword = $("#adminUserPassword").val();
    var trueName = $("#trueName").val();
    var channelId = $("#channelId").val();
    if (channelName == "") {
        layer.alert("渠道名不能为空！");
        return;
    } 
    if (channelType == "-1") {
        layer.alert("请选择渠道类型！");
        return;
    }
    if (status == "-1") {
        layer.alert("请选择渠道是否可用！");
        return;
    }
    
    if (adminUserName == "") {
        layer.alert("渠道用户不能为空！");
        return;
    }
    if (adminUserPassword == "") {
        layer.alert("渠道用户密码不能为空！");
        return;
    }
    var index = layer.msg("正在处理中，请稍后...", 2, 1);
    var url = "/Admin/Channel/DoEditor";
    var data = { channelId: channelId, channelName: channelName,channelType:channelType, status: status, adminUserName: adminUserName, adminUserPassword: adminUserPassword, trueName: trueName };
    _defaultData = null;
    defaultAjax(url, data, defaultCallback);
    layer.close(index);
    if (_defaultData.state == 0) {
        layer.alert(_defaultData.msg);
    }
    else {
        layer.alert(_defaultData.msg, 1, "渠道编辑信息", function () {
            //top.window.parent.location.reload();            
           window.parent.location.reload();
        });
    }
}
Channel.GotoEditor = function (channelId)
{
    var url = "/Admin/Channel/Editor?channelId=" + channelId;
    var addwindow = $.layer({
        type: 2,
        shadeClose: false,
        title: '编辑渠道信息',
        shade: [0.7, '#999'],
        offset: ['', ''],
        area: ['989px', "500px"],
        iframe: { src: url, scrolling: false }
    });
}
Channel.AddSubmitCheck = function ()
{
    var channelName = $("#channelName").val();
    var channelType = $("#channelType").val();
    var invitedcodeURL = $("#invitedcodeURL").val();
    var status = $("#status").val();
    var adminUserName = $("#adminUserName").val();
    var adminUserPassword = $("#adminUserPassword").val();
    var invitedcode = $("#invitedcode").val();
    var trueName = $("#trueName").val();
    if (channelName == "")
    {
        layer.alert("渠道名不能为空！");
        return;
    }
    if (channelType == "-1") {
        layer.alert("请选择渠道类型！");
        return;
    }
    if (status == "-1")
    {
        layer.alert("请选择渠道是否可用！");
        return;
    }
    if (adminUserName == "") {
        layer.alert("渠道用户不能为空！");
        return;
    }

    if (adminUserPassword == "") {
        layer.alert("渠道用户密码不能为空！");
        return;
    }
    var index = layer.msg("正在处理中，请稍后...", 2, 1);
    var url = "/Admin/Channel/DoAdd";
    var data = { channelName: channelName, invitedcode: invitedcode, channelType: channelType, status: status, adminUserName: adminUserName, adminUserPassword: adminUserPassword, trueName: trueName };
    _defaultData = null;
    defaultAjax(url, data, defaultCallback);
    layer.close(index);
    if (_defaultData.state == 0) {
        layer.alert(_defaultData.msg);
    }
    else
    {
        layer.alert(_defaultData.msg, 1, "添加渠道信息", function () {
           window.parent.location.reload();
        });
    }
    
}
Channel.Copy = function ()
{
        var e = document.getElementById("invitedcodeURL");
        e.select(); //选择对象
        document.execCommand("Copy");
        layer.msg("复制完成！", 1, 1);
}
Channel.CloseLayer = function ()
{
    parent.layer.closeAll();
}
Channel.ChangeCodeImg = function (id)
{
      document.getElementById(id).src = "/admin/login/ImageValidate?a=" + Math.random();
}


