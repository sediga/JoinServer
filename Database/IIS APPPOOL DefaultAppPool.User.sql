USE [JoinDB]
GO
/****** Object:  User [IIS APPPOOL\DefaultAppPool]    Script Date: 4/26/2019 5:11:40 PM ******/
CREATE USER [IIS APPPOOL\DefaultAppPool] FOR LOGIN [IIS APPPOOL\DefaultAppPool] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\DefaultAppPool]
GO
