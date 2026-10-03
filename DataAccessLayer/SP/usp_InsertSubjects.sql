CREATE OR ALTER PROCEDURE [dbo].[usp_InsertSubjects]
	@Name nvarchar(100),
	@CreatedDate datetime,
	@CreatedBy nvarchar(450),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[Subjects](Name, CreatedDate, CreatedBy, IsActive)
	VALUES(@Name, @CreatedDate, @CreatedBy, @IsActive);

	SELECT @@IDENTITY;
END