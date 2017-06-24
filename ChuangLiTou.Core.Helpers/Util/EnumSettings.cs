using System;
using System.ComponentModel;

namespace ChuangLiTou.Core.Helpers.Util
{



    //public enum SmsType
    //{
    //    [Description("语音短信验证码")]
    //    语音短信验证码 = 7,
    //    [Description("短信验证码")]
    //    短信验证码 = 8,
    //    [Description("一对一短信")]
    //    一对一短信 = 9,
    //    [Description("投资成功")]
    //    投资成功 = 10,
    //    [Description("投资回款")]
    //    投资回款 = 11,
    //    [Description("取现成功")]
    //    取现成功 = 12,
    //    [Description("修改密码")]
    //    修改密码 = 13,
    //    [Description("注册成功")]
    //    注册成功 = 14,
    //    [Description("活动奖励")]
    //    活动奖励 = 15
    //}




    [Serializable]
    public enum Domains
    {

        [EnumHelper("<font color='blue'>商户站点</font>")]
        Account,
        [EnumHelper("<font color='gray'>管理员站点</font>")]
        Manage,
        [EnumHelper("<font color='red'>前台站点</font>")]
        WebSite,
    }

    [Serializable]
    [EnumHelper("订单状态")]
    public enum OrderStatus
    {
        [EnumHelper("全部订单")]
        All = -1,
        [EnumHelper("<font color='red'>订单待付款</font>")]
        Pending = 0,
        [EnumHelper("<font color='blue'>已付款,等待发货</font>")]
        Processing = 1,
        [EnumHelper("<font color='blue'>订单已发货</font>")]
        Shipped = 2,
        [EnumHelper("<font color='green'>已确认收货</font>")]
        ConfirmReceipt = 3,
        [EnumHelper("<font color='green'>订单已完成</font>")]
        Complete = 4,
        [EnumHelper("<font color='gray'>订单已退款</font>")]
        Refunded = 5,
        [EnumHelper("<font color='gray'>订单已取消</font>")]
        Cancelled = 6,
        [EnumHelper("<font color='gray'>申请换货</font>")]
        ApplyGoods = 7,
        [EnumHelper("<font color='gray'>申请退款</font>")]
        ApplyRefund = 8
    }
    [Serializable]
    [EnumHelper("退款原因")]
    public enum RefundReason
    {
        [EnumHelper("请选择退款原因")]
        Please = -1,
        [EnumHelper("<font color='red'>商品错发/漏发</font>")]
        Wrong = 0,
        [EnumHelper("<font color='blue'>发票问题</font>")]
        Ticket = 1,
        [EnumHelper("<font color='green'>收到商品与描述不符</font>")]
        DescWrong = 2,
        [EnumHelper("<font color='green'>商品质量问题</font>")]
        Quality = 3,
        [EnumHelper("<font color='gray'>未按规定时间发货</font>")]
        SendTime = 4,
        [EnumHelper("<font color='gray'>7天无理由退换货</font>")]
        SevenDay = 5
    }
    [Serializable]
    [EnumHelper("日志类型")]
    public enum LogType
    {
        [EnumHelper("订单操作日志")]
        OrderLog = 1,
        [EnumHelper("退款操作日志")]
        OrderRefundLog = 2,
        [EnumHelper("登录日志")]
        LoginLog = 3,
        [EnumHelper("修改日志")]
        ModifyLog = 4, 
    }
     
    [Serializable]
    [EnumHelper("退款状态")]
    public enum RefundStatus
    {
        [EnumHelper("全部")]
        Please = -1,
        [EnumHelper("申请退款")]
        RefundFee = 0,
        [EnumHelper("申请退货")]
        RefundGoods = 1,
        [EnumHelper("已发货")]
        Shipped = 2,
        [EnumHelper("已收货")]
        Received = 3,
        [EnumHelper("同意申请")]
        Agree = 4,
        [EnumHelper("拒绝申请")]
        DisAgree = 5,
        [EnumHelper("已换货")]
        Changed = 6,
        [EnumHelper("完成")]
        Complate= 7,
    }
     



