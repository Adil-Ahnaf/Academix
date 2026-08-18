CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateClassesById]
	@Id bigint,
	@ClassName nvarchar(100),
	@Section nvarchar(10),
	@AcademicYear nvarchar(10),
	@MaxCapacity int,
	@ClassGuid uniqueidentifier,
	@ModifiedDate datetime,
	@ModifiedBy nvarchar(450),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Classes] SET ClassName = @ClassName, Section = @Section, AcademicYear = @AcademicYear, MaxCapacity = @MaxCapacity, ClassGuid = @ClassGuid, ModifiedDate = @ModifiedDate, ModifiedBy = @ModifiedBy, IsActive = @IsActive
	WHERE Id = @Id;
END