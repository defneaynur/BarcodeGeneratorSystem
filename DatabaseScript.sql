
--CREATE DATABASE SCRIPT
CREATE DATABASE [BarcodeSystem]
 
--CREATE TABLES SCRIPT

--CREATE Barcodes TABLE
USE [BarcodeSystem]
GO

/****** Object:  Table [dbo].[Barcodes]    Script Date: 25.05.2025 19:11:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Barcodes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Gtin12] [nvarchar](12) NULL,
	[Created] [datetime] NULL,
	[Creator] [nvarchar](50) NULL,
	[Changed] [datetime] NULL,
	[Changer] [nvarchar](50) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Barcode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--CREATE Product TABLE
USE [BarcodeSystem]
GO

/****** Object:  Table [dbo].[Product]    Script Date: 25.05.2025 19:12:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Product](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Sku] [nvarchar](50) NULL,
	[Created] [datetime] NULL,
	[Creator] [nvarchar](50) NULL,
	[Changed] [datetime] NULL,
	[Changer] [nvarchar](50) NULL,
	[IsDeleted] [bit] NULL,
	CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--CREATE Users TABLE
USE [BarcodeSystem]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 25.05.2025 19:14:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[Name] [nvarchar](50) NULL,
	[Created] [datetime] NULL,
	[Creator] [nvarchar](100) NULL,
	[Changed] [datetime] NULL,
	[Changer] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO







