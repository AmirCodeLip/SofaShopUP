Create Database IOTASystem
ON
Primary
(Name=IOTASystem,FileName = 'D:\FileData\IOTASystem.mdf'),
FileGroup IOTAFileStreams Contains FileStream 
(Name=IOTAFiles,FileName='D:\FileData\IOTAFiles')
LOG ON (Name=IOTALogs,FileName='D:\FileData\IOTALogs')
GO
--<-----update files----->
ALTER TABLE [WebFileVersions] ALTER COLUMN Id ADD ROWGUIDCOL  
GO

ALTER TABLE [WebFileVersions]
ADD CONSTRAINT df_Id
DEFAULT  NEWSEQUENTIALID() FOR Id;

ALTER TABLE [WebFileVersions]
DROP COLUMN FileData;
GO

ALTER TABLE [WebFileVersions]
ADD FileData varbinary(MAX) FILESTREAM NULL;
GO

