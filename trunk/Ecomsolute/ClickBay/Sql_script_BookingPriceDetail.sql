USE [ClickBay]
GO

/****** Object:  Table [dbo].[BookingPriceDetail]    Script Date: 02/12/2014 5:37:14 CH ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[BookingPriceDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookingInfoFlightId] [int] NOT NULL,
	[PassengerType] [smallint] NOT NULL,
	[Quantity] [smallint] NOT NULL,
	[TicketType] [varchar](20) NULL,
	[PriceTicket] [decimal](18, 2) NOT NULL,
	[PriceCostTicket] [decimal](18, 2) NOT NULL,
	[FeeAmount] [decimal](18, 2) NOT NULL,
	[TaxAmount] [decimal](18, 2) NOT NULL,
	[FeeOtherAmount] [decimal](18, 2) NOT NULL,
	[TotalPrice] [decimal](18, 2) NOT NULL,
	[TotalFee] [decimal](18, 2) NOT NULL,
	[TotalTax] [decimal](18, 2) NOT NULL,
	[TotalFeeOther] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_BookingTicket] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingTicket_PassengerType]  DEFAULT ((0)) FOR [PassengerType]
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingTicket_Quantity]  DEFAULT ((1)) FOR [Quantity]
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingTicket_PriceTicketDepart]  DEFAULT ((0)) FOR [PriceTicket]
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingTicket_PriceCostTicketDepart]  DEFAULT ((0)) FOR [PriceCostTicket]
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingTicket_FeeAmountDepart]  DEFAULT ((0)) FOR [FeeAmount]
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingTicket_TaxAmountDepart]  DEFAULT ((0)) FOR [TaxAmount]
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingTicket_FeeOtherAmountDepart]  DEFAULT ((0)) FOR [FeeOtherAmount]
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingTicket_TotalAmount]  DEFAULT ((0)) FOR [TotalPrice]
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingTicket_TotalFee]  DEFAULT ((0)) FOR [TotalFee]
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingTicket_TotalTax]  DEFAULT ((0)) FOR [TotalTax]
GO

ALTER TABLE [dbo].[BookingPriceDetail] ADD  CONSTRAINT [DF_BookingPriceDetail_TotalFeeOther]  DEFAULT ((0)) FOR [TotalFeeOther]
GO

ALTER TABLE [dbo].[BookingPriceDetail]  WITH CHECK ADD  CONSTRAINT [FK_BookingPriceDetail_Booking] FOREIGN KEY([BookingInfoFlightId])
REFERENCES [dbo].[BookingInfoFlight] ([Id])
GO

ALTER TABLE [dbo].[BookingPriceDetail] CHECK CONSTRAINT [FK_BookingPriceDetail_Booking]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id booking' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'BookingInfoFlightId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loai hanh khach: 0-nguoi lon, 1-tre em; 2-em be' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'PassengerType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'so luong' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'Quantity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Gia ve ban luot di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'PriceTicket'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Gia ve (goc) luot di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'PriceCostTicket'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phi luot di (tinh tren gia ve ban)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'FeeAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Thue luot di (tinh theo gia ve ban)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'TaxAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phi khac luot di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'FeeOtherAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong tien = PriceTicket x Quantity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'TotalPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong Phi = FeeAmount x Quantity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'TotalFee'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong thue = TaxAmount x Quantity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'TotalTax'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong phi khac = FeeOtherAmount x Quantity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingPriceDetail', @level2type=N'COLUMN',@level2name=N'TotalFeeOther'
GO

