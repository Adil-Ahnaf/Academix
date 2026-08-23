USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllActiveClasses] 
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT *, S.Name AS SubjectName
	FROM [dbo].[Classes] AS C
	INNER JOIN [dbo].[Subjects] AS S ON S.Id = C.SubjectId
	WHERE C.IsActive = 1;
END
GO
