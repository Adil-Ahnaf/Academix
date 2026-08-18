CREATE OR ALTER PROCEDURE [dbo].[usp_UpdateSubjectsById]
	@Id bigint,
	@Name nvarchar(100),
	@ModifiedDate datetime,
	@ModifiedBy nvarchar(450),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Subjects] SET Name = @Name, ModifiedDate = @ModifiedDate, ModifiedBy = @ModifiedBy, IsActive = @IsActive
	WHERE Id = @Id;
END