IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTartgetLoanMonth]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTartgetLoanMonth](@lifeOfLoan [int], @unitDay [int])
RETURNS [int] WITH EXECUTE AS CALLER
AS 
BEGIN
	declare @modVal int;
	if(@unitDay=1)--这是月单位
		return @lifeOfLoan;
	if(@unitDay=3)--这是天单位
		return @lifeOfLoan/30;
	return 0;
END
' 
END