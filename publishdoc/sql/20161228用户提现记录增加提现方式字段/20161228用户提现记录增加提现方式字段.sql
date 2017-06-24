alter table hx_td_UserCash add CashChl nvarchar(50)



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[V_UserCash_Bank]
AS
SELECT   dbo.hx_td_UserCash.UserCashId, dbo.hx_td_UserCash.registerid, dbo.hx_td_UserCash.UsrCustId, 
                dbo.hx_td_UserCash.TransAmt, dbo.hx_td_UserCash.FeeAmt, dbo.hx_td_UserCash.OrdId, 
                dbo.hx_td_UserCash.OrdIdTime, dbo.hx_td_UserCash.OrdIdState, dbo.hx_td_UserCash.OperTime, 
                dbo.hx_td_UserCash.Reason, dbo.hx_td_UserCash.Remarks, dbo.hx_td_UserCash.TransState, 
                dbo.hx_td_UserCash.OpenAcctId, dbo.hx_td_UserCash.OpenBankId, dbo.hx_member_table.username, 
                dbo.hx_td_Bank.BankName, dbo.hx_member_table.realname, dbo.hx_member_table.available_balance, 
                dbo.hx_member_table.usertypes, dbo.hx_td_UserCash.FeeObjFlag, dbo.hx_member_table.mobile, 
                dbo.hx_member_table.useridentity, dbo.hx_td_UserCash.CashChl
FROM      dbo.hx_td_UserCash INNER JOIN
                dbo.hx_member_table ON dbo.hx_td_UserCash.registerid = dbo.hx_member_table.registerid INNER JOIN
                dbo.hx_td_Bank ON dbo.hx_td_UserCash.OpenBankId = dbo.hx_td_Bank.OpenBankId

GO
