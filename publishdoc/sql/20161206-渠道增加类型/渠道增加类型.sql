GO
alter table [hx_Channel] add [type] [nvarchar](50) ;
ALTER TABLE [dbo].[hx_Channel] ADD  CONSTRAINT [DF_hx_Channel_type]  DEFAULT (N'cps1') FOR [type];
GO
update [hx_Channel] set [type]='cps1' where [type] is null or [type]='';
GO
alter table [hx_Channel] alter column [type]  [nvarchar](50) NOT NULL;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'渠道类型：cps1 首投渠道，cps2 复投渠道，cpc 渠道' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'hx_Channel', @level2type=N'COLUMN',@level2name=N'type'
GO