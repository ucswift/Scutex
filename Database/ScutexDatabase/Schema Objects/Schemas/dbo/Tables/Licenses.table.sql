CREATE TABLE [dbo].[Licenses] (
    [LicenseId]           INT              IDENTITY (1, 1) NOT NULL,
    [ProductId]           INT              NOT NULL,
    [KeyGeneratorTypeId]  INT              NOT NULL,
    [UniqueId]            UNIQUEIDENTIFIER NOT NULL,
    [Name]                NVARCHAR (100)   NOT NULL,
    [PrivateKey]          NVARCHAR (MAX)   NOT NULL,
    [PublicKey]           NVARCHAR (MAX)   NOT NULL,
    [BuyNowUrl]           NVARCHAR (250)   NULL,
    [ProductUrl]          NVARCHAR (250)   NULL,
    [EulaUrl]             NVARCHAR (250)   NULL,
    [CreatedOn]           DATETIME         NOT NULL,
    [UpdatedOn]           DATETIME         NULL,
    [TrialTryButtonDelay] INT              NOT NULL,
    [SupportEmail]        VARCHAR (250)    NULL,
    [SalesEmail]          VARCHAR (250)    NULL,
    [ServiceId]           INT              NULL
);

