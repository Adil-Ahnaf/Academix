USE AcademixDB;
GO

SET NOCOUNT ON;

-------------------------------------------------------
-- PASSWORD HASHES (ASP.NET Identity V3)
-------------------------------------------------------
DECLARE @AdminHash NVARCHAR(MAX)='AQAAAAIAAYagAAAAELr1fj44vyrW4GHfuLh13O1MPgVpubj68TBIxJQQX9iNbO/PY3ceom9tDMiUlgXeYw==';
DECLARE @TeacherHash NVARCHAR(MAX)='AQAAAAIAAYagAAAAEBi4y9ugmkJUw8C0VlZlnrHEjI2T2pkoN/WqZg34DZXOIR1/2GNjgredcW7dSKy6Sw==';
DECLARE @StudentHash NVARCHAR(MAX)='AQAAAAIAAYagAAAAEMHA0O6nk8l1GhOrBUGVDYTimVquWXdB/ZDTggsxCpJHffTfeKsi1FQOeS8Fvzb7jg==';

-------------------------------------------------------
-- ROLES
-------------------------------------------------------
DECLARE @AdminRoleId NVARCHAR(450)=NEWID();
DECLARE @TeacherRoleId NVARCHAR(450)=NEWID();
DECLARE @StudentRoleId NVARCHAR(450)=NEWID();

INSERT INTO AspNetRoles(Id,Name,NormalizedName,ConcurrencyStamp)
VALUES
(@AdminRoleId,'Admin','ADMIN',NEWID()),
(@TeacherRoleId,'Teacher','TEACHER',NEWID()),
(@StudentRoleId,'Student','STUDENT',NEWID());

-------------------------------------------------------
-- ADMIN USER
-------------------------------------------------------
DECLARE @AdminId NVARCHAR(450)=NEWID();

INSERT INTO AspNetUsers
(
Id,UserName,NormalizedUserName,Email,NormalizedEmail,
EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,
PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,
LockoutEnd,LockoutEnabled,AccessFailedCount,
UserType,IsActive
)
VALUES
(
@AdminId,
'admin@academix.com',
'ADMIN@ACADEMIX.COM',
'admin@academix.com',
'ADMIN@ACADEMIX.COM',
1,
@AdminHash,
NEWID(),
NEWID(),
NULL,
0,
0,
NULL,
1,
0,
0,
1
);

INSERT INTO AspNetUserRoles(UserId,RoleId)
VALUES(@AdminId,@AdminRoleId);

-------------------------------------------------------
-- TEACHERS (5)
-------------------------------------------------------
DECLARE @i INT=1;

WHILE @i<=5
BEGIN

    DECLARE @TeacherId NVARCHAR(450)=NEWID();
    DECLARE @Email NVARCHAR(100)='teacher'+CAST(@i AS NVARCHAR(10))+'@academix.com';
    DECLARE @Name NVARCHAR(100)='teacher'+CAST(@i AS NVARCHAR(10));

    INSERT INTO AspNetUsers
    (
    Id,UserName,NormalizedUserName,Email,NormalizedEmail,
    EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,
    PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,
    LockoutEnd,LockoutEnabled,AccessFailedCount,
    UserType,IsActive
    )
    VALUES
    (
    @TeacherId,
    @Email,
    UPPER(@Email),
    @Email,
    UPPER(@Email),
    1,
    @TeacherHash,
    NEWID(),
    NEWID(),
    NULL,
    0,
    0,
    NULL,
    1,
    0,
    1,
    1
    );

    INSERT INTO AspNetUserRoles(UserId,RoleId)
    VALUES(@TeacherId,@TeacherRoleId);

    INSERT INTO Teachers
    (
    AspNetUserId,
    FullName,
    Gender,
    Department,
    ProfileImage,
    TeacherGuid,
    CreatedDate,
    CreatedBy,
    ModifiedDate,
    ModifiedBy,
    IsActive
    )
    VALUES
    (
    @TeacherId,
    @Name,
    CASE WHEN @i%2=0 THEN 'Female' ELSE 'Male' END,
    CASE
        WHEN @i<=2 THEN 'Bangla'
        WHEN @i<=4 THEN 'English'
        ELSE 'Mathematics'
    END,
    NULL,
    NEWID(),
    GETDATE(),
    @AdminId,
    NULL,
    NULL,
    1
    );

    SET @i=@i+1;
END

-------------------------------------------------------
-- STUDENTS (30)
-------------------------------------------------------
SET @i=1;

WHILE @i<=30
BEGIN

    DECLARE @StudentId NVARCHAR(450)=NEWID();
    DECLARE @StudentEmail NVARCHAR(100)='student'+CAST(@i AS NVARCHAR(10))+'@academix.com';
    DECLARE @StudentName NVARCHAR(100)='student'+CAST(@i AS NVARCHAR(10));

    INSERT INTO AspNetUsers
    (
    Id,UserName,NormalizedUserName,Email,NormalizedEmail,
    EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,
    PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,
    LockoutEnd,LockoutEnabled,AccessFailedCount,
    UserType,IsActive
    )
    VALUES
    (
    @StudentId,
    @StudentEmail,
    UPPER(@StudentEmail),
    @StudentEmail,
    UPPER(@StudentEmail),
    1,
    @StudentHash,
    NEWID(),
    NEWID(),
    NULL,
    0,
    0,
    NULL,
    1,
    0,
    2,
    1
    );

    INSERT INTO AspNetUserRoles(UserId,RoleId)
    VALUES(@StudentId,@StudentRoleId);

    INSERT INTO Students
    (
    AspNetUserId,
    StudentCode,
    FullName,
    Gender,
    StudentGuid,
    CreatedDate,
    CreatedBy,
    ModifiedDate,
    ModifiedBy,
    IsActive
    )
    VALUES
    (
    @StudentId,
    'STD'+RIGHT('000'+CAST(@i AS NVARCHAR(3)),3),
    @StudentName,
    CASE WHEN @i%2=0 THEN 'Female' ELSE 'Male' END,
    NEWID(),
    GETDATE(),
    @AdminId,
    NULL,
    NULL,
    1
    );

    SET @i=@i+1;
END

-------------------------------------------------------
-- SUBJECTS
-------------------------------------------------------
INSERT INTO Subjects
(
Name,
CreatedDate,
CreatedBy,
ModifiedDate,
ModifiedBy,
IsActive
)
VALUES
('Bangla',GETDATE(),@AdminId,NULL,NULL,1),
('English',GETDATE(),@AdminId,NULL,NULL,1),
('Math',GETDATE(),@AdminId,NULL,NULL,1);

PRINT 'Seed data inserted successfully.';
GO