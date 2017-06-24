------------------------------Generate Table Script------------------------------
---------Table Name : ApplicationAuthorization-----App接口使用
CREATE TABLE [dbo].[ApplicationAuthorization]([AppId] int NOT NULL ,[AppName] nvarchar(250) NULL ,[AppSafeCode] nvarchar(68) NULL ,[AppSecret] nvarchar(68) NULL ,[AppServerIps] nvarchar(800) NULL ,[AppStatus] int NULL ,[CreatedOn] datetime NULL ,[IsDelete] int NULL ,[UpdatedOn] datetime NULL ,CONSTRAINT [PK_APPLICATIONAUTHORIZATION] PRIMARY KEY CLUSTERED ([AppId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON[PRIMARY]

---------Table Name : hx_ActivityTable
CREATE TABLE [dbo].[hx_ActivityTable]([ActID] int IDENTITY(1,1) NOT NULL ,[ActEndtime] datetime NULL ,[ActName] varchar(200) NULL ,[ActRule] varchar(max) NULL ,[ActStarttime] datetime NULL ,[ActState] int NULL ,[ActTypeId] int NULL ,[ActUser] int NULL ,[createtime] datetime NULL ,[RewTypeID] int NULL ,CONSTRAINT [PK_ACTIVITYTABLE] PRIMARY KEY CLUSTERED ([ActID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON[PRIMARY]
ALTER TABLE[dbo].[hx_ActivityTable] ADD CONSTRAINT[DF_hx_ActivityTable_ActState]  DEFAULT(((0))) FOR[ActState]
ALTER TABLE[dbo].[hx_ActivityTable] ADD CONSTRAINT[DF_hx_ActivityTable_createtime]  DEFAULT((getdate())) FOR[createtime]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0  默认(未上线)     1 进行中(上线)    2 结束(下线)   3 停止' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_ActivityTable', @level2type=N'COLUMN',@level2name=N'ActState'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0  首次用户注册
   1 首次投资用户
   2 非首投用户
   3 每标首投用户
   4 每标最大投资用户
   5 所有投资用户
   
   ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_ActivityTable', @level2type=N'COLUMN',@level2name=N'ActUser'
---------Table Name : hx_ActivityType---活动类型
CREATE TABLE [dbo].[hx_ActivityType]([ActTypeId] int IDENTITY(1,1) NOT NULL ,[ActName] varchar(200) NULL ,CONSTRAINT [PK_ACTIVITYTYPE] PRIMARY KEY CLUSTERED ([ActTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON[PRIMARY]
---------Table Name : hx_AdminLimitInfo---？？？？
CREATE TABLE [dbo].[hx_AdminLimitInfo]([id] int IDENTITY(1,1) NOT NULL ,[ActionName] varchar(100) NOT NULL ,[ControllerName] varchar(100) NOT NULL ,[CreatTime] datetime NULL ,[isDel] int NULL ,[lastOper] varchar(50) NULL ,[lastTime] datetime NULL ,[level] int NULL ,[ParentId] int NOT NULL ,[SortId] int NULL ,[title] varchar(100) NULL ,[UrlPara] varchar(100) NULL ,CONSTRAINT [PK_ADMINLIMITINFO] PRIMARY KEY CLUSTERED ([id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON[PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AdminLimitInfo', @level2type=N'COLUMN',@level2name=N'CreatTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AdminLimitInfo', @level2type=N'COLUMN',@level2name=N'isDel'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后操作人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AdminLimitInfo', @level2type=N'COLUMN',@level2name=N'lastOper'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AdminLimitInfo', @level2type=N'COLUMN',@level2name=N'lastTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'级别：1-栏目级别，2-组级别，3-页面级别，4-按钮功能级别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AdminLimitInfo', @level2type=N'COLUMN',@level2name=N'level'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父节点id，无父节点则为0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AdminLimitInfo', @level2type=N'COLUMN',@level2name=N'ParentId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序，正序排列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AdminLimitInfo', @level2type=N'COLUMN',@level2name=N'SortId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AdminLimitInfo', @level2type=N'COLUMN',@level2name=N'title'
---------Table Name : hx_DepUserLimit部门用户权限表
CREATE TABLE [dbo].[hx_DepUserLimit]([id] int IDENTITY(1,1) NOT NULL ,[adminUserId] int NULL ,[createTime] datetime NULL ,[departmentId] int NULL ,[limitId] int NULL ,[limitType] int NULL ,CONSTRAINT [PK_hx_DepUserLimit] PRIMARY KEY CLUSTERED ([id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON[PRIMARY]
ALTER TABLE[dbo].[hx_DepUserLimit] ADD CONSTRAINT[DF_hx_DepUserLimit_createTime]  DEFAULT((getdate())) FOR[createTime]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_DepUserLimit', @level2type=N'COLUMN',@level2name=N'adminUserId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_DepUserLimit', @level2type=N'COLUMN',@level2name=N'departmentId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_DepUserLimit', @level2type=N'COLUMN',@level2name=N'limitId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型：1-部门权限，2-用户权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_DepUserLimit', @level2type=N'COLUMN',@level2name=N'limitType'
---------Table Name : hx_RewardType----- 奖励类型
CREATE TABLE [dbo].[hx_RewardType]([RewTypeID] int IDENTITY(1,1) NOT NULL ,[RewTypeName] varchar(200) NULL ,CONSTRAINT [PK_REWARDTYPE] PRIMARY KEY CLUSTERED ([RewTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON[PRIMARY]
---------Table Name : hx_SMSContext----ef中引用，业务层未使用，和hx_userACT存在冗余？
CREATE TABLE [dbo].[hx_SMSContext]([SmsID] int IDENTITY(1,1) NOT NULL ,[ActID] int NULL ,[createtime] datetime NULL ,[SmsFifteen] varchar(300) NULL ,[SmsOne] varchar(300) NULL ,[SmsSeven] varchar(300) NULL ,[SmsSixteen] varchar(300) NULL ,[SmsThree] varchar(300) NULL ,CONSTRAINT [PK_SMSCONTEXT] PRIMARY KEY CLUSTERED ([SmsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON[PRIMARY]
ALTER TABLE[dbo].[hx_SMSContext] ADD CONSTRAINT[DF_hx_SMSContext_createtime]  DEFAULT((getdate())) FOR[createtime]
---------Table Name : hx_td_usrlogininfo---- 前台用户登录日志
CREATE TABLE [dbo].[hx_td_usrlogininfo]([loginid] int IDENTITY(1,1) NOT NULL ,[logincity] varchar(200) NULL ,[loginIP] varchar(20) NULL ,[loginsource] int NULL ,[loginstate] int NULL ,[logintime] datetime NULL ,[Loginusrname] varchar(200) NULL ,[loginusrpass] varchar(200) NULL ,[registerid] int NULL ,CONSTRAINT [PK_USRLOGININFO] PRIMARY KEY CLUSTERED ([loginid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON[PRIMARY]
ALTER TABLE[dbo].[hx_td_usrlogininfo] ADD CONSTRAINT[DF_hx_td_usrlogininfo_loginstate]  DEFAULT(((0))) FOR[loginstate]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 pc   1微信  2 安桌  3 苹果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_usrlogininfo', @level2type=N'COLUMN',@level2name=N'loginsource'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 登录成功  1失败' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_usrlogininfo', @level2type=N'COLUMN',@level2name=N'loginstate'
---------Table Name : hx_UserAct-----用户奖励记录表，替代以前的bonus标？
CREATE TABLE [dbo].[hx_UserAct]([UserAct] int IDENTITY(1,1) NOT NULL ,[ActID] int NULL ,[ActTypeId] int NULL ,[Amt] decimal(20,2) NULL ,[AmtEndtime] datetime NULL ,[AmtProid] int NULL ,[AmtUses] int NULL ,[Createtime] datetime NULL ,[isSmsFifteen] int NULL ,[ISSmsOne] int NULL ,[IsSmsSeven] int NULL ,[isSmsSixteen] int NULL ,[IsSmsThree] int NULL ,[OrderID] decimal(20,0) NULL ,[registerid] int NULL ,[RewTypeID] int NULL ,[Title] nvarchar(100) NULL ,[Usehight] decimal(20,2) NULL ,[Uselower] decimal(20,2) NULL ,[UseState] int NULL ,[UseTime] datetime NULL ,CONSTRAINT [PK_hx_UserAct] PRIMARY KEY CLUSTERED ([UserAct] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON[PRIMARY]
ALTER TABLE[dbo].[hx_UserAct] ADD CONSTRAINT[DF_hx_UserAct_Amt]  DEFAULT(((0.00))) FOR[Amt]
ALTER TABLE[dbo].[hx_UserAct] ADD CONSTRAINT[DF_hx_UserAct_AmtProid]  DEFAULT(((0))) FOR[AmtProid]
ALTER TABLE[dbo].[hx_UserAct] ADD CONSTRAINT[DF_hx_UserAct_isSmsFifteen]  DEFAULT(((0))) FOR[isSmsFifteen]
ALTER TABLE[dbo].[hx_UserAct] ADD CONSTRAINT[DF_hx_UserAct_ISSmsOne]  DEFAULT(((0))) FOR[ISSmsOne]
ALTER TABLE[dbo].[hx_UserAct] ADD CONSTRAINT[DF_hx_UserAct_IsSmsSeven]  DEFAULT(((0))) FOR[IsSmsSeven]
ALTER TABLE[dbo].[hx_UserAct] ADD CONSTRAINT[DF_hx_UserAct_isSmsSixteen]  DEFAULT(((0))) FOR[isSmsSixteen]
ALTER TABLE[dbo].[hx_UserAct] ADD CONSTRAINT[DF_hx_UserAct_IsSmsThree]  DEFAULT(((0))) FOR[IsSmsThree]
ALTER TABLE[dbo].[hx_UserAct] ADD CONSTRAINT[DF_hx_UserAct_Usehight]  DEFAULT(((0.00))) FOR[Usehight]
ALTER TABLE[dbo].[hx_UserAct] ADD CONSTRAINT[DF_hx_UserAct_Uselower]  DEFAULT(((0.00))) FOR[Uselower]
ALTER TABLE[dbo].[hx_UserAct] ADD CONSTRAINT[DF_hx_UserAct_UseState]  DEFAULT(((0))) FOR[UseState]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 仅单独使用  2 可组合使用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_UserAct', @level2type=N'COLUMN',@level2name=N'AmtUses'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 默认未通知   1通知' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_UserAct', @level2type=N'COLUMN',@level2name=N'ISSmsOne'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 不限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_UserAct', @level2type=N'COLUMN',@level2name=N'Usehight'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 不限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_UserAct', @level2type=N'COLUMN',@level2name=N'Uselower'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 未使用   1已使用  2 已过期  3 锁定中' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_UserAct', @level2type=N'COLUMN',@level2name=N'UseState'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动主题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_UserAct', @level2type=N'COLUMN',@level2name=N'Title'

----------------Table Name : Statistics_ActiveSepteberCashback 复投返现表-- fangjianmin---------------------------
------------------------ 表追加 用户 注册ID    UserId 列
CREATE TABLE [dbo].[Statistics_ActiveSepteberCashback](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[InvestTerm] [int] NOT NULL,
	[InvestTimes] [int] NOT NULL,
	[InvestTotalAmount] [numeric](38, 2) NOT NULL,
	[CashbackAmount] [numeric](38, 2) NOT NULL,
	[HasCashback] [int] NOT NULL,
 CONSTRAINT [PK_hx_Statistics_ActiveSepteberCashback] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Statistics_ActiveSepteberCashback] ADD  CONSTRAINT [DF_Statistics_ActiveSepteberCashback_UserId]  DEFAULT ((0)) FOR [UserId]
GO

ALTER TABLE [dbo].[Statistics_ActiveSepteberCashback] ADD  CONSTRAINT [DF_Statistics_ActiveSepteberCashback_InvestTerm]  DEFAULT ((0)) FOR [InvestTerm]
GO

ALTER TABLE [dbo].[Statistics_ActiveSepteberCashback] ADD  CONSTRAINT [DF_Statistics_ActiveSepteberCashback_InvestTimes]  DEFAULT ((0)) FOR [InvestTimes]
GO

ALTER TABLE [dbo].[Statistics_ActiveSepteberCashback] ADD  CONSTRAINT [DF_Statistics_ActiveSepteberCashback_InvestTotalAmount]  DEFAULT ((0.00)) FOR [InvestTotalAmount]
GO

ALTER TABLE [dbo].[Statistics_ActiveSepteberCashback] ADD  CONSTRAINT [DF_Statistics_ActiveSepteberCashback_CashbackAmount]  DEFAULT ((0.00)) FOR [CashbackAmount]
GO

ALTER TABLE [dbo].[Statistics_ActiveSepteberCashback] ADD  CONSTRAINT [DF_Statistics_ActiveSepteberCashback_HasCashback]  DEFAULT ((0)) FOR [HasCashback]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户注册唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistics_ActiveSepteberCashback', @level2type=N'COLUMN',@level2name=N'UserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistics_ActiveSepteberCashback', @level2type=N'COLUMN',@level2name=N'UserName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投资期限,仅包括 1个月、2个月、6个月投资期限类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistics_ActiveSepteberCashback', @level2type=N'COLUMN',@level2name=N'InvestTerm'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投资次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistics_ActiveSepteberCashback', @level2type=N'COLUMN',@level2name=N'InvestTimes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投资金额总计' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistics_ActiveSepteberCashback', @level2type=N'COLUMN',@level2name=N'InvestTotalAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返现金额总计' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistics_ActiveSepteberCashback', @level2type=N'COLUMN',@level2name=N'CashbackAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已返现' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistics_ActiveSepteberCashback', @level2type=N'COLUMN',@level2name=N'HasCashback'
GO


------------------------------Generate Changed Table Member Script------------------------------
---------Checked Table : ApplicationAuthorization
---------Checked Table : area
---------Checked Table : bonus_account
---------Checked Table : bonus_account_water----？？是否还在使用？
Alter table bonus_account_water add [Createtime] datetime NULL ;
ALTER TABLE[dbo].[bonus_account_water] ADD CONSTRAINT[DF_bonus_account_water_Createtime]  DEFAULT((getdate())) FOR[Createtime];
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'bonus_account_water', @level2type=N'COLUMN',@level2name=N'Createtime';
---------Checked Table : city
---------Checked Table : Contract_management
---------Checked Table : GrabIphone
---------Checked Table : guarantee_way
---------Checked Table : hx_Activity_schedule
---------Checked Table : hx_ActivityTable
---------Checked Table : hx_ActivityType
---------Checked Table : hx_AdminLimitInfo
---------Checked Table : hx_Bid_records-----增加 代金券金额/加息券加息比例
Alter table hx_Bid_records add [BonusAmt] decimal(10,2) NULL 
Alter table hx_Bid_records add [JiaxiNum] decimal(6,2) NULL 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'代金券金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Bid_records', @level2type=N'COLUMN',@level2name=N'BonusAmt'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加息券加息比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Bid_records', @level2type=N'COLUMN',@level2name=N'JiaxiNum'
---------Checked Table : hx_bonding_company
---------Checked Table : hx_borrower_guarantor_picture---？图片排序？
Alter table hx_borrower_guarantor_picture add [picture_index] int NULL 
ALTER TABLE[dbo].[hx_borrower_guarantor_picture] ADD CONSTRAINT[DF_hx_borrower_guarantor_picture_picture_index]  DEFAULT(((1))) FOR[picture_index]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片索引位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_borrower_guarantor_picture', @level2type=N'COLUMN',@level2name=N'picture_index'
---------Checked Table : hx_borrowing_target---抵押物Collateral；逻辑删除isDel；是否加息Isinterest在用；IsIRC是否可用加息劵（未使用）；PaySource还款来源渠道？；Purpose借款用途
Alter table hx_borrowing_target add [Collateral] varchar(200) NULL 
Alter table hx_borrowing_target add [isDel] int NULL 
ALTER TABLE[dbo].[hx_borrowing_target] ADD CONSTRAINT[DF_hx_borrowing_target_isDel]  DEFAULT(((0))) FOR[isDel]
Alter table hx_borrowing_target add [Isinterest] int NULL 
ALTER TABLE[dbo].[hx_borrowing_target] ADD CONSTRAINT[DF_hx_borrowing_target_Isinterest]  DEFAULT(((0))) FOR[Isinterest]
Alter table hx_borrowing_target add [IsIRC] int NULL 
ALTER TABLE[dbo].[hx_borrowing_target] ADD CONSTRAINT[DF_hx_borrowing_target_IsIRC]  DEFAULT(((0))) FOR[IsIRC]
Alter table hx_borrowing_target add [PaySource] varchar(200) NULL 
Alter table hx_borrowing_target add [Purpose] varchar(200) NULL 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'抵押物' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_borrowing_target', @level2type=N'COLUMN',@level2name=N'Collateral'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_borrowing_target', @level2type=N'COLUMN',@level2name=N'isDel'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否加息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_borrowing_target', @level2type=N'COLUMN',@level2name=N'Isinterest'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可用加息劵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_borrowing_target', @level2type=N'COLUMN',@level2name=N'IsIRC'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'还款来源渠道' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_borrowing_target', @level2type=N'COLUMN',@level2name=N'PaySource'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'借款用途' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_borrowing_target', @level2type=N'COLUMN',@level2name=N'Purpose'
---------Checked Table : hx_borrowing_target_detailed
---------Checked Table : hx_Capital_account_water
---------Checked Table : hx_CashAwards
---------Checked Table : hx_Contract_template
---------Checked Table : hx_contract_type
---------Checked Table : hx_DepUserLimit
---------Checked Table : hx_income_statement
---------Checked Table : hx_investsearch
---------Checked Table : hx_member_ChannelKeys
---------Checked Table : hx_member_table------ 修改不为空 [account_total_assets]总资产；修改不为空[available_balance]可用；用户头像avatar
---calltime最近沟通时间；提现次数CashNum ；客服沟通次数CommNum；投资次数InvestNum？？？？老用户投资记录需要补充投资次数；LoginNum登录次数；LostInvitation邀请是否有效？？；充值次数RechargeNum  
Alter table hx_member_table alter column [account_total_assets] numeric(38,2) NOT NULL 
Alter table hx_member_table alter column [available_balance] numeric(38,2) NOT NULL 
Alter table hx_member_table add [avatar] varchar(200) NULL 
ALTER TABLE[dbo].[hx_member_table] ADD CONSTRAINT[DF_hx_member_table_avatar]  DEFAULT(((0))) FOR[avatar]
Alter table hx_member_table add [calltime] datetime NULL 
Alter table hx_member_table add [CashNum] int NULL 
ALTER TABLE[dbo].[hx_member_table] ADD CONSTRAINT[DF_hx_member_table_CashNum]  DEFAULT(((0))) FOR[CashNum]
Alter table hx_member_table alter column [collect_total_amount] numeric(38,2) NOT NULL 
Alter table hx_member_table add [CommNum] int NULL 
ALTER TABLE[dbo].[hx_member_table] ADD CONSTRAINT[DF_hx_member_table_CommNum]  DEFAULT(((0))) FOR[CommNum]
Alter table hx_member_table alter column [frozen_sum] numeric(38,2) NOT NULL 
Alter table hx_member_table add [InvestNum] int NULL 
ALTER TABLE[dbo].[hx_member_table] ADD CONSTRAINT[DF_hx_member_table_InvestNum]  DEFAULT(((0))) FOR[InvestNum]
Alter table hx_member_table add [LoginNum] int NULL 
ALTER TABLE[dbo].[hx_member_table] ADD CONSTRAINT[DF_hx_member_table_LoginNum]  DEFAULT(((0))) FOR[LoginNum]
Alter table hx_member_table add [LostInvitation] int NULL 
ALTER TABLE[dbo].[hx_member_table] ADD CONSTRAINT[DF_hx_member_table_LostInvitation]  DEFAULT(((0))) FOR[LostInvitation]
Alter table hx_member_table add [RechargeNum] int NULL 
ALTER TABLE[dbo].[hx_member_table] ADD CONSTRAINT[DF_hx_member_table_RechargeNum]  DEFAULT(((0))) FOR[RechargeNum]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户头像' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_member_table', @level2type=N'COLUMN',@level2name=N'avatar'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近沟通时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_member_table', @level2type=N'COLUMN',@level2name=N'calltime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提现次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_member_table', @level2type=N'COLUMN',@level2name=N'CashNum'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客服沟通次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_member_table', @level2type=N'COLUMN',@level2name=N'CommNum'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投资次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_member_table', @level2type=N'COLUMN',@level2name=N'InvestNum'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_member_table', @level2type=N'COLUMN',@level2name=N'LoginNum'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邀请是否有效' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_member_table', @level2type=N'COLUMN',@level2name=N'LostInvitation'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'充值次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_member_table', @level2type=N'COLUMN',@level2name=N'RechargeNum'
---------Checked Table : hx_Mention_charges
---------Checked Table : hx_Project_type
---------Checked Table : hx_Recharge_history
---------Checked Table : hx_repayment_plan
---------Checked Table : hx_RewardType
---------Checked Table : hx_SMSContext
---------Checked Table : hx_td_about_news
Alter table hx_td_about_news alter column comm int NULL 
Alter table hx_td_about_news add CONSTRAINT[PK_newid]  PRIMARY KEY ([newId]) ----修改为增加主键？？？？ 已修改
Alter table hx_td_about_news add [newimg] nvarchar(550) NULL ---新闻图片路径
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻图片路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_about_news', @level2type=N'COLUMN',@level2name=N'newimg'
---------Checked Table : hx_td_Ad
---------Checked Table : hx_td_admin_role
---------Checked Table : hx_td_adminuser
---------Checked Table : hx_td_Bank
Alter table hx_td_Bank add [CardImage] varchar(100) NULL --银行logo图片路径	
Alter table hx_td_Bank add [isGren] int NULL --- 猜测该列用于普通充值 ，而Isordinary未使用？
ALTER TABLE[dbo].[hx_td_Bank] ADD CONSTRAINT[DF_hx_td_Bank_isGren]  DEFAULT(((0))) FOR[isGren]
Alter table hx_td_Bank add [Isordinary] int NULL --- 普通充值    Isordinary  int  (0 默认    1 快捷)
ALTER TABLE[dbo].[hx_td_Bank] ADD CONSTRAINT[DF_hx_td_Bank_Isordinary]  DEFAULT(((0))) FOR[Isordinary]
Alter table hx_td_Bank add [Isquick] int NULL --- 快捷充值    Isquick     int  (0 默认    1 快捷)
ALTER TABLE[dbo].[hx_td_Bank] ADD CONSTRAINT[DF_hx_td_Bank_Isquick]  DEFAULT(((0))) FOR[Isquick]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行logo图片路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_Bank', @level2type=N'COLUMN',@level2name=N'CardImage'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否是绿色通道' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_Bank', @level2type=N'COLUMN',@level2name=N'isGren'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否是普通充值(0 默认    1 快捷)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_Bank', @level2type=N'COLUMN',@level2name=N'Isordinary'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否是快捷充值(0 默认    1 快捷)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_Bank', @level2type=N'COLUMN',@level2name=N'Isquick'
---------Checked Table : hx_td_department
---------Checked Table : hx_td_department_child
---------Checked Table : hx_td_frozen
---------Checked Table : hx_td_Links
Alter table hx_td_Links add CONSTRAINT [PK_Linkid]  PRIMARY KEY (Linkid) ----修改为增加主键？？？？ 已修改
---------Checked Table : hx_td_LL_cash
---------Checked Table : hx_td_LLBack
---------Checked Table : hx_td_LLPay_bindCard
---------Checked Table : hx_td_LLpay_city
---------Checked Table : hx_td_LLpay_re_cash
---------Checked Table : hx_td_LLpay_Recharge
---------Checked Table : hx_td_Loan_records
---------Checked Table : hx_td_LoginInfo
---------Checked Table : hx_td_menu
---------Checked Table : hx_td_Myborrow
---------Checked Table : hx_td_Phone_records--- gtType增加沟通方式，problemType问题类别 
Alter table hx_td_Phone_records add [gtType] varchar(50) NULL 
Alter table hx_td_Phone_records add [problemType] varchar(50) NULL 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'沟通方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_Phone_records', @level2type=N'COLUMN',@level2name=N'gtType'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'问题类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_Phone_records', @level2type=N'COLUMN',@level2name=N'problemType'
---------Checked Table : hx_td_reviewremarks
---------Checked Table : hx_td_SMS_record
---------Checked Table : hx_td_SMSEmail
---------Checked Table : hx_td_System_message
---------Checked Table : hx_td_UserCash   PayAmt 实际支付（取现金额-手续费） update更新历史记录为TransAmt-FeeAmt？？？？
Alter table hx_td_UserCash add [PayAmt] decimal(38,2) NULL 
ALTER TABLE[dbo].[hx_td_UserCash] ADD CONSTRAINT[DF_hx_td_UserCash_PayAmt]  DEFAULT(((0.00))) FOR[PayAmt] 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际支付（取现金额-手续费）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_UserCash', @level2type=N'COLUMN',@level2name=N'PayAmt'
---------Checked Table : hx_td_Userinvitation -- UserAct用户活动表的奖励id
Alter table hx_td_Userinvitation add [UserAct] int NULL 
ALTER TABLE[dbo].[hx_td_Userinvitation] ADD CONSTRAINT[DF_hx_td_Userinvitation_UserAct]  DEFAULT(((0))) FOR[UserAct]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户活动表的奖励id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_td_Userinvitation', @level2type=N'COLUMN',@level2name=N'UserAct'
---------Checked Table : hx_td_usrlogininfo
---------Checked Table : hx_td_web_Ad_type
---------Checked Table : hx_td_web_type
---------Checked Table : hx_Test_records
---------Checked Table : hx_UserAct
---------Checked Table : hx_UsrBindCardC  ---绑卡方式	BindCardType（0 普通    1快捷绑卡 QP）
Alter table hx_UsrBindCardC add [BindCardType] int NULL 
ALTER TABLE[dbo].[hx_UsrBindCardC] ADD CONSTRAINT[DF_hx_UsrBindCardC_BindCardType]  DEFAULT(((0))) FOR[BindCardType]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'绑卡方式	BindCardType（0 普通    1快捷绑卡 QP）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_UsrBindCardC', @level2type=N'COLUMN',@level2name=N'BindCardType'
---------Checked Table : LuckDrawRecord
---------Checked Table : NormalArea
---------Checked Table : Overdue_repayment
---------Checked Table : province
---------Checked Table : Statistics_Member
---------Checked Table : UserAddress
Alter table UserAddress add [mobile] nvarchar(20) NULL 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAddress', @level2type=N'COLUMN',@level2name=N'mobile'
Alter table UserAddress add [userName] nvarchar(50) NULL 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAddress', @level2type=N'COLUMN',@level2name=N'userName'
Alter table UserAddress add [zipCode] nvarchar(10) NULL 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮政编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAddress', @level2type=N'COLUMN',@level2name=N'zipCode'
---------Checked Table : Activity
---------Checked Table : ActivityLogs
---------Checked Table : ActivityRewardLogs
---------Checked Table : hx_ChouJiang
---------Checked Table : hx_member_table
GO


--------------------- 测试库手工变更补录---------fangjianmin
--nvarchar
---Alter Table ApplicationAuthorization Alter Column [AppName] varchar(250) NULL 
--Alter Table ApplicationAuthorization Alter Column [AppSafeCode] varchar(68) NULL 
--Alter Table ApplicationAuthorization Alter Column [AppSecret] varchar(68) NULL 
--Alter Table ApplicationAuthorization Alter Column [AppServerIps] varchar(800) NULL 
--Alter Table hx_td_about_news Alter Column [newimg] varchar(300) NULL 
Alter Table hx_td_admin_role Add CONSTRAINT PK_id PRIMARY KEY (id)
--Alter Table hx_UserAct Alter Column [Title] nvarchar(100) NULL 
--Alter Table UserAddress Alter Column [mobile] nvarchar(20) NULL 
--Alter Table UserAddress Alter Column [userName] nvarchar(50) NULL 
--Alter Table UserAddress Alter Column [zipCode] nvarchar(10) NULL 


-----------为新增整型字段添加默认值---fangjianmin-----------------------
update hx_member_table set LostInvitation=0;
update hx_member_table set LoginNum=0;
update hx_UsrBindCardC set BindCardType=0;
update hx_td_UserCash set PayAmt=0.00;
update hx_borrowing_target set isDel=0;
update hx_borrowing_target set IsIRC=0;
update hx_borrowing_target set Isinterest=0;
update hx_Bid_records set JiaxiNum=0.00,BonusAmt=0.00;
update hx_td_Userinvitation set UserAct=0;

ALTER TABLE [hx_Bid_records] add CONSTRAINT  DF_hx_Bid_records_JiaxiNum DEFAULT 0 FOR [JiaxiNum]
ALTER TABLE [hx_Bid_records] add CONSTRAINT  DF_hx_Bid_records_BonusAmt DEFAULT 0 FOR [BonusAmt]




------------------------------Generate Views Script------------------------------
---------View Name : ViewUserInviteInfor---- 增加distinct
if object_id(N'ViewUserInviteInfor') is not null drop view ViewUserInviteInfor
GO 
CREATE VIEW dbo.ViewUserInviteInfor
AS
SELECT DISTINCT 
                invitationid, InviteId, InviteName, InviteRealName, InvitedId, InvitedName, InvitedRealName, InviteCode, 
                CreatedOn, InvestedOn, InvestAmount, OrderId, BorrowTitle, DeadLine, DeadLineNumber, 
                (CASE WHEN IsBindedCard = 0 AND NOT EXISTS
                    (SELECT   cardstate
                     FROM      [hx_td_LLPay_bindCard] lb
                     WHERE   (lb.cardstate = 1 OR
                                     lb.cardstate = 0) AND lb.Usrid = InvitedId) THEN 0 ELSE 1 END) AS IsBindedCard
FROM      (SELECT   ui.invitationid, ui.Invpeopleid AS InviteId, imt.username AS InviteName, imt.realname AS InviteRealName, 
                                 mt.registerid AS InvitedId, mt.username AS InvitedName, mt.realname AS InvitedRealName, 
                                 ui.invcode AS InviteCode, ui.invtime AS CreatedOn, br.invest_time AS InvestedOn, 
                                 br.investment_amount AS InvestAmount, br.OrdId AS OrderId, bt.borrowing_title AS BorrowTitle, 
                                 CONVERT(VARCHAR(19), bt.life_of_loan) 
                                 + CASE WHEN bt.unit_day = 1 THEN '月' WHEN bt.unit_day = 3 THEN '天' END AS DeadLine, 
                                 (CASE WHEN bt.unit_day = 3 THEN life_of_loan WHEN bt.unit_day = 1 THEN life_of_loan * 30 END) 
                                 AS DeadLineNumber, (CASE WHEN bc.UsrBindCardID IS NULL THEN 0 ELSE 1 END) AS IsBindedCard
                 FROM      dbo.hx_member_table AS mt WITH (NOLOCK) RIGHT OUTER JOIN
                                 dbo.hx_td_Userinvitation AS ui WITH (NOLOCK) ON mt.registerid = ui.invpersonid LEFT OUTER JOIN
                                 dbo.hx_UsrBindCardC AS bc WITH (NOLOCK) ON bc.UsrCustId = mt.UsrCustId AND 
                                 bc.defCard = 1 LEFT OUTER JOIN
                                 dbo.hx_Bid_records AS br ON br.investor_registerid = mt.registerid AND br.bid_records_id =
                                     (SELECT   MIN(bid_records_id) AS Expr1
                                      FROM      dbo.hx_Bid_records AS b2
                                      WHERE   (investor_registerid = br.investor_registerid) AND (ordstate = 1)) LEFT OUTER JOIN
                                 dbo.hx_borrowing_target AS bt WITH (NOLOCK) ON bt.targetid = br.targetid INNER JOIN
                                 dbo.hx_member_table AS imt WITH (NOLOCK) ON imt.registerid = ui.Invpeopleid) AS abc

 GO
---------View Name : View_testss----猜测为无用视图
if object_id(N'View_testss') is not null drop view View_testss
  GO 
CREATE VIEW dbo.View_testss
AS
SELECT   dbo.hx_td_Userinvitation.invpersonid, dbo.hx_member_table.username, dbo.hx_td_Userinvitation.invitationid, 
                dbo.hx_td_Userinvitation.invcode, dbo.hx_td_Userinvitation.invtime, dbo.hx_td_Userinvitation.Invpeopleid, 
                dbo.hx_td_Userinvitation.InvitesStates, dbo.hx_td_Userinvitation.Invitereward, dbo.hx_member_table.userstate, 
                dbo.hx_member_table.registration_time, b.invest_time, b.investment_amount
FROM      dbo.hx_td_Userinvitation INNER JOIN
                dbo.hx_member_table ON dbo.hx_td_Userinvitation.invpersonid = dbo.hx_member_table.registerid LEFT OUTER JOIN
                dbo.hx_Bid_records AS b ON b.investor_registerid = dbo.hx_member_table.registerid
 GO
---------View Name : V_YaoQinList------邀请奖励 使用water_type 关联邀请人
if object_id(N'V_YaoQinList') is not null drop view V_YaoQinList
  GO 
CREATE VIEW dbo.V_YaoQinList
AS
SELECT   dbo.bonus_account_water.account_water_id, dbo.bonus_account_water.bonus_account_id, 
                dbo.bonus_account_water.membertable_registerid, dbo.bonus_account_water.income, 
                dbo.bonus_account_water.expenditure, dbo.bonus_account_water.time_of_occurrence, 
                dbo.bonus_account_water.reward_balance, dbo.bonus_account_water.award_description, 
                dbo.bonus_account_water.water_type, dbo.bonus_account_water.Createtime, dbo.hx_member_table.username
FROM      dbo.bonus_account_water INNER JOIN
                dbo.hx_member_table ON dbo.bonus_account_water.water_type = dbo.hx_member_table.registerid
WHERE   (dbo.bonus_account_water.water_type > 2)
 GO
---------View Name : V_ACT-----用户活动奖励
if object_id(N'V_ACT') is not null drop view V_ACT
  GO 
CREATE VIEW [dbo].[V_ACT]
AS
SELECT   dbo.hx_UserAct.UserAct, dbo.hx_UserAct.RewTypeID, dbo.hx_UserAct.Amt, dbo.hx_UserAct.Uselower, 
                dbo.hx_UserAct.Usehight, dbo.hx_UserAct.AmtEndtime, dbo.hx_UserAct.UseState, dbo.hx_UserAct.AmtProid, 
                dbo.hx_UserAct.UseTime, dbo.hx_UserAct.ISSmsOne, dbo.hx_UserAct.IsSmsThree, dbo.hx_UserAct.IsSmsSeven, 
                dbo.hx_UserAct.isSmsFifteen, dbo.hx_UserAct.isSmsSixteen, dbo.hx_UserAct.OrderID, dbo.hx_UserAct.Createtime, 
                dbo.hx_ActivityTable.ActStarttime, dbo.hx_ActivityTable.ActEndtime, dbo.hx_ActivityTable.ActState, 
                dbo.hx_ActivityTable.ActName, dbo.hx_member_table.username, dbo.hx_member_table.mobile, 
                dbo.hx_member_table.useridentity, dbo.hx_member_table.Channelsource, dbo.hx_ActivityType.ActTypeId, 
                dbo.hx_ActivityTable.ActID, dbo.hx_member_table.realname, dbo.hx_member_table.registration_time, 
                dbo.hx_UserAct.registerid
FROM      dbo.hx_UserAct INNER JOIN
                dbo.hx_ActivityTable ON dbo.hx_UserAct.ActID = dbo.hx_ActivityTable.ActID INNER JOIN
                dbo.hx_member_table ON dbo.hx_UserAct.registerid = dbo.hx_member_table.registerid INNER JOIN
                dbo.hx_ActivityType ON dbo.hx_ActivityTable.ActTypeId = dbo.hx_ActivityType.ActTypeId
 GO
---------View Name : V_AD_type--- 无变化
----if object_id(N'V_AD_type') is not null drop view V_AD_type

----  GO 

----CREATE VIEW dbo.V_AD_type
----AS
----SELECT     dbo.hx_td_Ad.Adid, dbo.hx_td_Ad.AdName, dbo.hx_td_Ad.Adcreatetime, dbo.hx_td_Ad.AdState, dbo.hx_td_Ad.AdTypeId, dbo.hx_td_Ad.AdLink, dbo.hx_td_Ad.AdPath, 
----                      dbo.hx_td_web_Ad_type.AdtypeName
----FROM         dbo.hx_td_Ad INNER JOIN
----                      dbo.hx_td_web_Ad_type ON dbo.hx_td_Ad.AdTypeId = dbo.hx_td_web_Ad_type.AdTypeId


---- GO
---------View Name : v_adminuser_department---无变化
--if object_id(N'v_adminuser_department') is not null drop view v_adminuser_department

--  GO 

--CREATE VIEW dbo.v_adminuser_department
--AS
--SELECT     dbo.hx_td_adminuser.adminuserid, dbo.hx_td_adminuser.adminuser, dbo.hx_td_adminuser.userpass, dbo.hx_td_adminuser.state, dbo.hx_td_adminuser.datetime, 
--                      dbo.hx_td_adminuser.trueName, dbo.hx_td_adminuser.province, dbo.hx_td_adminuser.city, dbo.hx_td_adminuser.email, dbo.hx_td_adminuser.tel, 
--                      dbo.hx_td_adminuser.phone_number, dbo.hx_td_adminuser.lastLoginTime, dbo.hx_td_adminuser.lastLoginIP, dbo.hx_td_adminuser.loginTimes, 
--                      dbo.hx_td_adminuser.worknum, dbo.hx_td_adminuser.sex, dbo.hx_td_adminuser.department_id, dbo.hx_td_adminuser.area_id, dbo.hx_td_department.parentid, 
--                      dbo.hx_td_department.rootid, dbo.hx_td_department.department_name
--FROM         dbo.hx_td_adminuser INNER JOIN
--                      dbo.hx_td_department ON dbo.hx_td_adminuser.department_id = dbo.hx_td_department.department_id


-- GO
---------View Name : V_Bid_Frozen---无变化 
----if object_id(N'V_Bid_Frozen') is not null drop view V_Bid_Frozen

----  GO 

----CREATE VIEW [dbo].[V_Bid_Frozen]
----AS
----SELECT     dbo.hx_Bid_records.bid_records_id, dbo.hx_Bid_records.OrdId, dbo.hx_Bid_records.ordstate, dbo.hx_td_frozen.Frozenid, dbo.hx_td_frozen.UsrCustId, 
----                      dbo.hx_td_frozen.FreezeTrxId, dbo.hx_td_frozen.FrozenState, dbo.hx_td_frozen.FrozenidNo, dbo.hx_td_frozen.targetid
----FROM         dbo.hx_Bid_records INNER JOIN
----                      dbo.hx_td_frozen ON dbo.hx_Bid_records.bid_records_id = dbo.hx_td_frozen.bid_records_id


---- GO
---------View Name : V_Bid_income_statement---无变化
--if object_id(N'V_Bid_income_statement') is not null drop view V_Bid_income_statement

--  GO 


--CREATE VIEW [dbo].[V_Bid_income_statement]
--AS
--SELECT     dbo.hx_income_statement.income_statement_id, dbo.hx_income_statement.targetid, dbo.hx_income_statement.bid_records_id, 
--                      dbo.hx_income_statement.loan_number, dbo.hx_income_statement.borrower_registerid, dbo.hx_income_statement.OutCustId, 
--                      dbo.hx_income_statement.annual_revenue, dbo.hx_income_statement.investment_amount, dbo.hx_income_statement.invest_time, 
--                      dbo.hx_income_statement.current_investment_period, dbo.hx_income_statement.value_date, dbo.hx_income_statement.interest_payment_date, 
--                      dbo.hx_income_statement.repayment_amount, dbo.hx_income_statement.repayment_period, dbo.hx_income_statement.investor_registerid, 
--                      dbo.hx_income_statement.InCustId, dbo.hx_income_statement.payment_status, dbo.hx_income_statement.interestpayment, dbo.hx_income_statement.orderid, 
--                      dbo.hx_income_statement.BidOrderid, dbo.hx_Bid_records.ordstate
--FROM         dbo.hx_Bid_records INNER JOIN
--                      dbo.hx_income_statement ON dbo.hx_Bid_records.bid_records_id = dbo.hx_income_statement.bid_records_id
--WHERE     (dbo.hx_Bid_records.ordstate > 0)


-- GO
---------View Name : V_OwnBonuses ----活动奖励
if object_id(N'V_OwnBonuses') is not null drop view V_OwnBonuses
  GO 
CREATE VIEW [dbo].[V_OwnBonuses]
AS
SELECT dbo.hx_UserAct.UserAct, dbo.hx_UserAct.registerid, dbo.hx_UserAct.RewTypeID, 
      dbo.hx_UserAct.Amt, dbo.hx_UserAct.Uselower, dbo.hx_UserAct.UseState, 
      dbo.hx_ActivityTable.ActName, dbo.hx_ActivityTable.ActStarttime, 
      dbo.hx_ActivityTable.ActEndtime, dbo.hx_UserAct.AmtEndtime, 
      dbo.hx_ActivityType.ActName AS TypeName
FROM dbo.hx_UserAct INNER JOIN
      dbo.hx_ActivityType ON 
      dbo.hx_UserAct.ActTypeId = dbo.hx_ActivityType.ActTypeId LEFT OUTER JOIN
      dbo.hx_ActivityTable ON 
      dbo.hx_ActivityType.ActTypeId = dbo.hx_ActivityTable.ActTypeId AND 
      dbo.hx_UserAct.ActID = dbo.hx_ActivityTable.ActID
 GO
---------View Name : V_bid_records_bonus_account 无变化 
--if object_id(N'V_bid_records_bonus_account') is not null drop view V_bid_records_bonus_account

--  GO 

--CREATE VIEW [dbo].[V_bid_records_bonus_account]
--AS
--SELECT     dbo.hx_Bid_records.bid_records_id, dbo.hx_Bid_records.borrower_registerid, dbo.hx_Bid_records.targetid, dbo.hx_Bid_records.loan_number, 
--                      dbo.hx_Bid_records.annual_interest_rate, dbo.hx_Bid_records.current_period, dbo.hx_Bid_records.investment_amount, dbo.hx_Bid_records.OrdId, 
--                      dbo.bonus_account.proid, dbo.bonus_account.bonus_account_id, dbo.bonus_account.activity_schedule_id, dbo.bonus_account.activity_schedule_name, 
--                      dbo.bonus_account.amount_of_reward, dbo.bonus_account.use_lower_limit, dbo.bonus_account.reward, dbo.bonus_account.start_date, 
--                      dbo.bonus_account.membertable_registerid, dbo.bonus_account.reward_state
--FROM         dbo.hx_Bid_records INNER JOIN
--                      dbo.bonus_account ON dbo.hx_Bid_records.bid_records_id = dbo.bonus_account.proid


-- GO
---------View Name : ViewInvestDetail----无变化
--if object_id(N'ViewInvestDetail') is not null drop view ViewInvestDetail

--  GO 

--CREATE VIEW [dbo].[ViewInvestDetail]
--as
--SELECT
--br.bid_records_id AS recordId
--,br.targetid AS targetId
--,bt.borrowing_title AS targetTitle
--,br.investor_registerid AS investMemberId
--,mt.username AS investMemberName
--,bt.life_of_loan AS deadLine--期限
--,bt.unit_day AS unitDay--单位(月/天) 1 月  3 天
--,br.annual_interest_rate AS rate --年化收益
--,bt.borrowing_balance AS borrowTotalAmount--项目总额
--,bt.fundraising_amount AS fundTotalAmount--已投总金额
--,br.investment_amount AS investAmount--投资金额
--,bt.payment_options AS paymentOption--1 按月等额本息  3 每月还息，到期还本   4 一次性还本付息
--,br.payment_status AS paymentStatus
--,br.value_date AS rateBeginOn--开始计算利息日期
--,br.investment_maturity AS investMaturity--投资到期日
--,bt.repayment_date AS paymentDate--还款日期
--,br.invest_time AS createdOn
--,b.investNumber--已购买人数
--,bt.guarantee_way_id--担保类型
--,bt.month_payment_date--每月付息日
--FROM dbo.hx_Bid_records (NOLOCK) br
--LEFT JOIN dbo.hx_member_table (NOLOCK) mt ON br.investor_registerid=mt.registerid
--LEFT JOIN dbo.hx_borrowing_target (NOLOCK) bt ON bt.targetid=br.targetid 
--LEFT JOIN 
--     (SELECT  targetid,COUNT(DISTINCT(investor_registerid)) AS investNumber  
--              FROM hx_Bid_records   WITH (NOLOCK) 
--              GROUP BY targetid) b ON br.targetid=b.targetid
--GO
---------View Name : V_Bid_records_Lost---新增 无效投资？
if object_id(N'V_Bid_records_Lost') is not null drop view V_Bid_records_Lost
  GO 
CREATE VIEW [dbo].[V_Bid_records_Lost]
AS
SELECT   dbo.hx_Bid_records.bid_records_id, dbo.hx_Bid_records.investment_amount, dbo.hx_Bid_records.OrdId, 
                dbo.hx_Bid_records.invest_time, dbo.hx_borrowing_target.borrowing_title, dbo.hx_borrowing_target.borrowing_balance, 
                dbo.hx_borrowing_target.annual_interest_rate, dbo.hx_Bid_records.investor_registerid, 
                dbo.hx_member_table.username, dbo.hx_member_table.mobile, dbo.hx_member_table.realname, 
                dbo.hx_Bid_records.ordstate, dbo.hx_member_table.UsrCustId, dbo.hx_td_frozen.FrozenidNo, 
                dbo.hx_td_frozen.FreezeTrxId
FROM      dbo.hx_Bid_records INNER JOIN
                dbo.hx_borrowing_target ON dbo.hx_Bid_records.targetid = dbo.hx_borrowing_target.targetid INNER JOIN
                dbo.hx_member_table ON dbo.hx_Bid_records.investor_registerid = dbo.hx_member_table.registerid INNER JOIN
                dbo.hx_td_frozen ON dbo.hx_Bid_records.bid_records_id = dbo.hx_td_frozen.bid_records_id
WHERE   (dbo.hx_Bid_records.ordstate = 0)
 GO
---------View Name : ViewInvestList------ 投资列表 receivableInterest从字段haveinterest变为withoutinterest
if object_id(N'ViewInvestList') is not null drop view ViewInvestList
GO
CREATE VIEW [dbo].[ViewInvestList]
AS 
SELECT 
br.bid_records_id AS recordId
,br.targetid AS targetId
,bt.borrowing_title AS targetTitle
,br.investor_registerid AS investMemberId
,mt.username AS investMemberName
,bt.life_of_loan AS deadLine--期限
,bt.unit_day AS unitDay--单位(月/天) 1 月  3 天
,br.annual_interest_rate AS rate --年化收益
,br.investment_amount AS investAmount--投资金额
,br.payment_status AS paymentStatus
,br.value_date AS rateBeginOn--开始计算利息日期
,bt.month_payment_date --付息日期
,br.investment_maturity AS investMaturity--投资到期日
,br.invest_time AS createdOn
,br.withoutinterest AS receivableInterest--应得利息
,br.ordstate
 FROM dbo.hx_Bid_records (NOLOCK) br
LEFT JOIN dbo.hx_member_table (NOLOCK) mt ON br.investor_registerid=mt.registerid
LEFT JOIN dbo.hx_borrowing_target (NOLOCK) bt ON bt.targetid=br.targetid
GO
---------View Name : V_bonus_account 无变化
--if object_id(N'V_bonus_account') is not null drop view V_bonus_account

--  GO 

--CREATE VIEW [dbo].[V_bonus_account]
--AS
--SELECT     dbo.bonus_account.bonus_account_id, dbo.bonus_account.activity_schedule_id, dbo.bonus_account.activity_schedule_name, dbo.bonus_account.amount_of_reward, 
--                      dbo.bonus_account.use_lower_limit, dbo.bonus_account.reward, dbo.bonus_account.start_date, dbo.bonus_account.end_date, dbo.bonus_account.reward_state, 
--                      dbo.bonus_account.reward_remarks, dbo.bonus_account.entry_time, dbo.bonus_account.act_state, dbo.bonus_account.proid, dbo.hx_member_table.username, 
--                      dbo.hx_member_table.realname, dbo.bonus_account.membertable_registerid, dbo.hx_member_table.mobile, dbo.hx_member_table.iD_number
--FROM dbo.bonus_account 
--INNER JOIN dbo.hx_Activity_schedule ON dbo.bonus_account.activity_schedule_id = dbo.hx_Activity_schedule.activity_schedule_id 
--INNER JOIN dbo.hx_member_table ON dbo.bonus_account.membertable_registerid = dbo.hx_member_table.registerid
--GO
---------View Name : ViewTargetWithRecords  --无变化，规范sql
if object_id(N'ViewTargetWithRecords') is not null drop view ViewTargetWithRecords
  GO 
CREATE VIEW [dbo].[ViewTargetWithRecords] 
AS

SELECT  a.targetid ,a.borrowing_title ,a.indexorder ,a.borrower_registerid ,mt.username ,a.tender_state ,
    a.repayment_date ,a.month_payment_date ,a.project_type_id ,a.borrowing_balance ,a.annual_interest_rate ,
    a.life_of_loan ,a.start_time ,a.release_date ,a.unit_day ,a.payment_options ,a.H_Repayment_Amt ,a.end_time ,
    b.invest_num ,a.fundraising_amount , a.minimum ,a.maxmum ,det.createtime AS CreatedOn

    FROM    dbo.hx_borrowing_target a WITH ( NOLOCK )
        LEFT JOIN dbo.hx_borrowing_target_detailed det WITH ( NOLOCK ) ON det.targetid = a.targetid
        LEFT JOIN ( SELECT  targetid ,
                            COUNT(bid_records_id) AS invest_num ,
                            MAX(invest_time) AS end_time
                    FROM    hx_Bid_records WITH ( NOLOCK )
                    GROUP BY targetid
                  ) b ON a.targetid = b.targetid
        LEFT join	dbo.hx_member_table mt ON mt.registerid = det.borrower_registerid 
		INNER JOIN    dbo.hx_bonding_company x ON a.companyid = x.companyid 
		INNER JOIN    dbo.guarantee_way xx ON a.guarantee_way_id = xx.guarantee_way_id 
		INNER JOIN    dbo.hx_Project_type xxx ON a.project_type_id = xxx.project_type_id
 WHERE   (a.isDel = 0)
GO
---------View Name : V_bonus_account_water  --无变化
--if object_id(N'V_bonus_account_water') is not null drop view V_bonus_account_water

--  GO
--CREATE VIEW [dbo].[V_bonus_account_water]
--AS
--SELECT     dbo.bonus_account_water.account_water_id, dbo.bonus_account_water.bonus_account_id, dbo.bonus_account_water.membertable_registerid, 
--                      dbo.bonus_account_water.income, dbo.bonus_account_water.expenditure, dbo.bonus_account_water.time_of_occurrence, dbo.bonus_account_water.reward_balance, 
--                      dbo.bonus_account_water.award_description, dbo.bonus_account_water.water_type, dbo.bonus_account.start_date, dbo.bonus_account.end_date, 
--                      dbo.bonus_account.act_state, dbo.bonus_account.activity_schedule_name, dbo.bonus_account.reward_state
--FROM dbo.bonus_account 
--INNER JOIN dbo.bonus_account_water ON dbo.bonus_account.bonus_account_id = dbo.bonus_account_water.bonus_account_id


-- GO
---------View Name : ViewIncomeList---新增？？
if object_id(N'ViewIncomeList') is not null drop view ViewIncomeList
  GO 
CREATE VIEW [ViewIncomeList]
  as
	  select  b.bid_records_id,a.income_statement_id, 
		a.investor_registerid, a.investment_amount, 
		a.interest_payment_date,a.interest_payment_date as day, 
		a.repayment_amount,a.payment_status,a.repayment_period,a.invest_time  
	  from hx_income_statement a
	  	left join hx_Bid_records b on b.bid_records_id=a.bid_records_id
	  where b.ordstate=1 
 GO
---------View Name : V_borrow_repayment_plan--- 无变化 
--if object_id(N'V_borrow_repayment_plan') is not null drop view V_borrow_repayment_plan

--  GO 


--CREATE VIEW [dbo].[V_borrow_repayment_plan]
--AS
--SELECT     dbo.hx_borrowing_target.targetid, dbo.hx_borrowing_target.borrower_registerid, dbo.hx_borrowing_target.loan_number, dbo.hx_borrowing_target.borrowing_title, 
--                      dbo.hx_borrowing_target.project_type_id, dbo.hx_borrowing_target.borrowing_thumbnail, dbo.hx_borrowing_target.annual_interest_rate, 
--                      dbo.hx_borrowing_target.borrowing_balance, dbo.hx_borrowing_target.life_of_loan, dbo.hx_borrowing_target.unit_day, dbo.hx_borrowing_target.release_date, 
--                      dbo.hx_borrowing_target.value_date, dbo.hx_borrowing_target.month_payment_date, dbo.hx_borrowing_target.repayment_date, dbo.hx_borrowing_target.minimum, 
--                      dbo.hx_borrowing_target.maxmum, dbo.hx_borrowing_target.companyid, dbo.hx_borrowing_target.guarantee_way_id, dbo.hx_borrowing_target.payment_options, 
--                      dbo.hx_borrowing_target.end_time, dbo.hx_borrowing_target.service_charge, dbo.hx_borrowing_target.loan_management_fee, 
--                      dbo.hx_borrowing_target.investors_management_fee, dbo.hx_borrowing_target.ordinary_overdue_management_fees, 
--                      dbo.hx_borrowing_target.seriously_overdue_management_fees, dbo.hx_borrowing_target.ordinary_overdue_penalty, 
--                      dbo.hx_borrowing_target.seriously_overdue_penalty, dbo.hx_borrowing_target.transfer_Expenses, dbo.hx_borrowing_target.fundraising_amount, 
--                      dbo.hx_borrowing_target.tender_state, dbo.hx_borrowing_target.full_scale_loan, dbo.hx_borrowing_target.flow_return, dbo.hx_borrowing_target.recommend, 
--                      dbo.hx_borrowing_target.sys_time, dbo.hx_borrowing_target.repaymentperiods, dbo.hx_borrowing_target.reviewremarks, dbo.hx_borrowing_target.recheckremarks, 
--                      dbo.hx_borrowing_target.guarantee_fee, dbo.hx_borrowing_target.consultingAMT, dbo.hx_borrowing_target.guaranteeAMT, dbo.hx_borrowing_target.B_Rates, 
--                      dbo.hx_borrowing_target.H_Repayment_Amt, dbo.hx_borrowing_target.Repay_Time, dbo.hx_repayment_plan.repayment_plan_id, 
--                      dbo.hx_repayment_plan.current_period, dbo.hx_repayment_plan.repayment_period, dbo.hx_repayment_plan.repayment_type, 
--                      dbo.hx_repayment_plan.repayment_amount, dbo.hx_repayment_plan.actual_amount_repayment, dbo.hx_repayment_plan.repayment_state, 
--                      dbo.hx_repayment_plan.createtime, dbo.hx_repayment_plan.interestpayment, dbo.hx_repayment_plan.fees, dbo.hx_repayment_plan.O_penalty, 
--                      dbo.hx_member_table.username, dbo.hx_member_table.realname, dbo.hx_repayment_plan.shall_repayment, dbo.hx_member_table.available_balance
--FROM         dbo.hx_borrowing_target INNER JOIN
--                      dbo.hx_repayment_plan ON dbo.hx_borrowing_target.targetid = dbo.hx_repayment_plan.targetid INNER JOIN
--                      dbo.hx_member_table ON dbo.hx_borrowing_target.borrower_registerid = dbo.hx_member_table.registerid


-- GO
---------View Name : tempViewUser  无变化
--if object_id(N'tempViewUser') is not null drop view tempViewUser

--GO
--create view tempViewUser
--as
--select registerid,useridentity as '等级', account_total_assets as '资产' ,available_balance as '可用' ,frozen_sum as '冻结'  ,isnull(investment_amount,0) as '在投资产',
-- registration_time as '注册时间' 
--From hx_member_table 
--LEFT JOIN 
--     (SELECT COALESCE(sum(investment_amount),0) AS investment_amount,investor_registerid 
--             FROM V_hx_Bid_records_borrowing_target 
--             WHERE tender_state between 2 and 4 GROUP BY investor_registerid) v ON investor_registerid=registerid
--GO
---------View Name : V_borrowing_Bid_records_income_statement--- 增加三列  dbo.hx_Bid_records.contractpath, dbo.hx_income_statement.orderid, dbo.hx_Bid_records.current_period
if object_id(N'V_borrowing_Bid_records_income_statement') is not null drop view V_borrowing_Bid_records_income_statement
  GO 
CREATE VIEW dbo.V_borrowing_Bid_records_income_statement
AS
SELECT   dbo.hx_borrowing_target.targetid, dbo.hx_borrowing_target.loan_number, dbo.hx_borrowing_target.borrowing_title, 
                dbo.hx_borrowing_target.borrowing_balance, dbo.hx_Bid_records.investment_amount, 
                dbo.hx_income_statement.income_statement_id, dbo.hx_income_statement.borrower_registerid, 
                dbo.hx_income_statement.annual_revenue, dbo.hx_income_statement.invest_time, 
                dbo.hx_income_statement.current_investment_period, dbo.hx_income_statement.value_date, 
                dbo.hx_income_statement.interest_payment_date, dbo.hx_income_statement.repayment_amount, 
                dbo.hx_income_statement.repayment_period, dbo.hx_income_statement.investor_registerid, 
                dbo.hx_income_statement.payment_status, dbo.hx_income_statement.bid_records_id, 
                dbo.hx_borrowing_target.annual_interest_rate, dbo.hx_member_table.registerid, 
                dbo.hx_member_table.available_balance, dbo.hx_borrowing_target.payment_options, 
                hx_member_table_1.UsrCustId AS BorrUsrCustId, dbo.hx_member_table.UsrCustId AS OutCustId, 
                dbo.hx_td_Loan_records.bid_orderid, dbo.hx_td_Loan_records.SubOrdid, dbo.hx_td_Loan_records.SubOrdDate, 
                dbo.hx_borrowing_target.loan_management_fee, dbo.hx_member_table.username, dbo.hx_member_table.mobile, 
                dbo.hx_member_table.email, dbo.hx_income_statement.Principal, dbo.hx_income_statement.interestpayment, 
                dbo.hx_Bid_records.contractpath, dbo.hx_income_statement.orderid, dbo.hx_Bid_records.current_period
FROM      dbo.hx_borrowing_target 
INNER JOIN dbo.hx_Bid_records ON dbo.hx_borrowing_target.targetid = dbo.hx_Bid_records.targetid
INNER JOIN dbo.hx_income_statement ON 
                dbo.hx_Bid_records.bid_records_id = dbo.hx_income_statement.bid_records_id 
INNER JOIN dbo.hx_member_table ON dbo.hx_Bid_records.investor_registerid = dbo.hx_member_table.registerid 
INNER JOIN dbo.hx_member_table AS hx_member_table_1 ON 
                dbo.hx_borrowing_target.borrower_registerid = hx_member_table_1.registerid 
INNER JOIN dbo.hx_td_Loan_records ON dbo.hx_Bid_records.OrdId = dbo.hx_td_Loan_records.bid_orderid
GO
---------View Name : V_borrowing_Bid_records_income_statement_uc---- 增加三列 dbo.hx_member_table.mobile, dbo.hx_income_statement.orderid,dbo.hx_Bid_records.JiaxiNum
if object_id(N'V_borrowing_Bid_records_income_statement_uc') is not null drop view V_borrowing_Bid_records_income_statement_uc
  GO 
CREATE VIEW dbo.V_borrowing_Bid_records_income_statement_uc
AS
SELECT   dbo.hx_borrowing_target.targetid, dbo.hx_borrowing_target.loan_number, dbo.hx_borrowing_target.borrowing_title, 
                dbo.hx_borrowing_target.borrowing_balance, dbo.hx_Bid_records.investment_amount, 
                dbo.hx_income_statement.income_statement_id, dbo.hx_income_statement.borrower_registerid, 
                dbo.hx_income_statement.annual_revenue, dbo.hx_income_statement.invest_time, 
                dbo.hx_income_statement.current_investment_period, dbo.hx_income_statement.value_date, 
                dbo.hx_income_statement.interest_payment_date, dbo.hx_income_statement.repayment_amount, 
                dbo.hx_income_statement.repayment_period, dbo.hx_income_statement.investor_registerid, 
                dbo.hx_income_statement.payment_status, dbo.hx_income_statement.bid_records_id, 
                dbo.hx_borrowing_target.annual_interest_rate, dbo.hx_member_table.registerid, 
                dbo.hx_member_table.available_balance, dbo.hx_borrowing_target.payment_options, 
                dbo.hx_member_table.UsrCustId AS OutCustId, dbo.hx_borrowing_target.loan_management_fee, 
                dbo.hx_income_statement.Principal, dbo.hx_income_statement.interestDay, 
                dbo.hx_income_statement.TotalInstallments, dbo.hx_Bid_records.ordstate, dbo.hx_member_table.username, 
                dbo.hx_member_table.realname, dbo.hx_member_table.mobile, dbo.hx_income_statement.orderid, 
                dbo.hx_Bid_records.JiaxiNum
FROM      dbo.hx_borrowing_target 
INNER JOIN dbo.hx_Bid_records ON dbo.hx_borrowing_target.targetid = dbo.hx_Bid_records.targetid
INNER JOIN dbo.hx_income_statement ON 
                dbo.hx_Bid_records.bid_records_id = dbo.hx_income_statement.bid_records_id
INNER JOIN dbo.hx_member_table ON dbo.hx_Bid_records.investor_registerid = dbo.hx_member_table.registerid
WHERE dbo.hx_Bid_records.ordstate = 1
GO
---------View Name : V_borrowing_target_addlist---增加列  dbo.hx_borrowing_target.IsIRC, dbo.hx_borrowing_target.Purpose,dbo.hx_borrowing_target.PaySource,
-- dbo.hx_borrowing_target.Collateral, dbo.hx_Project_type.project_type_name,dbo.hx_borrowing_target.Isinterest
--增加关联表及where条件
--INNER JOIN dbo.hx_Project_type ON dbo.hx_borrowing_target.project_type_id = dbo.hx_Project_type.project_type_id WHERE   (dbo.hx_borrowing_target.isDel = 0)

if object_id(N'V_borrowing_target_addlist') is not null drop view V_borrowing_target_addlist
  GO 
CREATE VIEW [dbo].[V_borrowing_target_addlist]
AS
SELECT   dbo.hx_borrowing_target.targetid, dbo.hx_borrowing_target.borrower_registerid, dbo.hx_borrowing_target.loan_number, 
                dbo.hx_borrowing_target.borrowing_title, dbo.hx_borrowing_target.borrowing_thumbnail, 
                dbo.hx_borrowing_target.project_type_id, dbo.hx_borrowing_target.annual_interest_rate, 
                dbo.hx_borrowing_target.borrowing_balance, dbo.hx_borrowing_target.life_of_loan, dbo.hx_borrowing_target.unit_day, 
                dbo.hx_borrowing_target.release_date, dbo.hx_borrowing_target.value_date, 
                dbo.hx_borrowing_target.month_payment_date, dbo.hx_borrowing_target.repayment_date, 
                dbo.hx_borrowing_target.minimum, dbo.hx_borrowing_target.maxmum, dbo.hx_borrowing_target.companyid, 
                dbo.hx_borrowing_target.guarantee_way_id, dbo.hx_borrowing_target.payment_options, 
                dbo.hx_borrowing_target.end_time, dbo.hx_borrowing_target.service_charge, 
                dbo.hx_borrowing_target.loan_management_fee, dbo.hx_borrowing_target.investors_management_fee, 
                dbo.hx_borrowing_target.ordinary_overdue_management_fees, 
                dbo.hx_borrowing_target.seriously_overdue_management_fees, dbo.hx_borrowing_target.ordinary_overdue_penalty, 
                dbo.hx_borrowing_target.seriously_overdue_penalty, dbo.hx_borrowing_target.transfer_Expenses, 
                dbo.hx_borrowing_target.fundraising_amount, dbo.hx_borrowing_target.tender_state, 
                dbo.hx_borrowing_target.full_scale_loan, dbo.hx_borrowing_target.flow_return, dbo.hx_borrowing_target.recommend, 
                dbo.hx_borrowing_target.sys_time, dbo.hx_borrowing_target.repaymentperiods, 
                dbo.hx_bonding_company.company_name, dbo.guarantee_way.guarantee_way_name, 
                dbo.hx_member_table.username, dbo.hx_member_table.realname, 
                dbo.hx_borrowing_target_detailed.target_detailed_id, dbo.hx_borrowing_target.reviewremarks, 
                dbo.hx_borrowing_target.recheckremarks, dbo.hx_borrowing_target.guarantee_fee, 
                dbo.hx_borrowing_target.consultingAMT, dbo.hx_borrowing_target.guaranteeAMT, dbo.hx_borrowing_target.B_Rates, 
                dbo.hx_borrowing_target.H_Repayment_Amt, dbo.hx_borrowing_target.Repay_Time, 
                dbo.hx_borrowing_target.start_time, dbo.hx_borrowing_target.G_contract_Path, 
                dbo.hx_member_table.UsrCustId AS BorrUsrCustId, dbo.hx_bonding_company.GuarType, 
                dbo.hx_bonding_company.UsrCustId AS GuarCompUsrCustId, 
                dbo.hx_borrowing_target_detailed.borrower_circumstances, dbo.hx_borrowing_target_detailed.item_details, 
                dbo.hx_borrowing_target_detailed.borrower_base_material, dbo.hx_borrowing_target_detailed.use_funds, 
                dbo.hx_borrowing_target_detailed.independent_advice, dbo.hx_borrowing_target_detailed.guarantee_agency_views, 
                dbo.hx_borrowing_target_detailed.risk_control_measures, dbo.hx_borrowing_target.IsUse, 
                dbo.hx_borrowing_target.indexorder, dbo.hx_borrowing_target.IsIRC, dbo.hx_borrowing_target.Purpose, 
                dbo.hx_borrowing_target.PaySource, dbo.hx_borrowing_target.Collateral, dbo.hx_Project_type.project_type_name, 
                dbo.hx_borrowing_target.Isinterest
FROM      dbo.hx_borrowing_target 
INNER JOIN dbo.hx_bonding_company ON dbo.hx_borrowing_target.companyid = dbo.hx_bonding_company.companyid 
INNER JOIN dbo.guarantee_way ON 
                dbo.hx_borrowing_target.guarantee_way_id = dbo.guarantee_way.guarantee_way_id INNER JOIN dbo.hx_member_table ON dbo.hx_borrowing_target.borrower_registerid = dbo.hx_member_table.registerid 
INNER JOIN dbo.hx_borrowing_target_detailed ON 
                dbo.hx_borrowing_target.targetid = dbo.hx_borrowing_target_detailed.targetid INNER JOIN dbo.hx_Project_type ON dbo.hx_borrowing_target.project_type_id = dbo.hx_Project_type.project_type_id
WHERE dbo.hx_borrowing_target.isDel = 0
GO
---------View Name : V_borrowing_target_bonding ---增加列 dbo.hx_bonding_company.registered_capital, dbo.hx_bonding_company.Date_incorporation, dbo.hx_bonding_company.company_address, 
--- dbo.hx_bonding_company.company_profile, dbo.hx_bonding_company.business_licence, dbo.hx_bonding_company.business_certificate, dbo.hx_bonding_company.agent, dbo.hx_bonding_company.Tax_NO, 
--- dbo.hx_bonding_company.UsrCustId, dbo.hx_bonding_company.CardId, dbo.hx_bonding_company.UsrId
-- 增加where WHERE   (dbo.hx_borrowing_target.isDel = 0)
if object_id(N'V_borrowing_target_bonding') is not null drop view V_borrowing_target_bonding
  GO 
CREATE VIEW [dbo].[V_borrowing_target_bonding]
AS
SELECT   dbo.hx_borrowing_target.targetid, dbo.hx_borrowing_target.loan_number, dbo.hx_borrowing_target.borrower_registerid, 
                dbo.hx_borrowing_target.borrowing_title, dbo.hx_borrowing_target.annual_interest_rate, 
                dbo.hx_borrowing_target.borrowing_balance, dbo.hx_borrowing_target.life_of_loan, dbo.hx_borrowing_target.unit_day, 
                dbo.hx_borrowing_target.release_date, dbo.hx_borrowing_target.month_payment_date, 
                dbo.hx_borrowing_target.repayment_date, dbo.hx_borrowing_target.end_time, dbo.hx_bonding_company.companyid, 
                dbo.hx_bonding_company.company_name, dbo.hx_bonding_company.agent_name, 
                dbo.hx_bonding_company.agent_id_card, dbo.hx_member_table.username, dbo.hx_member_table.mobile, 
                dbo.hx_member_table.email, dbo.hx_member_table.realname, dbo.hx_member_table.iD_number, 
                dbo.hx_borrowing_target.payment_options, dbo.hx_bonding_company.legal_representative, 
                dbo.hx_member_table.usertypes, dbo.hx_member_table.CopName, dbo.hx_bonding_company.registered_capital, 
                dbo.hx_bonding_company.Date_incorporation, dbo.hx_bonding_company.company_address, 
                dbo.hx_bonding_company.company_profile, dbo.hx_bonding_company.business_licence, 
                dbo.hx_bonding_company.business_certificate, dbo.hx_bonding_company.agent, dbo.hx_bonding_company.Tax_NO, 
                dbo.hx_bonding_company.UsrCustId, dbo.hx_bonding_company.CardId, dbo.hx_bonding_company.UsrId
FROM dbo.hx_borrowing_target 
INNER JOIN dbo.hx_bonding_company ON dbo.hx_borrowing_target.companyid = dbo.hx_bonding_company.companyid 
INNER JOIN dbo.hx_member_table ON dbo.hx_borrowing_target.borrower_registerid = dbo.hx_member_table.registerid
WHERE dbo.hx_borrowing_target.isDel = 0
GO
---------View Name : V_borrowing_target_review --- 增加列 dbo.hx_borrowing_target.isDel 增加 WHERE   (dbo.hx_borrowing_target.isDel = 0)
if object_id(N'V_borrowing_target_review') is not null drop view V_borrowing_target_review
  GO 
CREATE VIEW [dbo].[V_borrowing_target_review]
AS
SELECT   dbo.hx_borrowing_target.targetid, dbo.hx_borrowing_target.borrower_registerid, dbo.hx_borrowing_target.loan_number, 
                dbo.hx_borrowing_target.borrowing_title, dbo.hx_borrowing_target.borrowing_thumbnail, 
                dbo.hx_borrowing_target.project_type_id, dbo.hx_borrowing_target.annual_interest_rate, 
                dbo.hx_borrowing_target.borrowing_balance, dbo.hx_borrowing_target.life_of_loan, dbo.hx_borrowing_target.unit_day, 
                dbo.hx_borrowing_target.release_date, dbo.hx_borrowing_target.value_date, 
                dbo.hx_borrowing_target.month_payment_date, dbo.hx_borrowing_target.repayment_date, 
                dbo.hx_borrowing_target.minimum, dbo.hx_borrowing_target.maxmum, dbo.hx_borrowing_target.companyid, 
                dbo.hx_borrowing_target.guarantee_way_id, dbo.hx_borrowing_target.payment_options, 
                dbo.hx_borrowing_target.end_time, dbo.hx_borrowing_target.service_charge, 
                dbo.hx_borrowing_target.loan_management_fee, dbo.hx_borrowing_target.investors_management_fee, 
                dbo.hx_borrowing_target.ordinary_overdue_management_fees, 
                dbo.hx_borrowing_target.seriously_overdue_management_fees, dbo.hx_borrowing_target.ordinary_overdue_penalty, 
                dbo.hx_borrowing_target.seriously_overdue_penalty, dbo.hx_borrowing_target.transfer_Expenses, 
                dbo.hx_borrowing_target.fundraising_amount, dbo.hx_borrowing_target.full_scale_loan, 
                dbo.hx_borrowing_target.flow_return, dbo.hx_borrowing_target.recommend, dbo.hx_borrowing_target.sys_time, 
                dbo.hx_borrowing_target.repaymentperiods, dbo.hx_td_reviewremarks.reviewid, 
                dbo.hx_td_reviewremarks.reviewremarks, dbo.hx_td_reviewremarks.reviewtime, 
                dbo.hx_td_reviewremarks.admin_operator, dbo.hx_td_adminuser.adminuserid, dbo.hx_td_adminuser.trueName, 
                dbo.hx_td_reviewremarks.tender_state, dbo.hx_borrowing_target.isDel
FROM      dbo.hx_borrowing_target 
INNER JOIN dbo.hx_td_reviewremarks ON dbo.hx_borrowing_target.targetid = dbo.hx_td_reviewremarks.targetid 
INNER JOIN dbo.hx_td_adminuser ON dbo.hx_td_reviewremarks.admin_operator = dbo.hx_td_adminuser.adminuserid
WHERE dbo.hx_borrowing_target.isDel = 0
GO
---------View Name : V_contract_type_template--- 无变化
--if object_id(N'V_contract_type_template') is not null drop view V_contract_type_template

--  GO 

--CREATE VIEW [dbo].[V_contract_type_template]
--AS
--SELECT dbo.hx_Contract_template.contract_template_id, dbo.hx_Contract_template.contract_template_name, dbo.hx_Contract_template.contract_template_context, 
--dbo.hx_Contract_template.usestate, dbo.hx_Contract_template.cretatetime, dbo.hx_contract_type.contract_type_name,
--dbo.hx_contract_type.contract_type_id
--FROM dbo.hx_Contract_template 
--INNER JOIN dbo.hx_contract_type ON dbo.hx_Contract_template.contract_type_id = dbo.hx_contract_type.contract_type_id
--GO
---------View Name : V_DepUserLimitInfo--- 新增，部门用户权限表
if object_id(N'V_DepUserLimitInfo') is not null drop view V_DepUserLimitInfo
  GO 
CREATE VIEW dbo.V_DepUserLimitInfo
AS
SELECT   dbo.hx_AdminLimitInfo.id, dbo.hx_AdminLimitInfo.ParentId, dbo.hx_AdminLimitInfo.ControllerName, 
                dbo.hx_AdminLimitInfo.ActionName, dbo.hx_AdminLimitInfo.[level], dbo.hx_AdminLimitInfo.SortId, 
                dbo.hx_AdminLimitInfo.isDel, dbo.hx_AdminLimitInfo.lastOper, dbo.hx_AdminLimitInfo.lastTime, 
                dbo.hx_AdminLimitInfo.CreatTime, dbo.hx_DepUserLimit.limitType, dbo.hx_DepUserLimit.departmentId, 
                dbo.hx_DepUserLimit.adminUserId, dbo.hx_DepUserLimit.id AS relationId, dbo.hx_AdminLimitInfo.title
FROM      dbo.hx_DepUserLimit INNER JOIN
                dbo.hx_AdminLimitInfo ON dbo.hx_DepUserLimit.limitId = dbo.hx_AdminLimitInfo.id
 GO
---------View Name : V_hx_Bid_records_borrowing_target--- 增加列   ISNULL(dbo.hx_Bid_records.JiaxiNum,0.00) AS JiaxiNum, ISNULL(dbo.hx_Bid_records.BonusAmt,0.00) AS BonusAmt, dbo.hx_borrowing_target.release_date, dbo.hx_borrowing_target.repayment_date, 
----- dbo.hx_member_table.registration_time, dbo.hx_member_table.Channelsource, dbo.hx_member_table.Tid, dbo.hx_borrowing_target.service_charge
if object_id(N'V_hx_Bid_records_borrowing_target') is not null drop view V_hx_Bid_records_borrowing_target
  GO 
CREATE VIEW dbo.V_hx_Bid_records_borrowing_target
AS
SELECT DISTINCT 
dbo.hx_borrowing_target.borrowing_title, dbo.hx_Bid_records.bid_records_id, dbo.hx_Bid_records.borrower_registerid,dbo.hx_Bid_records.targetid,dbo.hx_Bid_records.loan_number,dbo.hx_Bid_records.annual_interest_rate,dbo.hx_Bid_records.current_period,dbo.hx_Bid_records.investment_amount,dbo.hx_Bid_records.value_date,dbo.hx_Bid_records.investment_maturity,dbo.hx_Bid_records.invest_time,dbo.hx_Bid_records.invest_state,dbo.hx_Bid_records.flow_return,dbo.hx_Bid_records.repayment_amount,dbo.hx_Bid_records.repayment_period,dbo.hx_Bid_records.investor_registerid,dbo.hx_Bid_records.payment_status,dbo.hx_Bid_records.withoutinterest,dbo.hx_Bid_records.haveinterest,dbo.hx_member_table.username,dbo.hx_member_table.realname,dbo.hx_member_table.registerid, dbo.hx_borrowing_target.payment_options,dbo.hx_Bid_records.contractid,dbo.hx_Bid_records.contractpath,dbo.hx_borrowing_target.life_of_loan,dbo.hx_borrowing_target.unit_day,dbo.hx_member_table.mobile,dbo.hx_member_table.UsrCustId,dbo.hx_td_frozen.FrozenidAmount,
                dbo.hx_td_frozen.FrozenidNo,dbo.hx_td_frozen.FrozenState,dbo.hx_td_frozen.FreezeTrxId,
                hx_member_table_1.UsrCustId AS borrUsrCustid,dbo.hx_member_table.available_balance,
                dbo.hx_borrowing_target.tender_state, dbo.hx_Bid_records.invitationcode, 
                hx_member_table_1.realname AS borrealname, hx_member_table_1.username AS borusername,
                dbo.hx_Bid_records.OrdId, dbo.hx_borrowing_target.borrowing_balance, dbo.hx_Bid_records.ordstate,
                dbo.hx_Bid_records.IsLoans, dbo.hx_borrowing_target.project_type_id, ISNULL(dbo.hx_Bid_records.JiaxiNum,0.00) AS JiaxiNum, 

              ISNULL(dbo.hx_Bid_records.BonusAmt,0.00) AS BonusAmt, dbo.hx_borrowing_target.release_date, dbo.hx_borrowing_target.repayment_date, 
                dbo.hx_member_table.registration_time, dbo.hx_member_table.Channelsource, dbo.hx_member_table.Tid,dbo.hx_borrowing_target.service_charge
FROM      dbo.hx_borrowing_target 
INNER JOIN dbo.hx_Bid_records ON dbo.hx_borrowing_target.targetid = dbo.hx_Bid_records.targetid INNER JOIN dbo.hx_member_table ON dbo.hx_Bid_records.investor_registerid = dbo.hx_member_table.registerid 
INNER JOIN dbo.hx_member_table AS hx_member_table_1 ON dbo.hx_Bid_records.borrower_registerid = hx_member_table_1.registerid 
INNER JOIN dbo.hx_td_frozen ON dbo.hx_Bid_records.investor_registerid = dbo.hx_td_frozen.MBT_Registerid AND dbo.hx_Bid_records.bid_records_id = dbo.hx_td_frozen.bid_records_id
WHERE dbo.hx_Bid_records.ordstate = 1
GO
---------View Name : V_hx_Bid_records_borrowing_target_uc--- 无变化
--if object_id(N'V_hx_Bid_records_borrowing_target_uc') is not null drop view V_hx_Bid_records_borrowing_target_uc

--  GO 

--CREATE VIEW [dbo].[V_hx_Bid_records_borrowing_target_uc]
--AS
--SELECT DISTINCT 
--                      dbo.hx_borrowing_target.borrowing_title, dbo.hx_Bid_records.bid_records_id, dbo.hx_Bid_records.borrower_registerid, dbo.hx_Bid_records.targetid, 
--                      dbo.hx_Bid_records.loan_number, dbo.hx_Bid_records.annual_interest_rate, dbo.hx_Bid_records.current_period, dbo.hx_Bid_records.investment_amount, 
--                      dbo.hx_Bid_records.value_date, dbo.hx_Bid_records.investment_maturity, dbo.hx_Bid_records.invest_time, dbo.hx_Bid_records.invest_state, 
--                      dbo.hx_Bid_records.flow_return, dbo.hx_Bid_records.repayment_amount, dbo.hx_Bid_records.repayment_period, dbo.hx_Bid_records.investor_registerid, 
--                      dbo.hx_Bid_records.payment_status, dbo.hx_Bid_records.withoutinterest, dbo.hx_Bid_records.haveinterest, dbo.hx_member_table.username, 
--                      dbo.hx_member_table.realname, dbo.hx_member_table.registerid, dbo.hx_borrowing_target.payment_options, dbo.hx_Bid_records.contractid, 
--                      dbo.hx_Bid_records.contractpath, dbo.hx_borrowing_target.life_of_loan, dbo.hx_borrowing_target.unit_day, dbo.hx_member_table.mobile, 
--                      dbo.hx_member_table.UsrCustId, dbo.hx_td_frozen.FrozenidAmount, dbo.hx_td_frozen.FrozenidNo, dbo.hx_td_frozen.FrozenState, dbo.hx_td_frozen.FreezeTrxId, 
--                      hx_member_table_1.UsrCustId AS borrUsrCustid, dbo.hx_member_table.available_balance, dbo.hx_borrowing_target.tender_state, 
--                      dbo.hx_Bid_records.invitationcode, hx_member_table_1.realname AS borrealname, hx_member_table_1.username AS borusername, dbo.hx_Bid_records.OrdId, 
--                      dbo.hx_borrowing_target.borrowing_balance, dbo.hx_Bid_records.ordstate, dbo.hx_member_table.frozen_sum
--FROM         dbo.hx_borrowing_target INNER JOIN
--                      dbo.hx_Bid_records ON dbo.hx_borrowing_target.targetid = dbo.hx_Bid_records.targetid INNER JOIN
--                      dbo.hx_member_table ON dbo.hx_Bid_records.investor_registerid = dbo.hx_member_table.registerid INNER JOIN
--                      dbo.hx_member_table AS hx_member_table_1 ON dbo.hx_Bid_records.borrower_registerid = hx_member_table_1.registerid INNER JOIN
--                      dbo.hx_td_frozen ON dbo.hx_Bid_records.investor_registerid = dbo.hx_td_frozen.MBT_Registerid AND 
--                      dbo.hx_Bid_records.bid_records_id = dbo.hx_td_frozen.bid_records_id
--WHERE     (dbo.hx_Bid_records.ordstate = 0)


-- GO
---------View Name : V_incomeborr_count ---增加一列  dbo.hx_borrowing_target.service_charge
if object_id(N'V_incomeborr_count') is not null drop view V_incomeborr_count
  GO 
CREATE VIEW dbo.V_incomeborr_count
AS
SELECT   dbo.hx_income_statement.income_statement_id, dbo.hx_income_statement.targetid, 
                dbo.hx_income_statement.bid_records_id, dbo.hx_income_statement.loan_number, 
                dbo.hx_income_statement.borrower_registerid, dbo.hx_income_statement.OutCustId, 
                dbo.hx_income_statement.annual_revenue, dbo.hx_income_statement.investment_amount, 
                dbo.hx_income_statement.invest_time, dbo.hx_income_statement.current_investment_period, 
                dbo.hx_income_statement.value_date, dbo.hx_income_statement.interest_payment_date, 
                dbo.hx_income_statement.repayment_amount, dbo.hx_income_statement.repayment_period, 
                dbo.hx_income_statement.investor_registerid, dbo.hx_income_statement.InCustId, 
                dbo.hx_income_statement.payment_status, dbo.hx_income_statement.Principal, 
                dbo.hx_income_statement.interestpayment, dbo.hx_income_statement.orderid, dbo.hx_income_statement.BidOrderid, 
                dbo.hx_income_statement.interestDay, dbo.hx_income_statement.TotalInstallments, 
                dbo.hx_income_statement.BorrFees, dbo.hx_income_statement.InveFess, 
                dbo.hx_borrowing_target.loan_management_fee, dbo.hx_borrowing_target.service_charge
FROM      dbo.hx_income_statement INNER JOIN
                dbo.hx_borrowing_target ON dbo.hx_income_statement.targetid = dbo.hx_borrowing_target.targetid
WHERE   (dbo.hx_income_statement.payment_status = 0)
 GO
---------View Name : V_invitation_record--- 新增，邀请记录
if object_id(N'V_invitation_record') is not null drop view V_invitation_record
  GO 
CREATE VIEW dbo.V_invitation_record
AS
SELECT DISTINCT 
                dbo.hx_td_Userinvitation.invitationid, dbo.hx_td_Userinvitation.invcode, dbo.hx_td_Userinvitation.invtime, 
                dbo.hx_td_Userinvitation.invpersonid, dbo.hx_td_Userinvitation.Invpeopleid, dbo.bonus_account_water.income, 
                dbo.bonus_account_water.award_description, dbo.bonus_account_water.water_type, dbo.hx_member_table.registerid, 
                dbo.hx_UserAct.ActTypeId, hx_member_table_1.username
FROM      dbo.hx_td_Userinvitation INNER JOIN
                dbo.bonus_account_water ON 
                dbo.hx_td_Userinvitation.Invpeopleid = dbo.bonus_account_water.membertable_registerid INNER JOIN
                dbo.hx_UserAct ON dbo.hx_UserAct.UserAct = dbo.bonus_account_water.bonus_account_id INNER JOIN
                dbo.hx_member_table ON dbo.hx_td_Userinvitation.Invpeopleid = dbo.hx_member_table.registerid INNER JOIN
                dbo.hx_member_table AS hx_member_table_1 ON 
                dbo.hx_td_Userinvitation.invpersonid = hx_member_table_1.registerid
WHERE   (dbo.hx_UserAct.ActTypeId = 4)
 GO
---------View Name : V_LL_Cash_User --- 新库缺少on 条件 and hx_td_LL_cash.ordertime=hx_td_LLpay_re_cash.OrdTime，保留旧库
--??? onchuangtou 被变更过？？？
--if object_id(N'V_LL_Cash_User') is not null drop view V_LL_Cash_User

--  GO 

--CREATE VIEW [dbo].[V_LL_Cash_User]

--AS
--SELECT     dbo.hx_td_LL_cash.LLcashid, dbo.hx_td_LL_cash.no_order, dbo.hx_td_LL_cash.dt_order, dbo.hx_td_LL_cash.money_order, dbo.hx_td_LL_cash.acct_name, 

--                      dbo.hx_td_LL_cash.province_code, dbo.hx_td_LL_cash.city_code, dbo.hx_td_LL_cash.brabank_name, dbo.hx_td_LL_cash.ordertime, dbo.hx_td_LL_cash.card_no, 

--                      dbo.hx_td_LL_cash.paystate, dbo.hx_td_LL_cash.Usrid, dbo.hx_member_table.registerid, dbo.hx_member_table.username, dbo.hx_member_table.mobile, 

--                      dbo.hx_td_LLBack.BankName, dbo.hx_td_LL_cash.bank_code, dbo.hx_td_LL_cash.OperTime, dbo.hx_td_LLpay_re_cash.h_state, 

--                      dbo.hx_td_LLpay_re_cash.htype

--FROM         dbo.hx_td_LL_cash INNER JOIN

--                      dbo.hx_member_table ON dbo.hx_td_LL_cash.Usrid = dbo.hx_member_table.registerid INNER JOIN

--                      dbo.hx_td_LLBack ON dbo.hx_td_LL_cash.bank_code = dbo.hx_td_LLBack.BankCode INNER JOIN

--                      dbo.hx_td_LLpay_re_cash ON dbo.hx_td_LL_cash.no_order = dbo.hx_td_LLpay_re_cash.no_order and hx_td_LL_cash.ordertime=hx_td_LLpay_re_cash.OrdTime

--WHERE     (dbo.hx_td_LLpay_re_cash.htype = 1) 

-- GO
---------View Name : V_LLBack_Card_List--- 无变化
--if object_id(N'V_LLBack_Card_List') is not null drop view V_LLBack_Card_List

--  GO 

--CREATE VIEW [dbo].[V_LLBack_Card_List] AS 
--SELECT     dbo.hx_td_LLPay_bindCard.BankCode, dbo.hx_td_LLPay_bindCard.bindCardid, dbo.hx_td_LLPay_bindCard.Usrid, dbo.hx_td_LLPay_bindCard.BankCard, 
--                      dbo.hx_td_LLPay_bindCard.Bindtime, dbo.hx_td_LLPay_bindCard.Re_Amt, dbo.hx_td_LLPay_bindCard.Quota, dbo.hx_td_LLPay_bindCard.Citycode, 
--                      dbo.hx_td_LLPay_bindCard.branch, dbo.hx_td_LLPay_bindCard.no_agree, dbo.hx_td_LLPay_bindCard.cardstate, dbo.hx_td_LLBack.BankName, 
--                      dbo.hx_td_LLBack.CardImage, dbo.hx_member_table.registerid, dbo.hx_member_table.realname, dbo.hx_td_LLPay_bindCard.province_code
--FROM         dbo.hx_td_LLBack INNER JOIN
--                      dbo.hx_td_LLPay_bindCard ON dbo.hx_td_LLBack.BankCode = dbo.hx_td_LLPay_bindCard.BankCode INNER JOIN
--                      dbo.hx_member_table ON dbo.hx_td_LLPay_bindCard.Usrid = dbo.hx_member_table.registerid

-- GO
---------View Name : V_LLPay_Re--- 无变化
--if object_id(N'V_LLPay_Re') is not null drop view V_LLPay_Re

--  GO 

--CREATE VIEW [dbo].[V_LLPay_Re]
--AS
--SELECT     dbo.hx_td_LLpay_Recharge.Rechargeid, dbo.hx_td_LLpay_Recharge.UsrId, dbo.hx_td_LLpay_Recharge.no_order, dbo.hx_td_LLpay_Recharge.ordertime, 
--                      dbo.hx_td_LLpay_Recharge.info_order, dbo.hx_td_LLpay_Recharge.money_order, dbo.hx_td_LLpay_Recharge.BankCode, dbo.hx_td_LLpay_Recharge.card_no, 
--                      dbo.hx_td_LLpay_Recharge.ReState, dbo.hx_td_LLpay_Recharge.oid_paybill, dbo.hx_td_LLpay_re_cash.re_cashid, dbo.hx_td_LLpay_re_cash.OrdId, 
--                      dbo.hx_td_LLpay_re_cash.TransAmt, dbo.hx_td_LLpay_re_cash.h_state, dbo.hx_td_LLpay_re_cash.htype, dbo.hx_member_table.registerid, 
--                      dbo.hx_td_LLpay_re_cash.OrdTime, dbo.hx_member_table.username, dbo.hx_member_table.realname
--FROM         dbo.hx_td_LLpay_Recharge INNER JOIN
--                      dbo.hx_td_LLpay_re_cash ON dbo.hx_td_LLpay_Recharge.no_order = dbo.hx_td_LLpay_re_cash.no_order INNER JOIN
--                      dbo.hx_member_table ON dbo.hx_td_LLpay_re_cash.Usrid = dbo.hx_member_table.registerid
--WHERE     (dbo.hx_td_LLpay_re_cash.htype = 0)



-- GO
---------View Name : V_menu_toptype-- 无变化
--if object_id(N'V_menu_toptype') is not null drop view V_menu_toptype

--  GO 


--CREATE VIEW [dbo].[V_menu_toptype]
--AS
--SELECT     hx_td_admin_role_1.adminuserid, dbo.hx_td_admin_role.menu_id, dbo.hx_td_menu.menu_name, dbo.hx_td_menu.parentid, 
--                      hx_td_menu_1.menu_name AS topmenu_name, hx_td_menu_1.parentid AS topparentid, hx_td_admin_role_1.isread, hx_td_admin_role_1.isadd, 
--                      hx_td_admin_role_1.isedit, hx_td_admin_role_1.isdelete
--FROM         dbo.hx_td_admin_role AS hx_td_admin_role_1 INNER JOIN
--                      dbo.hx_td_admin_role ON hx_td_admin_role_1.menu_id = dbo.hx_td_admin_role.menu_id INNER JOIN
--                      dbo.hx_td_menu ON hx_td_admin_role_1.menu_id = dbo.hx_td_menu.menu_id INNER JOIN
--                      dbo.hx_td_menu AS hx_td_menu_1 ON dbo.hx_td_menu.parentid = hx_td_menu_1.menu_id
--WHERE     (hx_td_admin_role_1.isread = 1)


-- GO
---------View Name : V_Phone_records -- 增加列 , dbo.hx_td_Phone_records.gtType, dbo.hx_td_Phone_records.problemType
if object_id(N'V_Phone_records') is not null drop view V_Phone_records
  GO 
CREATE VIEW [dbo].[V_Phone_records]
AS
SELECT   dbo.hx_td_Phone_records.recordsid, dbo.hx_td_Phone_records.recordcontext, dbo.hx_td_Phone_records.recordtime, 
                dbo.hx_td_Phone_records.registerid, dbo.hx_td_Phone_records.adminid, dbo.hx_member_table.username, 
                dbo.hx_member_table.realname, dbo.hx_td_adminuser.adminuser, dbo.hx_td_Phone_records.gtType, 
                dbo.hx_td_Phone_records.problemType
FROM      dbo.hx_td_Phone_records INNER JOIN
                dbo.hx_member_table ON dbo.hx_td_Phone_records.registerid = dbo.hx_member_table.registerid INNER JOIN
                dbo.hx_td_adminuser ON dbo.hx_td_Phone_records.adminid = dbo.hx_td_adminuser.adminuserid
 GO
---------View Name : V_Recharge_user_bank-- 增加列 dbo.hx_member_table.mobile
if object_id(N'V_Recharge_user_bank') is not null drop view V_Recharge_user_bank
  GO 
CREATE VIEW dbo.V_Recharge_user_bank
AS
SELECT   dbo.hx_Recharge_history.recharge_history_id, dbo.hx_Recharge_history.membertable_registerid, 
                dbo.hx_Recharge_history.recharge_amount, dbo.hx_Recharge_history.recharge_time, 
                dbo.hx_Recharge_history.account_amount, dbo.hx_Recharge_history.order_No, 
                dbo.hx_Recharge_history.recharge_condition, dbo.hx_Recharge_history.recharge_bank, 
                dbo.hx_member_table.username, dbo.hx_td_Bank.BankName, dbo.hx_member_table.realname, 
                dbo.hx_member_table.mobile
FROM      dbo.hx_Recharge_history INNER JOIN
                dbo.hx_member_table ON 
                dbo.hx_Recharge_history.membertable_registerid = dbo.hx_member_table.registerid INNER JOIN
                dbo.hx_td_Bank ON dbo.hx_Recharge_history.recharge_bank = dbo.hx_td_Bank.OpenBankId
 GO
---------View Name : V_Repayment_plan_loan_records -- 无变化
--if object_id(N'V_Repayment_plan_loan_records') is not null drop view V_Repayment_plan_loan_records

--  GO 

--CREATE VIEW [dbo].[V_Repayment_plan_loan_records]
--AS
--SELECT     dbo.hx_repayment_plan.repayment_plan_id, dbo.hx_repayment_plan.BorrUsrCustId, dbo.hx_repayment_plan.borrower_registerid, dbo.hx_repayment_plan.targetid, 
--                      dbo.hx_repayment_plan.current_period, dbo.hx_repayment_plan.repayment_period, dbo.hx_repayment_plan.repayment_type, 
--                      dbo.hx_repayment_plan.repayment_amount, dbo.hx_repayment_plan.actual_amount_repayment, dbo.hx_repayment_plan.repayment_state, 
--                      dbo.hx_repayment_plan.createtime, dbo.hx_repayment_plan.interestpayment, dbo.hx_repayment_plan.fees, dbo.hx_repayment_plan.O_penalty, 
--                      dbo.hx_repayment_plan.shall_repayment, dbo.hx_repayment_plan.pla_amt_generation, dbo.hx_repayment_plan.Interest_spreads, 
--                      dbo.hx_repayment_plan.Rep_Remarks, dbo.hx_td_Loan_records.targetid AS Expr1, dbo.hx_td_Loan_records.Loan_records_id, 
--                      dbo.hx_td_Loan_records.bid_records_id, dbo.hx_td_Loan_records.InCustId, dbo.hx_td_Loan_records.OutCustId, dbo.hx_td_Loan_records.LoanAMT, 
--                      dbo.hx_td_Loan_records.LoanOrdId, dbo.hx_td_Loan_records.LoanDate, dbo.hx_td_Loan_records.SubOrdid, dbo.hx_td_Loan_records.SubOrdDate, 
--                      dbo.hx_td_Loan_records.unFreezeOrdId, dbo.hx_td_Loan_records.FreezeTrxId, dbo.hx_Bid_records.investor_registerid, 
--                      dbo.hx_member_table.available_balance
--FROM         dbo.hx_repayment_plan INNER JOIN
--                      dbo.hx_td_Loan_records ON dbo.hx_repayment_plan.targetid = dbo.hx_td_Loan_records.targetid INNER JOIN
--                      dbo.hx_Bid_records ON dbo.hx_td_Loan_records.bid_records_id = dbo.hx_Bid_records.bid_records_id INNER JOIN
--                      dbo.hx_member_table ON dbo.hx_Bid_records.investor_registerid = dbo.hx_member_table.registerid


-- GO
---------View Name : V_SMS_Record-- 无变化
--if object_id(N'V_SMS_Record') is not null drop view V_SMS_Record

--  GO 

--CREATE VIEW [dbo].[V_SMS_Record]
--AS
--SELECT     dbo.hx_td_SMS_record.sms_record_id, dbo.hx_td_SMS_record.senduserid, dbo.hx_td_SMS_record.smscontext, dbo.hx_td_SMS_record.phone_number, 
--                      dbo.hx_td_SMS_record.smstype, dbo.hx_td_SMS_record.orderid, dbo.hx_td_SMS_record.sendtime, dbo.hx_td_SMS_record.vcode, dbo.hx_member_table.registerid, 
--                      dbo.hx_member_table.username, dbo.hx_member_table.realname
--FROM         dbo.hx_td_SMS_record INNER JOIN
--                      dbo.hx_member_table ON dbo.hx_td_SMS_record.senduserid = dbo.hx_member_table.registerid


-- GO
---------View Name : V_td_LLpay_bank-- 无变化
--if object_id(N'V_td_LLpay_bank') is not null drop view V_td_LLpay_bank

--  GO 

--CREATE VIEW [dbo].[V_td_LLpay_bank]
--AS
--SELECT     dbo.hx_td_LLpay_Recharge.Rechargeid, dbo.hx_td_LLpay_Recharge.UsrId, dbo.hx_td_LLpay_Recharge.no_order, dbo.hx_td_LLpay_Recharge.ordertime, 
--                      dbo.hx_td_LLpay_Recharge.info_order, dbo.hx_td_LLpay_Recharge.money_order, dbo.hx_td_LLpay_Recharge.BankCode, dbo.hx_td_LLpay_Recharge.card_no, 
--                      dbo.hx_td_LLpay_Recharge.ReState, dbo.hx_td_LLpay_Recharge.oid_paybill, dbo.hx_td_LLBack.BankCode AS Expr1, dbo.hx_td_LLBack.BankName
--FROM         dbo.hx_td_LLpay_Recharge INNER JOIN
--                      dbo.hx_td_LLBack ON dbo.hx_td_LLpay_Recharge.BankCode = dbo.hx_td_LLBack.BankCode


-- GO
---------View Name : V_type_news --增加列  dbo.hx_td_about_news.newimg  
if object_id(N'V_type_news') is not null drop view V_type_news
  GO 
CREATE VIEW V_type_news
AS
SELECT  dbo.hx_td_about_news.newid, dbo.hx_td_about_news.web_Type_menu_id, dbo.hx_td_about_news.News_title, 
	dbo.hx_td_about_news.News_Key, dbo.hx_td_about_news.news_Des, dbo.hx_td_about_news.context, 
	dbo.hx_td_about_news.createtime, dbo.hx_td_about_news.adminuserid, 
	dbo.hx_td_web_type.menu_name, dbo.hx_td_web_type.parentid, 
	dbo.hx_td_web_type.rootid, dbo.hx_td_web_type.path1, dbo.hx_td_web_type.orderid,
	hx_td_web_type_1.menu_name AS topmenuname, dbo.hx_td_about_news.comm,
	dbo.hx_td_about_news.listcomm, dbo.hx_td_about_news.newimg
FROM	hx_td_about_news 
	left join hx_td_web_type 
		ON hx_td_about_news.web_Type_menu_id=hx_td_web_type.menu_id 
	left join hx_td_web_type AS hx_td_web_type_1
		ON dbo.hx_td_web_type.rootid = hx_td_web_type_1.menu_id
GO
---------View Name : V_UserCash_Bank --- 增加列 , dbo.hx_member_table.mobile
if object_id(N'V_UserCash_Bank') is not null drop view V_UserCash_Bank
  GO 
CREATE VIEW dbo.V_UserCash_Bank
AS
SELECT   dbo.hx_td_UserCash.UserCashId, dbo.hx_td_UserCash.registerid, dbo.hx_td_UserCash.UsrCustId, 
                dbo.hx_td_UserCash.TransAmt, dbo.hx_td_UserCash.FeeAmt, dbo.hx_td_UserCash.OrdId, 
                dbo.hx_td_UserCash.OrdIdTime, dbo.hx_td_UserCash.OrdIdState, dbo.hx_td_UserCash.OperTime, 
                dbo.hx_td_UserCash.Reason, dbo.hx_td_UserCash.Remarks, dbo.hx_td_UserCash.TransState, 
                dbo.hx_td_UserCash.OpenAcctId, dbo.hx_td_UserCash.OpenBankId, dbo.hx_member_table.username, 
                dbo.hx_td_Bank.BankName, dbo.hx_member_table.realname, dbo.hx_member_table.available_balance, 
                dbo.hx_member_table.usertypes, dbo.hx_td_UserCash.FeeObjFlag, dbo.hx_member_table.mobile
FROM      dbo.hx_td_UserCash INNER JOIN
                dbo.hx_member_table ON dbo.hx_td_UserCash.registerid = dbo.hx_member_table.registerid INNER JOIN
                dbo.hx_td_Bank ON dbo.hx_td_UserCash.OpenBankId = dbo.hx_td_Bank.OpenBankId
 GO
---------View Name : V_UserInvitation--- 保留老库，新库缺少去重where条件  AND b.bid_records_id =(SELECT     MIN(bid_records_id) AS Expr1 FROM          dbo.hx_Bid_records  WHERE      (ordstate = 1) AND (investor_registerid = b.investor_registerid))

--if object_id(N'V_UserInvitation') is not null drop view V_UserInvitation

--  GO 

--CREATE VIEW dbo.V_UserInvitation
--AS
--SELECT     dbo.hx_td_Userinvitation.invpersonid, dbo.hx_member_table.username, dbo.hx_td_Userinvitation.invitationid, dbo.hx_td_Userinvitation.invcode, 
--                      dbo.hx_td_Userinvitation.invtime, dbo.hx_td_Userinvitation.Invpeopleid, dbo.hx_td_Userinvitation.InvitesStates, dbo.hx_td_Userinvitation.Invitereward, 
--                      dbo.hx_member_table.userstate, dbo.hx_member_table.registration_time, b.invest_time, b.investment_amount
--FROM         dbo.hx_td_Userinvitation INNER JOIN
--                      dbo.hx_member_table ON dbo.hx_td_Userinvitation.invpersonid = dbo.hx_member_table.registerid LEFT OUTER JOIN
--                      dbo.hx_Bid_records AS b ON b.investor_registerid = dbo.hx_member_table.registerid AND b.bid_records_id =
--                          (SELECT     MIN(bid_records_id) AS Expr1
--                            FROM          dbo.hx_Bid_records
--                            WHERE      (ordstate = 1) AND (investor_registerid = b.investor_registerid))


-- GO
---------View Name : V_Usr_Phone_adminUsr-- 新增  客服电话记录
if object_id(N'V_Usr_Phone_adminUsr') is not null drop view V_Usr_Phone_adminUsr
  GO 
CREATE VIEW [dbo].[V_Usr_Phone_adminUsr]
AS
SELECT   dbo.hx_member_table.registerid, dbo.hx_member_table.username, dbo.hx_member_table.mobile, 
                dbo.hx_member_table.realname, dbo.hx_member_table.account_total_assets, 
                dbo.hx_member_table.available_balance, dbo.hx_member_table.registration_time, dbo.hx_member_table.lastlogintime, 
                dbo.hx_member_table.useridentity, dbo.hx_member_table.CommNum, dbo.hx_member_table.RechargeNum, 
                dbo.hx_member_table.CashNum, dbo.hx_member_table.InvestNum, dbo.hx_member_table.LoginNum, 
                dbo.hx_member_table.calltime, dbo.hx_td_Phone_records.recordsid, dbo.hx_td_Phone_records.recordcontext, 
                dbo.hx_td_Phone_records.recordtime, dbo.hx_td_Phone_records.gtType, dbo.hx_td_Phone_records.problemType, 
                dbo.hx_td_adminuser.adminuser
FROM      dbo.hx_member_table INNER JOIN
                dbo.hx_td_Phone_records ON dbo.hx_member_table.registerid = dbo.hx_td_Phone_records.registerid INNER JOIN
                dbo.hx_td_adminuser ON dbo.hx_td_Phone_records.adminid = dbo.hx_td_adminuser.adminuserid
 GO
---------View Name : V_UsrBindCardBank--- 增加列  dbo.hx_td_Bank.CardImage, dbo.hx_td_Bank.Isquick, dbo.hx_td_Bank.Isordinary？
if object_id(N'V_UsrBindCardBank') is not null drop view V_UsrBindCardBank
  GO 
CREATE VIEW [dbo].[V_UsrBindCardBank]
AS
SELECT   dbo.hx_UsrBindCardC.UsrBindCardID, dbo.hx_UsrBindCardC.UsrCustId, dbo.hx_UsrBindCardC.OpenAcctId, 
                dbo.hx_UsrBindCardC.OpenBankId, dbo.hx_UsrBindCardC.defCard, dbo.hx_td_Bank.BankName, 
                dbo.hx_td_Bank.OpenBankId AS Expr1, dbo.hx_member_table.username, dbo.hx_member_table.realname, 
                dbo.hx_member_table.registerid, dbo.hx_td_Bank.CardImage, dbo.hx_td_Bank.Isquick, 
                dbo.hx_td_Bank.Isordinary
FROM      dbo.hx_UsrBindCardC INNER JOIN
                dbo.hx_td_Bank ON dbo.hx_UsrBindCardC.OpenBankId = dbo.hx_td_Bank.OpenBankId INNER JOIN
                dbo.hx_member_table ON dbo.hx_UsrBindCardC.UsrCustId = dbo.hx_member_table.UsrCustId
 GO
---------View Name : V_yaoqin_count--- 无变化
--if object_id(N'V_yaoqin_count') is not null drop view V_yaoqin_count

--  GO 

--CREATE VIEW [dbo].[V_yaoqin_count]
--AS
--SELECT     dbo.hx_Bid_records.bid_records_id, dbo.hx_Bid_records.targetid, dbo.hx_Bid_records.investment_amount, dbo.hx_Bid_records.OrdId, dbo.hx_Bid_records.ordstate, 
--                      dbo.hx_Bid_records.invitationcode, dbo.hx_member_table.username AS yaousername, dbo.hx_member_table.registerid AS yaouid, 
--                      hx_member_table_1.registerid AS invuid, hx_member_table_1.username AS invusername, hx_member_table_1.realname, dbo.hx_Bid_records.invest_time, 
--                      dbo.hx_member_table.realname AS yaorealname
--FROM         dbo.hx_Bid_records INNER JOIN
--                      dbo.hx_member_table ON dbo.hx_Bid_records.invitationcode = dbo.hx_member_table.invitedcode INNER JOIN
--                      dbo.hx_member_table AS hx_member_table_1 ON dbo.hx_Bid_records.investor_registerid = hx_member_table_1.registerid


-- GO
---------View Name : View_BId_userAct --新增
if object_id(N'View_BId_userAct') is not null drop view View_BId_userAct
  GO 
CREATE VIEW [dbo].[View_BId_userAct]
AS
SELECT   dbo.hx_Bid_records.bid_records_id, dbo.hx_Bid_records.borrower_registerid, dbo.hx_Bid_records.targetid, 
                dbo.hx_Bid_records.loan_number, dbo.hx_Bid_records.annual_interest_rate, dbo.hx_Bid_records.current_period, 
                dbo.hx_Bid_records.investment_amount, dbo.hx_Bid_records.OrdId, dbo.hx_Bid_records.BonusAmt, 
                dbo.hx_Bid_records.JiaxiNum, dbo.hx_UserAct.UserAct, dbo.hx_UserAct.registerid, dbo.hx_UserAct.ActID, 
                dbo.hx_UserAct.RewTypeID, dbo.hx_UserAct.ActTypeId, dbo.hx_UserAct.Amt, dbo.hx_UserAct.Uselower, 
                dbo.hx_UserAct.Usehight, dbo.hx_UserAct.AmtEndtime, dbo.hx_UserAct.AmtUses, dbo.hx_UserAct.UseState, 
                dbo.hx_UserAct.AmtProid, dbo.hx_UserAct.UseTime, dbo.hx_UserAct.ISSmsOne, dbo.hx_UserAct.IsSmsThree, 
                dbo.hx_UserAct.IsSmsSeven, dbo.hx_UserAct.isSmsFifteen, dbo.hx_UserAct.isSmsSixteen, 
                dbo.hx_member_table.username, dbo.hx_member_table.mobile, dbo.hx_member_table.realname, 
                dbo.hx_member_table.useridentity, dbo.hx_member_table.Channelsource
FROM      dbo.hx_Bid_records INNER JOIN
                dbo.hx_UserAct ON dbo.hx_Bid_records.bid_records_id = dbo.hx_UserAct.AmtProid INNER JOIN
                dbo.hx_member_table ON dbo.hx_UserAct.registerid = dbo.hx_member_table.registerid
 GO
---------View Name : ViewInvestRecord -- 无变化
if object_id(N'ViewInvestRecord') is not null drop view ViewInvestRecord
  GO 
CREATE VIEW [dbo].[ViewInvestRecord]
AS
    SELECT  a.bid_records_id,a.targetid,
            a.investor_registerid,mt.username ,
            a.investment_amount ,a.invest_state ,
            a.invest_time ,a.ordstate
    FROM    [dbo].hx_Bid_records AS a WITH ( NOLOCK )
        	LEFT JOIN dbo.hx_member_table AS mt WITH ( NOLOCK )   
        	ON a.investor_registerid = mt.registerid;
GO
---------View Name : ViewThirdWangDaiZhongGuo--- 无变化-- 规范sql
if object_id(N'ViewThirdWangDaiZhongGuo') is not null drop view ViewThirdWangDaiZhongGuo
  GO 
CREATE VIEW [dbo].[ViewThirdWangDaiZhongGuo]
AS
SELECT 		'http://www.chuanglitou.com/invest_borrow_'+CAST( t.targetid AS NVARCHAR(MAX))+'.html' AS url,t.targetid AS id,t.borrowing_title AS title,t.borrowing_balance AS account,t.annual_interest_rate AS apr,t.life_of_loan AS limit
,t.unit_day AS limit_type
,t.payment_options AS way
,t.H_Repayment_Amt AS account_yes
,t.minimum AS account_min
,t.maxmum AS account_max
,'' AS username ,
(DATEDIFF(S,'1970-01-01 00:00:00',td.createtime) - 8 * 3600 ) AS add_time
,(DATEDIFF(S,'1970-01-01 00:00:00',t.sys_time) - 8 * 3600 )   AS verify_time
,(DATEDIFF(S,'1970-01-01 00:00:00',t.sys_time) - 8 * 3600 )  AS success_time
,'默认' AS [type]
--1  录入 0 审核中 1 初审通过  2 复审通过(开标上线)    3 满标 (还款中)     4放款 (还款中)   5 已还清   6初审未通过   7 复审未通过  8 流标
,( CASE  t.tender_state  WHEN 0 THEN 0  --0 新标待审 1 初审通过 2 初审未通过 3 成功借款 4 复审未通过 5已结束 6 用户取消 7 坏账
WHEN 1 THEN 1 --1 初审通过
WHEN 6 THEN 2
WHEN 2 THEN 3
WHEN 3 THEN 3
WHEN 4 THEN 3
WHEN 7 THEN 4
WHEN 5 THEN 5
END
) AS [status] 
,'' AS reward
,1 AS is_diya
,1 AS is_danbao
,0 AS is_mima
,0 AS is_miao
,0 AS is_zhuan
FROM hx_borrowing_target t  WITH  (NOLOCK) 
LEFT JOIN dbo.hx_borrowing_target_detailed  td  WITH  (NOLOCK) ON t.targetid=td.targetid
GO
---旧库新增两个视图  V_LuckDrawData  大转盘  ViewUserCenter用户分配？客服系统对接 

--------------------------------Generate trigger Script------------------------------------
---------trigger name:hx_Bid_records_insert 增加   用户投资次数递增逻辑
if object_id(N'hx_Bid_records_insert') is not null drop trigger hx_Bid_records_insert
  GO 
CREATE TRIGGER [dbo].[hx_Bid_records_insert]
ON [dbo].[hx_Bid_records]
AFTER INSERT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
declare @targetid int,@investment_amount numeric(38,6),@investor_registerid int ;  --定义一个变量id

select @targetid=targetid,@investment_amount=investment_amount,@investor_registerid=investor_registerid from inserted;
update hx_borrowing_target  set fundraising_amount=fundraising_amount+@investment_amount where targetid=@targetid;
update hx_member_table set InvestNum=InvestNum+1 where registerid=@investor_registerid;
--因为有代金券这里不能直接相减，得单独处理
--update hx_member_table  set available_balance=available_balance-@investment_amount where registerid=@investor_registerid;
    -- Insert statements for trigger here
END
 GO

---------trigger name:hx_Bid_records_insert 增加用户投资次数递减逻辑；新库中缺少activitylog相关更新，保留老库，待进一步完善
if object_id(N'hx_Bid_records_targetdelete') is not null drop trigger hx_Bid_records_targetdelete
  GO 
CREATE TRIGGER [dbo].[hx_Bid_records_targetdelete]
ON [dbo].[hx_Bid_records]
INSTEAD OF DELETE
AS
BEGIN
	SET NOCOUNT ON;
	declare @bid_records_id int,@investment_amount numeric(38,6),@targetid int,@investor_registerid int ;  --定义一个变量id

	select @bid_records_id=bid_records_id,@investment_amount=investment_amount,@targetid=targetid,@investor_registerid=investor_registerid from deleted;
	update hx_borrowing_target  set fundraising_amount=fundraising_amount-@investment_amount where targetid=@targetid;
    update hx_member_table set InvestNum=InvestNum-1 where registerid=@investor_registerid;
	--delete hx_td_frozen where bid_records_id=@bid_records_id  --先删除该项目下的投资冻结 这个位置得手动处理
	delete hx_income_statement where bid_records_id=@bid_records_id  --先删除该项目下的投资记录
	delete hx_Bid_records where bid_records_id=@bid_records_id  --然后再删除 该项目
	--update hx_member_table  set available_balance=available_balance+@investment_amount where registerid=@investor_registerid; --删除时同时还原金额
    ----？？？？待完善，新库不再使用activitylog表
	update ActivityLogs set usedRecordID=0,usedtargetId=0,useStatus=0 where Logid in(select Logid from dbo.ActivityLogs where usedrecordid>0 and usedrecordid not in (select bid_records_id from hx_Bid_records)) 
END
 GO
 ---------trigger name:hx_Bid_records_insert 增加


------------------------------Generate User Function Script------------------------------
---------User Function Name : getInvestCountByDate-- 无变化 
--if object_id(N'getInvestCountByDate') is not null drop function getInvestCountByDate

--  GO 

--CREATE function getInvestCountByDate(@investDate varchar(10))   
--returns int --返回值类型  
--as  
--begin  
--declare @investCount int   
--SELECT @investCount=COUNT(DISTINCT(ba.registerid))
--from hx_member_table ba (nolock) 
--LEFT JOIN   hx_Bid_records br ON br.investor_registerid=ba.registerid  
--WHERE br.bid_records_id IN 
--      (SELECT MIN(hb2.bid_records_id)
--              FROM dbo.hx_Bid_records hb2 
--              GROUP BY hb2.investor_registerid ) AND   
--convert(varchar(10),br.invest_time,120)=@investDate  
--return @investCount   
--end
--GO
---------User Function Name : GetSplitLength 新增 获取指定字符出现的次数
if object_id(N'GetSplitLength') is not null drop function GetSplitLength
  GO 
CREATE function [dbo].[GetSplitLength] 
( 
@str varchar(1024), --要分割的字符串 
@split varchar(10) --分隔符号 
) 
returns int 
as 
begin 
declare @location int 
declare @start int 
declare @length int 
set @str=ltrim(rtrim(@str)) 
set @location=charindex(@split,@str) 
set @length=1 
while @location<>0 
	begin 
	set @start=@location+1 
	set @location=charindex(@split,@str,@start) 
	set @length=@length+1 
	end 
return @length 
end 
 GO
---------User Function Name : GetSplitStrOfIndex 新增 ，返回第index个分割符号和index+1分割字符之间的字符串
if object_id(N'GetSplitStrOfIndex') is not null drop function GetSplitStrOfIndex
  GO 
CREATE FUNCTION [dbo].[GetSplitStrOfIndex] 
( 
@str NVARCHAR(MAX), --要分割的字符串 
@split VARCHAR(10), --分隔符号 
@index INT --取第几个元素 
) 
RETURNS NVARCHAR(MAX) 
AS 
BEGIN 
DECLARE @location INT 
DECLARE @start INT 
DECLARE @next INT 
DECLARE @seed INT 
SET @str=LTRIM(RTRIM(@str)) 
SET @start=1 
SET @next=1 
SET @seed=LEN(@split) 
SET @location=CHARINDEX(@split,@str) 
WHILE @location<>0 AND @index>@next 
	BEGIN 
	SET @start=@location+@seed 
	SET @location=CHARINDEX(@split,@str,@start) 
	SET @next=@next+1 
	END 
IF @location =0 SELECT @location =LEN(@str)+1  
RETURN SUBSTRING(@str,@start,@location-@start) 
END 
 GO
------------------------------Generate Stored Procedure Script------------------------------
---------Stored Procedure Name : prSubmitTender -- 新增，投标存储过程 webapi使用
if object_id(N'prSubmitTender') is not null drop proc prSubmitTender
  GO 
CREATE PROC [dbo].[prSubmitTender]
(
	@usrId INT,
	@targetId INT,
	@investAmount DECIMAL(18, 2),--投资金额
	@ids NVARCHAR(500),--优惠券ids
	@cPeriod INT,--
	@code NVARCHAR(500),--邀请码
	@ordCode DECIMAL(20),--订单号
	@withoutInterest DECIMAL(18, 2)--投资到期可获取总收益
	,@frozenidNo NVARCHAR(500)--冻结号
	,@frozenidAmount DECIMAL(18,2)--冻结金额
	,@incomeStatementStr NVARCHAR(4000)--投资人利息收入表
)
AS
BEGIN
DECLARE @returnValue INT;
BEGIN TRAN submitTender;    --开始事务
	DECLARE @tran_error INT;
	SET @tran_error = 0;
 	--BEGIN TRY 
	DECLARE @targetBalance DECIMAL;--剩余金额
	DECLARE @maxV DECIMAL;--最高投资
	SELECT @targetBalance = ( borrowing_balance-fundraising_amount) FROM dbo.hx_borrowing_target WHERE targetid=@targetId;
	SELECT @maxV = maxmum FROM dbo.hx_borrowing_target WHERE targetid=@targetId;
	PRINT  @targetBalance;
	PRINT  @maxV;
	PRINT  @investAmount;
	IF(@investAmount>@maxV AND @maxV<>0)
	BEGIN
		PRINT @maxV;
		PRINT '投资金额不能超过最高可投限额';
		SET @returnValue= -100;
	END
	ELSE
	BEGIN
		IF(@investAmount>@maxV AND @maxV<>0)
		BEGIN
			PRINT '投资金额超出可投金额';
			SET @returnValue= -200;
		END
		ELSE
		BEGIN--判断用户余额
			DECLARE @currentTime DATETIME;
			SET @currentTime=GETDATE();
			DECLARE @aor DECIMAL;
			DECLARE @b DECIMAL;
			DECLARE @str VARCHAR(MAX);
			DECLARE @next INT;
			DECLARE @bonusId INT;
			SET @str=@ids;
			SET @next=1;
			IF(@str<>'')
			BEGIN
				WHILE @next<=dbo.GetSplitLength(@str,',') 
				BEGIN
					DECLARE @lmt DECIMAL;
					SET @bonusId=CAST(dbo.GetSplitStrOfIndex(@str,',',@next) AS INT); 
					--IF(EXISTS(SELECT COUNT(1) FROM dbo.bonus_account WHERE membertable_registerid=@usrId AND bonus_account_id=@bonusId))
					BEGIN
						SELECT @lmt=use_lower_limit FROM dbo.bonus_account WHERE membertable_registerid=@usrId AND bonus_account_id=@bonusId;
						PRINT @lmt;
						IF(@lmt>@investAmount)
						BEGIN
							PRINT '个别优惠券不可用';
							ROLLBACK TRAN;
							RETURN -500;
							BREAK;
						END
						SET @bonusId=CAST(dbo.GetSplitStrOfIndex(@str,',',@next) AS INT); 
						DECLARE @temm DECIMAL;
						SELECT @temm=amount_of_reward FROM dbo.bonus_account WHERE bonus_account_id=@bonusId AND membertable_registerid=@usrId;
						SET @aor=@aor+@temm;
						SET  @next=@next+1; 
					END
					--ELSE
				END
				PRINT @str;
				PRINT '-------------';
				IF(@next<>dbo.GetSplitLength(@str,','))
				BEGIN
					PRINT '优惠券数据有问题，数据回滚';
					ROLLBACK TRAN;
					RETURN -600;
				END
			END
			
			PRINT @aor;
			SELECT @b=available_balance FROM dbo.hx_member_table WHERE registerid=@usrId;
			IF(@investAmount>@aor+@b)
			BEGIN
				PRINT '帐户余额不足，请充值'
				SET @returnValue= -300;
			END
			ELSE
			BEGIN--判断当前用户 当前标无效标记录
				DECLARE @c int;
				SELECT @c=(COALESCE(count(ordstate),0)) from hx_Bid_records where targetid=@targetId and investor_registerid=@usrId and ordstate=0;
				IF(@c>=3)
				BEGIN
					PRINT '本标的投资未付款已超过三次';
					SET @returnValue= -400;
				END
				ELSE
				BEGIN
					DECLARE @brId INT;--borrower_registerid 借款人id
					DECLARE @lNo DECIMAL;--loan_number 借款编号
					DECLARE @air INT;--annual_interest_rate 年化收益
					DECLARE @borrowCusId NVARCHAR(500);
					DECLARE @InCusId NVARCHAR(500);
					--DECLARE @imy INT--investment_maturity 投资到期结束日期
					DECLARE @repaymentDate DATETIME--repayment_date
					SELECT @brId=borrower_registerid,@lNo=loan_number,@air=annual_interest_rate, 
						@repaymentDate=repayment_date 
						FROM dbo.hx_borrowing_target
						WHERE targetid=@targetId;
					SELECT @borrowCusId=UsrCustId FROM dbo.hx_member_table WHERE registerid=@brId;
					SELECT @InCusId=UsrCustId FROM dbo.hx_member_table WHERE registerid=@usrId;
					INSERT into hx_Bid_records(borrower_registerid,targetid,
							loan_number,annual_interest_rate,current_period,
							investment_amount,value_date,investment_maturity,
							invest_time,invest_state,flow_return,repayment_amount,
							repayment_period,investor_registerid,payment_status,
							withoutinterest,invitationcode,OrdId
					)
					values (
						@brId,@targetid,@lNo,@air,@cPeriod,@investAmount,@currentTime
						,@repaymentDate,@currentTime,1,1,0,@repaymentDate,@usrId,0,
						@withoutInterest,@code,@ordCode
					)
					DECLARE @recordId INT;
					SELECT @recordId=@@IDENTITY;
					PRINT '投资记录表数据添加成功'

					--冻结优惠券数据开始	
					IF(@ids<>'')
					BEGIN
						set @str=@ids;
						set @next=1;
						while @next<=dbo.GetSplitLength(@str,',')
						BEGIN
							PRINT @str;
							SET @bonusId=CAST(dbo.GetSplitStrOfIndex(@str,',',@next) AS INT);

							PRINT '-------------------------------';
							PRINT @bonusId;
							PRINT '-------------------------------';
							IF(EXISTS(SELECT COUNT(1) FROM dbo.bonus_account WHERE membertable_registerid=@usrId AND bonus_account_id=@bonusId))
							BEGIN
							--锁定优惠券
								update bonus_account set reward_state=3,proid= @recordId WHERE bonus_account_id =@bonusId  and membertable_registerid=@usrId;
             				END
							ELSE
							BEGIN
							 	ROLLBACK TRAN;
								PRINT '执行出错，回滚事务';
								BREAK
							END
							SET @next=@next+1;
						END
					END
					--冻结优惠券数据结束						

					--优惠券数据更新开始
					IF(@code<>'')
					BEGIN
                    	update hx_td_Userinvitation set InvitesStates=2 where  invcode=@code and invpersonid=@usrId and InvitesStates=0;
        			END
					--优惠券数据更新结束

					--循环向 投资人利息收入表添加数据
					set @str=@incomeStatementStr;
					PRINT @str;
					set @next=1;
					while @next<=dbo.GetSplitLength(@str,'|')
					BEGIN
						DECLARE @xxx NVARCHAR(3000);
						SET @xxx=dbo.GetSplitStrOfIndex(@str,'|',@next)
						IF(@xxx<>'')
						BEGIN
						 		PRINT @xxx ;
					 		insert into hx_income_statement(
								targetid,bid_records_id,loan_number
								,borrower_registerid,annual_revenue,investment_amount
								,invest_time,current_investment_period,value_date
								,interest_payment_date,repayment_amount,investor_registerid
								,payment_status,interestpayment,OutCustId,InCustId
								,BidOrderid,Principal,TotalInstallments,interestDay
							)
					  		values (
								@targetId,@recordId,@lNo
								,@brId,@air,@investAmount,@currentTime
								,CAST(dbo.GetSplitStrOfIndex(@xxx ,',',1) AS INT)
								,dbo.GetSplitStrOfIndex(@xxx ,',',2)
								,dbo.GetSplitStrOfIndex(@xxx ,',',3)
								,CONVERT(DECIMAL(20,2),dbo.GetSplitStrOfIndex(@xxx ,',',4))
								,@usrId
								,0
								,CAST(dbo.GetSplitStrOfIndex(@xxx ,',',5) AS DECIMAL(38,2))
								,@borrowCusId,@InCusId,@ordCode
								,CAST(dbo.GetSplitStrOfIndex(@xxx ,',',6) AS DECIMAL(38,2))
								,CAST(dbo.GetSplitStrOfIndex(@xxx ,',',7) AS INT)
								,CAST(dbo.GetSplitStrOfIndex(@xxx ,',',8) AS INT)
						  	)
						END
						PRINT 'hx_income_statement 插入数据';
						PRINT @next;
						set @next=@next+1;
					END
					  --冻结记录写入
					insert into hx_td_frozen
					(
					  	MBT_Registerid,FrozenidAmount,FrozenidNo,FrozenState,FrozenDate,
					  	targetid,UsrCustId,bid_records_id
					)
					values 
					(
						@usrId,@frozenidAmount,@frozenidNo,0,
						@currentTime,@targetid,@InCusId,@recordId
					);
					PRINT '冻结表记录数据';
					SET @returnValue=200;
					SET @tran_error = @tran_error + @@ERROR;
				END
			END
		END
	END
	--END TRY
	--BEGIN CATCH
	--	PRINT '出现异常'+ error_message()
	--	SET @tran_error = @tran_error + 1
	--END CATCH
	IF(@tran_error > 0)
	BEGIN
		--执行出错，回滚事务
		ROLLBACK TRAN;
		PRINT '执行出错，回滚事务';
	END
	ELSE
	BEGIN
		--没有异常，提交事务
		COMMIT TRAN;
		PRINT '没有异常，提交事务';
	END
	RETURN @returnValue;
END
GO

---------Stored Procedure Name : prRepayment --- 无变化
--if object_id(N'prRepayment') is not null drop proc prRepayment

--  GO
--CREATE PROC prRepayment
--@recordId int,
--@amt decimal(18,2)
--as
--BEGIN
--DECLARE @ordId DECIMAL(20)
--DECLARE @targetId int
--DECLARE @subOrdId VARCHAR(50)
--DECLARE @inCustId VARCHAR(50)
--DECLARE @outCustId VARCHAR(50)
--DECLARE @unFreezeOrdId VARCHAR(50)
--DECLARE @freezeTrxId VARCHAR(50)
--DECLARE @loanOrdId VARCHAR(50)
--DECLARE @loanDate DATETIME
--select @ordId=OrdId,@targetId=targetid from hx_Bid_records where bid_records_id= @recordId

--select @unFreezeOrdId=FrozenidNo,@freezeTrxId=FreezeTrxId from dbo.hx_td_frozen where bid_records_id=@recordId 

--SELECT @loanOrdId=LoanOrdId,@loanDate=LoanDate FROM dbo.hx_td_Loan_records WHERE bid_records_id=@recordId

--DECLARE @free DECIMAL(10,2)
--SELECT @free=Free,@inCustId=InCustId,@outCustId=OutCustId FROM hx_td_Loan_records WHERE targetid=@targetId AND LoanAMT=@amt
--print '================================='
--print @loanOrdId
--print '================================='
--if(@loanOrdId is null or @loanOrdId='' )
--begin
--	print '暂时无放款数据,生成放款数据'
--	SELECT @loanOrdId=FrozenidNo,@loanDate=FrozenDate FROM dbo.hx_td_frozen WHERE bid_records_id=@recordId
--	IF(@free IS NULL )
--	BEGIN
--		print '无放款记录'
--	END
--	PRINT @free
--	INSERT INTO [dbo].[hx_td_Loan_records]
--           ([targetid]
--           ,[bid_records_id]
--           ,[InCustId]
--           ,[OutCustId]
--           ,[LoanAMT]
--           ,[LoanOrdId]
--           ,[LoanDate]
--           ,[Free]
--           ,[DivDetails]
--           ,[SubOrdid]
--           ,[SubOrdDate]
--           ,[unFreezeOrdId]
--           ,[FreezeTrxId]
--           ,[bid_orderid])
--     VALUES
--           (@targetId
--           ,@recordId
--           ,@inCustId
--           ,@outCustId
--           ,@amt
--           ,@loanOrdId
--           ,@loanDate
--           ,@free
--           ,''
--           ,@ordId
--           ,@loanDate
--           ,@unFreezeOrdId
--           ,@freezeTrxId
--           ,@ordId)
--	PRINT 'targetId：'+CAST(@targetId AS NVARCHAR(50))
--	PRINT '入账商户号：'+@inCustId
--	PRINT '出账商户号：'+@outCustId
--	PRINT '订单号：'+CAST(@ordId AS NVARCHAR(50))
--	PRINT '唯一冻结标识：'+CAST(@freezeTrxId AS NVARCHAR(50))
--	PRINT '解冻号：'+CAST(@unFreezeOrdId AS NVARCHAR(50))
--	PRINT '放款号：'+CAST(@loanOrdId AS NVARCHAR(50))
--	PRINT '放款时间：'+CAST(@loanDate AS NVARCHAR(50))
--	PRINT 'subOrdid=订单号 unFreezeOrdId=解冻号,FreezeTrxId=唯一冻结标识,bid_orderid=订单号'
--end
--else
--begin
--PRINT '存在放款记录，去放款'
--end
--end
--GO
---------Stored Procedure Name : procPagination --无变化
--if object_id(N'procPagination') is not null drop proc procPagination

--  GO 

--CREATE PROCEDURE [dbo].[procPagination]
--(
--@tblName   NVarChar(255),       -- 表名
--@strGetFields NVarChar(1000),  -- 需要返回的列
--@fldName NVarChar(255),      -- 排序的字段名
--@prKey NVarChar(255),      -- 主键
--@pageSize   int ,          -- 页尺寸
--@pageIndex  int ,           -- 页码
--@strWhere  NVarChar(1500),  -- 查询条件(注意: 不要加where)
--@sort NVarChar(255),      --排序的方法
--@recordCount int OUTPUT  --总记录数(存储过程输出参数)
--)
--AS
--declare @strSQL   NVarChar(max)       -- 主语句 
--declare @strOrder NVarChar(400)        -- 排序类型
--declare @TmpSelect NVarChar(600)      -- 记录统计
--if @strWhere != '' 
--begin
--set @TmpSelect = 'select @recordCount = count(*) from '+ @tblName +' where ' + @strWhere
--end
--else
--begin
--set @TmpSelect = 'select @recordCount = count(*) from '+ @tblName
--end
--execute sp_executesql 
--@TmpSelect,    --执行上面的sql语句
--N'@recordCount int OUTPUT' ,   --执行输出数据的sql语句，output出总记录数 
--@recordCount  OUTPUT
----if (@recordCount = 0)    --如果没有贴子，则返回零
--  --     return 0
--if @sort = 'desc'
--begin 
--set @strOrder = ' order by ' + @fldName +' desc'
----如果@OrderType不是，就执行降序，这句很重要！
--end
--else
--begin 
--set @strOrder = ' order by ' + @fldName +' asc'
--end

--if @pageIndex = 1
--begin
--if @strWhere != ''   
--  begin
--set @strSQL = 'select top ' + str(@pageSize) +' '+@strGetFields+ '  from ' + @tblName + ' where ' + @strWhere + ' ' + @strOrder
--  end
--else
--  begin
--set @strSQL = 'select top ' + str(@pageSize) +' '+@strGetFields+ '  from '+ @tblName + ' '+ @strOrder
--  end
----如果是第一页就执行以上代码，这样会加快执行速度
--end
--else
--begin
----以下代码赋予了@strSQL以真正执行的SQL代码
--set @strSQL = 'select top ' + str(@pageSize) +' '+@strGetFields+ '  from '
--+ @tblName + ' where ' + @prKey + ' not in'  + '(select top ' + str((@pageIndex-1) * @pageSize) + ' '+ @prKey + ' from ' + @tblName + '' + @strOrder + ') '+ @strOrder
 
--if @strWhere != ''

--set @strSQL = 'select top ' + str(@pageSize) +' '+@strGetFields+ '  from '
--+ @tblName + ' where '+ @strWhere+' and ' + @prKey + ' not in'  + '(select top ' + str((@pageIndex-1) * @pageSize) + ' '+ @prKey + ' from ' + @tblName + ' where ' +@strWhere + @strOrder + ') '+ @strOrder
 
--END
--PRINT @strSQL
--exec (@strSQL)
--RETURN
--GO
---------Stored Procedure Name : prSaveMemberAddress  -- 增加三个参数及相关数据处理  userName，mobile，zipCode

if object_id(N'prSaveMemberAddress') is not null drop proc prSaveMemberAddress
 GO
CREATE PROC [dbo].[prSaveMemberAddress]
(
@userId INT,
@provId INT,
@cityId INT,
@counId INT,
@detAdds NVARCHAR(500),
@userName NVARCHAR(50),
@mobile NVARCHAR(20),
@zipCode NVARCHAR(10)
)
AS
BEGIN
DECLARE @provName NVARCHAR(50);
DECLARE @cityName NVARCHAR(50);
DECLARE @countyName NVARCHAR(50);
SELECT @provName=AreaName FROM NormalArea WHERE AreaId=@provId;
SELECT @cityName=AreaName FROM NormalArea WHERE AreaId=@cityId AND parentId=@provId;
SELECT @countyName=AreaName FROM NormalArea WHERE AreaId=@counId AND parentId=@cityId;
IF(@provName='' or @provName is null)
BEGIN
RETURN -500;
END
IF(@cityName='' or @cityName is null)
BEGIN
RETURN -400;
END
IF(@countyName='' or @countyName is null)
BEGIN
RETURN -300;
END
IF(NOT EXISTS(SELECT registerid FROM dbo.hx_member_table WHERE registerid=@userId))
BEGIN
RETURN -200;
END
IF(EXISTS(SELECT userId FROM UserAddress WHERE userId=@userId))
BEGIN
UPDATE UserAddress SET 
cityId=@cityId,
cityName=@cityName,
provinceId=@provId,
provinceName=@provName,
countyId=@counId,
countyName=@countyName,
detailAddress=@detAdds,
updatedOn=GETDATE(),
userName=@userName,
mobile=@mobile,
zipCode=@zipCode
WHERE userId=@userId
END	
ELSE
BEGIN
INSERT INTO [dbo].[UserAddress]
           ([userId]
           ,[provinceId]
           ,[provinceName]
           ,[cityId]
           ,[cityName]
           ,[countyId]
           ,[countyName]
           ,[detailAddress]
		   ,[userName]
		   ,[mobile]
		   ,[zipCode])
     VALUES
           (@userId
           ,@provId, @provName,@cityId, @cityName,@counId,@countyName, @detAdds,@userName,@mobile,@zipCode)
END
RETURN 1;
END
GO


--Alter table hx_UserAct add [Title] nvarchar(100) NULL 
--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动主题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAddress', @level2type=N'COLUMN',@level2name=N'Title';



---------Stored Procedure Name : GetPageRecord---- 无变化
--if object_id(N'GetPageRecord') is not null drop proc GetPageRecord

--  GO 

--CREATE PROCEDURE [dbo].[GetPageRecord]
--    ( 
--    @tblName varchar(255),--表名或视图表
--    @strGetFields varchar(1000),  -- 欲选择字段列表
--    @fldName varchar(255),      -- 排序字段及类型(多个条件用逗号分开)如：JobID DESC,Checkintime
--    @PageSize int,--页尺寸
--    @PageIndex int,--页号,从0开始
--    @strWhere varchar(1500),--条件  
--    @RecordCount int OUTPUT  --总记录数(存储过程输出参数) 
--    ) 
--AS
--BEGIN TRAN
--DECLARE @SqlQuery varchar(4000)
----返回记录总数
--    DECLARE @SearchSql AS Nvarchar(4000)
--    SET @SearchSql= 'SELECT @RecordCount=Count(*) FROM '+@tblName+' WHERE '+@strWhere
--    execute sp_executesql 
--    @SearchSql,    --执行上面的sql语句
--    N'@RecordCount int OUTPUT' ,   --执行输出数据的sql语句，output出总记录数 
--    @RecordCount  OUTPUT

----返回记录数
--    SET @SqlQuery='SELECT '+@strGetFields+'
--    FROM (SELECT row_number() over(ORDER BY '+@fldName+') as rownum, 
--            '+@strGetFields+'
--          FROM '+@tblName+' WHERE '+@strWhere +') as temp
--    WHERE rownum BETWEEN ('+cast(@PageIndex as varchar)+'-1)*'+cast(@PageSize as varchar)+'+1 and '+cast(@PageIndex as varchar)+'*'+cast(@PageSize as varchar) + ' ORDER BY '+@fldName
--    --print @SqlQuery
--    SET NOCOUNT ON
--    execute(@SqlQuery)
--    SET NOCOUNT OFF
--COMMIT TRAN
--GO
---------Stored Procedure Name : pagination  无变化
--if object_id(N'pagination') is not null drop proc pagination

--  GO 

--/***************************************************** 
--存储过程名称 pagination 
--功能:通用分页 
--参数
--1.表名(字符串必要参数) 
--2.主键或者唯一约束字段 字符串 必要参数  
--3.需不需要进行总数统计.布尔类型 可选参数默认为1。1或者true是只统计总条数,不进行查询 0或者false进行分页查询,不进行统计  
--4.查询条件 字符串 可选参数 默认为无条件查询。     注意不要加where  
--5.排序的字段名字符串可选参数默认按照主键排序  
--6.页码 整数 可选参数 默认第1页  
--7.每页大小整数 可选参数 默认每页10条记录  
--8.排序类型 整数或布尔 可选参数 默认升序排序     1或true按降序排序     0或false按升序排列  
--9.需要返回的列(字符串 可选参数,默认返回所有列)  
--*****************************************************/ 
--CREATE PROCEDURE [dbo].[pagination] 
--@TableName NVARCHAR(255), -- 表名 
--@PrimaryKey NVARCHAR(255),--主键或者唯一约束字段 
--@StrWhere NVARCHAR(1500) = '', -- 查询条件 (注意: 不要加 where)  
--@OrderField NVARCHAR(255)=@PrimaryKey, -- 排序的字段名,默认按照主键排序 
--@PageIndex INT = 1, -- 页码 
--@PageSize INT = 10, -- 页尺寸 
--@OrderType BIT = 0 ,-- 设置排序类型, 非 0 值则降序 
--@StrGetFields NVARCHAR(1000) = '*',-- 需要返回的列  
--@RecordCount INT=0 OUTPUT,--输出记录总数
--@PageCount INT=0 OUTPUT--输出页面总数
--AS
--BEGIN
--	SET NOCOUNT ON;
--	DECLARE @strSQL NVARCHAR(4000),@strTmp NVARCHAR(110),@strOrder NVARCHAR(400)
--	IF (@StrWhere!='')
--	BEGIN 
--			SET @strSQL = 'select @RecordCount=count(*) from ' + @TableName + ' where '+@strWhere
--	END
--	ELSE
--	BEGIN   
--			SET @strSQL = 'select @RecordCount=count(*) from ' + @TableName
--	END	
--	--统计记录总数 
--	EXEC SP_EXECUTESQL @strSQL,N'@RecordCount INT OUTPUT ',@RecordCount OUTPUT;
--	PRINT @RecordCount;
--	--统计分页总数
--	SET @PageCount = (@RecordCount+@PageSize-1)/@PageSize;
--	/******  确定是升序还是降序  ******/  
--	IF (@OrderType != 0)
--	BEGIN   
--		SET @strTmp = '<(select min'   
--		SET @strOrder = 'order by ' + @OrderField +' desc'  
--	END   
--	ELSE  
--	BEGIN
--	   SET @strTmp = '>(select max'
--	   SET @strOrder = 'order by ' + @OrderField +' asc'
--	END   
--	/*****为了加快执行速度判断一下是不是第一页******/
--	IF (@PageIndex = 1)
--	BEGIN
--		IF (@strWhere != '')
--			 SET @strSQL ='select top ' + STR(@PageSize) +' '+@StrGetFields+ ' from ' + @TableName + ' where ' + @strWhere + ' ' + @strOrder   
--		ELSE
--			 SET @strSQL ='select top ' + STR(@PageSize) +' '+@strGetFields+ ' from '+ @TableName + ' '+ @strOrder  
--	END  /******不是第一页******/   
--	ELSE 
--	BEGIN   
--		IF @strWhere=''    
--			SET @strSQL='select top '+STR(@PageSize)+' '+@strGetFields+' from '+ @TableName +' where '+@PrimaryKey+@strTmp+'('+@PrimaryKey+') as '+@PrimaryKey+' from (select top '+STR((@PageIndex-1)*@PageSize)+' '+@PrimaryKey+' from '+@TableName+' '+@strOrder+') as T) '+@strOrder   ELSE    SET @strSQL='select top '+STR(@PageSize)+' '+@strGetFields+' from '+ @TableName +' where '+@PrimaryKey+@strTmp+'('+@PrimaryKey+') as '+@PrimaryKey+' from (select top '+STR((@PageIndex-1)*@PageSize)+' '+@PrimaryKey+' from '+@TableName+' where '+@StrWhere+' '+@strOrder+') as T) and '+@StrWhere+' '+@strOrder     
--	END  
--	PRINT @strSQL 
--	EXEC SP_EXECUTESQL @strSQL    
--END
--GO
---------Stored Procedure Name : prStatisticsMember--- 无变化
--if object_id(N'prStatisticsMember') is not null drop proc prStatisticsMember

--  GO 

--CREATE proc prStatisticsMember
--as
--begin
--	--step1 初始化用户注册数据
--	 Insert into Statistics_Member(StatisticsDate,RegistCount,RealNameCount)  
--	 SELECT * FROM 
--	 (  
--	 SELECT  
--	CONVERT(varchar(10),registration_time,120) as registration_time,
--	COUNT(DISTINCT(registerid)) as regcount  
--	,sum(case when isrealname=1 then 1 else 0 end) AS realNameCount 
--	--,dbo.getInvestCountByDate(CONVERT(varchar(10),registration_time,120))  AS investCount
--	FROM  hx_member_table  a 
--	WHERE registerid>0  group by  convert(varchar(10),registration_time,120) 
--	) AS bx WHERE bx.registration_time NOT IN (
--	SELECT StatisticsDate FROM Statistics_Member
--	)

--	PRINT('初始化用户注册数据成功')

--	----step2 初始化首次投资数据 只运行一次即可
--	--　　DECLARE @investDate varchar(10)   
--	--    DECLARE My_Cursor CURSOR --定义游标  
--	--    FOR (SELECT StatisticsDate FROM dbo.Statistics_Member WHERE IsFinish=0 AND StatisticsDate<CONVERT(varchar(10),GETDATE(),120) ) --查出<当天 并且没有统计完成的 集合放到游标中  
--	--    OPEN My_Cursor; --打开游标  
--	--    FETCH NEXT FROM My_Cursor INTO @investDate; --读取第一行数据
--	--    WHILE @@FETCH_STATUS = 0  
--	--        BEGIN  
--	--            PRINT @investDate; --打印数据
--	--            UPDATE dbo.Statistics_Member SET InvestCount =dbo.getInvestCountByDate(@investDate),IsFinish=1 WHERE StatisticsDate = @investDate; --更新数据  
--	--            FETCH NEXT FROM My_Cursor INTO @investDate; --读取下一行数据
--	--        END  
--	--    CLOSE My_Cursor; --关闭游标  
--	--    DEALLOCATE My_Cursor; --释放游标  
--	--    GO  
	 
--	--PRINT('初始化首次投资数据成功')
--	--更新当天数据
--	 DECLARE @registration_time varchar(10)
--	 DECLARE @regcount int
--	 DECLARE @realNameCount INT
--	 DECLARE @investCount int 
--	 SELECT  @registration_time=
--	CONVERT(varchar(10),GETDATE(),120),
--	@regcount=COUNT(DISTINCT(registerid))    
--	,@realNameCount=SUM(case when isrealname=1 then 1 else 0 end) 
--	,@investCount=dbo.getInvestCountByDate(CONVERT(varchar(10),GETDATE(),120))
--	FROM  hx_member_table  a 
--	WHERE registerid>0  and  convert(varchar(10),registration_time,120) =CONVERT(varchar(10),GETDATE(),120)

--	UPDATE Statistics_Member SET RegistCount=@regcount,RealNameCount=@realNameCount,InvestCount=@investCount,UpDatedOn=GETDATE() WHERE StatisticsDate=@registration_time 
--	AND (RegistCount<>@regcount OR RealNameCount<>@realNameCount OR InvestCount<>@investCount)
--	PRINT('更新当天数据成功')

--	UPDATE Statistics_Member SET IsFinish=1 ,UpDatedOn=GETDATE() WHERE StatisticsDate=@registration_time 
--	AND (IsFinish=0)
--	PRINT('更新当天完成状态')
--end

--GO
---------Stored Procedure Name : upPhone-- 无变化
--if object_id(N'upPhone') is not null drop proc upPhone
--GO
--CREATE procedure [dbo].[upPhone]
--@phone nvarchar(30),
--@oldph nvarchar(30)
--as
--	if((select COUNT(*) from hx_member_table where mobile=@oldph)>1)
--	begin
--		select '手机号出现重复'
--		select * from hx_member_table where mobile=@oldph
--	end
--	else
--	begin
--	 update hx_member_table set mobile=@phone where mobile=@oldph
--	end
--GO
