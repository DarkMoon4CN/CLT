ALTER VIEW [dbo].[ViewTargetWithRecords]
AS
SELECT   a.targetid, a.borrowing_title, a.indexorder, a.borrower_registerid, mt.username, a.tender_state, a.repayment_date, 
                a.month_payment_date, a.project_type_id, a.borrowing_balance, a.annual_interest_rate, a.life_of_loan, a.start_time, 
                a.release_date, a.unit_day, a.payment_options, a.H_Repayment_Amt, a.end_time, b.invest_num, a.fundraising_amount, 
                a.minimum, a.maxmum, det.createtime AS CreatedOn, a.sys_time
FROM      dbo.hx_borrowing_target AS a WITH (NOLOCK) LEFT OUTER JOIN
                dbo.hx_borrowing_target_detailed AS det WITH (NOLOCK) ON det.targetid = a.targetid LEFT OUTER JOIN
                    (SELECT   targetid, COUNT(bid_records_id) AS invest_num, MAX(invest_time) AS end_time
                     FROM      dbo.hx_Bid_records WITH (NOLOCK)
                     GROUP BY targetid) AS b ON a.targetid = b.targetid LEFT OUTER JOIN
                dbo.hx_member_table AS mt ON mt.registerid = det.borrower_registerid INNER JOIN
                dbo.hx_bonding_company AS x ON a.companyid = x.companyid INNER JOIN
                dbo.guarantee_way AS xx ON a.guarantee_way_id = xx.guarantee_way_id INNER JOIN
                dbo.hx_Project_type AS xxx ON a.project_type_id = xxx.project_type_id
WHERE   (a.isDel = 0)

GO