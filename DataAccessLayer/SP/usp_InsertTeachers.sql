CREATE OR ALTER PROCEDURE [dbo].[usp_InsertTeachers]
	@AspNetUserId nvarchar(450),
	@FullName nvarchar(150),
	@Gender nvarchar(10),
	@Department nvarchar(50),
	@ProfileImage nvarchar(400),
	@TeacherGuid uniqueidentifier,
	@CreatedDate datetime,
	@CreatedBy nvarchar(450),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[Teachers](AspNetUserId, FullName, Gender, Department, ProfileImage, TeacherGuid, CreatedDate, CreatedBy, IsActive)
	VALUES(@AspNetUserId, @FullName, @Gender, @Department, @ProfileImage, @TeacherGuid, @CreatedDate, @CreatedBy, @IsActive);

	SELECT @@IDENTITY;
END