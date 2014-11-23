CREATE TABLE [dbo].[FlightCountry] (
    [Id]             INT           NOT NULL,
    [Code]           NCHAR (10)    NULL,
    [Name]           NVARCHAR (50) NULL,
    [EnlishName]     VARCHAR (50)  NULL,
    [JetStarCode]    NCHAR (10)    NULL,
    [VietJetAirCode] NCHAR (10)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

