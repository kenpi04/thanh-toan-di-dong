SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BookingInfoCondition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookingInfoFlightId] [int] NOT NULL,
	[ConditionType] [nvarchar](100) NOT NULL,
	[ConditionDescription] [nvarchar](400) NULL,
 CONSTRAINT [PK_BookingInfoCondition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BookingInfoCondition]  WITH CHECK ADD  CONSTRAINT [FK_BookingInfoCondition_BookingInfoFlight] FOREIGN KEY([BookingInfoFlightId])
REFERENCES [dbo].[BookingInfoFlight] ([Id])
GO

ALTER TABLE [dbo].[BookingInfoCondition] CHECK CONSTRAINT [FK_BookingInfoCondition_BookingInfoFlight]
GO


