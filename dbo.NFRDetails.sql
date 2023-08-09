CREATE TABLE [dbo].[NFRDetails] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [applicationName]      VARCHAR (255) NOT NULL,
    [releaseID]            VARCHAR (255) NOT NULL,
    [businessScenario]     VARCHAR (255) NOT NULL,
    [transactionNames]     VARCHAR (255) NOT NULL,
    [SLA]                  FLOAT (53)    NULL,
    [TPS]                  FLOAT (53)    NULL,
    [backendCall]          VARCHAR (255) NULL,
    [callType]             VARCHAR (255) NULL,
	[comments]             VARCHAR (255) NULL,
    [discrepancyIndicator] VARCHAR (255) NULL, 
    [additionalDetails] VARCHAR(255) NULL,
    [createdBy]            VARCHAR (255) NOT NULL,
    [created_date]         DATETIME      DEFAULT (getdate()) NOT NULL,
    [modifiedBy]           VARCHAR (255) NULL,
    [modifed_date]         DATETIME      NULL
    
);

