USE [onchuangtou]
GO

/****** Object:  StoredProcedure [dbo].[prSubmitTender]    Script Date: 2016/10/31 14:24:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[prSubmitTender]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[prSubmitTender]
	@usrId [int],
	@targetId [int],
	@investAmount [decimal](18, 2),
	@ids [nvarchar](500),
	@cPeriod [int],
	@code [nvarchar](500),
	@ordCode [decimal](20, 0),
	@withoutInterest [decimal](18, 2),
	@frozenidNo [nvarchar](500),
	@frozenidAmount [decimal](18, 2),
	@incomeStatementStr [nvarchar](4000)
WITH EXECUTE AS CALLER
AS
BEGIN
	DECLARE @returnValue INT;
    DECLARE @aor decimal(18, 2);
	DECLARE @JiaxiNum DECIMAL;--��Ϣȯʹ��ֵ
	DECLARE @BonusAmt DECIMAL;--���ʹ���ܺ�
	DECLARE @RewTypeID int;--����ȯ����
	DECLARE @Amt DECIMAL;--����ȯ���
	SET @aor=0;
	SET @JiaxiNum=0;
	SET @BonusAmt=0;
	SET @RewTypeID=0;
	SET @Amt=0;
BEGIN TRAN submitTender;    --��ʼ����
	DECLARE @tran_error INT;
	SET @tran_error = 0;
 	--BEGIN TRY 
	DECLARE @targetBalance DECIMAL;--ʣ����
	DECLARE @maxV DECIMAL;--���Ͷ��
	SELECT @targetBalance = ( borrowing_balance-fundraising_amount) FROM dbo.hx_borrowing_target WHERE targetid=@targetId;
	SELECT @maxV = maxmum FROM dbo.hx_borrowing_target WHERE targetid=@targetId;
	PRINT  @targetBalance;
	PRINT  @maxV;
	PRINT  @investAmount;
	IF(@investAmount>@maxV AND @maxV<>0)
	BEGIN
		PRINT @maxV;
		PRINT ''Ͷ�ʽ��ܳ�����߿�Ͷ�޶�'';
		SET @returnValue= -100;
	END
	ELSE
	BEGIN
		IF(@investAmount>@targetBalance)
		BEGIN
			PRINT ''Ͷ�ʽ�����Ͷ���'';
			SET @returnValue= -200;
		END
		ELSE
		BEGIN--�ж��û����
			DECLARE @currentTime DATETIME;
			--SET @currentTime=GETDATE();
			SET @currentTime=convert(datetime, convert(varchar(10),getdate(),120));
			print ''@currentTime'';
			print @currentTime;
			DECLARE @b DECIMAL;
			DECLARE @str VARCHAR(MAX);
			DECLARE @next INT;
			DECLARE @bonusId INT;
			SET @str=@ids;
			SET @next=0;
			IF(@str<>'''')
			BEGIN
				SET @BonusAmt=0;
				SET @JiaxiNum=0;
				SET @Amt=0;
				WHILE @next<dbo.GetSplitLength(@str,'','') 
				BEGIN
					DECLARE @lmt DECIMAL;
					DECLARE @temm DECIMAL;
					SET @bonusId=CAST(dbo.GetSplitStrOfIndex(@str,'','',@next) AS INT); 
					PRINT ''*****----'';
					print @bonusId;
					If(EXISTS(SELECT COUNT(1) FROM dbo.hx_useract WHERE registerid=@usrId AND useract=@bonusId))
					begin
					SET @RewTypeID=0;
					SELECT @lmt=uselower,@RewTypeID=RewTypeID,@Amt=Amt FROM dbo.hx_useract WHERE registerid=@usrId AND useract=@bonusId;
						PRINT @lmt;
						IF(@lmt>@investAmount)
						BEGIN
							PRINT ''�����Ż�ȯ���ߺ��������'';
							ROLLBACK TRAN;
							RETURN -500;
							BREAK;
						END
						SET @bonusId=CAST(dbo.GetSplitStrOfIndex(@str,'','',@next) AS INT);						
						SELECT @temm=amt FROM dbo.hx_useract WHERE registerid=@usrId AND useract=@bonusId And RewTypeID=2;
						if(@temm <> 0)
						   SET @aor=@aor+@temm;
						SET @next=@next+1;
						PRINT ''*****��'';
						PRINT @bonusId;
						PRINT @next;
						if(@Amt is null)
							SET @Amt=0;
						if(@RewTypeID=2)--���
							SET @BonusAmt=@BonusAmt+@Amt;
						if(@RewTypeID=3)--��Ϣ��
							SET @JiaxiNum=@JiaxiNum+@Amt;
					end
				END
				PRINT @str;
				PRINT ''-------------'';
				print @next;
				print dbo.GetSplitLength(@str,'','');
				IF(@next<>dbo.GetSplitLength(@str,'',''))
				BEGIN
					PRINT ''�Ż�ȯ���������⣬���ݻع�'';
					ROLLBACK TRAN;
					RETURN -600;
				END
			END
			
			PRINT @aor;
			SELECT @b=available_balance FROM dbo.hx_member_table WHERE registerid=@usrId;
			IF(@investAmount>@aor+@b)
			BEGIN
				PRINT ''�ʻ����㣬���ֵ''
				SET @returnValue= -300;
			END
			ELSE
			BEGIN--�жϵ�ǰ�û� ��ǰ����Ч���¼
				DECLARE @c int;
				SELECT @c=(COALESCE(count(ordstate),0)) from hx_Bid_records where targetid=@targetId and investor_registerid=@usrId and ordstate=0;
				IF(@c>=3)
				BEGIN
					PRINT ''�����Ͷ��δ�����ѳ�������'';
					SET @returnValue= -400;
				END
				ELSE
				BEGIN
					DECLARE @brId INT;--borrower_registerid �����id
					DECLARE @lNo DECIMAL;--loan_number �����
					DECLARE @air INT;--annual_interest_rate �껯����
					DECLARE @borrowCusId NVARCHAR(500);
					DECLARE @InCusId NVARCHAR(500);
					--DECLARE @imy INT--investment_maturity Ͷ�ʵ��ڽ�������
					DECLARE @repaymentDate DATETIME--repayment_date
					SELECT @brId=borrower_registerid,@lNo=loan_number,@air=annual_interest_rate, 
						@repaymentDate=repayment_date 
						FROM dbo.hx_borrowing_target
						WHERE targetid=@targetId;
					SELECT @borrowCusId=UsrCustId FROM dbo.hx_member_table WHERE registerid=@brId;
					SELECT @InCusId=UsrCustId FROM dbo.hx_member_table WHERE registerid=@usrId;

					INSERT into hx_Bid_records(borrower_registerid,targetid,
							loan_number,annual_interest_rate,current_period,
							investment_amount,value_date,investment_maturity,
							invest_time,invest_state,flow_return,repayment_amount,
							repayment_period,investor_registerid,payment_status,
							withoutinterest,invitationcode,OrdId,JiaxiNum,BonusAmt
					)
					values (
						@brId,@targetid,@lNo,@air,@cPeriod,@investAmount,@currentTime
						,@repaymentDate,getdate(),1,1,0,@repaymentDate,@usrId,0,
						@withoutInterest,@code,@ordCode,@JiaxiNum,@BonusAmt
					)
					DECLARE @recordId INT;
					SELECT @recordId=@@IDENTITY;
					PRINT ''Ͷ�ʼ�¼��������ӳɹ�''
					PRINT @recordId;

					--�����Ż�ȯ���ݿ�ʼ
					IF(@ids<>'''')
					BEGIN
						print ''�����Ż�ȯ���ݿ�ʼ''
						set @str=@ids;
						set @next=0;
						while @next<dbo.GetSplitLength(@str,'','')
						BEGIN
							print ''******''
							PRINT @str;
							SET @bonusId=CAST(dbo.GetSplitStrOfIndex(@str,'','',@next) AS INT);

							PRINT ''-------------------------------'';
							PRINT @bonusId;
							PRINT ''-------------------------------'';
							IF(EXISTS(SELECT COUNT(1) FROM dbo.hx_UserAct WHERE registerid=@usrId AND UserAct=@bonusId))
							BEGIN
							--�����Ż�ȯ
								update hx_UserAct set UseState=3,AmtProid=@recordId where UserAct=@bonusId and registerid=@usrId
             				END
							ELSE
							BEGIN
							 	ROLLBACK TRAN;
								PRINT ''ִ�г����ع�����'';
								BREAK
							END
							SET @next=@next+1;
						END
					END
					--�����Ż�ȯ���ݽ���						

					--�Ż�ȯ���ݸ��¿�ʼ
					IF(@code<>'''')
					BEGIN
                    	print ''�Ż�ȯ���ݸ��¿�ʼ''
						update hx_td_Userinvitation set InvitesStates=2 where  invcode=@code and invpersonid=@usrId and InvitesStates=0;
						
        			END
					--�Ż�ȯ���ݸ��½���

					--ѭ���� Ͷ������Ϣ������������
					set @str=@incomeStatementStr;
					PRINT @str;
					set @next=0;
					while @next<=dbo.GetSplitLength(@str,''|'')
					BEGIN
						print ''Ͷ������Ϣ������������''
						DECLARE @xxx NVARCHAR(3000);
						SET @xxx=dbo.GetSplitStrOfIndex(@str,''|'',@next)
						IF(@xxx<>'''')
						BEGIN
						 		PRINT @xxx ;
					 		insert into hx_income_statement(
								targetid,bid_records_id,loan_number
								,borrower_registerid,annual_revenue,investment_amount
								,invest_time,current_investment_period,value_date
								,interest_payment_date,repayment_amount,investor_registerid
								,payment_status,interestpayment,OutCustId,InCustId
								,BidOrderid,Principal,TotalInstallments,interestDay
							)
					  		values (
								@targetId,@recordId,@lNo
								,@brId,@air,@investAmount,@currentTime
								,CAST(dbo.GetSplitStrOfIndex(@xxx ,'','',0) AS INT)
								,dbo.GetSplitStrOfIndex(@xxx ,'','',1)
								,dbo.GetSplitStrOfIndex(@xxx ,'','',2)
								,CONVERT(DECIMAL(20,2),dbo.GetSplitStrOfIndex(@xxx ,'','',3))
								,@usrId
								,0
								,CAST(dbo.GetSplitStrOfIndex(@xxx ,'','',4) AS DECIMAL(38,2))
								,@borrowCusId,@InCusId,@ordCode
								,CAST(dbo.GetSplitStrOfIndex(@xxx ,'','',5) AS DECIMAL(38,2))
								,CAST(dbo.GetSplitStrOfIndex(@xxx ,'','',6) AS INT)
								,CAST(dbo.GetSplitStrOfIndex(@xxx ,'','',7) AS INT)
						  	)
						END
						PRINT ''hx_income_statement ��������'';
						PRINT @next;
						set @next=@next+1;
					END
					--�����¼д��
					insert into hx_td_frozen
					(
					  	MBT_Registerid,FrozenidAmount,FrozenidNo,FrozenState,FrozenDate,
					  	targetid,UsrCustId,bid_records_id
					)
					values 
					(
						@usrId,@frozenidAmount-@aor ,@frozenidNo,0,
						@currentTime,@targetid,@InCusId,@recordId
					);
					PRINT ''������¼����'';
					SET @returnValue=200;
					SET @tran_error = @tran_error + @@ERROR;
				END
			END
		END
	END
	--END TRY
	--BEGIN CATCH
	--	PRINT ''''�����쳣''''+ error_message()
	--	SET @tran_error = @tran_error + 1
	--END CATCH
	IF(@tran_error > 0)
	BEGIN
		--ִ�г����ع�����
		ROLLBACK TRAN;
		PRINT ''ִ�г����ع�����'';
	END
	ELSE
	BEGIN
		--û���쳣���ύ����
		COMMIT TRAN;
		PRINT ''û���쳣���ύ����'';
	END
	RETURN @returnValue;
END' 
END
GO


