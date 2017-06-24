///////////////////////增加 加息卷的  发放时间
ALTER VIEW [dbo].[V_OwnBonuses]
AS
SELECT   dbo.hx_UserAct.UserAct, dbo.hx_UserAct.registerid, dbo.hx_UserAct.RewTypeID, dbo.hx_UserAct.Amt, 
                dbo.hx_UserAct.Uselower, dbo.hx_UserAct.UseState, dbo.hx_ActivityTable.ActName, dbo.hx_ActivityTable.ActStarttime, 
                dbo.hx_ActivityTable.ActEndtime, dbo.hx_UserAct.AmtEndtime, dbo.hx_ActivityType.ActName AS TypeName, 
                dbo.hx_UserAct.Createtime
FROM      dbo.hx_UserAct INNER JOIN
                dbo.hx_ActivityType ON dbo.hx_UserAct.ActTypeId = dbo.hx_ActivityType.ActTypeId LEFT OUTER JOIN
                dbo.hx_ActivityTable ON dbo.hx_ActivityType.ActTypeId = dbo.hx_ActivityTable.ActTypeId AND 
                dbo.hx_UserAct.ActID = dbo.hx_ActivityTable.ActID

GO
