CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateTeachersById]
	@Id bigint,
	@AspNetUserId nvarchar(450),
	@FullName nvarchar(150),
	@Gender nvarchar(10),
	@Department nvarchar(50),
	@ProfileImage nvarchar(400),
	@TeacherGuid uniqueidentifier,
	@ModifiedDate datetime,
	@ModifiedBy nvarchar(450),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Teachers] SET AspNetUserId = @AspNetUserId, FullName = @FullName, Gender = @Gender, Department = @Department, ProfileImage = @ProfileImage, TeacherGuid = @TeacherGuid, ModifiedDate = @ModifiedDate, ModifiedBy = @ModifiedBy, IsActive = @IsActive
	WHERE Id = @Id;
END