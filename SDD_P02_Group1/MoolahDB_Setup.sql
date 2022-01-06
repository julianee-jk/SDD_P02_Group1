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
	AssetType		varchar(50)		NOT NULL,
	AssetDesc		varchar(200)	NULL,
	InitialValue	money			NOT NULL,
	CurrentValue	money			NOT NULL,
	PredictedValue	money			NULL,
	UserID			int				NOT NULL,
	CONSTRAINT PK_UserAsset PRIMARY KEY NONCLUSTERED (AssetID),
	CONSTRAINT FK_UserAsset_UserID FOREIGN KEY (UserID)
    REFERENCES dbo.AccountUser(UserID)
)
GO

CREATE TABLE dbo.UserLiability
(
	LiabilityID			int				NOT NULL IDENTITY,
	LiabilityName		varchar(50)		NOT NULL,
	LiabilityType		varchar(50)		NOT NULL,
	LiabilityDesc		varchar(200)	NULL,
	Cost				money			NOT NULL,
	DueDate				datetime		NULL,
	RecurringType		varchar(50)		NOT NULL,
	RecurringDuration	int				NULL,
	UserID				int				NOT NULL,
	CONSTRAINT PK_UserLiability PRIMARY KEY NONCLUSTERED (LiabilityID),
	CONSTRAINT FK_UserLiability_UserID FOREIGN KEY (UserID)
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

CREATE TABLE dbo.AssetChanges
(
	UserID				int				NOT NULL,
	AssetID				int				NOT NULL,
	Timestamp			VARCHAR(100)	NOT NULL,
	AssetTypeOld		varchar(50)		NULL,
	AssetTypeNew		varchar(50)		NULL,
	AssetDescOld		varchar(200)	NULL,
	AssetDescNew		varchar(200)	NULL,
	CurrentValueOld		money			NULL,
	CurrentValueNew		money			NULL,

	CONSTRAINT PK_AssetChanges PRIMARY KEY NONCLUSTERED (UserID, AssetID, Timestamp),

)
GO


/*** Dummy Values ***/


INSERT INTO AccountUser VALUES ('John','john123','john@gmail.com')
INSERT INTO AccountUser VALUES ('Andy','andy123','andy@outlook.com')

INSERT INTO UserAsset VALUES ('Apple Inc','Stocks',NULL,$100,$120,NULL,1)
INSERT INTO UserAsset VALUES ('Microsoft Corp','Stocks',NULL,$200,$190,NULL,1)

INSERT INTO UserAsset VALUES ('Tesla Inc','Stocks',NULL,$500,$550,NULL,2)
INSERT INTO UserAsset VALUES ('Nvidia Corp','Stocks',NULL,$300,$270,NULL,2)

INSERT INTO UserWeeklySpending VALUES ('2021/11/01',$50,$30,$100,$15,$79,$230,$170,$647,1)
INSERT INTO UserWeeklySpending VALUES ('2021/11/01',$10,$7,$10,$9,$21,$47,$84,$188,2)

INSERT INTO UserLiability VALUES ('Water Bill','Bills','Monthly water bills charged by the government',200,'2022-01-05','Monthly',NULL,1)
INSERT INTO UserLiability VALUES ('Insurance Policies','Bills','Yearly subscription for personal insurance',230,'2021-12-31','Yearly',NULL,1)
INSERT INTO UserLiability VALUES ('Phone Bill','Bills','Phone plan charged by phone company',60,'2021-12-31','Monthly',NULL,2)
INSERT INTO UserLiability VALUES ('64 inch TV Installment','Purchase','24 months installment plan for television',52,'2021-12-31','Monthly',24,2)
INSERT INTO UserLiability VALUES ('Housing Payments','Purchase','2 years till payments are fully paid',370,'2021-12-31','Yearly',2,2)

SELECT * FROM AssetChanges

