CREATE TABLE [dbo].[ActivationLogs] (
    [ActivationLogId]  INT           IDENTITY (1, 1) NOT NULL,
    [LicenseKey]       VARCHAR (250) NOT NULL,
    [ActivationResult] INT           NOT NULL,
    [IPAddress]        VARCHAR (250) NULL,
    [Timestamp]        DATETIME      NOT NULL
);



