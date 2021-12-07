
IF NOT EXISTS(SELECT * FROM sys.objects where name='GetInvoiceNumber')
BEGIN
	CREATE SEQUENCE dbo.GetInvoiceNumber
		AS INT
		START WITH 1
		INCREMENT BY 1
		MINVALUE 1 
		MAXVALUE 999
		CYCLE
		CACHE 10;
END
GO


-- -- 01 -- -- Update Medical Payment

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Update_MedicalPayment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Sp_Update_MedicalPayment]
GO
/*

Begin Tran
select * from MedicalPayments
		Exec Sp_Update_MedicalPayment 5,'MedicalTest', 'Cash', 1, 990, 900, 30
		select * from MedicalPayments
Rollback Tran

*/
CREATE PROCEDURE [dbo].[Sp_Update_MedicalPayment]
	@TransactionId BIGINT,
	@TransactionType NVARCHAR(50),
	@PaymentType NVARCHAR(50),
	@PatientId BIGINT,
	@TotalAmount DECIMAL,
	@PaidAmount DECIMAL,
	@DueAmount DECIMAL
	AS
BEGIN
	SET NOCOUNT ON;
	declare @RecordID BIGINT
	set @RecordID = 0
	BEGIN TRY  

	--select * from MedicalPaymentDetails

	BEGIN TRANSACTION t
		IF Exists(select * from MedicalPayments where TransactionId=@TransactionId and TransactionType = @TransactionType)
		Begin
		print 'update'
			update MedicalPayments set TotalAmount = @TotalAmount, DueAmount = @DueAmount, ModifiedDate=getdate() where TransactionId=@TransactionId
			
			select @RecordID=Id from MedicalPayments where TransactionId=@TransactionId

			insert into MedicalPaymentDetails(
			MedicalPaymentId, PaymentType,Amount,IsActive,IsDeleted,IsLocked,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate
			)
			values(
			@RecordID, @PaymentType, @PaidAmount, 1,0,0,1,getdate(),1,GETDATE()
			)
		End
		Else
		Begin
		print 'insert'
			insert into MedicalPayments(
			TransactionId,TransactionType,PatientId,InvoiceNo,InvoiceDate,DateOfInvoice,TotalAmount, DueAmount,IsActive,IsDeleted,IsLocked,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate
			)
			values(
			@TransactionId, @TransactionType, @PatientId, CONVERT(VARCHAR(4), GETDATE(), 12) + RIGHT('0000' + CAST( NEXT VALUE FOR dbo.GetInvoiceNumber AS VARCHAR(3)),4), 
			getdate(),CONVERT(VARCHAR(10), GETDATE(), 101),@TotalAmount,@DueAmount,1,0,0,1,getdate(),1,GETDATE()
			)
			SET @RecordID = SCOPE_IDENTITY()

			insert into MedicalPaymentDetails(
			MedicalPaymentId, PaymentType,Amount,IsActive,IsDeleted,IsLocked,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate
			)
			values(
			@RecordID, @PaymentType, @PaidAmount, 1,0,0,1,getdate(),1,GETDATE()
			)
		End

	COMMIT TRANSACTION t
	END TRY
	BEGIN CATCH
		  ROLLBACK TRANSACTION t
	END CATCH;
END
GO












IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounting_Update_UpdateAccountCurrentBalance_SanchaySamity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Accounting_Update_UpdateAccountCurrentBalance_SanchaySamity]
GO
/*

Begin Tran
SELECT Id,* FROM AccountHeads WHERE AccountNo='3-2-0010'
SELECT Id,* FROM AccountHeads WHERE AccountNo='2-1-0004'
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-1-0001'
		Exec Accounting_Update_UpdateAccountCurrentBalance_SanchaySamity 'Sanchay', 50, 0
SELECT Id,* FROM AccountHeads WHERE AccountNo='3-2-0010'
SELECT Id,* FROM AccountHeads WHERE AccountNo='2-1-0004'
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-1-0001'
Rollback Tran

*/
CREATE PROCEDURE [dbo].[Accounting_Update_UpdateAccountCurrentBalance_SanchaySamity]
	@TransactionType NVARCHAR(15),
	@Amount DECIMAL
	--@RecordCount INT OUTPUT
	AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY  
		
	DECLARE @CurrentBalance DECIMAL(18,2)
	DECLARE @OpeningBalance DECIMAL(18,2)
	DECLARE @TotalCurrentBalance DECIMAL(18,2)
	DECLARE @ActualCurrentBalance DECIMAL(18,2)

	DECLARE @TotalIncome DECIMAL(18,2)
	DECLARE @TotalExpense DECIMAL(18,2)

	DECLARE @TodayCurrentBalance DECIMAL(18,2)

	DECLARE @TenPercentAmount DECIMAL(18,2)
	BEGIN TRANSACTION t

		IF @TransactionType = 'Sanchay'
		BEGIN
			-- Income ++	Income	Sanchay Income	3-2-0010'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads where AccountNo='3-2-0010'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() where AccountNo='3-2-0010'
			
			-- Liability ++   Liability	Payable	Sanchay Principal Payable	2-1-0004'
			select @CurrentBalance = CurrentBalance, @OpeningBalance = OpeningBalance from AccountHeads where AccountNo='2-1-0004'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() where AccountNo='2-1-0004'

			-- Liability ++   Liability	Payable	Sanchay Interest Payable	 2-1-0007'
			set @TenPercentAmount = isnull(@Amount,0) * 0.1
			select @CurrentBalance = CurrentBalance, @OpeningBalance = OpeningBalance from AccountHeads where AccountNo='2-1-0007'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + @TenPercentAmount
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() where AccountNo='2-1-0007'

		END
		ELSE IF @TransactionType = 'LoanPrin'
		BEGIN
			
			-- Income ++	Loan Principal Receivable Income	3-2-0008'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0008'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0008'

			-- Asset Receivable --	Receivable	Loan Principle Receivable	1-3-0002'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='1-3-0002'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='1-3-0002'

		END
		ELSE IF @TransactionType = 'LoanInt'
		BEGIN
			
			-- Income ++	Income	Loan Interest Receivable Income	3-2-0009'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0009'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0009'

			-- Asset Receivable -- Receivable	Loan Interest Receivable	1-3-0003'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='1-3-0003'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='1-3-0003'

		END
		ELSE IF @TransactionType = 'OwnerEqt'
		BEGIN
			
			-- Income ++	Owners Equity	3-2-0011'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0011'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0011'

			-- Liability ++	Owners Equity	2-3-0019'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='2-3-0019'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='2-3-0019'

		END
		ELSE IF @TransactionType = 'MemBook'
		BEGIN
			
			-- Income ++	Member Book	3-1-0012'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-1-0012'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-1-0012'

		END
		ELSE IF @TransactionType = 'BimaInc'
		BEGIN
			
			-- Income ++	Bima Income	3-2-0013'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0013'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0013'

			-- Liability ++	Bima Payable	2-1-0019'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='2-1-0019'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='2-1-0019'

		END
		ELSE IF @TransactionType = 'OtherInc'
		BEGIN
			
			-- Income ++	Other	3-2-0014'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0014'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0014'

		END
		ELSE IF @TransactionType = 'ExUtility'
		BEGIN
			
			-- Expense ++	Utility Expense	4-1-0015'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='4-1-0015'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='4-1-0015'

		END
		
		ELSE IF @TransactionType = 'ExSnPrinCsBk'
		BEGIN
			
			-- Expense ++
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='4-1-0016'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='4-1-0016'

			-- Liability ++
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='2-1-0004'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='2-1-0004'

		END
		ELSE IF @TransactionType = 'ExSnIntCsBk'
		BEGIN
			
			-- Expense ++
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='4-1-0017'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='4-1-0017'

			-- Liability ++
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='2-1-0007'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='2-1-0007'

		END
		
		ELSE IF @TransactionType = 'ExLnDisburse'
		BEGIN
			
			-- Expense ++ Expense	Loan Disburse
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='4-1-0018'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='4-1-0018'

		END
		ELSE IF @TransactionType = 'LoanPrinRec'
		BEGIN
			
			-- Asset ++	Receivable	Loan Principle Receivable
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='1-3-0002'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='1-3-0002'

		END
		ELSE IF @TransactionType = 'LoanIntRec'
		BEGIN
			
			-- Asset ++	Receivable	Loan Interest Receivable
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='1-3-0003'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='1-3-0003'
			
		END
		ELSE IF @TransactionType = 'SecurityAdvance'
		BEGIN
			
			-- Asset ++	Receivable	Security Advance Receivable
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='1-3-0019'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='1-3-0019'
			
		END

		Exec Accounting_Update_UpdateAccountPettyCash_SanchaySamity

	COMMIT TRANSACTION t
	END TRY
	BEGIN CATCH
		  ROLLBACK TRANSACTION t
	END CATCH;
