CREATE TABLE [dbo].[LicenseKeys] (
    [LicenseKeyId]      INT           IDENTITY (1, 1) NOT NULL,
    [LicenseSetId]      INT           NOT NULL,
    [Key]               VARCHAR (MAX) NOT NULL,
    [CreatedOn]         DATETIME      NOT NULL,
    [ActivationCount]   INT           NOT NULL,
    [Deactivated]       BIT           NOT NULL,
    [DeactivatedReason] INT           NULL,
    [DeactivatedOn]     DATETIME      NULL
);



