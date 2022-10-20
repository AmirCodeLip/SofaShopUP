Create Database ArchiveDB
ON
Primary
(Name=IOTASystem,FileName = 'D:\FileData\IOTASystem.mdf'),
FileGroup IOTAFileStreams Contains FileStream 
(Name=IOTAFiles,FileName='D:\FileData\IOTAFiles')
LOG ON (Name=IOTALogs,FileName='D:\FileData\IOTALogs')
GO