USE [onchuangtou]
GO

/****** Object:  View [dbo].[ViewDetailsReport]    Script Date: 2016/12/7 17:13:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

 CREATE view [dbo].[ViewDetailsReport] 
  as
  select a.registerid,a.mobile,a.username,a.realname,a.registration_time,a.useridentity,
isnull(SUM(investment_amount),0) as 'InvestAllMoney',
isnull(SUM(e.investment_amount*e.current_period/12),0) as 'FoldAllMoney',
isnull((SUM(recharge_amount)),0)+isnull(SUM(money_order),0) as 'RechargeAllMoney',
(CASE WHEN b.investcount=1 THEN '首投'  WHEN b.investcount>1 THEN '复投' ELSE '未投资' END ) AS 'IsFirstInvest',
isnull((select sum(investment_amount) from hx_Bid_records where targetid in (select targetid from hx_borrowing_target where unit_day=1 and life_of_loan=1) and a.registerid=investor_registerid group by investor_registerid),0) as 'JanMoney',
isnull((select sum(investment_amount) from hx_Bid_records where targetid in (select targetid from hx_borrowing_target where unit_day=1 and life_of_loan=3) and a.registerid=investor_registerid group by investor_registerid),0) as 'MarMoney',
isnull((select sum(investment_amount) from hx_Bid_records where targetid in (select targetid from hx_borrowing_target where unit_day=1 and life_of_loan=6) and a.registerid=investor_registerid group by investor_registerid),0) as 'JunMoney'
 from hx_member_table a
Left join (select investor_registerid,count(*) as investcount from hx_Bid_records group by investor_registerid) b 
on a.registerid=b.investor_registerid
Left join hx_Recharge_history c on a.registerid=c.membertable_registerid
Left join hx_td_LLpay_Recharge d on a.registerid=d.UsrId
Left join (select investor_registerid,invest_time,invest_state,investment_amount,current_period from hx_Bid_records ) e on a.registerid=e.investor_registerid

group by a.registerid,a.mobile,a.username,a.realname,a.registration_time,b.investcount,a.useridentity



GO


