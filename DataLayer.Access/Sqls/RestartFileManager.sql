delete WebFileVersionActorOrArtists
delete WebFileVersions  
delete WebFiles
delete WebFolders
CHECKPOINT
GO 
EXEC sp_filestream_force_garbage_collection  'IOTASystem'
GO