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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ˮID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlowWater', @level2type=N'COLUMN',@level2name=N'ActivityFlowWaterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlowWater', @level2type=N'COLUMN',@level2name=N'RegisterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���׳�ֵ������ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlowWater', @level2type=N'COLUMN',@level2name=N'OrderNO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ҵ������ 0ע�ᣬ1Ͷ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlowWater', @level2type=N'COLUMN',@level2name=N'WaterType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlowWater', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO


