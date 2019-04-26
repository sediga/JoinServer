USE [JoinDB]
GO
/****** Object:  Table [dbo].[ProfileReviews]    Script Date: 4/26/2019 5:11:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileReviews](
	[Id] [uniqueidentifier] ROWGUIDCOL  NULL CONSTRAINT [DF_ProfileReviews_Id]  DEFAULT (newid()),
	[reviewfrom] [nvarchar](256) NOT NULL,
	[deviceid] [nvarchar](256) NOT NULL,
	[username] [nvarchar](256) NULL,
	[review] [nvarchar](500) NULL,
	[revieweddate] [datetime] NULL,
	[rating] [float] NULL
) ON [PRIMARY]

GO
