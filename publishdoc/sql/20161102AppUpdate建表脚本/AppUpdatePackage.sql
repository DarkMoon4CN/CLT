
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hx_AppUpdatePackage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[hx_AppUpdatePackage](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](32) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Platform] [nvarchar](10) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Version] [nvarchar](12) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[UpdateLevel] [int] NOT NULL,
	[Description] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[DependCode] [nvarchar](32) COLLATE Chinese_PRC_CI_AS NULL,
	[Channel] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[ValideCode] [nvarchar](32) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[VirtualPath] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[Ways] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IsEnable] [int] NOT NULL,
	[DownloadCount] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_hx_AppUpdatePackage] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

ALTER AUTHORIZATION ON [dbo].[hx_AppUpdatePackage] TO  SCHEMA OWNER 
GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_hx_AppUpdatePackage_UpdateLevel]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[hx_AppUpdatePackage] ADD  CONSTRAINT [DF_hx_AppUpdatePackage_UpdateLevel]  DEFAULT ((5)) FOR [UpdateLevel]
END

GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_hx_AppUpdatePackage_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[hx_AppUpdatePackage] ADD  CONSTRAINT [DF_hx_AppUpdatePackage_Description]  DEFAULT (N'������°�') FOR [Description]
END

GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_hx_AppUpdatePackage_Ways]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[hx_AppUpdatePackage] ADD  CONSTRAINT [DF_hx_AppUpdatePackage_Ways]  DEFAULT (N'Http') FOR [Ways]
END

GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_hx_AppUpdatePackage_DownloadCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[hx_AppUpdatePackage] ADD  CONSTRAINT [DF_hx_AppUpdatePackage_DownloadCount]  DEFAULT ((0)) FOR [DownloadCount]
END

GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_hx_AppUpdatePackage_CreateTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[hx_AppUpdatePackage] ADD  CONSTRAINT [DF_hx_AppUpdatePackage_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
END

GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ψһ��ʶ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'id'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Code'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���°����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Code'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Platform'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ƽ̨����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Platform'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Version'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���°��汾��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Version'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'UpdateLevel'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������м���  1:�ӿڱ��-�ǳ�����;2:ҵ������-������;3:ҵ����-����;4:�׶���Bug�޸�-һ��;5:�ճ�����-һ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'UpdateLevel'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���°�������Ϣ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Description'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'DependCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���°��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'DependCode'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Channel'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Channel'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'ValideCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���°�У����.Ĭ��MD5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'ValideCode'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'VirtualPath'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������������������·��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'VirtualPath'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Ways'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���·�ʽ��Ĭ��Http' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Ways'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'IsEnable'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���°��Ƿ���á�0:�����ã�1:����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'DownloadCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Դ���ش���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'DownloadCount'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'CreateTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���°�����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
