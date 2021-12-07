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