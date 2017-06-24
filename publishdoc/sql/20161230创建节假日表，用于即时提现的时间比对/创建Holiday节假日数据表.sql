IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Holiday]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Holiday](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Starttime] [datetime] NOT NULL,
	[Endtime] [datetime] NOT NULL,
	[DuringDays] [int] NOT NULL,
	[Comments] [varchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IsEnable] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK__Holiday__3213E83F05CFD091] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO

ALTER AUTHORIZATION ON [dbo].[Holiday] TO  SCHEMA OWNER 
GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Holiday_CreateTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Holiday] ADD  CONSTRAINT [DF_Holiday_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
END

GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Holiday', N'COLUMN',N'ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ψһ��ʶ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Holiday', @level2type=N'COLUMN',@level2name=N'ID'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Holiday', N'COLUMN',N'Year'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ǰ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Holiday', @level2type=N'COLUMN',@level2name=N'Year'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Holiday', N'COLUMN',N'Starttime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ڼ��տ�ʼʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Holiday', @level2type=N'COLUMN',@level2name=N'Starttime'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Holiday', N'COLUMN',N'Endtime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ڼ��ս���ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Holiday', @level2type=N'COLUMN',@level2name=N'Endtime'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Holiday', N'COLUMN',N'DuringDays'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ڼ��ճ�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Holiday', @level2type=N'COLUMN',@level2name=N'DuringDays'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Holiday', N'COLUMN',N'Comments'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Holiday', @level2type=N'COLUMN',@level2name=N'Comments'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Holiday', N'COLUMN',N'IsEnable'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Holiday', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO