USE [Newdata]
GO

/****** Object:  Table [dbo].[ActivityFlow]    Script Date: 10/18/2016 15:05:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ActivityFlow](
	[ActivityFlowID] [int] IDENTITY(1,1) NOT NULL,
	[RegisterID] [int] NULL,
	[ActivityFlowType] [int] NULL,
	[IsGrant] [int] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_ActivityFlow] PRIMARY KEY CLUSTERED 
(
	[ActivityFlowID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动送流量ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlow', @level2type=N'COLUMN',@level2name=N'ActivityFlowID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlow', @level2type=N'COLUMN',@level2name=N'RegisterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册是否发放 0未发放，1已发放' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlow', @level2type=N'COLUMN',@level2name=N'ActivityFlowType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投资是否发放 0未发放，1已发放' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlow', @level2type=N'COLUMN',@level2name=N'IsGrant'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlow', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO


