CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateStudentsById]
	@Id bigint,
	@AspNetUserId nvarchar(450),
	@StudentCode nvarchar(30),
	@FullName nvarchar(150),
	@Gender nvarchar(10),
	@StudentGuid uniqueidentifier,
	@ModifiedDate datetime,
	@ModifiedBy nvarchar(450),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Students] SET AspNetUserId = @AspNetUserId, StudentCode = @StudentCode, FullName = @FullName, Gender = @Gender, StudentGuid = @StudentGuid, ModifiedDate = @ModifiedDate, ModifiedBy = @ModifiedBy, IsActive = @IsActive
	WHERE Id = @Id;
END