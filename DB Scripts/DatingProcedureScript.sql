GO
CREATE PROCEDURE AddUser
@Username NVARCHAR(MAX),
@Password NVARCHAR(MAX)
AS
BEGIN
INSERT INTO [Users] ([Username], [Password])
VALUES (@Username, @Password)
END;
GO

GO
CREATE PROCEDURE RemoveUser
@Id UNIQUEIDENTIFIER
AS
BEGIN
DELETE FROM Users
WHERE Id = @Id;
END;
GO

GO
CREATE PROCEDURE GetIdAndUserName
@userName NVARCHAR(MAX) = NULL,
@password NVARCHAR(MAX) = NULL,
@id UNIQUEIDENTIFIER = NULL
AS
BEGIN
SELECT [Id], [Username]
FROM [Users]
WHERE @userName = Username AND @password = [Password] OR @id = [Id];
END
GO


GO
CREATE PROCEDURE CreateProfile(
@Name NVARCHAR(100),
@BirthDate DATE,
@ModelId INT,
@ProfileImg VARBINARY(MAX),
@PostalCode NVARCHAR(10),
@UserId UNIQUEIDENTIFIER
)
AS
BEGIN
INSERT INTO ProfileInformation([Name], Age, Model, ProfileImg, PostalCode, UserId)
VALUES(@Name, @BirthDate, @ModelId, @ProfileImg, @PostalCode, @UserId)
END
GO

GO
CREATE PROCEDURE GetCityByPostalCode(
@PostalCode NVARCHAR(10)
)
AS
BEGIN
SELECT CityName
FROM PostalCodes
WHERE PostalCode = @PostalCode
END
GO

GO
CREATE PROCEDURE GetHelicopterModel(
@HelicopterModelId INT
)
AS
BEGIN
SELECT ModelName
FROM ModelTable
WHERE Id = @HelicopterModelId
END
GO

GO
CREATE PROCEDURE GetModelInterest(
@ProfileId UNIQUEIDENTIFIER
)
AS
BEGIN
SELECT *
FROM ModelTable AS MT
JOIN ProfileModelInterest AS [PMI] ON MT.Id=[PMI].ModelId
WHERE [PMI].ProfileId = @ProfileId
END
GO

GO
CREATE PROCEDURE GetAllModels
AS
BEGIN
SELECT * FROM ModelTable
END
GO

GO
CREATE PROCEDURE AddModelInterestToProfile(
@ProfileId UNIQUEIDENTIFIER,
@ModelId INT
)
AS
BEGIN
INSERT INTO ProfileModelInterest(ProfileId, ModelId)
VALUES (@ProfileId, @ModelId)
END
GO

GO
CREATE PROCEDURE RemoveModelInterestForProfile(
@ProfileId UNIQUEIDENTIFIER,
@ModelInterestId INT
)
AS
BEGIN
DELETE FROM ProfileModelInterest
WHERE @ProfileId = ProfileId AND @ModelInterestId = ModelId
END
GO

GO
CREATE PROCEDURE GetInterests(
@ProfileId UNIQUEIDENTIFIER
)
AS
BEGIN
SELECT *
FROM InterestTable AS IT
JOIN ProfileInterests AS [PI] ON IT.Id=[PI].InterestId
WHERE [PI].ProfileId = @ProfileId
END
GO

GO
CREATE PROCEDURE GetAllInterests
AS
BEGIN
SELECT * FROM InterestTable
END
GO

GO
CREATE PROCEDURE AddInterestToProfile(
@ProfileId UNIQUEIDENTIFIER,
@InterestId INT
)
AS
BEGIN
INSERT INTO ProfileInterests (ProfileId, InterestId)
VALUES (@ProfileId, @InterestId)
END

GO
CREATE PROCEDURE RemoveInterestForProfile(
@ProfileId UNIQUEIDENTIFIER,
@InterestId INT
)
AS
BEGIN
DELETE FROM ProfileInterests
WHERE @ProfileId = ProfileId AND @InterestId = InterestId
END
GO

GO
CREATE PROCEDURE UpdateProfile(
@ProfileId UNIQUEIDENTIFIER,
@Name NVARCHAR(100),
@Age DATE,
@ModelId INT,
@ProfileImg VARBINARY(MAX),
@PostalCode NVARCHAR(10)
)
AS
BEGIN
UPDATE ProfileInformation
SET [Name] = @Name, Age = @Age, Model = @ModelId, ProfileImg = @ProfileImg, PostalCode = @PostalCode
WHERE Id = @ProfileId
END
GO

GO
CREATE PROCEDURE RemoveAllInterests(
@ProfileId UNIQUEIDENTIFIER
)
AS
BEGIN
DELETE FROM ProfileInterests
WHERE ProfileId = @ProfileId
END
GO

GO
CREATE PROCEDURE RemoveAllModelInterests(
@ProfileId UNIQUEIDENTIFIER
)
AS
BEGIN
DELETE FROM ProfileModelInterest
WHERE ProfileId = @ProfileId
END
GO

GO
CREATE PROCEDURE DoesUserExist(
@Username NVARCHAR(255)
)
AS
BEGIN
SELECT COUNT(1)
FROM Users
WHERE Username = @Username;
END
GO

GO
CREATE PROCEDURE DeleteProfile(
@ProfileIdToDelete UNIQUEIDENTIFIER
)
AS
BEGIN

EXEC RemoveAllInterests
@ProfileId = @ProfileIdToDelete;

EXEC RemoveAllModelInterests
@ProfileId = @ProfileIdToDelete;

DELETE FROM ProfileInformation
WHERE Id = @ProfileIdToDelete;
END
GO

GO
CREATE PROCEDURE GetProfileByUserId(
@UserId UNIQUEIDENTIFIER
)
AS
BEGIN
SELECT * FROM ProfileInformation
WHERE UserId = @UserId
END
GO

GO
CREATE PROCEDURE GetProfileId(
@UserId UNIQUEIDENTIFIER
)
AS
BEGIN
SELECT Id FROM ProfileInformation
WHERE UserId = @UserId
END
GO