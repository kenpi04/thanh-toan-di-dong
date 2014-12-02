SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

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
	[RoundTrip] [bit] NOT NULL,
	[BookingInfoFlightToId] [int] NULL,
	[BookingInfoFlightReturnId] [int] NULL,
	[Adult] [smallint] NOT NULL,
	[Child] [smallint] NOT NULL,
	[Infant] [smallint] NOT NULL,
	[CurrencyType] [varchar](10) NULL,
	[CurrentcyRate] [decimal](12, 2) NOT NULL,
	[CallBackUrl] [nvarchar](500) NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[TotalFeeAmount] [decimal](18, 2) NOT NULL,
	[TotalTaxAmount] [decimal](18, 2) NOT NULL,
	[TotalFeeOtherAmount] [decimal](18, 2) NOT NULL,
	[TotalBaggageFeeAmount] [decimal](18, 2) NOT NULL,
	[TotalDiscountAmount] [decimal](18, 2) NOT NULL,
	[PaymentAmount] [decimal](18, 2) NOT NULL,
	[BookingStatusId] [smallint] NOT NULL,
	[PaymentStatusId] [smallint] NOT NULL,
	[PaymentMethodId] [smallint] NOT NULL,
	[ReasonCancel] [nvarchar](200) NULL,
	[ContactStatusId] [smallint] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CustomerReceivedDate] [datetime] NULL,
	[CustomerNote] [nvarchar](200) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_RoundTrip]  DEFAULT ((0)) FOR [RoundTrip]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_Adult]  DEFAULT ((1)) FOR [Adult]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_Child]  DEFAULT ((0)) FOR [Child]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_Infant]  DEFAULT ((0)) FOR [Infant]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Booking_CurrentcyRate]  DEFAULT ((1)) FOR [CurrentcyRate]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_TotalAmount]  DEFAULT ((0)) FOR [TotalAmount]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Booking_FeeAmount]  DEFAULT ((0)) FOR [TotalFeeAmount]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Booking_TaxAmount]  DEFAULT ((0)) FOR [TotalTaxAmount]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Booking_TotalFeeOtherAmount]  DEFAULT ((0)) FOR [TotalFeeOtherAmount]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Booking_TotalBaggageFeeAmount]  DEFAULT ((0)) FOR [TotalBaggageFeeAmount]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_DiscountAmount]  DEFAULT ((0)) FOR [TotalDiscountAmount]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Booking_TicketStatusId]  DEFAULT ((0)) FOR [BookingStatusId]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Booking_PaymentStatusId]  DEFAULT ((0)) FOR [PaymentStatusId]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_PaymentMethodId]  DEFAULT ((0)) FOR [PaymentMethodId]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Booking_ContactStatusId]  DEFAULT ((0)) FOR [ContactStatusId]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Booking_CustomerId]  DEFAULT ((0)) FOR [CustomerId]
GO

ALTER TABLE [dbo].[Booking] ADD  CONSTRAINT [DF_Ticket_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_BookingInfoFlight] FOREIGN KEY([BookingInfoFlightToId])
REFERENCES [dbo].[BookingInfoFlight] ([Id])
GO

ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_BookingInfoFlight]
GO

ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_BookingInfoFlight1] FOREIGN KEY([BookingInfoFlightReturnId])
REFERENCES [dbo].[BookingInfoFlight] ([Id])
GO

ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_BookingInfoFlight1]
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Co phai khu hoi: 1: co; 0: khong' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'RoundTrip'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id thong tin chuyen bay luot di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'BookingInfoFlightToId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id thong tin chuyen bay luot ve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'BookingInfoFlightReturnId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So luong nguoi lon' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'Adult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So luong tre em' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'Child'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So luong em be' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'Infant'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ma tien te' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'CurrencyType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ty gia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'CurrentcyRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Url tra ket qua ve web' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'CallBackUrl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong tien: SUM( TotalAmount_BookingInfoFlight)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'TotalAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong phi = sum( TotalFeeAmount_BookingInfoFlight)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'TotalFeeAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong Thue = Sum( TotalTaxAmount_BookingInfoFlight )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'TotalTaxAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong phi khac: SUM ( TotalFeeOther _ BookingInfoFlight )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'TotalFeeOtherAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tong phi hanh ly cong them: SUM ( TotalBaggageFee  _ BookingInfoFlight )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'TotalBaggageFeeAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So tien giam gia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'TotalDiscountAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So tien phai tra = TotalAmount + TotalFeeAmount + TotalTaxAmount + TotalFeeOtherAmout + TotalBaggageFeeAmout - DiscountAmount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'PaymentAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Trang thai ve: 10-chua xu ly; 20-giu cho; 30-hoan thanh; 40-hoan ve; 50- da huy;(booking thuc va da dua ve cho khach; 40-huy;' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'BookingStatusId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tinh trang thanh toan: 0-chua thanh toan; 10-da thanh toan;' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'PaymentStatusId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ma phuong thuc thanh toan: 1-cong thanh toan, 2-tien mat, 3-qua ATM' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'PaymentMethodId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ly do huy ve khi TicketStatusId = 40' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ReasonCancel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tinh trang lien lac voi khach hang: 0-chua lien lac;10-da lien lac; 20-khong lien lac duoc;' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'ContactStatusId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id Customer  - ma nhan vien' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngay nhan vien tiep nhan' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'CustomerReceivedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nhan vien ghi chu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'CustomerNote'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngay tao' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'CreatedOn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngay cap nhat cuoi cung' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'UpdatedOn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'La da xoa' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Booking', @level2type=N'COLUMN',@level2name=N'Deleted'
GO


