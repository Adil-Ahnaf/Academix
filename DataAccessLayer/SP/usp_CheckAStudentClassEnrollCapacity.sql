USE [AcademixDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[usp_CheckAStudentClassEnrollCapacity]
	@ClassGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @MaximumCapacity INT;
	DECLARE @EnrolledStudent INT;

	-- Set decleared variables value
	SELECT @MaximumCapacity = MaxCapacity FROM [dbo].[Classes] WHERE ClassGuid = @ClassGuid;

	SELECT @EnrolledStudent = COUNT(E.ClassId)
	FROM [dbo].[StudentEnrollments] AS E
	INNER JOIN [dbo].[Classes] AS C ON C.Id = E.ClassId
	WHERE C.ClassGuid = @ClassGuid;
	
	-- Check class student enroll capacity
	IF (@EnrolledStudent < @MaximumCapacity)
	BEGIN
		SELECT 1 AS EnrollAccess 
	END
	ELSE
	BEGIN
		SELECT 0 AS EnrollAccess
	END
END
GO
