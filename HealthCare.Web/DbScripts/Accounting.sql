

-- -- DoctorVisit Income -- 5900
select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) DoctorVisit from MedicalPayments where TransactionType='DoctorVisit' and IsDeleted=0

-- -- MedicalTest Income -- 45470
--select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) MedicalTest from MedicalPayments where TransactionType='MedicalTest' and IsDeleted=0
select isnull(sum(isnull(b.Amount,0)),0) from MedicalPayments a INNER JOIN MedicalPaymentDetails b on b.MedicalPaymentId=a.Id WHERE a.IsDeleted=0 AND a.TransactionType='MedicalTest'

-- -- Admission Income -- 20400
--select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) Admission from MedicalPayments where TransactionType='Admission' and IsDeleted=0
select isnull(sum(isnull(b.Amount,0)),0) from MedicalPayments a INNER JOIN MedicalPaymentDetails b on b.MedicalPaymentId=a.Id WHERE a.IsDeleted=0 AND a.TransactionType='Admission'

-- -- Pharmacy Incomes -- 35820
select isnull(sum(isnull(Amount,0)),0) PharmacyIncomes from PharmacyIncomes where IsDeleted=0


-- -- Expenses
select isnull(sum(isnull(Amount,0)),0) Expenses from Expenses where IsDeleted=0

-- -- Less Expense
SELECT SUM(ISNULL(LessAmount,0)) LessAmount FROM MedicalPayments WHERE InvoiceDate >= '2019-01-01' AND IsDeleted=0


-- -- Payment Receivable
SELECT SUM(ISNULL(DueAmount,0)) Receivable FROM MedicalPayments WHERE InvoiceDate >= '2019-01-01' AND IsDeleted=0



-- -- Petty Cash--------------------------------------------------------------------------------------------------------------------


select(
-- -- DoctorVisit Income -- 5900
select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType='DoctorVisit' and IsDeleted=0
)+(
-- -- MedicalTest Income -- 45470
--select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType='MedicalTest' and IsDeleted=0
select isnull(sum(isnull(b.Amount,0)),0) from MedicalPayments a INNER JOIN MedicalPaymentDetails b on b.MedicalPaymentId=a.Id WHERE a.IsDeleted=0 AND a.TransactionType='MedicalTest'
)+(
-- -- Admission Income -- 20400
--select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType='Admission' and IsDeleted=0
select isnull(sum(isnull(b.Amount,0)),0) from MedicalPayments a INNER JOIN MedicalPaymentDetails b on b.MedicalPaymentId=a.Id WHERE a.IsDeleted=0 AND a.TransactionType='Admission'
)+(
-- -- Pharmacy Incomes -- 35820
select sum(Amount) from PharmacyIncomes where IsDeleted=0
) - (
-- -- Expenses  -- 105341
select isnull(sum(isnull(Amount,0)),0) from Expenses where CollectionDate >= '2019-01-01'  and IsDeleted=0
) - (
-- -- Expenses  -- 105341
select isnull(sum(isnull(LessAmount,0)),0) from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType in('MedicalTest', 'Admission') and IsDeleted=0
)

