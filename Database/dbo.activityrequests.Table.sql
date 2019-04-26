USE [JoinDB]
GO
/****** Object:  Table [dbo].[activityrequests]    Script Date: 4/26/2019 5:11:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[activityrequests](
	[activityrequestid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Table_1_activitynotificationid]  DEFAULT (newid()),
	[requestfrom] [nvarchar](50) NOT NULL,
	[requestto] [nvarchar](50) NOT NULL,
	[requestdate] [datetime] NULL,
	[status] [int] NULL,
	[activityid] [uniqueidentifier] NULL,
	[changedate] [datetime] NULL,
	[reqesttype] [int] NULL
) ON [PRIMARY]

GO
