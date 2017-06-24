GO

/****** Object:  View [dbo].[ViewInvestRecord]    Script Date: 2016/10/27 21:48:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[ViewInvestRecord]
AS
   
    SELECT  a.bid_records_id,a.targetid,
            a.investor_registerid, STUFF(mt.mobile,4,4,'****') as username,
            a.investment_amount ,a.invest_state ,
            a.invest_time ,a.ordstate
    FROM    [dbo].hx_Bid_records AS a WITH ( NOLOCK )
        	LEFT JOIN dbo.hx_member_table AS mt WITH ( NOLOCK )   
        	ON a.investor_registerid = mt.registerid;


GO


