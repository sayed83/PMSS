--
	--Project Name	:: Patient Mangement System
	--DB Name		:: PMSDB
	--Total Table	:: 10
	--Store Proce	:: 3
--
GO
	--//--Table#1--//--
	CREATE TABLE Hospital
	(
		HospitalID int NOT NULL PRIMARY KEY IDENTITY(1,1),
		HospitalName varchar(70) NOT NULL,
		Address varchar(50) NOT NULL,
		PhoneNumber varchar(11) NOT NULL,
	)
GO
	--//--Table#4--//--
	CREATE TABLE RoomCategory
	(
		RoomCategoryID int NOT NULL Primary Key Identity,
		RoomCategory varchar(50) NOT NULL,
		RoomFee money NOT NULL
	)
GO
	--//--Table#5--//--
	CREATE TABLE Room
	(
		RoomID int NOT NULL Primary key identity,
		RoomNO AS 'RM'+RIGHT('000'+CAST(RoomID as varchar(3)), 3),
		RoomCategoryID int Foreign key References RoomCategory(RoomCategoryID),
		IsActive bit DEFAULT 1
	)
GO
	--//--Table#8--//--
	CREATE TABLE Patient
	(
		PatientID int NOT NULL Primary key identity,
		PatCardID AS 'PAT'+RIGHT('0000'+CAST(PatientID as varchar(4)), 4),
		PatientName varchar(50) NOT NULL,
		Age varchar(10) NOT NULL,
		ContactNumber varchar(11) NOT NULL,
		Address varchar(50) NULL,
		AdmissionDate datetime DEFAULT GETDATE(),
		RoomID int Foreign Key References Room(RoomID)
	)
GO
	--//--Table#6--//--
	CREATE TABLE Staff
	(
		StaffID int NOT NULL Primary key identity,
		StaffCardID AS 'EXE'+RIGHT('0000'+CAST(StaffID as varchar(4)), 4),
		StaffName varchar(50) NOT NULL,
		Gender varchar(20) NOT NULL,
		ContactNumber varchar(11) NOT NULL,
		Dob date NULL,
		Address varchar(50) NULL,
		City varchar(50) NULL,
		ZipCode nchar(10) NULL,
		StaffAvater nvarchar(90) NULL,
		JoiningDate datetime NULL,
		AddeddDate Date Null,
		IsActive bit DEFAULT 1
	)
GO
	--//--Table#7--//--
	CREATE TABLE Qualification
	(
		QualificationID int Primary key NOT NULL Identity,
		QualificationName varchar(70) NOT NULL,
	)
GO
	--//--Table#8--//--
	CREATE TABLE Speciality
	(
		SpecialityID int Primary key Identity NOT NULL,
		SpecialityTag varchar(50) NOT NULL,
	)
GO
GO
	--//--Table#7--//--
	CREATE TABLE Doctor
	(
		DoctorID int NOT NULL Primary key identity,
		DoctorCardID AS 'DOC'+RIGHT('000'+CAST(DoctorID as varchar(3)),3),
		DoctorName varchar(50) NOT NULL,
		Gender varchar(20) NOT NULL,
		ContactNumber varchar(11) NOT NULL,
		Dob date NULL,
		Address varchar(50) NULL,
		City varchar(50) NULL,
		ZipCode nchar(10) NULL,
		QualificationID int Foreign key References Qualification(QualificationID),--Multiple value
		SpecialityID int Foreign Key References Speciality(SpecialityID),
		JoiningDate datetime NOT NULL,
		AddeddDate Date Null,
		IsActive bit DEFAULT 1
	)
GO
	--//--Table#11--//--
	CREATE TABLE Treatment
	(
		TreatmentID int NOT NULL PRIMARY KEY IDENTITY(1,1),
		TreatmentName varchar(40) NOT NULL,
	)
GO	
	--//--Table#12--//--
	CREATE TABLE TreatmentDetials
	(
		TreatmentDetailID int primary key Identity,
		TreatmentID int Foreign key References Treatment(TreatmentID),
		[Description] varchar(120) NULL,
		DoctorID int Foreign Key References Doctor(DoctorID), 
		PatientID int Foreign Key References Patient(PatientID) 
	)
