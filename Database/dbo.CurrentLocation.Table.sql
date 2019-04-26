USE [JoinDB]
GO
/****** Object:  Table [dbo].[CurrentLocation]    Script Date: 4/26/2019 5:11:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CurrentLocation](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[deviceId] [nchar](100) NULL,
	[Lat] [decimal](18, 6) NULL,
	[Long] [decimal](18, 6) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
