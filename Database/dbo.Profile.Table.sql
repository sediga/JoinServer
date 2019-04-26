USE [JoinDB]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 4/26/2019 5:11:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Profile](
	[deviceid] [nchar](100) NOT NULL,
	[username] [nchar](30) NULL,
	[profilephoto] [varbinary](1500) NULL,
	[profilename] [nchar](50) NULL,
	[hobies] [nchar](100) NULL,
	[about] [nchar](500) NULL,
	[rating] [smallint] NULL,
	[reviews] [bigint] NULL,
	[views] [bigint] NULL,
	[imagepath] [nvarchar](246) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
