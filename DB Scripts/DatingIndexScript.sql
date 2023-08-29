GO
USE NaughtyChoppersDB;
GO

GO
CREATE UNIQUE INDEX idx_users
ON Users (Id)
GO

GO
CREATE UNIQUE INDEX idx_profiles
ON ProfileInformation (Id)
GO

GO
CREATE 