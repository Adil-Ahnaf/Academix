CREATE OR ALTER PROCEDURE [dbo].[usp_InsertStudentEnrollments]
	@StudentId bigint,
	@ClassId bigint,
	@CreatedDate datetime,
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[StudentEnrollments](StudentId, ClassId, CreatedDate, IsActive)
	VALUES(@StudentId, @ClassId, @CreatedDate, @IsActive);

	SELECT @@IDENTITY;
END