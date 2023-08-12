SELECT a.[applicationName]
      ,a.[transactionNames],
    ISNULL(str(MAX(CASE WHEN releaseID='2023.M02' THEN SLA END)),'NA') M02_SLA,
    MAX(str(CASE WHEN releaseID='2023.M03' THEN SLA END) M03_SLA
FROM [dbo].[NFRProTable] a

GROUP BY a.[applicationName]
      ,a.[transactionNames];