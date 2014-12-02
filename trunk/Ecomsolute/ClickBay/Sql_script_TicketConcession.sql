SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TicketConcession](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TicketType] [nvarchar](50) NOT NULL,
	[RoundTrip] [bit] NOT NULL,
	[PassengerName] [nvarchar](50) NULL,
	[FromPlace] [nvarchar](50) NULL,
	[ToPlace] [nvarchar](50) NULL,
	[DepartDate] [datetime] NULL,
	[ReturnDate] [datetime] NULL,
	[TicketPrice] [decimal](18, 2) NOT NULL,
	[CurrencyCode] [varchar](10) NULL,
	[IsHelper] [bit] NULL,
	[ContactPhone] [nvarchar](50) NULL,
	[ContactEmail] [nvarchar](50) NULL,
	[ContactName] [nvarchar](50) NULL,
	[Description] [nvarchar](200) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_TicketConcession] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[TicketConcession] ADD  CONSTRAINT [DF_TicketConcession_RoundTrip]  DEFAULT ((0)) FOR [RoundTrip]
GO

ALTER TABLE [dbo].[TicketConcession] ADD  CONSTRAINT [DF_TicketConcession_TicketPrice]  DEFAULT ((0)) FOR [TicketPrice]
GO

ALTER TABLE [dbo].[TicketConcession] ADD  CONSTRAINT [DF_TicketConcession_IsHelper]  DEFAULT ((0)) FOR [IsHelper]
GO

ALTER TABLE [dbo].[TicketConcession] ADD  CONSTRAINT [DF_TicketConcession_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ten loai ve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'TicketType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mot chieu / khu hoi; 0-mot chieu;1-khu hoi' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'RoundTrip'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ten hanh khach' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'PassengerName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ten noi di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'FromPlace'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ten noi den' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'ToPlace'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ngay di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'DepartDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'gia ve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'TicketPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ma tien te' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'CurrencyCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'nho ClickBay giao dich' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'IsHelper'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'dien thoai lien lac' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'ContactPhone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'email lien he' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'ContactEmail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ten nguoi lien he' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'ContactName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ghi chu them' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TicketConcession', @level2type=N'COLUMN',@level2name=N'Description'
GO


