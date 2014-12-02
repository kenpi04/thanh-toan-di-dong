USE [ClickBay]
GO

/****** Object:  Table [dbo].[BookingPassenger]    Script Date: 02/12/2014 5:36:56 CH ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[BookingPassenger](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookingId] [int] NOT NULL,
	[PassengerType] [smallint] NOT NULL,
	[Gender] [varchar](10) NULL,
	[Title] [varchar](10) NULL,
	[FirstName] [nvarchar](20) NULL,
	[LastName] [nvarchar](20) NULL,
	[MiddleName] [nvarchar](20) NULL,
	[MobileNumber] [nvarchar](20) NULL,
	[Baggage] [int] NOT NULL,
	[ReturnBaggage] [int] NOT NULL,
	[BirthDay] [datetime] NULL,
	[Email] [nvarchar](50) NULL,
	[PassportNumber] [nvarchar](50) NULL,
	[PassportExpired] [datetime] NULL,
	[BaggageFee] [decimal](18, 2) NOT NULL,
	[ReturnBaggageFee] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_BookingPassenger] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[BookingPassenger] ADD  CONSTRAINT [DF_BookingPassenger_PassengerType]  DEFAULT ((0)) FOR [PassengerType]
GO

ALTER TABLE [dbo].[BookingPassenger] ADD  CONSTRAINT [DF_BookingPassenger_Baggage]  DEFAULT ((0)) FOR [Baggage]
GO

ALTER TABLE [dbo].[BookingPassenger] ADD  CONSTRAINT [DF_BookingPassenger_ReturnBaggage]  DEFAULT ((0)) FOR [ReturnBaggage]
GO

ALTER TABLE [dbo].[BookingPassenger] ADD  CONSTRAINT [DF_BookingPassenger_BaggageFee]  DEFAULT ((0)) FOR [BaggageFee]
GO

ALTER TABLE [dbo].[BookingPassenger] ADD  CONSTRAINT [DF_BookingPassenger_ReturnBaggageFee]  DEFAULT ((0)) FOR [ReturnBaggageFee]
GO

ALTER TABLE [dbo].[BookingPassenger]  WITH CHECK ADD  CONSTRAINT [FK_BookingPassenger_Booking] FOREIGN KEY([BookingId])
REFERENCES [dbo].[Booking] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BookingPassenger] CHECK CONSTRAINT [FK_BookingPassenger_Booking]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Khoa ngoai Id booking ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'BookingId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loai hanh khach: 0-nguoi lon, 1-tre em; 2-em be' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'PassengerType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Gioi tinh' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'Gender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quy danh' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'Title'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ten' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'FirstName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ho' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'LastName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ten lot' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'MiddleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'so dien thoai' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'MobileNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So kg hanh ly khi di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'Baggage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So kg hanh ly khi ve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'ReturnBaggage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngay sinh' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'BirthDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'Email'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So Passprort : ap dung Abacus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'PassportNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngay het han Passprort : ap dung Abacus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'PassportExpired'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phi hanh ly cong them' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'BaggageFee'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phi hanh ly cong them luot ve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPassenger', @level2type=N'COLUMN',@level2name=N'ReturnBaggageFee'
GO


