SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[ViewInvestDetail]
AS
SELECT   br.bid_records_id AS recordId, br.targetid, bt.borrowing_title AS targetTitle, br.investor_registerid AS investMemberId, 
                mt.username AS investMemberName, bt.life_of_loan AS deadLine, bt.unit_day AS unitDay, 
                br.annual_interest_rate  AS rate, bt.borrowing_balance AS borrowTotalAmount, 
                bt.fundraising_amount AS fundTotalAmount, br.investment_amount AS investAmount, 
                bt.payment_options AS paymentOption, br.payment_status AS paymentStatus, br.value_date AS rateBeginOn, 
                br.investment_maturity AS investMaturity, bt.repayment_date AS paymentDate, br.invest_time AS createdOn, 
                b.investNumber, bt.guarantee_way_id, bt.month_payment_date,br.jiaxiNum,br.BonusAmt
FROM      dbo.hx_Bid_records AS br WITH (NOLOCK) LEFT OUTER JOIN
                dbo.hx_member_table AS mt WITH (NOLOCK) ON br.investor_registerid = mt.registerid LEFT OUTER JOIN
                dbo.hx_borrowing_target AS bt WITH (NOLOCK) ON bt.targetid = br.targetid LEFT OUTER JOIN
                    (SELECT   targetid, COUNT(DISTINCT investor_registerid) AS investNumber
                     FROM      dbo.hx_Bid_records WITH (NOLOCK)
                     GROUP BY targetid) AS b ON br.targetid = b.targetid

GO


