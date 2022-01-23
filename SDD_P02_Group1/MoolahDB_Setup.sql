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
  WHERE id = object_id('dbo.AccountUser') AND sysstat & 0xf = 3)
  DROP TABLE dbo.AccountUser
GO

IF EXISTS (SELECT * FROM sysobjects 
  WHERE id = object_id('dbo.UserAsset') AND sysstat & 0xf = 3)
  DROP TABLE dbo.UserAsset
GO

IF EXISTS (SELECT * FROM sysobjects 
  WHERE id = object_id('dbo.UserCard') AND sysstat & 0xf = 3)
  DROP TABLE dbo.UserCard
GO

IF EXISTS (SELECT * FROM sysobjects 
  WHERE id = object_id('dbo.UserCardSpending') AND sysstat & 0xf = 3)
  DROP TABLE dbo.UserCardSpending
GO

IF EXISTS (SELECT * FROM sysobjects 
  WHERE id = object_id('dbo.UserLiability') AND sysstat & 0xf = 3)
  DROP TABLE dbo.UserLiability
GO

IF EXISTS (SELECT * FROM sysobjects 
  WHERE id = object_id('dbo.UserWeeklySpending') AND sysstat & 0xf = 3)
  DROP TABLE dbo.UserWeeklySpending

IF EXISTS (SELECT * FROM sysobjects 
WHERE id = object_id('dbo.UserSpendingRecord') AND sysstat & 0xf = 3)
DROP TABLE dbo.UserSpendingRecord

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

CREATE TABLE dbo.UserCard
(
	CardID			int				NOT NULL IDENTITY,
	CardName		varchar(50)		NOT NULL,
	CardType		varchar(50)		NOT NULL,
	CardDesc		varchar(200)	NULL,
	UserID				int			NOT NULL,
	CONSTRAINT PK_UserCard PRIMARY KEY NONCLUSTERED (CardID),
	CONSTRAINT FK_UserCard_UserID FOREIGN KEY (UserID)
    REFERENCES dbo.AccountUser(UserID)
)
GO

CREATE TABLE dbo.UserCardSpending
(
	CardSpendingID		int			NOT NULL IDENTITY,
	DateOfTransaction	datetime	NOT NULL,
	AmountSpent			money		NOT NULL,
	CardID				int			NOT NULL,
	CONSTRAINT PK_UserCardSpending PRIMARY KEY NONCLUSTERED (CardSpendingID),
	CONSTRAINT FK_UserCardSpending_CardID FOREIGN KEY (CardID)
	REFERENCES dbo.UserCard(CardID)
)
GO

CREATE TABLE dbo.UserSpendingRecord
(
	RecordID			int				NOT NULL IDENTITY,
	DateOfTransaction	datetime		NOT NULL,
	CategoryOfSpending	varchar(50)		NOT NULL,
	AmountSpent			money			NOT NULL,
	UserID				int				NOT NULL,
	CONSTRAINT PK_UserSpendingRecord PRIMARY KEY NONCLUSTERED (RecordID),
	CONSTRAINT FK_UserSpendingRecord_UserID FOREIGN KEY (UserID)
	REFERENCES dbo.AccountUser(UserID)
)
GO

CREATE TABLE dbo.AssetChanges
(
	--AssetChangeID		VARCHAR(50) 	NOT NULL		IDENTITY(1,1),
	AssetChangeID		INT				NOT NULL		IDENTITY(1,1),
	--UserID				VARCHAR(50)		NOT NULL,
	UserID				INT				NOT NULL,
	AssetID				INT				NOT NULL,
	Timestamp			VARCHAR(100)	NOT NULL,
	AssetType			VARCHAR(50)		NOT NULL,
	AssetTypeNew		VARCHAR(50)		NULL,
	AssetDesc			VARCHAR(200)	NOT NULL,
	AssetDescNew		VARCHAR(200)	NULL,
	CurrentValue		MONEY			NOT NULL,
	CurrentValueNew		MONEY			NULL,

	CONSTRAINT PK_AssetChanges PRIMARY KEY NONCLUSTERED (AssetChangeID, Timestamp),
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

INSERT INTO UserCard VALUES ('POSB Everyday Card','Debit','Making a purchase everyday will give 2% rebate',1)
INSERT INTO UserCard VALUES ('OCBC FRANK Card','Credit','Spending at affiliated stores will grant 5% rebate',2)

INSERT INTO UserCardSpending VALUES ('2021-12-12',300,1)
INSERT INTO UserCardSpending VALUES ('2022-01-01',888,2)

INSERT INTO UserSpendingRecord VALUES ('2021-10-10','Food',72,1)
INSERT INTO UserSpendingRecord VALUES ('2021-11-11','Clothes',184,1)
INSERT INTO UserSpendingRecord VALUES ('2022-01-04','Clothes',145,2)

DECLARE @Time AS DATETIME = GETDATE()
INSERT INTO AssetChanges VALUES (1, 1, @Time, 'T1', 'T2', 'D1', 'D2', 1, 2)
INSERT INTO AssetChanges VALUES (1, 1, @Time, 'T1', 'TX', 'D1', 'D2', 1, 2)


/*
SELECT * FROM UserCard
SELECT * FROM UserCardSpending
SELECT * FROM UserSpendingRecord
*/