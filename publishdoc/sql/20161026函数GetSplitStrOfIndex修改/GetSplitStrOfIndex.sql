IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSplitStrOfIndex]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'ALTER FUNCTION [dbo].[GetSplitStrOfIndex](@str [nvarchar](max), @split [varchar](10), @index [int])
RETURNS [nvarchar](max) WITH EXECUTE AS CALLER
AS 
BEGIN 
DECLARE @location INT 
DECLARE @start INT 
DECLARE @next INT 
DECLARE @seed INT 
SET @str=LTRIM(RTRIM(@str)) 
SET @start=1 
SET @next=0 
SET @seed=LEN(@split) 
SET @location=CHARINDEX(@split,@str) 
WHILE @location<>0 AND @index>@next 
	BEGIN 
	SET @start=@location+@seed 
	SET @location=CHARINDEX(@split,@str,@start) 
	SET @next=@next+1 
	END 
IF @location =0 SELECT @location =LEN(@str)+1  
RETURN SUBSTRING(@str,@start,@location-@start) 
END 
' 
END