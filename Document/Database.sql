USE [master]
GO
/****** Object:  Database [ThanhToanDiDong]    Script Date: 01/05/2014 11:05:06 CH ******/
CREATE DATABASE [ThanhToanDiDong]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ThanhToanDiDong', FILENAME = N'D:\Data\ThanhToanDiDong.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ThanhToanDiDong_log', FILENAME = N'D:\Data\ThanhToanDiDong_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ThanhToanDiDong] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ThanhToanDiDong].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ThanhToanDiDong] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET ARITHABORT OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ThanhToanDiDong] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ThanhToanDiDong] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ThanhToanDiDong] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ThanhToanDiDong] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ThanhToanDiDong] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET RECOVERY FULL 
GO
ALTER DATABASE [ThanhToanDiDong] SET  MULTI_USER 
GO
ALTER DATABASE [ThanhToanDiDong] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ThanhToanDiDong] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ThanhToanDiDong] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ThanhToanDiDong] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ThanhToanDiDong', N'ON'
GO
USE [ThanhToanDiDong]
GO
/****** Object:  Table [dbo].[CardMobile]    Script Date: 01/05/2014 11:05:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CardMobile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryCardMobileId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UnitPrice] [decimal](18, 0) NOT NULL,
	[UnitSellingPrice] [decimal](18, 0) NOT NULL,
	[Deleted] [bit] NOT NULL,
	[Published] [bit] NOT NULL,
	[PictureId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_FrtCardMobile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CategoryCardMobile]    Script Date: 01/05/2014 11:05:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryCardMobile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](300) NOT NULL,
	[Published] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[PictureId] [int] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[IsSupportPayment] [bit] NOT NULL,
	[IsSupportTopup] [bit] NOT NULL,
 CONSTRAINT [PK_FrtCategoryCardMobile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 01/05/2014 11:05:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartnerId] [int] NOT NULL,
	[OrderGuid] [uniqueidentifier] NULL,
	[OrderStatusId] [int] NOT NULL,
	[OrderTypeId] [int] NOT NULL,
	[TotalAmount] [decimal](18, 0) NOT NULL,
	[ProviderId] [int] NOT NULL,
	[NumberPhone] [varchar](100) NULL,
	[CardMobileId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](18, 0) NOT NULL,
	[UnitSellingPrice] [decimal](18, 0) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[OrderNote] [nvarchar](200) NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CustomerIp] [varchar](50) NULL,
	[PaymentStatusId] [int] NOT NULL,
	[PaidDate] [datetime] NULL,
	[AuthorizationTransactionId] [nvarchar](max) NULL,
	[AuthorizationTransactionCode] [nvarchar](max) NULL,
	[AuthorizationTransactionResult] [nvarchar](max) NULL,
	[ResultCode] [int] NOT NULL,
	[ChannelCode] [int] NULL,
	[ResultName] [nvarchar](max) NULL,
	[Sessionid] [nvarchar](max) NULL,
	[DataSign] [nvarchar](max) NULL,
	[FunctionNameFinalCall] [nvarchar](250) NULL,
	[FunctionFinalReturnCode] [int] NULL,
	[IsReceiptBill] [bit] NOT NULL,
	[CompanyName] [nvarchar](200) NULL,
	[CompanyAddress] [nvarchar](200) NULL,
	[TaxCode] [varchar](20) NULL,
	[RecipientBillName] [nvarchar](200) NULL,
	[RecipientBillPhone] [nvarchar](200) NULL,
	[RecipientBillAddress] [nvarchar](200) NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderNote]    Script Date: 01/05/2014 11:05:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderNote](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[FunctionName] [nvarchar](100) NULL,
	[FunctionReturnCode] [int] NULL,
	[FunctionMessage] [nvarchar](max) NULL,
 CONSTRAINT [PK_OrderNote] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PromotionEvent]    Script Date: 01/05/2014 11:05:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PromotionEvent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Published] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_PromotionEvent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Provider]    Script Date: 01/05/2014 11:05:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provider](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProviderCode] [nvarchar](200) NULL,
	[ProviderName] [nvarchar](200) NOT NULL,
	[ServiceCode] [nvarchar](200) NULL,
	[ServiceId] [int] NOT NULL,
	[Published] [bit] NOT NULL,
	[IsOnline] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Provider] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Service]    Script Date: 01/05/2014 11:05:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceCode] [nvarchar](200) NOT NULL,
	[ServiceName] [nvarchar](max) NOT NULL,
	[Published] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[CardMobile] ON 

INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (2, 1, N'Thẻ cào viettel 10k', CAST(10000 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (3, 1, N'Thẻ cào viettel 20k', CAST(20000 AS Decimal(18, 0)), CAST(20000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (4, 1, N'Thẻ cào viettel 50k', CAST(50000 AS Decimal(18, 0)), CAST(48000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (5, 1, N'Thẻ cào viettel 100k', CAST(100000 AS Decimal(18, 0)), CAST(96000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (6, 1, N'Thẻ cào viettel 200k', CAST(200000 AS Decimal(18, 0)), CAST(192000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (7, 1, N'Thẻ cào viettel 300k', CAST(300000 AS Decimal(18, 0)), CAST(288000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (8, 1, N'Thẻ cào viettel 500k', CAST(500000 AS Decimal(18, 0)), CAST(480000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (9, 2, N'VinaPhone_20000', CAST(20000 AS Decimal(18, 0)), CAST(18620 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (10, 2, N'VinaPhone_50000', CAST(50000 AS Decimal(18, 0)), CAST(46800 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (11, 2, N'VinaPhone_100000', CAST(100000 AS Decimal(18, 0)), CAST(93600 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (12, 2, N'VinaPhone_200000', CAST(200000 AS Decimal(18, 0)), CAST(188000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (13, 2, N'VinaPhone_500000', CAST(500000 AS Decimal(18, 0)), CAST(480000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (14, 3, N'Thẻ cào mobi 10k', CAST(10000 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (15, 3, N'Thẻ cào mobi 20k', CAST(20000 AS Decimal(18, 0)), CAST(20000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (16, 3, N'Thẻ cào mobi 50k', CAST(50000 AS Decimal(18, 0)), CAST(48000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (17, 3, N'Thẻ cào mobi 100k', CAST(100000 AS Decimal(18, 0)), CAST(96000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (18, 3, N'Thẻ cào mobi 200k', CAST(200000 AS Decimal(18, 0)), CAST(192000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (19, 3, N'Thẻ cào mobi 300k', CAST(300000 AS Decimal(18, 0)), CAST(287000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (20, 3, N'Thẻ cào mobi 500k', CAST(500000 AS Decimal(18, 0)), CAST(480000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (21, 4, N'Thẻ cào beeline 10k', CAST(10000 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (22, 4, N'Thẻ cào beeline 20k', CAST(20000 AS Decimal(18, 0)), CAST(20000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (23, 4, N'Thẻ cào beeline 50k', CAST(50000 AS Decimal(18, 0)), CAST(47 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (24, 4, N'Thẻ cào beeline 100k', CAST(100000 AS Decimal(18, 0)), CAST(92000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (25, 2, N'VinaPhone_10000.0000', CAST(10000 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (26, 2, N'Vinaphone_30000.0000', CAST(30000 AS Decimal(18, 0)), CAST(30000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
INSERT [dbo].[CardMobile] ([Id], [CategoryCardMobileId], [Name], [UnitPrice], [UnitSellingPrice], [Deleted], [Published], [PictureId], [CreatedOn]) VALUES (27, 2, N'Vinaphone_300000.0000', CAST(300000 AS Decimal(18, 0)), CAST(300000 AS Decimal(18, 0)), 0, 1, 0, CAST(0x0000A31E0178E7EB AS DateTime))
SET IDENTITY_INSERT [dbo].[CardMobile] OFF
SET IDENTITY_INSERT [dbo].[CategoryCardMobile] ON 

INSERT [dbo].[CategoryCardMobile] ([Id], [Name], [Published], [Deleted], [PictureId], [DisplayOrder], [CreatedOn], [UpdatedOn], [IsSupportPayment], [IsSupportTopup]) VALUES (1, N'Viettel', 1, 0, 0, 2, CAST(0x0000A31E0177FB3F AS DateTime), CAST(0x0000A31E0177FB3F AS DateTime), 0, 1)
INSERT [dbo].[CategoryCardMobile] ([Id], [Name], [Published], [Deleted], [PictureId], [DisplayOrder], [CreatedOn], [UpdatedOn], [IsSupportPayment], [IsSupportTopup]) VALUES (2, N'Vinaphone', 1, 0, 0, 0, CAST(0x0000A31E0177FB3F AS DateTime), CAST(0x0000A31E0177FB3F AS DateTime), 0, 1)
INSERT [dbo].[CategoryCardMobile] ([Id], [Name], [Published], [Deleted], [PictureId], [DisplayOrder], [CreatedOn], [UpdatedOn], [IsSupportPayment], [IsSupportTopup]) VALUES (3, N'Mobifone', 1, 0, 0, 3, CAST(0x0000A31E0177FB3F AS DateTime), CAST(0x0000A31E0177FB3F AS DateTime), 0, 1)
INSERT [dbo].[CategoryCardMobile] ([Id], [Name], [Published], [Deleted], [PictureId], [DisplayOrder], [CreatedOn], [UpdatedOn], [IsSupportPayment], [IsSupportTopup]) VALUES (4, N'Beeline', 0, 0, 0, 4, CAST(0x0000A31E0177FB3F AS DateTime), CAST(0x0000A31E0177FB3F AS DateTime), 0, 0)
INSERT [dbo].[CategoryCardMobile] ([Id], [Name], [Published], [Deleted], [PictureId], [DisplayOrder], [CreatedOn], [UpdatedOn], [IsSupportPayment], [IsSupportTopup]) VALUES (5, N'Vietnamobile', 0, 0, 0, 5, CAST(0x0000A31E0177FB3F AS DateTime), CAST(0x0000A31E0177FB3F AS DateTime), 0, 1)
SET IDENTITY_INSERT [dbo].[CategoryCardMobile] OFF
SET IDENTITY_INSERT [dbo].[PromotionEvent] ON 

INSERT [dbo].[PromotionEvent] ([Id], [Description], [Published], [CreatedOn], [Deleted]) VALUES (1, N'MobilePhone khuyến mãi 50% từ ngày 30/04/2014 đến hết ngày 02/05/2014', 1, CAST(0x0000A31C00000000 AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[PromotionEvent] OFF
SET IDENTITY_INSERT [dbo].[Provider] ON 

INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (1, N'ACS', N'ACS', N'TTTG', 1, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (2, N'AVG', N'Truyền hình An Viên', N'THSO', 2, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (3, N'BT', N'Cấp Nước Bến Thành', N'NUOC', 3, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (4, N'CL', N'Cấp nước Chợ Lớn', N'NUOC', 3, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (5, N'CMCTI', N'CMCTI', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (6, N'CMCTIDNG', N'CMCTI Đà Nẵng', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (7, N'CMCTIHCM', N'CMCTI Hồ Chí Minh', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (8, N'CMCTIHNI', N'CMCTI Hà Nội', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (9, N'CNNB', N'Cấp nước Nhà Bè', N'NUOC', 3, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (10, N'CNPMH', N'Cấp nước Phú Mỹ Hưng', N'NUOC', 3, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (11, N'CNTA', N'Cấp nước Trung An', N'NUOC', 3, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (12, N'CNTD', N'Cấp Nước Thủ Đức', N'NUOC', 3, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (13, N'CNTH', N'Cấp Nước Tân Hoà', N'NUOC', 3, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (14, N'Consortio', N'Consortio', N'HTCS', 5, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (15, N'CSV', N'Consortio Việt Nam', N'HTCS', 5, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (16, N'EVNHN', N'Điện lực TP.Hà Nội', N'DIEN', 6, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (17, N'EVNSG', N'EVNHCMC', N'DIEN', 6, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (18, N'FPTHN', N'FPT khu vực miền Bắc và miền Trung', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (19, N'FPTSG', N'FPT khu vực miền Nam', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (20, N'HTVC', N'HTVC', N'CAP', 7, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (21, N'HTVC', N'HTVC', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (22, N'HTVCPMH', N'HTVC Phú Mỹ Hưng', N'CAP', 7, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (23, N'MicrosoftVN', N'MicrosoftVN (Demo only)', N'CAP', 7, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (24, N'MOBI', N'Mobifone', N'DTDDTQ', 8, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (25, N'PAYOO', N'Ví điện tử Payoo', N'TMDT', 9, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (26, N'PHT', N'Cấp nước Phú Hòa Tân', N'NUOC', 3, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (27, N'SCTV', N'SCTV (Khác Chi nhánh 6)', N'CAP', 7, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (28, N'SCTV', N'SCTV (Khác Chi nhánh 6)', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (29, N'SCTVTHC', N'SCTV Chi nhánh 6', N'CAP', 7, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (30, N'SFONE', N'Sfone', N'DTDDNT', 10, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (31, N'SGT', N'VNPT Hồ Chí Minh', N'DTCDCD', 11, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (32, N'SGT', N'VNPT Hồ Chí Minh', N'DTCDKD', 12, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (33, N'SGT', N'VNPT Hồ Chí Minh', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (34, N'SPT', N'SPT', N'DTCDCD', 11, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (35, N'SPT', N'SPT', N'DTCDKD', 12, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (36, N'SPT', N'SPT', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (37, N'SST', N'SPT Phú Mỹ Hưng', N'DTCDCD', 11, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (38, N'SST', N'SPT Phú Mỹ Hưng', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (39, N'VIETTEL', N'Viettel', N'DTDDTQ', 8, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (40, N'VIETTEL', N'Viettel', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (41, N'VIETTEL', N'Viettel', N'DTCDCD', 11, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (42, N'VIETTEL', N'Viettel', N'DTCDKD', 12, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (43, N'VINASG', N'Vinaphone Hồ Chí Minh', N'DTDDTQ', 8, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (44, N'VNMOBILE', N'VietnamMobile', N'DTDDTQ', 8, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (45, N'VNMOBILE', N'VietnamMobile', N'DTDDNT', 10, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (46, N'VNPTHN', N'VNPT Hà Nội', N'DTDDTQ', 8, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (47, N'VNPTHN', N'VNPT Hà Nội', N'DTCDCD', 11, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (48, N'VNPTHN', N'VNPT Hà Nội', N'DTCDKD', 12, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (49, N'VNPTHN', N'VNPT Hà Nội', N'NET', 4, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
INSERT [dbo].[Provider] ([Id], [ProviderCode], [ProviderName], [ServiceCode], [ServiceId], [Published], [IsOnline], [CreatedOn], [Deleted]) VALUES (50, N'VTC', N'VTC', N'THSO', 2, 1, 1, CAST(0x0000A31E017A495B AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Provider] OFF
SET IDENTITY_INSERT [dbo].[Service] ON 

INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (1, N'TTTG', N'Thanh toán trả góp', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (2, N'THSO', N'Truyền hình kỹ thuật số', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (3, N'NUOC', N'Nước', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (4, N'NET', N'Internet', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (5, N'HTCS', N'Hỗ trợ cuộc sống', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (6, N'DIEN', N'Điện', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (7, N'CAP', N'Truyền hình cáp', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (8, N'DTDDTQ', N'Điện thoại di động toàn quốc', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (9, N'TMDT', N'Thanh toán TMĐT', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (10, N'DTDDNT', N'Điện thoại di động nội thành', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (11, N'DTCDCD', N'Điện thoại cố định có dây', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
INSERT [dbo].[Service] ([Id], [ServiceCode], [ServiceName], [Published], [CreatedOn], [Deleted]) VALUES (12, N'DTCDKD', N'Điện thoại cố định không dây', 1, CAST(0x0000A31E01799E6E AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Service] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [Service_ServiceId]    Script Date: 01/05/2014 11:05:06 CH ******/
ALTER TABLE [dbo].[Service] ADD  CONSTRAINT [Service_ServiceId] UNIQUE NONCLUSTERED 
(
	[ServiceCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CardMobile] ADD  CONSTRAINT [DF_CardItem_UnitPrice]  DEFAULT ((0)) FOR [UnitPrice]
GO
ALTER TABLE [dbo].[CardMobile] ADD  CONSTRAINT [DF_CardItem_SellPrice]  DEFAULT ((0)) FOR [UnitSellingPrice]
GO
ALTER TABLE [dbo].[CardMobile] ADD  CONSTRAINT [DF_FrtCardMobile_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[CardMobile] ADD  CONSTRAINT [DF_FrtCardMobile_PurchasingPrice]  DEFAULT ((0)) FOR [Published]
GO
ALTER TABLE [dbo].[CardMobile] ADD  CONSTRAINT [DF_FrtCardMobile_PictureId]  DEFAULT ((0)) FOR [PictureId]
GO
ALTER TABLE [dbo].[CategoryCardMobile] ADD  CONSTRAINT [DF_TypeCard_Active]  DEFAULT ((1)) FOR [Published]
GO
ALTER TABLE [dbo].[CategoryCardMobile] ADD  CONSTRAINT [DF_TypeCard_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[CategoryCardMobile] ADD  CONSTRAINT [DF_CategoryCardMobile_PictureId]  DEFAULT ((0)) FOR [PictureId]
GO
ALTER TABLE [dbo].[CategoryCardMobile] ADD  CONSTRAINT [DF_CategoryCardMobile_DisplayOrder]  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[CategoryCardMobile] ADD  CONSTRAINT [DF_FrtCategoryCardMobile_IsSupportPayment]  DEFAULT ((0)) FOR [IsSupportPayment]
GO
ALTER TABLE [dbo].[CategoryCardMobile] ADD  DEFAULT ((1)) FOR [IsSupportTopup]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_PartnerId]  DEFAULT ((0)) FOR [PartnerId]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_OrderStatusId]  DEFAULT ((0)) FOR [OrderStatusId]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_OrderTypeId]  DEFAULT ((0)) FOR [OrderTypeId]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_ProviderId]  DEFAULT ((0)) FOR [ProviderId]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_CardMobileId]  DEFAULT ((0)) FOR [CardMobileId]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_Quantity]  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_UnitPrice]  DEFAULT ((0)) FOR [UnitPrice]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_UnitSellingPrice]  DEFAULT ((0)) FOR [UnitSellingPrice]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_PaymentStatusId]  DEFAULT ((0)) FOR [PaymentStatusId]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_ResultCode]  DEFAULT ((0)) FOR [ResultCode]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_ChannelCode]  DEFAULT ((0)) FOR [ChannelCode]
GO
ALTER TABLE [dbo].[PromotionEvent] ADD  CONSTRAINT [DF_PromotionEvent_Published]  DEFAULT ((0)) FOR [Published]
GO
ALTER TABLE [dbo].[PromotionEvent] ADD  CONSTRAINT [DF_PromotionEvent_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Provider] ADD  CONSTRAINT [DF_Provider_IsOnline]  DEFAULT ((1)) FOR [IsOnline]
GO
ALTER TABLE [dbo].[Provider] ADD  CONSTRAINT [DF_Provider_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[Service] ADD  CONSTRAINT [DF_Service_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[CardMobile]  WITH CHECK ADD  CONSTRAINT [FK_CardMobile_CategoryCardMobile] FOREIGN KEY([CategoryCardMobileId])
REFERENCES [dbo].[CategoryCardMobile] ([Id])
GO
ALTER TABLE [dbo].[CardMobile] CHECK CONSTRAINT [FK_CardMobile_CategoryCardMobile]
GO
ALTER TABLE [dbo].[OrderNote]  WITH CHECK ADD  CONSTRAINT [FK_OrderNote_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderNote] CHECK CONSTRAINT [FK_OrderNote_Order]
GO
ALTER TABLE [dbo].[Provider]  WITH CHECK ADD  CONSTRAINT [FK_Provider_Service] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([Id])
GO
ALTER TABLE [dbo].[Provider] CHECK CONSTRAINT [FK_Provider_Service]
GO
USE [master]
GO
ALTER DATABASE [ThanhToanDiDong] SET  READ_WRITE 
GO