    [Serializable]
    [EnumHelper("支付状态")]
    public enum PaymentStatus
    {
        [EnumHelper("<font color='red'>正在支付</font>")]
        Paying = 0,
        [EnumHelper("<font color='green'>已支付</font>")]
        Processing = 1,
        [EnumHelper("<font color='green'>部分支付</font>")]
        PayPart = 2,
        [EnumHelper("<font color='gray'>订单过期</font>")]
        Refunded = 3 
    }

    [Serializable]
    [EnumHelper("结算状态")]
    public enum SettlementStatus
    {
        [EnumHelper("未达到结算时间")]
        CanNotSettle = -1,
        [EnumHelper("<font color='red'>结算中</font>")]
        Settling = 0,
        [EnumHelper("<font color='green'>已结算</font>")]
        Settled = 1
    }



    [Serializable]
    [EnumHelper("支付方式")]
    public enum PaymentMethod
    {
        [EnumHelper("网银支付")]
        CyberBank,
        [EnumHelper("支付宝")]
        Alipay,
        [EnumHelper("货到付款")]
        COD,
        [EnumHelper("微信支付")]
        WeChatPay,
        [EnumHelper("其他")]
        OtherPay,
    }


    [Serializable]
    [EnumHelper("Navigation")]
    public enum Navigation
    {

        [EnumHelper("<font color='blue'>导航</font>")]
        Yes = 1,
        [EnumHelper("<font color='red'>操作按钮</font>")]
        No,
    }


    [Serializable]
    [EnumHelper("缓存类别")]
    public enum CacheType
    {
        LowCache,//2分钟
        LowerCache,//20分钟
        Normal,//2小时
        HighCache,//1天
        HigherCache,//7天
        HighestCache//30天
    }
    [Serializable]
    public enum WarterLocation
    {
        [EnumHelper("Left")]
        Left,
        Right,
        Top,
        LeftCenter,
        RightCenter,
        Center,
        LeftBottom,
        Botton,
    }


    /// <summary>
    /// 排序类型
    /// </summary>
    public enum SortType
    {
        /// <summary>
        ///按枚举顺序默认排序
        /// </summary>
        Default,

        /// <summary>
        /// 按描述值排序
        /// </summary>
        DisplayText,

        /// <summary>
        /// 按枚举值排序
        /// </summary>
        Rank
    }
    /// <summary>
    /// 删除状态
    /// </summary>
    [Serializable]
    [EnumHelper("删除状态")]
    public enum Delete
    {
        All = -1,
        [EnumHelper("未删除")]
        No,
        [EnumHelper("<font color=red>已删除</font>")]
        Yes,

    }
    public enum MessageType
    {
        [EnumHelper("send")]
        Send,
        [EnumHelper("receive")]
        Receive,

    }



    [Serializable]
    [EnumHelper("用户定制")]
    public enum UserMaker
    {
        All = -1,
        [EnumHelper("不可定制")]
        No,
        [EnumHelper("<font color=red>可定制</font>")]
        Yes,

    }
    [Serializable]
    [EnumHelper("是否与价格相关")]
    public enum IsForPrice
    {
        All = -1,
        [EnumHelper("非价格相关")]
        No,
        [EnumHelper("<font color=red>价格相关</font>")]
        Yes,
    }

    [Serializable]
    [EnumHelper("是否可修改")]
    public enum CanModify
    {
        All = -1,
        [EnumHelper("不可修改")]
        No,
        [EnumHelper("<font color=red>可修改值</font>")]
        Yes,
    }


    [Serializable]
    [EnumHelper("是否图片展示")]
    public enum NeedImage
    {
        All = -1,
        [EnumHelper("非图片展示")]
        No,
        [EnumHelper("<font color=red>必须上传图片</font>")]
        Yes,
    }


