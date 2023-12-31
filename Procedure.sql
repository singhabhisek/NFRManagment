USE [C:\USERS\ABHISEK\DOCUMENTS\VISUAL STUDIO 2022\REPOS\NFRPRO\NFRPRO\APP_DATA\DATABASE.MDF]
GO
/****** Object:  StoredProcedure [dbo].[NFRDependency_InsertUpdate]    Script Date: 8/13/2023 8:33:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[NFRDependency_InsertUpdate]
		@applicationName varchar(255),
		@releaseID varchar(255),
		@businessScenario varchar(255),
		@transactionName varchar(255),
		@backendCall varchar(255) = NULL,
		@callType varchar(255) = NULL,
		@createdBy varchar(255) ,
		@modifiedBy varchar(255) = NULL,
		@retValue varchar(255) OUT
AS
BEGIN
      SET NOCOUNT ON;

	
	IF EXISTS(SELECT 1 FROM NFRDetails WHERE applicationName = @applicationName and businessScenario = @businessScenario and transactionName = @transactionName and releaseID = @releaseID)
	BEGIN
		UPDATE NFROperationDependency SET applicationName = @applicationName, 
		releaseID = @releaseID, 
		businessScenario = @businessScenario , 
		transactionName = @transactionName, 
		backendCall = @backendCall, 
		callType = @callType,
		modifiedBy = @modifiedBy,
		modifed_date = GETDATE()
		WHERE applicationName = @applicationName 
		and businessScenario = @businessScenario
		and transactionName = @transactionName
		and releaseID = @releaseID

		set @retValue = 'UPDATED'
	END
	ELSE
	BEGIN
		INSERT INTO NFROperationDependency([applicationName]
           ,[releaseID]
           ,[businessScenario]
           ,[transactionName]
           ,[backendCall]
           ,[callType],
		   createdBy,
		   created_date) VALUES 
		   (@applicationName
			,@releaseID
			,@businessScenario
			,@transactionName
			,@backendCall
			,@callType,
			@createdBy,
			GETDATE())	
			
			set @retValue = 'INSERTED'
	END   
	
END