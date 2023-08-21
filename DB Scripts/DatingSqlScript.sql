GO
USE [master]
GO

GO
CREATE DATABASE NaughtyChoppersDB;
GO

GO
USE NaughtyChoppersDB;
GO

GO
CREATE TABLE Users (
[Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
[Username] NVARCHAR(255) UNIQUE,
[Password] NVARCHAR(MAX)
)
GO

GO
CREATE TABLE [ModelTable](
Id INT PRIMARY KEY IDENTITY(1,1),
ModelName NVARCHAR(MAX)
)
GO

GO
INSERT INTO [ModelTable] (ModelName)
VALUES
('CH-47 Chinook'),
('AH-64 Apache'),
('V-22 Osprey'),
('MH-139A Grey Wolf'),
('Tiger HAD'),
('NH90 TTH'),
('H225M')

GO

GO
CREATE TABLE PostalCodes (
    PostalCode NVARCHAR(10) PRIMARY KEY,
    CityName NVARCHAR(50) COLLATE Latin1_General_CI_AS
);
GO


GO
BULK INSERT PostalCodes
FROM 'C:\tempfold\postalcode.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
	CODEPAGE = '65001' -- UTF-8
);
GO

GO
CREATE TABLE ProfileInformation(
[Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
[Name] NVARCHAR(MAX),
[Age] DATE,
[Model] INT,
[ProfileImg] VARBINARY(MAX),
[PostalCode] NVARCHAR(10),

[UserId] UNIQUEIDENTIFIER,

FOREIGN KEY ([PostalCode]) REFERENCES [PostalCodes](PostalCode),
FOREIGN KEY ([UserId]) REFERENCES [Users](Id),
FOREIGN KEY ([Model]) REFERENCES [ModelTable](Id)
)
GO

GO
CREATE TABLE [ProfileModelInterest](
ProfileId UNIQUEIDENTIFIER,
ModelId INT,

FOREIGN KEY ([ProfileId]) REFERENCES [ProfileInformation](Id),
FOREIGN KEY ([ModelId]) REFERENCES [ModelTable](Id),

CONSTRAINT UQ_ProfileModelInterest UNIQUE (ProfileId, ModelId)
)
GO

GO
CREATE TABLE [LikesTable](
SenderId UNIQUEIDENTIFIER,
Receiver UNIQUEIDENTIFIER,
LikedBack BIT NULL
)
GO

GO
CREATE TABLE [InterestTable](
Id INT PRIMARY KEY IDENTITY(1,1),
InterestName NVARCHAR(100)
)
GO

GO
INSERT INTO [InterestTable] (InterestName)
VALUES
('Cloud Chasing'),
('Rotor Rhythms'),
('Aerial Adventures'),
('Mechanical Mixology'),
('Sunset Spotting'),
('Propeller Poetry'),
('Altitude Art'),
('Whirlwind Wanderlust'),
('Aviation Acoustics'),
('Sky-High Cuisine'),
('Landing Zone Landscaping'),
('Rotor-Blade Book Club'),
('Aero-Photography'),
('Skywriting Socials'),
('Altitude Yoga'),
('Whirlybird Whimsy'),
('Skyline Symphony'),
('Vertical Vineyards'),
('Hovering Haikus'),
('Avian Artistry'),
('Airborne Astrology'),
('Gyroscopic Gaming'),
('Rotor-therapy'),
('Flightline Fashion'),
('Altitude Architecture')
GO

GO
CREATE TABLE [ProfileInterests](
ProfileId UNIQUEIDENTIFIER,
InterestId INT,

FOREIGN KEY ([ProfileId]) REFERENCES [ProfileInformation](Id),
FOREIGN KEY (InterestId) REFERENCES InterestTable(Id),

CONSTRAINT UQ_ProfileInterest UNIQUE (ProfileId, InterestId)
)
GO
