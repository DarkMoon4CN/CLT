USE [Newdata]
GO

/****** Object:  Table [dbo].[ActivityFlowWater]    Script Date: 10/18/2016 15:04:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ActivityFlowWater](
	[ActivityFlowWaterID] [int] IDENTITY(1,1) NOT NULL,
	[RegisterID] [int] NULL,
	[OrderNO] [nvarchar](50) NULL,
	[WaterType] [int] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_ActivityFlowWater] PRIMARY KEY CLUSTERED 
(
	[ActivityFlowWaterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlowWater', @level2type=N'COLUMN',@level2name=N'ActivityFlowWaterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlowWater', @level2type=N'COLUMN',@level2name=N'RegisterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流米充值订单流水号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlowWater', @level2type=N'COLUMN',@level2name=N'OrderNO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务类型 0注册，1投资' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlowWater', @level2type=N'COLUMN',@level2name=N'WaterType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发生时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlowWater', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO


