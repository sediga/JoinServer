USE [JoinDB]
GO
/****** Object:  Table [dbo].[Devices]    Script Date: 4/26/2019 5:11:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Devices](
	[DeviceId] [nvarchar](50) NOT NULL,
	[EmailId] [nvarchar](256) NULL,
	[SoftwareVersion] [nvarchar](256) NULL,
	[NotificationToken] [nvarchar](256) NULL,
	[CreatedOn] [datetime] NULL CONSTRAINT [DF_Devices_CreatedOn]  DEFAULT (getdate())
) ON [PRIMARY]

GO