END
GO


-------------------------------------------------------------------------------------------------------------------------------------------------


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounting_Delete_UpdateAccountCurrentBalance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Accounting_Delete_UpdateAccountCurrentBalance]
GO
/*
Begin Tran
SELECT Id,* FROM AccountHeads WHERE AccountNo='3-2-0008'
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-3-0002'
		Exec Accounting_Delete_UpdateAccountCurrentBalance 'LoanPrin', 2946
SELECT Id,* FROM AccountHeads WHERE AccountNo='3-2-0008'
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-3-0002'

*/
CREATE PROCEDURE [dbo].[Accounting_Delete_UpdateAccountCurrentBalance]
	@TransactionType NVARCHAR(15),
	@Amount DECIMAL
	AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY  

	
	DECLARE @CurrentBalance DECIMAL(18,2)
	DECLARE @OpeningBalance DECIMAL(18,2)
	DECLARE @TotalCurrentBalance DECIMAL(18,2)
	DECLARE @ActualCurrentBalance DECIMAL(18,2)

	DECLARE @TotalIncome DECIMAL(18,2)
	DECLARE @TotalExpense DECIMAL(18,2)

	DECLARE @TenPercentAmount DECIMAL(18,2)
	BEGIN TRANSACTION t

		IF @TransactionType = 'Sanchay'
		BEGIN
			
			-- Income ++	Income	Sanchay Income	3-2-0010'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads where AccountNo='3-2-0010'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() where AccountNo='3-2-0010'
			
			-- Liability ++   Liability	Payable	Sanchay Principal Payable	2-1-0004'
			select @CurrentBalance = CurrentBalance, @OpeningBalance = OpeningBalance from AccountHeads where AccountNo='2-1-0004'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() where AccountNo='2-1-0004'

			-- Liability ++   Liability	Payable	Sanchay Interest Payable	 2-1-0007'
			set @TenPercentAmount = isnull(@Amount,0) * 0.1
			select @CurrentBalance = CurrentBalance, @OpeningBalance = OpeningBalance from AccountHeads where AccountNo='2-1-0007'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - @TenPercentAmount
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() where AccountNo='2-1-0007'

		END
		ELSE IF @TransactionType = 'LoanPrin'
		BEGIN
			
			-- Income ++	Loan Principal Receivable Income	3-2-0008'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0008'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0008'

			-- Asset Receivable --	Receivable	Loan Principle Receivable	1-3-0002'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='1-3-0002'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='1-3-0002'

		END
		ELSE IF @TransactionType = 'LoanInt'
		BEGIN
			
			-- Income ++	Income	Loan Interest Receivable Income	3-2-0009'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0009'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0009'

			-- Asset Receivable -- Receivable	Loan Interest Receivable	1-3-0003'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='1-3-0003'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='1-3-0003'

		END
		ELSE IF @TransactionType = 'OwnerEqt'
		BEGIN
			
			-- Income ++	Owners Equity	3-2-0011'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0011'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0011'

			-- Liability ++	Owners Equity	2-3-0019'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='2-3-0019'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='2-3-0019'

		END
		ELSE IF @TransactionType = 'MemBook'
		BEGIN
			
			-- Income ++	Member Book	3-1-0012'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-1-0012'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-1-0012'

		END
		ELSE IF @TransactionType = 'BimaInc'
		BEGIN
			
			-- Income ++	Bima Income	3-2-0013'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0013'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0013'

			-- Liability ++	Bima Payable	2-1-0019'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='2-1-0019'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='2-1-0019'

		END
		ELSE IF @TransactionType = 'OtherInc'
		BEGIN
			
			-- Income ++	Other	3-2-0014'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0014'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0014'

		END
		ELSE IF @TransactionType = 'ExUtility'
		BEGIN
			
			-- Expense ++	Utility Expense	4-1-0015'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='4-1-0015'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='4-1-0015'

		END
		ELSE IF @TransactionType = 'ExSnPrinCsBk'
		BEGIN
			
			-- Expense ++
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='4-1-0016'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='4-1-0016'

			-- Liability ++
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='2-1-0004'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='2-1-0004'

		END
		ELSE IF @TransactionType = 'ExSnIntCsBk'
		BEGIN
			
			-- Expense ++
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='4-1-0017'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='4-1-0017'

			-- Liability ++
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='2-1-0007'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='2-1-0007'

		END
		ELSE IF @TransactionType = 'ExLnDisburse'
		BEGIN
			
			-- Expense ++ Expense	Loan Disburse
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='4-1-0018'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='4-1-0018'

		END
		ELSE IF @TransactionType = 'LoanPrinRec'
		BEGIN
			
			-- Asset ++	Receivable	Loan Principle Receivable
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='1-3-0002'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='1-3-0002'

		END

		ELSE IF @TransactionType = 'LoanIntRec'
		BEGIN
			
			-- Asset ++	Receivable	Loan Interest Receivable
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='1-3-0003'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='1-3-0003'
			
		END
		ELSE IF @TransactionType = 'SecurityAdvance'
		BEGIN
			
			-- Asset ++	Receivable	Security Advance Receivable
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='1-3-0019'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='1-3-0019'
			
		END

		Exec Accounting_Update_UpdateAccountPettyCash_SanchaySamity

	COMMIT TRANSACTION t
	END TRY
	BEGIN CATCH
		  ROLLBACK TRANSACTION t
	END CATCH;
