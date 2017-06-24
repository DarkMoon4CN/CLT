

--select * from [hx_td_Bank] where BankName in ('工商银行','农业银行','招商银行','北京银行','中国银行','民生银行','渤海银行','光大银行','兴业银行','浙商银行','广发银行','华夏银行','平安银行','上海农村商业银行')
update [hx_td_Bank] set isGren = 1 where BankName in ('工商银行','农业银行','招商银行','北京银行','中国银行','民生银行','渤海银行','光大银行','兴业银行','浙商银行','广发银行','华夏银行','平安银行','上海农村商业银行')
update [hx_td_Bank] set isGren = 0 where BankName not in ('工商银行','农业银行','招商银行','北京银行','中国银行','民生银行','渤海银行','光大银行','兴业银行','浙商银行','广发银行','华夏银行','平安银行','上海农村商业银行')



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[V_UsrBindCardBank]
AS
SELECT   dbo.hx_UsrBindCardC.UsrBindCardID, dbo.hx_UsrBindCardC.UsrCustId, dbo.hx_UsrBindCardC.OpenAcctId, 
                dbo.hx_UsrBindCardC.OpenBankId, dbo.hx_UsrBindCardC.defCard, dbo.hx_td_Bank.BankName, 
                dbo.hx_td_Bank.OpenBankId AS Expr1, dbo.hx_member_table.username, dbo.hx_member_table.realname, 
                dbo.hx_member_table.registerid, dbo.hx_td_Bank.CardImage, dbo.hx_td_Bank.Isquick, dbo.hx_td_Bank.Isordinary, 
                dbo.hx_td_Bank.CardImageNew, dbo.hx_UsrBindCardC.BindCardType, dbo.hx_td_Bank.isGren
FROM      dbo.hx_UsrBindCardC INNER JOIN
                dbo.hx_td_Bank ON dbo.hx_UsrBindCardC.OpenBankId = dbo.hx_td_Bank.OpenBankId INNER JOIN
                dbo.hx_member_table ON dbo.hx_UsrBindCardC.UsrCustId = dbo.hx_member_table.UsrCustId

GO

