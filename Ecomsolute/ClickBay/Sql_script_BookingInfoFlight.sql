SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[BookingInfoFlight](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Brand] [varchar](50) NOT NULL,
	[AirlinesId] [int] NOT NULL,
	[Adult] [smallint] NOT NULL,
	[Child] [smallint] NOT NULL,
	[Infant] [smallint] NOT NULL,
	[FlightNumber] [varchar](20) NULL,
	[PRNCode] [varchar](50) NULL,
	[DepartDateTime] [datetime] NULL,
	[ArrivalDateTime] [datetime] NULL,
	[TicketType] [nvarchar](50) NULL,
	[FromPlaceId] [int] NOT NULL,
	[ToPlaceId] [int] NOT NULL,
	[FromPlaceCode] [varchar](20) NULL,
	[ToPlaceCode] [varchar](20) NULL,
	[FromPlaceName] [nvarchar](100) NULL,
	[ToPlaceName] [nvarchar](100) NULL,
	[FareBasis] [varchar](50) NULL,
	[IdBooking] [int] NOT NULL,
	[TotalPrice] [decimal](18, 2) NOT NULL,
	[TotalFee] [decimal](18, 2) NOT NULL,
	[TotalTax] [decimal](18, 2) NOT NULL,
	[TotalBaggageFee] [decimal](18, 2) NOT NULL,
	[TotalFeeOther] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[BrandName] [nvarchar](100) NULL,
	[RoundTrip] [bit] NULL,
	[FlightDuration] [int] NULL,
	[Stops] [smallint] NOT NULL,
	[TotalPriceNet] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_BookingInfoFlight] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[BookingInfoFlight] ADD  CONSTRAINT [DF_BookingInfoFlight_AirlinesId]  DEFAULT ((0)) FOR [AirlinesId]
GO

ALTER TABLE [dbo].[BookingInfoFlight] ADD  CONSTRAINT [DF_BookingInfoFlight_TotalPrice]  DEFAULT ((0)) FOR [TotalPrice]
GO

ALTER TABLE [dbo].[BookingInfoFlight] ADD  CONSTRAINT [DF_BookingInfoFlight_TotalFee]  DEFAULT ((0)) FOR [TotalFee]
GO

ALTER TABLE [dbo].[BookingInfoFlight] ADD  CONSTRAINT [DF_BookingInfoFlight_TotalTax]  DEFAULT ((0)) FOR [TotalTax]
GO

ALTER TABLE [dbo].[BookingInfoFlight] ADD  CONSTRAINT [DF_BookingInfoFlight_TotalBaggageFee]  DEFAULT ((0)) FOR [TotalBaggageFee]
GO

ALTER TABLE [dbo].[BookingInfoFlight] ADD  CONSTRAINT [DF_BookingInfoFlight_TotalFeeOther]  DEFAULT ((0)) FOR [TotalFeeOther]
GO

ALTER TABLE [dbo].[BookingInfoFlight] ADD  CONSTRAINT [DF_BookingInfoFlight_DiscountAmount]  DEFAULT ((0)) FOR [DiscountAmount]
GO

ALTER TABLE [dbo].[BookingInfoFlight] ADD  CONSTRAINT [DF_BookingInfoFlight_RoundTrip]  DEFAULT ((0)) FOR [RoundTrip]
GO

ALTER TABLE [dbo].[BookingInfoFlight] ADD  CONSTRAINT [DF_BookingInfoFlight_Stops]  DEFAULT ((0)) FOR [Stops]
GO

ALTER TABLE [dbo].[BookingInfoFlight] ADD  CONSTRAINT [DF_BookingInfoFlight_TotalPriceNet]  DEFAULT ((0)) FOR [TotalPriceNet]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'hang hang khong' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'so luong nguoi lon' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'Adult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'so luong tre em' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'Child'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'so luong tre so sinh' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'Infant'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ma chuyen bay' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'FlightNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PRN Code thuc' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'PRNCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Thoi gian khoi hanh' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'DepartDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Thoi gian den' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'ArrivalDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loai ve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'TicketType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id noi di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'FromPlaceId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id noi den' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'ToPlaceId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ma noi di. Ap dung cho VNA,Abacus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'FromPlaceCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ma noi den. Ap dung cho VNA,Abacus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'ToPlaceCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ten noi di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'FromPlaceName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ten noi den' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'ToPlaceName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hang ghe. Ap dung cho VNA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'FareBasis'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id do api tra ve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'IdBooking'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong gia = SUM( TotalPrice _ BookingPriceDetail )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'TotalPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong phi = SUM ( TotalFee _ BookingPriceDetail )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'TotalFee'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong thue = SUM ( TotalTax _ BookingPriceDetail )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'TotalTax'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong phi hanh ly cong them = SUM ( BaggageFee )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'TotalBaggageFee'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong phi khac = SUM ( TotalFeeOther _ BookingPriceDetail)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'TotalFeeOther'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So tien giam' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingInfoFlight', @level2type=N'COLUMN',@level2name=N'DiscountAmount'
GO


