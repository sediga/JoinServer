USE [JoinDB]
GO
/****** Object:  Table [dbo].[activitysettings]    Script Date: 4/26/2019 5:11:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[activitysettings](
	[activityid] [uniqueidentifier] NOT NULL,
	[starttime] [datetime] NOT NULL,
	[endtime] [datetime] NOT NULL,
	[activtyType] [int] NULL,
	[activitystatus] [int] NULL,
	[activityreviews] [bigint] NULL,
	[activityviews] [bigint] NULL,
	[comments] [nvarchar](256) NULL
) ON [PRIMARY]

GO
