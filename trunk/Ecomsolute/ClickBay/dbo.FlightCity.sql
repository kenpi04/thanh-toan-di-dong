CREATE TABLE [dbo].[FlightCity] (
    [Id]             INT           NOT NULL,
    [Code]           NCHAR (10)    NOT NULL,
    [Name]           NVARCHAR (50) NOT NULL,
    [CountryId]      INT           NOT NULL,
    [JetstarCode]    NCHAR (10)    NULL,
    [VietJetAirCode] NCHAR (10)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

