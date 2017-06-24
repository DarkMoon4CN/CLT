
/****** Object:  Table [dbo].[hx_QuanmamaRecord]    Script Date: 2016/10/20 13:47:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[hx_QuanmamaRecord](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RegisterMobile] [nvarchar](50) NOT NULL,
	[UsrCustID] [nvarchar](50) NOT NULL,
	[RegisterTime] [datetime] NOT NULL,
	[InvestTime] [datetime] NOT NULL,
	[InvestMoney] [money] NOT NULL,
	[TotalInvestMoney] [money] NOT NULL,
	[InvestPeriod] [nvarchar](50) NOT NULL,
	[GiftMoney] [money] NOT NULL,
	[SendState] [int] NOT NULL,
	[Creater] [int] NOT NULL,
	[CraeteTime] [datetime] NOT NULL,
	[Sender] [int] NULL,
	[SendTime] [datetime] NULL,
 CONSTRAINT [PK_QuanmamaRecord] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自增编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'RegisterMobile'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'UsrCustID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'RegisterTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投资时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'InvestTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首投金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'InvestMoney'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'总投资金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'TotalInvestMoney'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投资期限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'InvestPeriod'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返客户金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'GiftMoney'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发放状态（0-未发放，1-已发放）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'SendState'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'Creater'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'CraeteTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发放人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'Sender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发放时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_QuanmamaRecord', @level2type=N'COLUMN',@level2name=N'SendTime'
GO