    [Serializable]
    [EnumHelper("是否必填")]
    public enum IsRequired
    {
        All = -1,
        [EnumHelper("选填")]
        No,
        [EnumHelper("<font color=red>必填</font>")]
        Yes,

    }



    [Serializable]
    [EnumHelper("是否弹出显示")]
    public enum PopShow
    {
        All = -1,
        No,
        Yes
    }

    [Serializable]
    [EnumHelper("是否高亮显示(热门)")]
    public enum IsHot
    {
        All = -1,
        No,
        Yes
    }


    [EnumHelper("兜有影院枚举")]
    public enum DooyoMethods
    {
        [EnumHelper("查询区域列表方法-getAreaList")]
        GetAreaList,
        [EnumHelper("查询某一区域/某影院正在热映的影片信息-getHotFilms")]
        GetHotFilms,
        [EnumHelper("查询即将上映影片-getComingFilms")]
        GetComingFilms,
        [EnumHelper("查询指定城市影院列表-getCinemas")]
        GetCinemas,
        [EnumHelper("查询影院详情-getCinemaInfo")]
        GetCinemaInfo,
        [EnumHelper("查询影片详情-getFilmInfo")]
        GetFilmInfo,
        [EnumHelper("查询指定区域、指定影片的排期信息-getShowTimeByAreaNoFilmNo")]
        GetShowTimeByAreaNoFilmNo,
        [EnumHelper("查询指定区域、指定影院的所有影片排期信息-getShowTimeByAreaNoCinemaNo")]
        GetShowTimeByAreaNoCinemaNo,
        [EnumHelper("查询指定影院、指定影片的排期信息-getShowTimeByCinemaNoFilmNo")]
        GetShowTimeByCinemaNoFilmNo,
        [EnumHelper("查询影评列表(暂不支持)-getComments")]
        GetComments,
        [EnumHelper("查询通兑票列表-getCommTickets")]
        GetCommTickets,
        [EnumHelper("查询影厅实时座位-getSeat")]
        GetSeat,
        [EnumHelper("下选座票订单-createSeatTicketOrder")]
        CreateSeatTicketOrder,
        [EnumHelper("生成通兑票订单-createCommTicketOrder")]
        CreateCommTicketOrder,
        [EnumHelper("查询订单列表-qryOrders")]
        QryOrders,
        [EnumHelper("查询订单详情-qryOrderDetail")]
        QryOrderDetail,
        [EnumHelper("支付确认(第三方支付)-confirmOrder")]
        ConfirmOrder,
    }
    [EnumHelper("兜有影院枚举")]
    public enum DooyoProductSize
    {
        [EnumHelper("2D")]
        ProductSize1 = 1,
        [EnumHelper("3D")]
        ProductSize2,
        [EnumHelper("4D")]
        ProductSize3,
        [EnumHelper("IMAX")]
        ProductSize4,
        [EnumHelper("点卡")]
        ProductSize5,
    }
    [EnumHelper("兜有影院枚举")]
    public enum DooyoShowType
    {
        [EnumHelper("2D")]
        ShowType1,
        [EnumHelper("3D")]
        ShowType2,
        [EnumHelper("4D")]
        ShowType3,
        [EnumHelper("IMAX")]
        ShowType4,
    }


    public enum ImageUrlEnum
    {
        /// <summary>
        /// 小尺寸30x30像素
        /// </summary>
        small = 30,
        /// <summary>
        /// 中尺寸50x50像素
        /// </summary>
        middle = 50,
        /// <summary>
        /// 大尺寸180x180像素
        /// </summary>
        big = 180
    }





    public enum AttributeType
    {
        [EnumHelper("文本框")]
        TextBox = 1,

        [EnumHelper("下拉框")]
        DropdownList,

        [EnumHelper("多选框")]
        CheckBox,

        [EnumHelper("单选框")]
        Radio,
    }

}
