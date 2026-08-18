CREATE OR ALTER PROCEDURE [dbo].[usp_InsertStudents]
	@AspNetUserId nvarchar(450),
	@StudentCode nvarchar(30),
	@FullName nvarchar(150),
	@Gender nvarchar(10),
	@StudentGuid uniqueidentifier,
	@CreatedDate datetime,
	@CreatedBy nvarchar(450),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[Students](AspNetUserId, StudentCode, FullName, Gender, StudentGuid, CreatedDate, CreatedBy, IsActive)
	VALUES(@AspNetUserId, @StudentCode, @FullName, @Gender, @StudentGuid, @CreatedDate, @CreatedBy, @IsActive);

	SELECT @@IDENTITY;
END