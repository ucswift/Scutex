/*
Deployment script for the Scutex Web Service Database
*/


SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


PRINT N'Creating [dbo].[LicenseKeys]...';



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



PRINT N'Creating PK_LicenseKeys...';



ALTER TABLE [dbo].[LicenseKeys]
    ADD CONSTRAINT [PK_LicenseKeys] PRIMARY KEY CLUSTERED ([LicenseKeyId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[LicenseSets]...';



CREATE TABLE [dbo].[LicenseSets] (
    [LicenseSetId] INT           NOT NULL,
    [LicenseId]    INT           NOT NULL,
    [Name]         VARCHAR (250) NOT NULL,
    [LicenseType]  INT           NOT NULL,
    [MaxUsers]     INT           NULL
);



PRINT N'Creating PK_LicenseSets...';



ALTER TABLE [dbo].[LicenseSets]
    ADD CONSTRAINT [PK_LicenseSets] PRIMARY KEY CLUSTERED ([LicenseSetId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[LicenseActivations]...';



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



PRINT N'Creating PK_LicenseActivations...';



ALTER TABLE [dbo].[LicenseActivations]
    ADD CONSTRAINT [PK_LicenseActivations] PRIMARY KEY CLUSTERED ([LicenseActivationId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[Master]...';



CREATE TABLE [dbo].[Master] (
    [MasterId]              INT              IDENTITY (1, 1) NOT NULL,
    [ServiceId]             UNIQUEIDENTIFIER NOT NULL,
    [ClientInboundKey]      VARCHAR (MAX)    NOT NULL,
    [ClientOutboundKey]     VARCHAR (MAX)    NOT NULL,
    [ManagementInboundKey]  VARCHAR (MAX)    NOT NULL,
    [ManagementOutboundKey] VARCHAR (MAX)    NOT NULL,
    [Token]                 VARCHAR (MAX)    NOT NULL,
    [Initialized]           BIT              NOT NULL
);



PRINT N'Creating PK_Master...';



ALTER TABLE [dbo].[Master]
    ADD CONSTRAINT [PK_Master] PRIMARY KEY CLUSTERED ([MasterId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[ActivationLogs]...';



CREATE TABLE [dbo].[ActivationLogs] (
    [ActivationLogId]  INT           IDENTITY (1, 1) NOT NULL,
    [LicenseKey]       VARCHAR (250) NOT NULL,
    [ActivationResult] INT           NOT NULL,
    [IPAddress]        VARCHAR (250) NULL,
    [Timestamp]        DATETIME      NOT NULL
);



PRINT N'Creating PK_ActivationLogs...';



ALTER TABLE [dbo].[ActivationLogs]
    ADD CONSTRAINT [PK_ActivationLogs] PRIMARY KEY CLUSTERED ([ActivationLogId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[Licenses]...';



CREATE TABLE [dbo].[Licenses] (
    [LicenseId] INT           NOT NULL,
    [Name]      VARCHAR (250) NOT NULL
);



PRINT N'Creating PK_Licenses...';



ALTER TABLE [dbo].[Licenses]
    ADD CONSTRAINT [PK_Licenses] PRIMARY KEY CLUSTERED ([LicenseId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating DF_LicenseKeys_CreatedOn...';



ALTER TABLE [dbo].[LicenseKeys]
    ADD CONSTRAINT [DF_LicenseKeys_CreatedOn] DEFAULT (getdate()) FOR [CreatedOn];



PRINT N'Creating DF_LicenseKeys_ActivationCount...';



ALTER TABLE [dbo].[LicenseKeys]
    ADD CONSTRAINT [DF_LicenseKeys_ActivationCount] DEFAULT ((0)) FOR [ActivationCount];



PRINT N'Creating DF_LicenseKeys_Deactivated...';



ALTER TABLE [dbo].[LicenseKeys]
    ADD CONSTRAINT [DF_LicenseKeys_Deactivated] DEFAULT ((0)) FOR [Deactivated];



PRINT N'Creating FK_LicenseKeys_LicenseSets...';



ALTER TABLE [dbo].[LicenseKeys] WITH NOCHECK
    ADD CONSTRAINT [FK_LicenseKeys_LicenseSets] FOREIGN KEY ([LicenseSetId]) REFERENCES [dbo].[LicenseSets] ([LicenseSetId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_LicenseSets_Licenses...';



ALTER TABLE [dbo].[LicenseSets] WITH NOCHECK
    ADD CONSTRAINT [FK_LicenseSets_Licenses] FOREIGN KEY ([LicenseId]) REFERENCES [dbo].[Licenses] ([LicenseId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_LicenseActivations_LicenseKeys...';



ALTER TABLE [dbo].[LicenseActivations] WITH NOCHECK
    ADD CONSTRAINT [FK_LicenseActivations_LicenseKeys] FOREIGN KEY ([LicenseKeyId]) REFERENCES [dbo].[LicenseKeys] ([LicenseKeyId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Checking existing data against newly created constraints';



ALTER TABLE [dbo].[LicenseKeys] WITH CHECK CHECK CONSTRAINT [FK_LicenseKeys_LicenseSets];

ALTER TABLE [dbo].[LicenseSets] WITH CHECK CHECK CONSTRAINT [FK_LicenseSets_Licenses];

ALTER TABLE [dbo].[LicenseActivations] WITH CHECK CHECK CONSTRAINT [FK_LicenseActivations_LicenseKeys];



