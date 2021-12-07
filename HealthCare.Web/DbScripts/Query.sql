
-- -- 15-Oct-2019

SELECT * FROM Appointments

SELECT * FROM PatientTests
SELECT * FROM PatientTestDetails 

select * from MedicalPayments  -- Oct 15 2019 11:54PM
select * from MedicalPaymentDetails

--2019-10-15 09:07:21.160	Oct 15 2019  9:07AM
-- 2019-10-15 00:00:00.000	10/15/2019

--UPDATE MedicalPayments SET DateOfInvoice = CONVERT(VARCHAR(10), GETDATE(), 101)

-- -- 13-Oct-2019

SELECT * FROM dbo.AccountHeads
SELECT * FROM dbo.AccountHeadHistory

SELECT * FROM dbo.AccountHeadTypes

--truncate table AccountHeadHistory

--update AccountHeads set AccountNo='3-2-0003' where Id=2
--update AccountHeads set AccountNo='3-2-0004' where Id=3
--update AccountHeads set AccountNo='4-1-0002' where Id=4
--update AccountHeads set AccountNo='1-3-0006' where Id=5


-- -- 08-Oct-2019

SELECT * FROM Appointments

SELECT a.SerialNo,b.Name FROM Appointments a
inner join Patients b on b.Id=a.PatientId
where a.DoctorId=2 and a.DateOfAppointment='10/14/2019'

select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) from MedicalPayments 
WHERE InvoiceDate >= '2019-01-01' AND TransactionType='MedicalTest' and IsDeleted=0

SELECT * FROM PatientTests
SELECT * FROM PatientTestDetails 


truncate table Expenses
--drop table Appointments

select * from MedicalPayments
select * from MedicalPaymentDetails

select * from PatientTests
select * from PatientTestDetails


select * from Users
select * from Roles

select * from Doctors
select * from Patients

select * from Appointments
SELECT * FROM Departments

select * from Wards
select * from Rooms
select * from Beds

select * from Diagnosis
select * from Bills

select * from Suppliers
select * from Customers

select * from Staff
select * from StaffPicture
select * from SalaryBreakup
select * from StaffSalaryStructureDetails
select * from OtherSalaryBreakupSetup
select * from StaffWiseOtherSalaryDetails
select * from MonthlyStructureSalaryDetails
select * from EducationList
select * from BankList

SELECT * FROM Products

select * from PurchaseOrders

select * from PurchaseOrderDetails

SELECT * FROM Stocks

select * from TestCategories

select * from MedicalTests



/*

delete from Users
delete from Roles

delete from Doctors
delete from Patients

delete from Appointments
delete from Departments
delete from Rooms

delete from Diagnosis
delete from Bills

delete from Staff
delete from StaffPicture
delete from SalaryBreakup
delete from StaffSalaryStructureDetails
delete from OtherSalaryBreakupSetup
delete from StaffWiseOtherSalaryDetails
delete from MonthlyStructureSalaryDetails
delete from EducationList
delete from BankList

delete from Payments
delete from MedicalTests

delete from PurchaseOrders

delete from PurchaseOrderDetails


drop table Users
drop table Roles

drop table Doctors
drop table Patients

drop table Appointments
drop table Departments
drop table Rooms


Drop table Staff
Drop Table StaffPicture
Drop Table SalaryBreakup
Drop Table StaffSalaryStructureDetails
Drop Table OtherSalaryBreakupSetup
Drop Table StaffWiseOtherSalaryDetails
Drop Table MonthlyStructureSalaryDetails
Drop Table EducationList
Drop Table BankList


drop table Diagnosis
drop table Bills

drop table Payments
drop table MedicalTests

drop table PurchaseOrders


update Users set IsDeleted = 0
update Roles set IsDeleted = 0

update Doctors set IsDeleted = 0
update Patients set IsDeleted = 0

update Appointments set IsDeleted = 0
update Departments set IsDeleted = 0
update Rooms set IsDeleted = 0

*/


