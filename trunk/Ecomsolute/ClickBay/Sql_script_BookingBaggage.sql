USE [ClickBay1]
GO

/****** Object:  Table [dbo].[BookingBaggage]    Script Date: 08/12/2014 9:05:38 SA ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BookingBaggage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookingInfoFlightId] [int] NOT NULL,
	[PassengerType] [smallint] NOT NULL,
	[Baggage] [int] NOT NULL,
	[BaggageFee] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_BookingBaggage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BookingBaggage] ADD  CONSTRAINT [DF_BookingBaggage_Baggage]  DEFAULT ((0)) FOR [Baggage]
GO

ALTER TABLE [dbo].[BookingBaggage] ADD  CONSTRAINT [DF_BookingBaggage_BaggageFee]  DEFAULT ((0)) FOR [BaggageFee]
GO


