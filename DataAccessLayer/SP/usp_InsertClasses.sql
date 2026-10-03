CREATE OR ALTER PROCEDURE [dbo].[usp_InsertClasses]
	@ClassName nvarchar(100),
	@SubjectId bigint,
	@Section nvarchar(10),
	@AcademicYear nvarchar(10),
	@MaxCapacity int,
	@ClassGuid uniqueidentifier,
	@CreatedDate datetime,
	@CreatedBy nvarchar(450),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[Classes](ClassName, SubjectId, Section, AcademicYear, MaxCapacity, ClassGuid, CreatedDate, CreatedBy, IsActive)
	VALUES(@ClassName, @SubjectId, @Section, @AcademicYear, @MaxCapacity, @ClassGuid, @CreatedDate, @CreatedBy, @IsActive);

	SELECT @@IDENTITY;
END