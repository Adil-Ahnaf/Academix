USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetStudentByAspNetUserId] 
	@AspNetUserId NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT S.*, U.Email AS EmailAddress 
	FROM [dbo].[Students] AS S
	INNER JOIN [dbo].[AspNetUsers] AS U ON U.Id = S.AspNetUserId
	WHERE AspNetUserId = @AspNetUserId;
END
GO
