--后台抽奖列表  视图增加活动名称列


/****** Object:  View [dbo].[V_LuckDrawData]    Script Date: 2016/12/9 19:48:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER VIEW [dbo].[V_LuckDrawData]
AS
SELECT     dbo.LuckDrawRecord.Ldre_ID AS ID, dbo.LuckDrawRecord.Ldre_AwardName AS AwardName, dbo.LuckDrawRecord.Ldre_CreatTime AS AwardTime, 
                      dbo.LuckDrawRecord.Ldre_AwardType AS AwardType, dbo.hx_member_table.username, dbo.LuckDrawRecord.Ldre_AwardID AS AwardID,LuckDrawRecord.Ldre_ActivityName AS ActivityName
FROM         dbo.LuckDrawRecord LEFT OUTER JOIN
                      dbo.hx_member_table ON dbo.LuckDrawRecord.Ldre_UserID = dbo.hx_member_table.registerid




GO


