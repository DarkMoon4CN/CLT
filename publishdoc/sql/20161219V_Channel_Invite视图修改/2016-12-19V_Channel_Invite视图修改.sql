
/****** Object:  View [dbo].[V_Channel_Invite]    Script Date: 2016/12/19 18:11:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[V_Channel_Invite]
AS
SELECT   ISNULL(b.bid_records_id, 1) AS ID, b.OrdId, m.registerid, m.username, m.realname, c.ChannelID, c.ChannelName, 
                c.CreateTime, c.UpdateTime, c.Status, a.AdminUserName, m.registration_time, b.invest_time, b.ordstate, 
                m.channel_invitedcode, b.investment_amount, b.invest_state, bt.borrowing_title, CONVERT(VARCHAR(19), 
                bt.life_of_loan) + CASE WHEN bt.unit_day = 1 THEN 'ÔÂ' WHEN bt.unit_day = 3 THEN 'Ìì' END AS DeadLine, 
                (CASE WHEN bt.unit_day = 3 THEN life_of_loan WHEN bt.unit_day = 1 THEN life_of_loan * 30 END) 
                AS DeadLineNumber
FROM      dbo.hx_Bid_records AS b LEFT OUTER JOIN
                dbo.hx_member_table AS m ON m.registerid = b.investor_registerid RIGHT OUTER JOIN
                dbo.hx_Channel AS c ON m.channel_invitedcode = c.Invitedcode LEFT OUTER JOIN
                dbo.hx_Channel_AdminUser_Rel AS r ON r.ChannelID = c.ChannelID LEFT OUTER JOIN
                dbo.hx_Channel_AdminUser AS a ON a.AdminUserID = r.AdminUserID LEFT OUTER JOIN
                dbo.hx_borrowing_target AS bt ON b.targetid = bt.targetid INNER JOIN
                    (SELECT   MIN(bid_records_id) AS Expr1
                     FROM      dbo.hx_Bid_records
                     WHERE   (ordstate = 1)
                     GROUP BY investor_registerid) AS g ON b.bid_records_id = g.Expr1


GO