END
GO


-----------------------------------------------------------------------------------------------------------------------------------




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounting_Update_UpdateAccountPettyCash_SanchaySamity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Accounting_Update_UpdateAccountPettyCash_SanchaySamity]
GO
/*

Begin Tran
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-1-0001'
		Exec Accounting_Update_UpdateAccountPettyCash_SanchaySamity
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-1-0001'
Rollback Tran

*/
CREATE PROCEDURE [dbo].[Accounting_Update_UpdateAccountPettyCash_SanchaySamity]
	AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY  

	DECLARE @SanchayCollection DECIMAL(18,2)
	DECLARE @LoanCollection DECIMAL(18,2)
	DECLARE @MiscellaneousCollection DECIMAL(18,2)

	DECLARE @ExpenseCollection DECIMAL(18,2)
	DECLARE @CashBackCollection DECIMAL(18,2)
	DECLARE @LoanAmount DECIMAL(18,2)
	DECLARE @SecurityAdvance DECIMAL(18,2)

	DECLARE @TotalIncome DECIMAL(18,2)
	DECLARE @TotalExpense DECIMAL(18,2)

	DECLARE @CurrentBalance DECIMAL(18,2)
	DECLARE @OpeningBalance DECIMAL(18,2)

	DECLARE @TotalCurrentBalance DECIMAL(18,2)
	DECLARE @ActualCurrentBalance DECIMAL(18,2)

	BEGIN TRANSACTION t

		select @SanchayCollection = sum(Amount) from Collections where CollectionDate >= '2019-01-01'  and IsDeleted=0 -- 163660
		select @LoanCollection = sum(CollectedAmount) from LoanCollections where CollectionDate >= '2019-01-01'  and IsDeleted=0 -- 59530
		select @MiscellaneousCollection = sum(Amount) from MiscellaneousCollections where CollectionDate >= '2019-01-01' and IsDeleted=0 -- 3560


		select @ExpenseCollection = sum(Amount) from Expenses where CollectionDate >= '2019-01-01'  and IsDeleted=0 -- 10820
		select @CashBackCollection = sum(Amount) + sum(CashBackIntAmount) from CashBack where CollectionDate >= '2019-01-01' and IsDeleted=0 -- 19750
		select @LoanAmount = sum(Amount) from Loans where CollectionDate >= '2019-01-01' and IsDeleted=0 -- 100000
		select @SecurityAdvance = sum(Amount) from SecurityAdvances where CollectionDate >= '2019-01-01' and IsDeleted=0
		
		set @TotalIncome = @SanchayCollection + @LoanCollection + @MiscellaneousCollection
		set @TotalExpense = @ExpenseCollection + @CashBackCollection + @LoanAmount + isnull(@SecurityAdvance,0)

		select @CurrentBalance = CurrentBalance, @OpeningBalance = OpeningBalance from AccountHeads where AccountNo='1-1-0001'

		set @TotalCurrentBalance = isnull(@TotalIncome,0) - isnull(@TotalExpense,0)
		set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance

		--select @TotalIncome, @TotalExpense, @TotalCurrentBalance, @ActualCurrentBalance 

		update AccountHeads set CurrentBalance = @TotalCurrentBalance, ActualCurrentBalance = @ActualCurrentBalance, ModifiedDate = getdate() where AccountNo='1-1-0001'

	COMMIT TRANSACTION t
	END TRY
	BEGIN CATCH
		  ROLLBACK TRANSACTION t
	END CATCH;
