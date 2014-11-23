CREATE TABLE [dbo].[Country] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (100) NOT NULL,
    [AllowsBilling]      BIT            NOT NULL,
    [AllowsShipping]     BIT            NOT NULL,
    [TwoLetterIsoCode]   NVARCHAR (2)   NULL,
    [ThreeLetterIsoCode] NVARCHAR (3)   NULL,
    [NumericIsoCode]     INT            NOT NULL,
    [SubjectToVat]       BIT            NOT NULL,
    [Published]          BIT            NOT NULL,
    [DisplayOrder]       INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Country_DisplayOrder]
    ON [dbo].[Country]([DisplayOrder] ASC);

