IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IsBonusUseable]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[IsBonusUseable](@targetMonth [int], @useLifeLoan [nvarchar](50))
RETURNS [int] WITH EXECUTE AS CALLER
AS 
BEGIN
	declare @length int;
	declare @position int;
	declare @lessMonth int;
	declare @mostMonth int;
	set @length=len(@useLifeLoan);
	if(@length=0)
		return 1;
	if(@length>0)
	begin
		set @position=charindex(''-'',@useLifeLoan);
		set @lessMonth=convert(int,substring(@useLifeLoan,1,@position-1));
		set @mostMonth=convert(int,substring(@useLifeLoan,@position+1,1));
		if(@targetMonth>=@lessMonth and @targetMonth<=@mostMonth)
			return 1;
	end
	RETURN 0;
END
' 
END