SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ArilinesBaggageCondition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AirlinesId] [int] NOT NULL,
	[Baggage] [int] NOT NULL,
	[BaggageFee] [decimal](18, 2) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_ArilinesBaggageCondition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ArilinesBaggageCondition] ADD  CONSTRAINT [DF_ArilinesBaggageCondition_Baggage]  DEFAULT ((0)) FOR [Baggage]
GO

ALTER TABLE [dbo].[ArilinesBaggageCondition] ADD  CONSTRAINT [DF_ArilinesBaggageCondition_BaggageFee]  DEFAULT ((0)) FOR [BaggageFee]
GO

ALTER TABLE [dbo].[ArilinesBaggageCondition] ADD  CONSTRAINT [DF_ArilinesBaggageCondition_DisplayOrder]  DEFAULT ((0)) FOR [DisplayOrder]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id hang hang khong _  Arilines' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArilinesBaggageCondition', @level2type=N'COLUMN',@level2name=N'AirlinesId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'So kg hang lý ky gui' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArilinesBaggageCondition', @level2type=N'COLUMN',@level2name=N'Baggage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phi cong them' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArilinesBaggageCondition', @level2type=N'COLUMN',@level2name=N'BaggageFee'
GO


