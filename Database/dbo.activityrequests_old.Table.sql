USE [JoinDB]
GO
/****** Object:  Table [dbo].[activityrequests_old]    Script Date: 4/26/2019 5:11:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[activityrequests_old](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[requestid] [uniqueidentifier] NOT NULL,
	[requestfrom] [nvarchar](256) NULL,
	[requeststatus] [int] NOT NULL,
	[requestto] [nvarchar](256) NULL,
	[createddate] [datetime] NULL,
	[updateddate] [datetime] NULL,
	[activityid] [uniqueidentifier] NULL,
	[requesttype] [int] NULL
) ON [PRIMARY]

GO
