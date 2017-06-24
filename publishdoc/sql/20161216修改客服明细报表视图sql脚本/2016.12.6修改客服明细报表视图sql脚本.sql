
/****** Object:  View [dbo].[ViewDetailsReport]    Script Date: 2016/12/16 11:41:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


 ALTER view [dbo].[ViewDetailsReport] 
  as
SELECT a.registerid, a.mobile, a.username, a.realname, a.registration_time, a.useridentity,
sum(e.investment_amount) AS InvestAllMoney,sum(e.FoldAllMoney) AS FoldAllMoney, 
ISNULL(SUM(c.recharge_amount), 0) + ISNULL(SUM(d.money_order), 0) AS RechargeAllMoney, 
(CASE WHEN b.investcount = 1 THEN '首投' WHEN b.investcount > 1 THEN '复投' ELSE '未投资' END) AS IsFirstInvest, 
ISNULL
                    ((SELECT  SUM(investment_amount) AS Expr1 FROM  dbo.hx_Bid_records
                      WHERE  (targetid IN
                                          (SELECT  targetid FROM  dbo.hx_borrowing_target WHERE (unit_day = 1) AND (life_of_loan = 1))) AND (a.registerid = investor_registerid)GROUP BY investor_registerid), 0) AS JanMoney, ISNULL
                    ((SELECT   SUM(investment_amount) AS Expr1 FROM  dbo.hx_Bid_records
                      WHERE   (targetid IN
                                          (SELECT targetid FROM  dbo.hx_borrowing_target
                                           WHERE   (unit_day = 1) AND (life_of_loan = 3))) AND (a.registerid = investor_registerid)
                      GROUP BY investor_registerid), 0) AS MarMoney, 
					  ISNULL((SELECT  SUM(investment_amount) AS Expr1 FROM  dbo.hx_Bid_records
                      WHERE  (targetid IN
                                          (SELECT  targetid FROM dbo.hx_borrowing_target
                                           WHERE (unit_day = 1) AND (life_of_loan = 6))) AND (a.registerid = investor_registerid)
                      GROUP BY investor_registerid), 0) AS JunMoney FROM dbo.hx_member_table AS a LEFT OUTER JOIN
                    (SELECT  investor_registerid, COUNT(*) AS investcount
                     FROM  dbo.hx_Bid_records
                     GROUP BY investor_registerid) AS b ON a.registerid = b.investor_registerid LEFT OUTER JOIN
                ((select sum(recharge_amount) recharge_amount,membertable_registerid from hx_Recharge_history group by membertable_registerid)) AS c ON a.registerid = c.membertable_registerid LEFT OUTER JOIN
                (select UsrId,sum(money_order) money_order from hx_td_LLpay_Recharge group by UsrId) AS d ON a.registerid = d.UsrId LEFT OUTER JOIN
(SELECT  investor_registerid, sum(investment_amount) investment_amount, SUM(ISNull(investment_amount,0) * ISNULL(current_period,0) / 12) FoldAllMoney
 FROM dbo.hx_Bid_records group by investor_registerid) AS e ON a.registerid = e.investor_registerid
GROUP BY a.registerid, a.mobile, a.username, a.realname, a.registration_time, b.investcount, a.useridentity




GO


