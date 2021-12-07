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