END
GO






























-- -- Health Care 13-Oct-2019


-- -- 01

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounting_Update_UpdateAccountCurrentBalance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Accounting_Update_UpdateAccountCurrentBalance]
GO
/*

Begin Tran
SELECT Id,* FROM AccountHeads WHERE AccountNo='3-2-0003'
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-1-0001'
		Exec Accounting_Update_UpdateAccountCurrentBalance 'DoctorVisit', 50
SELECT Id,* FROM AccountHeads WHERE AccountNo='3-2-0003'
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-1-0001'
Rollback Tran

*/
CREATE PROCEDURE [dbo].[Accounting_Update_UpdateAccountCurrentBalance]
	@TransactionType NVARCHAR(15),
	@Amount DECIMAL
	--@RecordCount INT OUTPUT
	AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY  
		
	DECLARE @CurrentBalance DECIMAL(18,2)
	DECLARE @OpeningBalance DECIMAL(18,2)
	DECLARE @TotalCurrentBalance DECIMAL(18,2)
	DECLARE @ActualCurrentBalance DECIMAL(18,2)

	DECLARE @TotalIncome DECIMAL(18,2)
	DECLARE @TotalExpense DECIMAL(18,2)

	DECLARE @TodayCurrentBalance DECIMAL(18,2)

	DECLARE @TenPercentAmount DECIMAL(18,2)
	BEGIN TRANSACTION t

		IF @TransactionType = 'DoctorVisit'
		BEGIN
			-- Income ++	Income	3-2-0003'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads where AccountNo='3-2-0003'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() where AccountNo='3-2-0003'

		END
		ELSE IF @TransactionType = 'MedicalTest'
		BEGIN			
			-- Income ++	Income	3-2-0004'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0004'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0004'

		END
		ELSE IF @TransactionType = 'ExUtility'
		BEGIN
			
			-- Expense ++	Utility Expense	4-1-0015'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='4-1-0002'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) + isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='4-1-0002'

		END

		Exec Accounting_Update_UpdateAccountPettyCash

	COMMIT TRANSACTION t
	END TRY
	BEGIN CATCH
		  ROLLBACK TRANSACTION t
	END CATCH;
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounting_Delete_UpdateAccountCurrentBalance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Accounting_Delete_UpdateAccountCurrentBalance]
GO
/*

Begin Tran
SELECT Id,* FROM AccountHeads WHERE AccountNo='3-2-0004'
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-1-0001'
		Exec Accounting_Delete_UpdateAccountCurrentBalance 'MedicalTest', 500
SELECT Id,* FROM AccountHeads WHERE AccountNo='3-2-0004'
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-1-0001'
Rollback Tran

*/
CREATE PROCEDURE [dbo].[Accounting_Delete_UpdateAccountCurrentBalance]
	@TransactionType NVARCHAR(15),
	@Amount DECIMAL
	--@RecordCount INT OUTPUT
	AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY  
		
	DECLARE @CurrentBalance DECIMAL(18,2)
	DECLARE @OpeningBalance DECIMAL(18,2)
	DECLARE @TotalCurrentBalance DECIMAL(18,2)
	DECLARE @ActualCurrentBalance DECIMAL(18,2)

	DECLARE @TotalIncome DECIMAL(18,2)
	DECLARE @TotalExpense DECIMAL(18,2)

	DECLARE @TodayCurrentBalance DECIMAL(18,2)

	DECLARE @TenPercentAmount DECIMAL(18,2)
	BEGIN TRANSACTION t

		IF @TransactionType = 'DoctorVisit'
		BEGIN
			-- Income ++	Income	3-2-0003'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads where AccountNo='3-2-0003'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() where AccountNo='3-2-0003'

		END
		ELSE IF @TransactionType = 'MedicalTest'
		BEGIN			
			-- Income ++	Income	3-2-0004'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='3-2-0004'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='3-2-0004'

		END
		ELSE IF @TransactionType = 'ExUtility'
		BEGIN
			
			-- Expense ++	Utility Expense	4-1-0015'
			select @CurrentBalance = CurrentBalance, @OpeningBalance=OpeningBalance from AccountHeads WHERE AccountNo='4-1-0002'
			set @TotalCurrentBalance = isnull(@CurrentBalance,0) - isnull(@Amount,0)
			set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance
			update AccountHeads set CurrentBalance = isnull(@TotalCurrentBalance,0), ActualCurrentBalance = isnull(@ActualCurrentBalance,0), ModifiedDate = getdate() WHERE AccountNo='4-1-0002'

		END

		Exec Accounting_Update_UpdateAccountPettyCash

	COMMIT TRANSACTION t
	END TRY
	BEGIN CATCH
		  ROLLBACK TRANSACTION t
	END CATCH;
