USE [JoinDB]
GO
/****** Object:  Table [dbo].[activitynotifications]    Script Date: 4/26/2019 5:11:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[activitynotifications](
	[notificationid] [uniqueidentifier] NOT NULL,
	[activityid] [uniqueidentifier] NOT NULL,
	[deviceid] [nvarchar](50) NOT NULL,
	[message] [nvarchar](100) NOT NULL,
	[messagestatus] [int] NULL,
	[created] [datetime] NULL,
	[dismissed] [bit] NULL
) ON [PRIMARY]

GO
