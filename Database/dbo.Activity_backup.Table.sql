USE [JoinDB]
GO
/****** Object:  Table [dbo].[Activity_backup]    Script Date: 4/26/2019 5:11:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Activity_backup](
	[deviceid] [nvarchar](50) NULL,
	[what] [nvarchar](64) NULL,
	[when] [datetime] NULL,
	[lat] [decimal](18, 6) NULL,
	[long] [decimal](18, 6) NULL,
	[image] [varbinary](max) NULL,
	[description] [varchar](100) NULL,
	[Id] [uniqueidentifier] ROWGUIDCOL  NULL CONSTRAINT [DF_Activity_backup_Id]  DEFAULT (newid()),
	[ImagePath] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
