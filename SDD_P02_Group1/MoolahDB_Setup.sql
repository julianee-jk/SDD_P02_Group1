USE master
IF EXISTS(select * from sys.databases where name='MoolahDB')
DROP DATABASE MoolahDB
GO

CREATE DATABASE MoolahDB
GO

USE MoolahDB
GO

/*** Delete tables (if they exist) before creating ***/


IF EXISTS (SELECT * FROM sysobjects 
  WHERE id = object_id('dbo.AccountHolder') AND sysstat & 0xf = 3)
  DROP TABLE dbo.AccountHolder
GO

IF EXISTS (SELECT * FROM sysobjects 
  WHERE id = object_id('dbo.JointAccountInfo') AND sysstat & 0xf = 3)
  DROP TABLE dbo.JointAccountInfo
GO

IF EXISTS (SELECT * FROM sysobjects 
  WHERE id = object_id('dbo.AccountVerification') AND sysstat & 0xf = 3)
  DROP TABLE dbo.AccountVerification
GO


/*** Create tables ***/

CREATE TABLE dbo.AccountUser
(
	UserID		int				NOT NULL IDENTITY,
	Username	varchar(50)		NOT NULL,
	Password	varchar(50)		NOT NULL,
	Email		varchar(100)	NOT NULL,
	CONSTRAINT PK_AccountUser PRIMARY KEY NONCLUSTERED (UserID)
)
GO

CREATE TABLE dbo.UserAsset
(
	AssetID			int				NOT NULL IDENTITY,
	AssetName		varchar(50)		NOT NULL,
	InitialValue	money			NOT NULL,
	CurrentValue	money			NOT NULL,
	PredictedValue	money			NULL,
	UserID			int				NOT NULL,
	CONSTRAINT PK_UserAsset PRIMARY KEY NONCLUSTERED (AssetID),
	CONSTRAINT FK_UserAsset_UserID FOREIGN KEY (UserID)
    REFERENCES dbo.AccountUser(UserID)
)
GO

CREATE TABLE dbo.UserWeeklySpending
(
	SpendingID			int			NOT NULL IDENTITY,
	FirstDateOfWeek		datetime	NOT NULL,
	MonSpending			money		NULL,
	TueSpending			money		NULL,
	WedSpending			money		NULL,
	ThuSpending			money		NULL,
	FriSpending			money		NULL,
	SatSpending			money		NULL,
	SunSpending			money		NULL,
	TotalSpending		money		NULL,
	UserID				int			NOT NULL,
	CONSTRAINT PK_UserWeeklySpending PRIMARY KEY NONCLUSTERED (SpendingID),
	CONSTRAINT FK_UserWeeklySpending_UserID FOREIGN KEY (UserID)
    REFERENCES dbo.AccountUser(UserID)
)
GO


/*** Dummy Values ***/


INSERT INTO AccountUser VALUES ('John','john123','john@gmail.com')
INSERT INTO AccountUser VALUES ('Andy','andy123','andy@outlook.com')

INSERT INTO UserAsset VALUES ('Apple Inc',$100,$120,NULL,1)
INSERT INTO UserAsset VALUES ('Microsoft Corp',$200,$190,NULL,1)

INSERT INTO UserAsset VALUES ('Tesla Inc',$500,$550,NULL,2)
INSERT INTO UserAsset VALUES ('Nvidia Corp',$300,$270,NULL,2)

INSERT INTO UserWeeklySpending VALUES ('2021/11/01',$50,$30,$100,$15,$79,$230,$170,$647,1)
INSERT INTO UserWeeklySpending VALUES ('2021/11/01',$10,$7,$10,$9,$21,$47,$84,$188,2)