GO
	--//--Table#13--//--
	CREATE TABLE Appointment
	(
		AppoinmentID int Primary key identity NOT NULL,
		ApmntName varchar(30) NOT NULL,
		ApmntPhone varchar(11) NOT NULL,
		ApmntPayment money NOT NULL,
		ApmntDate date NOT NULL,
		DoctorID int Foreign key References Doctor(DoctorID),
		ApmntMakeDate date DEFAULT GETDATE(),
		Approved bit Default 0
	)
GO
	--//--Table#14--//--
	CREATE TABLE Schedule
	(
		ScheduleID int Primary Key identity NOT NULL,
		ScheduleDate date NOT NULL,
		ScheduleAllow int NULL,
		DoctorID int Foreign key References Doctor(DoctorID)
	)
GO	
	CREATE FUNCTION fn_roomCharge(@patientID int)
	RETURNS INT
	AS
	BEGIN
		DECLARE @admissionDate date, @getDay int, @roomFee money,@roomID int
		SELECT @roomID = RoomID FROM Patient WHERE PatientID = @patientID 
		SELECT @admissionDate = AdmissionDate FROM Patient WHERE PatientID = @patientID
		SET @getDay = DATEDIFF(day, GETDATE(), @admissionDate)
		SELECT @roomFee = RoomFee FROM RoomCategory WHERE RoomCategoryID =(SELECT RoomCategoryID FROM Room WHERE RoomID = @roomID)
		RETURN @roomFee * @getDay
	END
GO

	CREATE FUNCTION fn_grandTotal(@patientID int)
	RETURNS INT
	AS
	BEGIN
		DECLARE @pathologyFees money, @doctorFees money, @OtherFees money
		SELECT @pathologyFees = PathologyFee FROM Bill WHERE PatientID = @patientID
		SELECT @doctorFees = DoctorFees FROM Bill WHERE PatientID = @patientID
		SELECT @OtherFees = OtherFees FROM Bill WHERE PatientID = @patientID
		RETURN @pathologyFees+@doctorFees+@OtherFees
	END
GO

	CREATE FUNCTION fn_netTotal(@patientID int)
	RETURNS INT
	AS
	BEGIN
		DECLARE @pathologyFees money, @doctorFees money, @otherFees money,@discount money
		SELECT @pathologyFees = PathologyFee FROM Bill WHERE PatientID = @patientID
		SELECT @doctorFees = DoctorFees FROM Bill WHERE PatientID = @patientID
		SELECT @otherFees = OtherFees FROM Bill WHERE PatientID = @patientID
		SELECT @discount = Discount FROM Bill WHERE PatientID = @patientID
		RETURN (@pathologyFees+@doctorFees+@OtherFees)-@discount
	END

GO
	--//--Table#10--//--
	CREATE TABLE Bill
	(
		BillID int NOT NULL PRIMARY KEY IDENTITY(1,1),
		BillNO AS 'BILL'+RIGHT('0000'+CAST(BillID as varchar(4)), 4),
		StaffID int Foreign Key References Staff(StaffID), 
		PatientID int Foreign Key References Patient(PatientID),
		PrepareDate date NOT NULL,
		DischargeDate date NOT NULL,
		RoomCharge as dbo.fn_roomCharge(PatientID),--Computed Column
		PathologyFee money NULL,
		DoctorFee money NULL,
		OtherFees money NULL,
		GrandTotal as dbo.fn_grandTotal(PatientID),
		Discount money,
		TotalBill as dbo.fn_netTotal(PatientID),
	)

Go
	
CREATE TRIGGER tr_RoomStatusUpdate
    ON dbo.Patient
    FOR INSERT, UPDATE
    AS
    BEGIN
	DECLARE @roomID int

		SELECT @roomID = i.RoomID from inserted i;
		UPDATE Room SET IsActive = 'False' Where RoomID = @roomID;
    END

GO
	
CREATE TRIGGER tr_RoomStatusTrue
    ON dbo.Bill
    FOR DELETE, INSERT, UPDATE
    AS
    BEGIN
		DECLARE @patID int
		SELECT @patID = i.PatientID from inserted i;
		UPDATE Room SET IsActive = 'True'
		WHERE RoomID = (SELECT RoomID from Patient Where PatientID = @patID)
    END

