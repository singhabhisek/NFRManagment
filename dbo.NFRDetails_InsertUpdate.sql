
CREATE PROCEDURE [dbo].[NFRDetails_InsertUpdate]
		@applicationName varchar(255),
		@releaseID varchar(255),
		@businessScenario varchar(255),
		@transactionNames varchar(255),
		@SLA float,
		@TPS float,
		@backendCall varchar(255) = NULL,
		@callType varchar(255) = NULL,
		@createdBy varchar(255) ,
		@modifiedBy varchar(255) = NULL,
		@retValue varchar(255) OUT
AS
BEGIN
      SET NOCOUNT ON;

	
	IF EXISTS(SELECT 1 FROM NFRDetails WHERE applicationName = @applicationName and businessScenario = @businessScenario and transactionNames = @transactionNames and releaseID = @releaseID)
	BEGIN
		UPDATE NFRDetails SET applicationName = @applicationName, 
		releaseID = @releaseID, 
		businessScenario = @businessScenario , 
		transactionNames = @transactionNames, 
		SLA = @SLA, 
		TPS = @TPS, 
		backendCall = @backendCall, 
		callType = @callType,
		modifiedBy = @modifiedBy,
		modifed_date = GETDATE()
		WHERE applicationName = @applicationName 
		and businessScenario = @businessScenario
		and transactionNames = @transactionNames
		and releaseID = @releaseID

		set @retValue = 'UPDATED'
	END
	ELSE
	BEGIN
		INSERT INTO NFRDetails([applicationName]
           ,[releaseID]
           ,[businessScenario]
           ,[transactionNames]
           ,[SLA]
           ,[TPS]
           ,[backendCall]
           ,[callType],
		   createdBy,
		   created_date) VALUES 
		   (@applicationName
			,@releaseID
			,@businessScenario
			,@transactionNames
			,@SLA
			,@TPS
			,@backendCall
			,@callType,
			@createdBy,
			GETDATE())	
			
			set @retValue = 'INSERTED'
	END   
	
END