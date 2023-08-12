select ApplicationName, OperationName, SLA, TotalSyncSLA, MaxAsyncSLA, CASE WHEN TotalSyncSLA + MaxAsyncSLA = 0 then 'NA' ELSE CASE WHEN SLA > TotalSyncSLA + MaxAsyncSLA then 'Higher' else 'Lower' END END as 'Compare' FROM ( SELECT ApplicationName, OperationName, SLA, SUM( CASE WHEN CallType = 'Sync' THEN SLAComparison ELSE 0 END ) OVER (PARTITION BY OperationName) AS TotalSyncSLA, MAX( CASE WHEN CallType = 'Async' THEN SLAComparison ELSE 0 END ) OVER (PARTITION BY OperationName) AS MaxAsyncSLA FROM ( SELECT ApplicationName, OperationName, MethodCall, CallType, SLA, CASE WHEN CallType = 'Async' THEN ( SELECT MAX(SLA) FROM NFR WHERE OperationName = t.MethodCall AND t.CallType = 'Async' ) WHEN CallType = 'Sync' THEN ( SELECT SUM(SLA) FROM NFR WHERE OperationName = t.MethodCall AND t.CallType = 'Sync' ) ELSE 0 END AS SLAComparison FROM NFR t where ApplicationName = 'AppName1' ) as x ) as p;