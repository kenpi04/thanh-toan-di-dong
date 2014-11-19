CREATE TABLE [dbo].[Booking](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactName] [nvarchar](100) NOT NULL,
	[ContactGender] [nvarchar](10) NOT NULL,
	[ContactPhone] [nvarchar](50) NOT NULL,
	[ContactEmail] [varchar](50) NULL,
	[ContactAddress] [nvarchar](200) NULL,
	[ContactCountryId] [int] NOT NULL,
	[ContactCityName] [nvarchar](100) NULL,
	[ContactBirthDate] [datetime] NULL,
	[ContactRequestMore] [nvarchar](500) NULL,
	[VoucherCode] [varchar](20) NULL,
	[Brand] [varchar](50) NOT NULL,
	[Adult] [smallint] NOT NULL,
	[Child] [smallint] NOT NULL,
	[Infant] [smallint] NOT NULL,
	[RoundTrip] [bit] NOT NULL,
	[DepartDate] [datetime] NOT NULL,
	[ReturnDate] [datetime] NULL,
	[FlightNumber] [varchar](20) NOT NULL,
	[TicketPrice] [decimal](18, 2) NOT NULL,
	[TicketType] [varchar](20) NULL,
	[FromPlaceId] [int] NOT NULL,
	[ToPlaceId] [int] NOT NULL,
	[FromPlaceCode] [varchar](20) NOT NULL,
	[ToPlaceCode] [varchar](20) NOT NULL,
	[FareBasis] [varchar](50) NULL,
	[ReturnFareBasis] [varchar](20) NULL,
	[CurrencyType] [varchar](10) NULL,
	[CallBackUrl] [nvarchar](500) NULL,
	[PaymentMethodId] [smallint] NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[PaymentAmount] [decimal](18, 2) NOT NULL,
	[IdBooking] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_Adult]  DEFAULT ((1)) FOR [Adult]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_Child]  DEFAULT ((0)) FOR [Child]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_Infant]  DEFAULT ((0)) FOR [Infant]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_RoundTrip]  DEFAULT ((0)) FOR [RoundTrip]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_TicketPrice]  DEFAULT ((0)) FOR [TicketPrice]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_FromPlaceId]  DEFAULT ((0)) FOR [FromPlaceId]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_ToPlaceId]  DEFAULT ((0)) FOR [ToPlaceId]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_FromPlaceCode]  DEFAULT ('') FOR [FromPlaceCode]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_ToPlaceCode]  DEFAULT ('') FOR [ToPlaceCode]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_PaymentMethodId]  DEFAULT ((0)) FOR [PaymentMethodId]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_TotalAmount]  DEFAULT ((0)) FOR [TotalAmount]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_DiscountAmount]  DEFAULT ((0)) FOR [DiscountAmount]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_IdBooking]  DEFAULT ((0)) FOR [IdBooking]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ten nguoi lien lac' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ContactName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quy danh' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ContactGender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dien thoai lien he' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ContactPhone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email lien he' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ContactEmail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dia chi lien he' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ContactAddress'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id quoc gia lien he' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ContactCountryId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ten thanh pho lien he' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ContactCityName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngay sinh lien he' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ContactBirthDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Yeu cau them cu nguoi lien he' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ContactRequestMore'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ma code giam gia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'VoucherCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hang hang khong' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So luong nguoi lon' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'Adult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So luong tre em' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'Child'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So luong em be' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'Infant'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Co phai khu hoi: 1: co; 0: khong' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'RoundTrip'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngay di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'DepartDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngay ve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ReturnDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ma chuyen bay' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'FlightNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Gia ve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'TicketPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loai ve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'TicketType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id ma noi di: ko ap dung cho VNA, Abacus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'FromPlaceId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id ma noi den: ko ap dung cho VNA, Abacus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ToPlaceId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ma noi di. Ap dung cho VNA,Abacus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'FromPlaceCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ma noi den. Ap dung cho VNA,Abacus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ToPlaceCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hang ghe luc di. Ap dung cho VNA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'FareBasis'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hang ghe luc ve. Ap dung cho VNA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ReturnFareBasis'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ma tien te' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'CurrencyType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Url tra ket qua ve web' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'CallBackUrl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ma phuong thuc thanh toan: 1-cong thanh toan, 2-tien mat, 3-qua ATM' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'PaymentMethodId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong tien' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'TotalAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So tien giam gia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'DiscountAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So tien phai tra = TotalAmount - DiscountAmount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'PaymentAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id booking tra ve tu he thong API' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'IdBooking'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngay tao' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'CreatedOn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngay cap nhat cuoi cung' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'UpdatedOn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'La da xoa' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'Deleted'
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


