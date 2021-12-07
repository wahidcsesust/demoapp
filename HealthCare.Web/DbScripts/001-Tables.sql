

-- -- Create Roles Table By Shawon on 06-Jun-2017 drop table Roles
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Roles](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[IsDeleted] [BIT] NOT NULL DEFAULT(0)
	 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


-- -- Create Users Table By Shawon on 06-Jun-2017 drop table Users
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](450) NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
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
	[IsDeleted] [BIT] NOT NULL DEFAULT(0)
	 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Roles' AND column_name = 'IsDeleted')
BEGIN
	ALTER TABLE dbo.Roles ADD [IsDeleted] [BIT] NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Users' AND column_name = 'IsDeleted')
BEGIN
	ALTER TABLE dbo.Users ADD [IsDeleted] [BIT] NOT NULL DEFAULT(0)
END
GO

If Not Exists (select * from Roles where Id = 'e3aaa19a-c312-4a44-91ad-ec4501f0406f')
Begin
	insert into Roles(Id, ConcurrencyStamp, Name, NormalizedName, IsDeleted) values('e3aaa19a-c312-4a44-91ad-ec4501f0406f', '', 'Admin', 'ADMIN', 0)
End
Go

-- -- Create Doctor Table By Shawon on 06-Jun-2017 drop table Doctors
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Doctors]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Doctors](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Name] [NVARCHAR](500) NOT NULL,
	[BmdcRegNumber] [NVARCHAR](100) NULL,
	[Designation] [NVARCHAR](100) NULL,
	[Gender] [NVARCHAR](500) NULL,
	[Specilization] [NVARCHAR](500) NULL,
	[VisitPrice] [INT] NULL,
	[MobileNumber][NVARCHAR](500) NULL,
	[PhoneNumber][NVARCHAR](500) NULL,
	[EmailAddress][NVARCHAR](500) NULL,
	[DegreeOther][NVARCHAR](500) NULL,
	[DepartmentId] [BIGINT] NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [NVARCHAR](500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [NVARCHAR](500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Doctors] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Doctors' AND column_name = 'Designation')
BEGIN
	ALTER TABLE dbo.Doctors ADD Designation [NVARCHAR](100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Doctors' AND column_name = 'BmdcRegNumber')
BEGIN
	ALTER TABLE dbo.Doctors ADD BmdcRegNumber [NVARCHAR](100) NULL
END
GO

-- -- Create Patient Table By Shawon on 06-Junl-2017 drop table Patients
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Patients]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Patients](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Name] [NVARCHAR](500) NOT NULL,
	[Age] [INT] NULL,
	[Gender] [NVARCHAR](500) NULL,
	[DateOfBirth] [NVARCHAR](500) NULL,
	[BloodGroup] [NVARCHAR](500) NULL,
	[MobileNumber][NVARCHAR](500) NULL,
	[Address] [NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [NVARCHAR](500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [NVARCHAR](500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Patients] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Patients' AND column_name = 'Weight')
BEGIN
	ALTER TABLE dbo.Patients ADD 
		[Weight] INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Patients' AND column_name = 'RegNo')
BEGIN
	ALTER TABLE dbo.Patients ADD 
		RegNo INT NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Patients' AND column_name = 'BirthDate')
BEGIN
	ALTER TABLE dbo.Patients ADD 
		BirthDate DateTime NULL
END
GO


-- -- Create Appointment Table By Shawon on 14-August-2017 drop table Appointments
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Appointments]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Appointments](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[SequenceNo] INT NOT NULL DEFAULT(0),
	[AppointmentNo] [NVARCHAR](100) NOT NULL,
	[SerialNo] [INT] NOT NULL,
	[PatientId] [BIGINT] NULL,
	[DoctorId] [BIGINT] NULL,
	[DepartmentId] [BIGINT] NULL,
	[AppointmentDate] [DATETIME] NULL,
	[DateOfAppointment] [NVARCHAR](100) NULL,
	[VisitAmount] [DECIMAL] NULL,
	[DueAmount] DECIMAL NULL,
	[Problem] [NVARCHAR](500) NULL,
	[Remarks] [NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [NVARCHAR](500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [NVARCHAR](500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Appointments] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

-- -- Create PatientTests Tablet By Shawon on 06-Jun-2017 drop table PatientTests

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[PatientTests]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PatientTests](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[PatientId] [BIGINT] NOT NULL,
	[DoctorId] [BIGINT] NOT NULL,
	[AppointmentId] [BIGINT] NOT NULL,
	[TotalAmount] [DECIMAL] NULL,
	[DueAmount] [DECIMAL] NULL,
	[DeliveryDate] [DATETIME] NULL,
	[DateOfDelivery] [NVARCHAR](500) NULL,
	[Remarks] [NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [VARCHAR] (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [VARCHAR] (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_PatientTests] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


-- -- Create PatientTests Tablet By Shawon on 06-Jun-2019 drop table PatientTestDetails

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[PatientTestDetails]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PatientTestDetails](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[PatientTestId] [BIGINT] NOT NULL,
	[MedicalTestId] [BIGINT] NOT NULL,
	[TestRate] [DECIMAL] NULL,
	[Discount] [INT] NULL,
	[Amount] [DECIMAL] NULL,
	[Remarks] [NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [VARCHAR] (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [VARCHAR] (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_PatientTestDetails] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


-- -- Create PatientTests Tablet By Shawon on 06-Jun-2019 drop table MedicalPayments

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[MedicalPayments]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MedicalPayments](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[TransactionId] [BIGINT] NOT NULL,
	[TransactionType] [NVARCHAR](100) NULL,
	[PatientId] [BIGINT] NOT NULL,
	[InvoiceNo] [NVARCHAR](100) NULL,
	[InvoiceDate] [DATETIME] NULL,
	[DateOfInvoice] [NVARCHAR](100) NULL,
	[TotalAmount] [DECIMAL] NULL,
	[DueAmount] [DECIMAL] NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [VARCHAR] (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [VARCHAR] (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_MedicalPayments] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

-- -- Create PatientTests Tablet By Shawon on 06-Jun-2019 drop table MedicalPaymentDetails

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[MedicalPaymentDetails]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MedicalPaymentDetails](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[MedicalPaymentId] [BIGINT] NOT NULL,
	[PaymentType] [NVARCHAR](100) NULL,
	[Amount] [DECIMAL] NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [VARCHAR] (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [VARCHAR] (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_MedicalPaymentDetails] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO




-- -- Create Departments Table By Shawon on 06-Jun-2017
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Departments]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Departments](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Name][NVARCHAR](500) NOT NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [NVARCHAR](500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [NVARCHAR](500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


-- -- Create Rooms Table By Shawon on 06-Jun-2017
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Rooms]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Rooms](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[RoomNo] [NVARCHAR](500) NOT NULL,
	[Location][NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [NVARCHAR](500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [NVARCHAR](500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

-- -- Create TestCategories Table By Shawon on 07-Feb-2018
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[TestCategories]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[TestCategories](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Name][NVARCHAR](500) NOT NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [NVARCHAR](500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [NVARCHAR](500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_TestCategories] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


-- -- Create MedicalTests Tablet By Shawon on 07-Feb-2018


IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[MedicalTests]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MedicalTests](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Code] [NVARCHAR](500) NULL,
	[Name] [NVARCHAR](500) NULL,
	[Amount] [Decimal] NULL,
	[TestCategoryId] [BIGINT] NOT NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [VARCHAR] (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [VARCHAR] (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_MedicalTests] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


-- -- Create Diagnosis Tablet By Shawon on 06-Jun-2017

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Diagnosis]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Diagnosis](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,	
	[DiagNo] [INT] NOT NULL,
	[PatientId] [BIGINT] NOT NULL,
	[DoctorId] [BIGINT] NOT NULL,
	[DiagDate] [NVARCHAR](500) NULL,
	[Description] [NVARCHAR](500) NULL,
	[Advice] [NVARCHAR](500) NULL,
	[Remarks] [NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [VARCHAR] (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [VARCHAR] (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Diagnosis] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO



-- -- Create Bill Tablet By Shawon on 06-Jun-2017

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Bills]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Bills](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[BillNo] [INT] NOT NULL,
	[Amount] [decimal] NOT NULL,
	[BillDate] [NVARCHAR](500) NULL,
	[PatientId] [BIGINT] NOT NULL,
	[DoctorId] [BIGINT] NOT NULL,
	[Remarks] [NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [VARCHAR] (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [VARCHAR] (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Bills] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

-- -- Create Payments Tablet By Shawon on 06-Jun-2017

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Payments]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Payments](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[InvoiceNo] [INT] NOT NULL,
	[Amount] [decimal] NULL,
	[PaymentDate] [NVARCHAR](500) NULL,
	[DiagnosisId] [BIGINT] NULL,
	[Remarks] [NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [VARCHAR] (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [VARCHAR] (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO








----------------------------------------------------***********  Inventory Data Tables **************--------------------------------------------------------------


-- -- Create Categories Tablet By Shawon on 06-Jun-2017

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Categories](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Name][NVARCHAR](500) NOT NULL,
	[Code][NVARCHAR](500) NOT NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [BIGINT] NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [BIGINT] NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

If Exists (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Categories' AND COLUMN_NAME = 'CreatedBy' AND DATA_TYPE='bigint')
Begin
	ALTER TABLE Categories ALTER COLUMN CreatedBy VARCHAR (500) NOT NULL;
End
Go

If Exists (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Categories' AND COLUMN_NAME = 'ModifiedBy' AND DATA_TYPE='bigint')
Begin
	ALTER TABLE Categories ALTER COLUMN ModifiedBy VARCHAR (500) NOT NULL;
End
Go

-- -- Create ProductGroups Tablet By Shawon on 06-Jun-2017

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[ProductGroups]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[ProductGroups](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Name][NVARCHAR](500) NOT NULL,
	[Code][NVARCHAR](500) NOT NULL,
	[CategoryId] [BIGINT] NOT NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [BIGINT] NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [BIGINT] NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_ProductGroups] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

If Exists (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProductGroups' AND COLUMN_NAME = 'CreatedBy' AND DATA_TYPE='bigint')
Begin
	ALTER TABLE ProductGroups ALTER COLUMN CreatedBy VARCHAR (500) NOT NULL;
End
Go

If Exists (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProductGroups' AND COLUMN_NAME = 'ModifiedBy' AND DATA_TYPE='bigint')
Begin
	ALTER TABLE ProductGroups ALTER COLUMN ModifiedBy VARCHAR (500) NOT NULL;
End
Go
-- -- Create ProductBrands Tablet By Shawon on 07-Jun-2017

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[ProductBrands]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[ProductBrands](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Name][NVARCHAR](500) NOT NULL,
	[Code][NVARCHAR](500) NOT NULL,
	[CategoryId] [BIGINT] NOT NULL,
	[ProductGroupId] [BIGINT] NOT NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [BIGINT] NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [BIGINT] NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_ProductBrands] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

If Exists (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProductBrands' AND COLUMN_NAME = 'CreatedBy' AND DATA_TYPE='bigint')
Begin
	ALTER TABLE ProductBrands ALTER COLUMN CreatedBy VARCHAR (500) NOT NULL;
End
Go

If Exists (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProductBrands' AND COLUMN_NAME = 'ModifiedBy' AND DATA_TYPE='bigint')
Begin
	ALTER TABLE ProductBrands ALTER COLUMN ModifiedBy VARCHAR (500) NOT NULL;
End
Go

-- -- Create Products Tablet By Shawon on 07-Jun-2017

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Products](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Name][NVARCHAR](500) NOT NULL,
	[Code][NVARCHAR](500) NOT NULL,
	[CategoryId] [BIGINT] NOT NULL,
	[ProductGroupId] [BIGINT] NOT NULL,
	[ProductBrandId] [BIGINT] NOT NULL,
	[MeasurementUnit][NVARCHAR](500) NOT NULL,
	[MeasurementType][NVARCHAR](500) NOT NULL,
	[CostPrice] [Decimal](18,2) NULL,
    [SalePrice] [Decimal](18,2) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [BIGINT] NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [BIGINT] NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


If Exists (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'CreatedBy' AND DATA_TYPE='bigint')
Begin
	ALTER TABLE Products ALTER COLUMN CreatedBy VARCHAR (500) NOT NULL;
End
Go

If Exists (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'ModifiedBy' AND DATA_TYPE='bigint')
Begin
	ALTER TABLE Products ALTER COLUMN ModifiedBy VARCHAR (500) NOT NULL;
End
Go


-- -- Create Suppliers Table By Shawon on 06-Jun-2017


IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Suppliers]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Suppliers](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Code] [NVARCHAR](500) NOT NULL,
	[SupName][NVARCHAR](500) NOT NULL,
	[SupAddress][NVARCHAR](500) NOT NULL,
	[OfficePhone] [NVARCHAR](20) NOT NULL,
	[OfficeMail] [NVARCHAR](255) NULL,
	[WebAddress] [NVARCHAR](255) NULL,
	[ContactPerson] [NVARCHAR](500) Not NULL,
	[ContactPersonAddress] [NVARCHAR](500) NULL,
	[ContactPersonMobile] [NVARCHAR](255) Not NULL,
    [ContactPersonMail] [NVARCHAR](255) NULL,
	[BankAccountName] [NVARCHAR](255) NULL,
	[BankAccountNumber] [NVARCHAR](25) NULL,
	[Remarks] [NVARCHAR](255) NULL,
	[IsPaymentBeforeSale] [BIT] not NULL DEFAULT(0),
	[PaymentDay] [NVARCHAR](20) NULL,
	[IsPaymentAfterSale] [BIT] not NULL DEFAULT(0),
	[Status] [BIT] Not NULL DEFAULT(1),
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] VARCHAR (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] VARCHAR (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Customers]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Customers](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Code] [NVARCHAR](500) NOT NULL,
	[Name][NVARCHAR](500) NOT NULL,
	[AccountManagerCode] [NVARCHAR](500) NULL,
	[AccountManagerName][NVARCHAR](500) NULL,
	[Address][NVARCHAR](500) NULL,
	[PostCode][NVARCHAR](500) NULL,
	[MobileNumber][NVARCHAR](500) NULL,
	[PhoneNumber] [NVARCHAR](20) NULL,
	[MailAddress] [NVARCHAR](255) NULL,
	[DeliveryDate] [DATETIME] NULL,
	[Remarks] [NVARCHAR](255) NULL,
	[Status] [BIT] NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] VARCHAR (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] VARCHAR (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[PurchaseOrders]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PurchaseOrders](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[PONumber] [NVARCHAR](500) NOT NULL,
	[SupplierId] [bigint] NOT NULL,
	[OrderedBy] [nvarchar](200) NULL,
	[ReceivedByDate] [NVARCHAR](500) NULL,
	[Status] [nvarchar](200) NULL,
	[Comments] [nvarchar](200) NULL,
	[BookedBy] [nvarchar](200) NULL,
	[BookedByDate] [NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] VARCHAR (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] VARCHAR (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_PurchaseOrders] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[PurchaseOrderDetails]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PurchaseOrderDetails](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[POId] [NVARCHAR](500) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[CostPrice] [decimal](18, 2) NOT NULL,
	[ReceivedQuantity] [decimal](18, 2) NULL,
	[Comment] [nvarchar](50) NULL,
	[IsComplete] [bit] NOT NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] VARCHAR (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] VARCHAR (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_PurchaseOrderDetails] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO



-- -- 13-Jun-2020

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PatientTests' AND COLUMN_NAME = 'LessAmount')
Begin
	ALTER TABLE PatientTests ADD LessAmount DECIMAL NULL;
End
Go



-- -- 19-Jun-2020

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Wards]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Wards](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[Name][NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [NVARCHAR](500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [NVARCHAR](500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Wards] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Beds]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Beds](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[RoomId] [BIGINT] NOT NULL,
	[BedNo][NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [NVARCHAR](500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [NVARCHAR](500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_Beds] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

-- -- 21-Jun-2020

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[PatientAdmissions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PatientAdmissions](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[PatientId] [BIGINT] NOT NULL,
	[AdmissionDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[RoomId] [BIGINT] NOT NULL,
	[BedId] [BIGINT] NOT NULL,
	[AdmissionTime] [NVARCHAR](50) NULL,
	[DischargeDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[CareOf] [NVARCHAR](10) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [NVARCHAR](500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [NVARCHAR](500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_PatientAdmissions] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO



-- -- 20-Jun-2020

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[PharmacyIncomes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PharmacyIncomes](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[InvoiceNo] [INT] NOT NULL,
	[Amount] [decimal] NULL,
	[CollectionDate] [DATETIME] NULL,
	[Particulars] [NVARCHAR](500) NULL,
	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [VARCHAR] (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [VARCHAR] (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_PharmacyIncomes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO



-- -- 04-Jul-2020

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PatientTests' AND COLUMN_NAME = 'DateOfDelivery')
Begin
	ALTER TABLE PatientTests drop column DateOfDelivery
End
Go


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PatientAdmissions' AND COLUMN_NAME = 'LessAmount')
Begin
	ALTER TABLE PatientAdmissions ADD LessAmount DECIMAL NULL;
End
Go


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MedicalPayments' AND COLUMN_NAME = 'LessAmount')
Begin
	ALTER TABLE MedicalPayments ADD LessAmount DECIMAL NULL;
End
Go



-- -- 24-Mar-2021


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Members' AND column_name = 'PassportNo')
BEGIN
	ALTER TABLE dbo.Members ADD PassportNo [NVARCHAR](100) NULL
END
GO



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Members' AND column_name = 'AreaLocation')
BEGIN
	ALTER TABLE dbo.Members ADD AreaLocation [NVARCHAR](100) NULL
END
GO



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Members' AND column_name = 'Category')
BEGIN
	ALTER TABLE dbo.Members ADD Category [NVARCHAR](100) NULL
END
GO



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Members' AND column_name = 'Designation')
BEGIN
	ALTER TABLE dbo.Members ADD Designation [NVARCHAR](100) NULL
END
GO


-- -- 18-April-2021




IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[HelpCollections]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[HelpCollections](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[SerialNo] [INT] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Age] [int] NULL,
	[FatherName] [nvarchar](100) NULL,
	[MotherName] [nvarchar](100) NULL,
	[Gender] [nvarchar](500) NULL,

	[MobileNumber] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,	
	[Profession] [nvarchar](100) NULL,
	[NationalIdNo] [nvarchar](100) NULL,
	[Religion] [nvarchar](100) NULL,
	

	[Subject] [NVARCHAR](500) NOT NULL,
	[MemberId] [BIGINT] NOT NULL,
	[DateOfHelp] [datetime] NULL,

	[IsActive] [BIT] NOT NULL DEFAULT(1),
	[IsLocked] [BIT] NOT NULL DEFAULT(0),
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[CreatedBy] [VARCHAR] (500) NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),
	[ModifiedBy] [VARCHAR] (500) NOT NULL,
	[ModifiedDate] [DATETIME] NOT NULL DEFAULT(GETDATE()),	
	 CONSTRAINT [PK_HelpCollections] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Members' AND column_name = 'NomineeName')
BEGIN
	ALTER TABLE dbo.Members ADD NomineeName [NVARCHAR](100) NULL
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Members' AND column_name = 'NomineeRelation')
BEGIN
	ALTER TABLE dbo.Members ADD NomineeRelation [NVARCHAR](100) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Members' AND column_name = 'NomineeNationalIdNo')
BEGIN
	ALTER TABLE dbo.Members ADD NomineeNationalIdNo [NVARCHAR](100) NULL
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.columns WHERE table_name = 'Members' AND column_name = 'NomineeAge')
BEGIN
	ALTER TABLE dbo.Members ADD NomineeAge INT NULL
END
GO