/*

--It's create root folder
dotnet ef migrations add Initial

--It's update database 
dotnet ef database update

--It's remove folder
dotnet ef database remove

--It's re-create migration database
dotnet ef migrations add Initial -o Data/Migrations



If Not Exists (select * from Roles where Id = 'e3aaa19a-c312-4a44-91ad-ec4501f0406f')
Begin
	insert into Roles(Id, ConcurrencyStamp, Name, NormalizedName) values('e3aaa19a-c312-4a44-91ad-ec4501f0406f', '', 'Admin', 'ADMIN')
End

















-- -- Create Roles Table By Shawon on 06-Jun-2017
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Roles](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Roles' AND column_name = 'IsDeleted')
BEGIN
	ALTER TABLE dbo.Roles ADD [IsDeleted] [BIT] NOT NULL DEFAULT(0)
END
GO


If Not Exists (select * from Roles where Id = 'e3aaa19a-c312-4a44-91ad-ec4501f0406f')
Begin
	insert into Roles(Id, ConcurrencyStamp, Name, NormalizedName, IsDeleted) values('e3aaa19a-c312-4a44-91ad-ec4501f0406f', '', 'Admin', 'ADMIN', 0)
End
Go


-- -- Create AspNetUsers Table By Shawon on 06-Jun-2017
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

-- -- Create Users Table By Shawon on 06-Jun-2017
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](450) NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Users' AND column_name = 'IsDeleted')
BEGIN
	ALTER TABLE dbo.Users ADD [IsDeleted] [BIT] NOT NULL DEFAULT(0)
END
GO


-- -- Create AspNetUsers Table By Shawon on 06-Jun-2017
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO




*/



-- Account Neel

update AccountHeads set OpeningBalance=0,CurrentBalance=0,ActualCurrentBalance=0
truncate table MedicalPayments
truncate table MedicalPaymentDetails
truncate table PharmacyIncomes
truncate table Expenses

truncate table Appointments

truncate table PatientTests
truncate table PatientTestDetails

truncate table PatientAdmissions

--truncate table Patients
--truncate table Doctors





-- -- Support Query

select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType='DoctorVisit' and IsDeleted=0

select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType='MedicalTest' and IsDeleted=0

select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType='Admission' and IsDeleted=0

select sum(Amount) from PharmacyIncomes where IsDeleted=0

select isnull(sum(isnull(Amount,0)),0) from Expenses where CollectionDate >= '2019-01-01'  and IsDeleted=0







select * from Appointments
select * FROM MedicalPayments WHERE TransactionType='DoctorVisit'
select * from MedicalPaymentDetails

select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType='DoctorVisit' and IsDeleted=0
-- 2700


select * from PatientTests
select * from PatientTestDetails

select sum(TotalAmount) FROM MedicalPayments WHERE TransactionType='MedicalTest'
select * from MedicalPaymentDetails

select ISNULL(SUM(ISNULL(TotalAmount,0)-ISNULL(DueAmount,0)),0) 
from MedicalPayments where InvoiceDate >= '2019-01-01' AND TransactionType='MedicalTest' and IsDeleted=0
-- 17770

select * from PharmacyIncomes
-- 10620

--1.
--delete from MedicalPaymentDetails WHERE MedicalPaymentId=1
--DELETE FROM MedicalPayments WHERE TransactionType='MedicalTest' AND id=1
--2. Expense Delete
--3. Medical Income Update - 17770
-- update AccountHeads set CurrentBalance =17770.00,  ActualCurrentBalance=17770.00 where Id=3 and AccountNo='3-2-0003'

select * from PatientAdmissions

select sum(amount) from Expenses where IsDeleted=0
-- 26182

select isnull(sum(isnull(Amount,0)),0) from Expenses where CollectionDate >= '2019-01-01'  and IsDeleted=0


select * from AccountHeads

--update AccountHeads set CurrentBalance =17770.00,  ActualCurrentBalance=17770.00, AccountNo='3-2-0003' where Id=3