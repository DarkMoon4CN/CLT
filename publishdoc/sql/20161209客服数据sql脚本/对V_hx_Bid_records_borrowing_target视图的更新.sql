USE [onchuangtou]
GO

/****** Object:  View [dbo].[V_hx_Bid_records_borrowing_target]    Script Date: 2016/12/8 9:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[V_hx_Bid_records_borrowing_target]
AS
SELECT DISTINCT 
                dbo.hx_borrowing_target.borrowing_title, dbo.hx_Bid_records.bid_records_id, dbo.hx_Bid_records.borrower_registerid, 
                dbo.hx_Bid_records.targetid, dbo.hx_Bid_records.loan_number, dbo.hx_Bid_records.annual_interest_rate, 
                dbo.hx_Bid_records.current_period, dbo.hx_Bid_records.investment_amount, dbo.hx_Bid_records.value_date, 
                dbo.hx_Bid_records.investment_maturity, dbo.hx_Bid_records.invest_time, dbo.hx_Bid_records.invest_state, 
                dbo.hx_Bid_records.flow_return, dbo.hx_Bid_records.repayment_amount, dbo.hx_Bid_records.repayment_period, 
                dbo.hx_Bid_records.investor_registerid, dbo.hx_Bid_records.payment_status, dbo.hx_Bid_records.withoutinterest, 
                dbo.hx_Bid_records.haveinterest, dbo.hx_member_table.username, dbo.hx_member_table.realname, 
                dbo.hx_member_table.registerid, dbo.hx_borrowing_target.payment_options, dbo.hx_Bid_records.contractid, 
                dbo.hx_Bid_records.contractpath, dbo.hx_borrowing_target.life_of_loan, dbo.hx_borrowing_target.unit_day, 
                dbo.hx_member_table.mobile, dbo.hx_member_table.UsrCustId, dbo.hx_td_frozen.FrozenidAmount, 
                dbo.hx_td_frozen.FrozenidNo, dbo.hx_td_frozen.FrozenState, dbo.hx_td_frozen.FreezeTrxId, 
                hx_member_table_1.UsrCustId AS borrUsrCustid, dbo.hx_member_table.available_balance, 
                dbo.hx_borrowing_target.tender_state, dbo.hx_Bid_records.invitationcode, 
                hx_member_table_1.realname AS borrealname, hx_member_table_1.username AS borusername, 
                dbo.hx_Bid_records.OrdId, dbo.hx_borrowing_target.borrowing_balance, dbo.hx_Bid_records.ordstate, 
                dbo.hx_Bid_records.IsLoans, dbo.hx_borrowing_target.project_type_id, ISNULL(dbo.hx_Bid_records.JiaxiNum, 0.00) 
                AS JiaxiNum, ISNULL(dbo.hx_Bid_records.BonusAmt, 0.00) AS BonusAmt, dbo.hx_borrowing_target.release_date, 
                dbo.hx_borrowing_target.repayment_date, dbo.hx_member_table.registration_time, 
                dbo.hx_member_table.Channelsource, dbo.hx_member_table.Tid, dbo.hx_borrowing_target.service_charge, 
                dbo.hx_member_table.useridentity, dbo.hx_member_table.usertypes,dbo.hx_member_table.iD_number
FROM      dbo.hx_borrowing_target INNER JOIN
                dbo.hx_Bid_records ON dbo.hx_borrowing_target.targetid = dbo.hx_Bid_records.targetid INNER JOIN
                dbo.hx_member_table ON dbo.hx_Bid_records.investor_registerid = dbo.hx_member_table.registerid INNER JOIN
                dbo.hx_member_table AS hx_member_table_1 ON 
                dbo.hx_Bid_records.borrower_registerid = hx_member_table_1.registerid INNER JOIN
                dbo.hx_td_frozen ON dbo.hx_Bid_records.investor_registerid = dbo.hx_td_frozen.MBT_Registerid AND 
                dbo.hx_Bid_records.bid_records_id = dbo.hx_td_frozen.bid_records_id
WHERE   (dbo.hx_Bid_records.ordstate = 1)

GO


