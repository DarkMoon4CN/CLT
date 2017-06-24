
/****** Object:  Table [dbo].[hx_Channel]    Script Date: 2016/10/10 9:11:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[hx_Channel](
	[ChannelID] [int] IDENTITY(1,1) NOT NULL,
	[ChannelName] [nvarchar](100) NULL,
	[Invitedcode] [nvarchar](50) NOT NULL,
	[Creator] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[Status] [tinyint] NOT NULL,
 CONSTRAINT [PK_hx_Channel] PRIMARY KEY CLUSTERED 
(
	[ChannelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[hx_Channel] ADD  CONSTRAINT [DF_hx_Channel_Status]  DEFAULT ((0)) FOR [Status]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel', @level2type=N'COLUMN',@level2name=N'ChannelID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'渠道名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel', @level2type=N'COLUMN',@level2name=N'ChannelName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'渠道邀请码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel', @level2type=N'COLUMN',@level2name=N'Invitedcode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'渠道创建者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel', @level2type=N'COLUMN',@level2name=N'Creator'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'渠道创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'渠道修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态  0 不可用  1 可用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel', @level2type=N'COLUMN',@level2name=N'Status'
GO



/****** Object:  Table [dbo].[hx_Channel_AdminUser]    Script Date: 2016/10/10 9:12:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[hx_Channel_AdminUser](
	[AdminUserID] [int] IDENTITY(1,1) NOT NULL,
	[AdminUserName] [nvarchar](100) NOT NULL,
	[AdminUserPassword] [nvarchar](100) NULL,
	[TrueName] [nvarchar](100) NULL,
	[CreateTime] [datetime] NULL,
	[Status] [tinyint] NULL,
 CONSTRAINT [PK_hx_Channel_AdminUser] PRIMARY KEY CLUSTERED 
(
	[AdminUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel_AdminUser', @level2type=N'COLUMN',@level2name=N'AdminUserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户登录名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel_AdminUser', @level2type=N'COLUMN',@level2name=N'AdminUserName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel_AdminUser', @level2type=N'COLUMN',@level2name=N'AdminUserPassword'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'真实姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel_AdminUser', @level2type=N'COLUMN',@level2name=N'TrueName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态  0 不可用  1 可用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel_AdminUser', @level2type=N'COLUMN',@level2name=N'Status'
GO


/****** Object:  Table [dbo].[hx_Channel_AdminUser_Rel]    Script Date: 2016/10/10 9:12:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[hx_Channel_AdminUser_Rel](
	[ChannelID] [int] NOT NULL,
	[AdminUserID] [int] NOT NULL,
 CONSTRAINT [PK_hx_Channel_AdminUser_Rel] PRIMARY KEY CLUSTERED 
(
	[ChannelID] ASC,
	[AdminUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[hx_Channel_AdminUser_Rel]  WITH CHECK ADD  CONSTRAINT [FK_hx_Channel_AdminUser_Rel_hx_Channel] FOREIGN KEY([ChannelID])
REFERENCES [dbo].[hx_Channel] ([ChannelID])
GO

ALTER TABLE [dbo].[hx_Channel_AdminUser_Rel] CHECK CONSTRAINT [FK_hx_Channel_AdminUser_Rel_hx_Channel]
GO

ALTER TABLE [dbo].[hx_Channel_AdminUser_Rel]  WITH CHECK ADD  CONSTRAINT [FK_hx_Channel_AdminUser_Rel_hx_Channel_AdminUser] FOREIGN KEY([AdminUserID])
REFERENCES [dbo].[hx_Channel_AdminUser] ([AdminUserID])
GO

ALTER TABLE [dbo].[hx_Channel_AdminUser_Rel] CHECK CONSTRAINT [FK_hx_Channel_AdminUser_Rel_hx_Channel_AdminUser]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'渠道用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel_AdminUser_Rel', @level2type=N'COLUMN',@level2name=N'AdminUserID'
GO


/****** Object:  增加渠道字段 Script Date: 2016/10/10 9:11:39 ******/
ALTER TABLE  [dbo].[hx_member_table] ADD  channel_invitedcode nvarchar(20)
GO


















