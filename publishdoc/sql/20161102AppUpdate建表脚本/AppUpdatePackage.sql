
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
ALTER TABLE [dbo].[hx_AppUpdatePackage] ADD  CONSTRAINT [DF_hx_AppUpdatePackage_Description]  DEFAULT (N'常规更新包') FOR [Description]
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'id'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Code'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新包编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Code'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Platform'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平台类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Platform'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Version'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新包版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Version'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'UpdateLevel'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新迫切级别。  1:接口变更-非常严重;2:业务增加-较严重;3:业务变更-严重;4:阶段性Bug修复-一般;5:日常更新-一般' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'UpdateLevel'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新包描述信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Description'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'DependCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新包依赖编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'DependCode'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Channel'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'针对渠道' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Channel'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'ValideCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新包校验码.默认MD5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'ValideCode'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'VirtualPath'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务器上相对虚拟完整路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'VirtualPath'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'Ways'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新方式。默认Http' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'Ways'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'IsEnable'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新包是否可用。0:不可用，1:可用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'DownloadCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源下载次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'DownloadCount'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'hx_AppUpdatePackage', N'COLUMN',N'CreateTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新包发布时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_AppUpdatePackage', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
