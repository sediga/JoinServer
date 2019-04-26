USE [JoinDB]
GO
/****** Object:  Table [dbo].[DeviceNotifications]    Script Date: 4/26/2019 5:11:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeviceNotifications](
	[notificationid] [uniqueidentifier] NOT NULL,
	[deviceid] [nvarchar](50) NOT NULL,
	[activityid] [uniqueidentifier] NULL,
	[notificationtext] [nvarchar](1000) NOT NULL,
	[messagestatus] [int] NULL,
	[createdon] [datetime] NULL,
	[updatedon] [datetime] NULL,
	[dismissed] [bit] NULL,
	[messageobject] [varchar](max) NULL,
	[messageobjecttype] [nvarchar](50) NULL,
	[requestid] [uniqueidentifier] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
