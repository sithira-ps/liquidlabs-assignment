IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'liquidlabs-assignment')
BEGIN
    CREATE DATABASE [liquidlabs-assignment];
END
GO

USE [liquidlabs-assignment];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'countries')
BEGIN

	CREATE TABLE [liquidlabs-assignment].dbo.countries (
		uuid varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS PRIMARY KEY,
		name varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		continent varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		sync_level int NOT NULL
	);
	
END
GO
