CREATE TABLE [dbo].[LicenseActivations] (
    [LicenseActivationId]       INT              IDENTITY (1, 1) NOT NULL,
    [LicenseKeyId]              INT              NOT NULL,
    [ActivationToken]           UNIQUEIDENTIFIER NOT NULL,
    [OriginalToken]             UNIQUEIDENTIFIER NULL,
    [ActivatedOn]               DATETIME         NOT NULL,
    [HardwareHash]              VARCHAR (MAX)    NULL,
    [ActivationStatus]          INT              NOT NULL,
    [ActivationStatusUpdatedOn] DATETIME         NOT NULL
);





