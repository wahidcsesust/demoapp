


Begin Tran


--drop table __EFMigrationsHistory
drop table [dbo].[AspNetRoleClaims]
drop table [dbo].[AspNetUserClaims]
drop table [dbo].[AspNetUserLogins]
drop table AspNetUserRoles
drop table [dbo].[AspNetUserTokens]
drop table BankList
drop table Bills
drop table [dbo].[Categories]
drop table [dbo].[Customers]
drop table [dbo].[Departments]
drop table [dbo].[Diagnosis]

drop table [dbo].[Doctors]
drop table [dbo].[EducationList]
drop table [dbo].[MedicalTests]
drop table [dbo].[MonthlyStructureSalaryDetails]
drop table [dbo].[OtherSalaryBreakupSetup]
drop table [dbo].[Patients]
drop table [dbo].[Payments]
drop table [dbo].[ProductBrands]
drop table [dbo].[ProductGroups]
drop table [dbo].[Products]
drop table [dbo].[PurchaseOrderDetails]
drop table [dbo].[PurchaseOrders]
drop table [dbo].[Rooms]
drop table [dbo].[SalaryBreakup]
drop table [dbo].[Staff]
drop table [dbo].[StaffPicture]
drop table [dbo].[StaffSalaryStructureDetails]
drop table [dbo].[StaffWiseOtherSalaryDetails]
drop table [dbo].[Suppliers]
drop table [dbo].[TestCategories]


truncate table Appointments
truncate table PatientTests
truncate table PatientTestDetails
truncate table MedicalPayments
truncate table MedicalPaymentDetails
TRUNCATE TABLE Expenses
--TRUNCATE TABLE AccountHeads
--TRUNCATE TABLE AccountHeadHistory

--UPDATE dbo.AccountHeads SET OpeningBalance=0, CurrentBalance=0, ActualCurrentBalance=0



truncate table Patients
truncate table Doctors
truncate table Departments
truncate table Appointments

truncate table PatientTests
truncate table PatientTestDetails
truncate table TestCategories
truncate table MedicalTests

truncate table MedicalPayments
truncate table MedicalPaymentDetails

truncate table Expenses
 
update  AccountHeads SET OpeningBalance=0, CurrentBalance=0, ActualCurrentBalance=0

Rollback Tran