END
GO


-- -- 02



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounting_Update_UpdateAccountPettyCash]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Accounting_Update_UpdateAccountPettyCash]
GO
/*

Begin Tran
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-1-0001'
		Exec Accounting_Update_UpdateAccountPettyCash
SELECT Id,* FROM AccountHeads WHERE AccountNo='1-1-0001'
Rollback Tran

*/
CREATE PROCEDURE [dbo].[Accounting_Update_UpdateAccountPettyCash]
	AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY  

	DECLARE @DoctorVisitCollection DECIMAL(18,2) -- 3-2-0003
	DECLARE @MedicalTestCollection DECIMAL(18,2) -- 3-2-0004
	DECLARE @ExpenseCollection DECIMAL(18,2) -- 4-1-0002 
	DECLARE @PaymentReceivable DECIMAL(18,2)
	

	DECLARE @TotalIncome DECIMAL(18,2)
	DECLARE @TotalExpense DECIMAL(18,2)

	DECLARE @CurrentBalance DECIMAL(18,2)
	DECLARE @OpeningBalance DECIMAL(18,2)

	DECLARE @TotalCurrentBalance DECIMAL(18,2)
	DECLARE @ActualCurrentBalance DECIMAL(18,2)

	BEGIN TRANSACTION t

		select @DoctorVisitCollection = ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType='DoctorVisit' and IsDeleted=0
		select @MedicalTestCollection = ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType='MedicalTest' and IsDeleted=0
		select @ExpenseCollection = sum(Amount) from Expenses where CollectionDate >= '2019-01-01'  and IsDeleted=0
		
		set @TotalIncome = @DoctorVisitCollection + @MedicalTestCollection
		set @TotalExpense = @ExpenseCollection

		select @CurrentBalance = CurrentBalance, @OpeningBalance = OpeningBalance from AccountHeads where AccountNo='1-1-0001'

		set @TotalCurrentBalance = isnull(@TotalIncome,0) - isnull(@TotalExpense,0)
		set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @TotalCurrentBalance

		--select @TotalIncome, @TotalExpense, @TotalCurrentBalance, @ActualCurrentBalance 

		update AccountHeads set CurrentBalance = @TotalCurrentBalance, ActualCurrentBalance = @ActualCurrentBalance, ModifiedDate = getdate() where AccountNo='1-1-0001'


		-- -- Payment Receivable Update	

		select @CurrentBalance = CurrentBalance, @OpeningBalance = OpeningBalance from AccountHeads where AccountNo='1-3-0006'
		SELECT @PaymentReceivable = SUM(ISNULL(DueAmount,0)) FROM MedicalPayments WHERE InvoiceDate >= '2019-01-01' AND IsDeleted=0

		set @ActualCurrentBalance = isnull(@OpeningBalance,0) + @PaymentReceivable
		update AccountHeads set CurrentBalance = @PaymentReceivable, ActualCurrentBalance = @ActualCurrentBalance, ModifiedDate = getdate() where AccountNo='1-3-0006'


	COMMIT TRANSACTION t
	END TRY
	BEGIN CATCH
		  ROLLBACK TRANSACTION t
	END CATCH;
END
GO
