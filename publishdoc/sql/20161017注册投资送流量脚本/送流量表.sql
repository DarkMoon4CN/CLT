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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlow', @level2type=N'COLUMN',@level2name=N'ActivityFlowID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlow', @level2type=N'COLUMN',@level2name=N'RegisterID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ע���Ƿ񷢷� 0δ���ţ�1�ѷ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlow', @level2type=N'COLUMN',@level2name=N'ActivityFlowType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ͷ���Ƿ񷢷� 0δ���ţ�1�ѷ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlow', @level2type=N'COLUMN',@level2name=N'IsGrant'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityFlow', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO


