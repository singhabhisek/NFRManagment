select distinct n.[Application Name], n.[Transaction Name],n.[2022.4_SLA]
,n.[2022.4_TPS],n.[2023.6_SLA],n.[2023.6_TPS],n.[2023.7_SLA],n.[2023.7_TPS]
from [NFRDetails] m, (
SELECT a.[applicationName] as 'Application Name',a.[transactionNames] as 'Transaction Name', ISNULL(str(MAX(CASE WHEN releaseID='2022.4' THEN SLA END)),'NA') '2022.4_SLA', ISNULL(str(MAX(CASE WHEN releaseID='2023.6' THEN SLA END)),'NA') '2023.6_SLA',ISNULL(str(MAX(CASE WHEN releaseID='2023.7' THEN SLA END)),'NA') '2023.7_SLA', ISNULL(str(MAX(CASE WHEN releaseID='2022.4' THEN TPS END)),'NA') '2022.4_TPS', ISNULL(str(MAX(CASE WHEN releaseID='2023.6' THEN TPS END)),'NA') '2023.6_TPS', ISNULL(str(MAX(CASE WHEN releaseID='2023.7' THEN TPS END)),'NA') '2023.7_TPS'  FROM [dbo].[NFRDetails] a WHERE applicationName = 'Mobile' GROUP BY a.[applicationName],a.[transactionNames]
) n
where n.[Application Name] = m.applicationName
and n.[Transaction Name] = m.transactionNames
and COALESCE(n.[2022.4_SLA], n.[2023.6_SLA] ,n.[2023.7_SLA]) != 'NA'
;


/*select * from (select * from NFRDetails)*/