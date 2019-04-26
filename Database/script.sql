USE [master]
GO
/****** Object:  Database [JoinDB]    Script Date: 4/26/2019 5:12:50 PM ******/
CREATE DATABASE [JoinDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'JoinDB', FILENAME = N'D:\Programs\Sql Express\MSSQL12.SQLEXPRESS\MSSQL\DATA\JoinDB.mdf' , SIZE = 336896KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'JoinDB_log', FILENAME = N'D:\Programs\Sql Express\MSSQL12.SQLEXPRESS\MSSQL\DATA\JoinDB_log.ldf' , SIZE = 1108800KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [JoinDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JoinDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JoinDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JoinDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JoinDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JoinDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JoinDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [JoinDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [JoinDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JoinDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JoinDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JoinDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JoinDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JoinDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JoinDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JoinDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JoinDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [JoinDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JoinDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JoinDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JoinDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JoinDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JoinDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [JoinDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JoinDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [JoinDB] SET  MULTI_USER 
GO
ALTER DATABASE [JoinDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [JoinDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [JoinDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [JoinDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [JoinDB] SET DELAYED_DURABILITY = DISABLED 
GO
USE [JoinDB]
GO
/****** Object:  User [IIS APPPOOL\DefaultAppPool]    Script Date: 4/26/2019 5:12:50 PM ******/
CREATE USER [IIS APPPOOL\DefaultAppPool] FOR LOGIN [IIS APPPOOL\DefaultAppPool] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\DefaultAppPool]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 4/26/2019 5:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Activity]    Script Date: 4/26/2019 5:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Activity](
	[deviceid] [nvarchar](50) NULL,
	[what] [nvarchar](64) NULL,
	[when] [datetime] NULL,
	[lat] [decimal](18, 6) NULL,
	[long] [decimal](18, 6) NULL,
	[image] [varbinary](max) NULL,
	[description] [varchar](100) NULL,
	[Id] [uniqueidentifier] ROWGUIDCOL  NULL CONSTRAINT [DF_Activity_Id]  DEFAULT (newid()),
	[ImagePath] [nvarchar](max) NULL,
	[activityowner] [nvarchar](256) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Activity_backup]    Script Date: 4/26/2019 5:12:50 PM ******/
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
/****** Object:  Table [dbo].[activitynotifications]    Script Date: 4/26/2019 5:12:50 PM ******/
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
/****** Object:  Table [dbo].[activityrequests]    Script Date: 4/26/2019 5:12:50 PM ******/
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
/****** Object:  Table [dbo].[activityrequests_old]    Script Date: 4/26/2019 5:12:50 PM ******/
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
/****** Object:  Table [dbo].[activitysettings]    Script Date: 4/26/2019 5:12:50 PM ******/
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
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 4/26/2019 5:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 4/26/2019 5:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 4/26/2019 5:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 4/26/2019 5:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 4/26/2019 5:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CurrentLocation]    Script Date: 4/26/2019 5:12:50 PM ******/
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
/****** Object:  Table [dbo].[DeviceNotifications]    Script Date: 4/26/2019 5:12:50 PM ******/
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
/****** Object:  Table [dbo].[Devices]    Script Date: 4/26/2019 5:12:50 PM ******/
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
/****** Object:  Table [dbo].[Profile]    Script Date: 4/26/2019 5:12:50 PM ******/
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
/****** Object:  Table [dbo].[ProfileReviews]    Script Date: 4/26/2019 5:12:50 PM ******/
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
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
USE [master]
GO
ALTER DATABASE [JoinDB] SET  READ_WRITE 
GO
