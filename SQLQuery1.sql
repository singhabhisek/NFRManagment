USE [C:\USERS\ABHISEK\DOCUMENTS\VISUAL STUDIO 2022\REPOS\NFRPRO\NFRPRO\APP_DATA\DATABASE.MDF]
GO

/****** Object:  Table [dbo].[NFROperationDependency]    Script Date: 8/13/2023 8:22:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NFROperationDependency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[applicationName] [varchar](255) NOT NULL,
	[releaseID] [varchar](255) NOT NULL,
	[businessScenario] [varchar](255) NOT NULL,
	[transactionNames] [varchar](255) NOT NULL,
	[backendCall] [varchar](255) NULL,
	[callType] [varchar](255) NULL,
	[comments] [varchar](255) NULL,
	[createdBy] [varchar](255) NOT NULL,
	[created_date] [datetime] NOT NULL,
	[modifiedBy] [varchar](255) NULL,
	[modifed_date] [datetime] NULL
) ON [PRIMARY]
GO


