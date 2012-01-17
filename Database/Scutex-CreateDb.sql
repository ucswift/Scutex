/*
Deployment script for the Scutex Management Database
*/


SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;



PRINT N'Creating [dbo].[ProductVersions]...';



CREATE TABLE [dbo].[ProductVersions] (
    [ProductVersionId] INT           IDENTITY (1, 1) NOT NULL,
    [ProductId]        INT           NOT NULL,
    [VersionDisplay]   NVARCHAR (50) NOT NULL,
    [VersionMask]      NVARCHAR (50) NOT NULL
);



PRINT N'Creating PK_ProductVersions...';



ALTER TABLE [dbo].[ProductVersions]
    ADD CONSTRAINT [PK_ProductVersions] PRIMARY KEY CLUSTERED ([ProductVersionId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[ProductEditions]...';



CREATE TABLE [dbo].[ProductEditions] (
    [ProductEditionId] INT           IDENTITY (1, 1) NOT NULL,
    [ProductId]        INT           NOT NULL,
    [Name]             NVARCHAR (50) NOT NULL
);



PRINT N'Creating PK_ProductEditions...';



ALTER TABLE [dbo].[ProductEditions]
    ADD CONSTRAINT [PK_ProductEditions] PRIMARY KEY CLUSTERED ([ProductEditionId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[LicenseSets]...';



CREATE TABLE [dbo].[LicenseSets] (
    [LicenseSetId] INT              IDENTITY (1, 1) NOT NULL,
    [LicenseId]    INT              NOT NULL,
    [Name]         NVARCHAR (50)    NOT NULL,
    [LicenseType]  INT              NOT NULL,
    [UniquePad]    UNIQUEIDENTIFIER NOT NULL,
    [MaxUsers]     INT              NULL
);



PRINT N'Creating PK_LicenseSets...';



ALTER TABLE [dbo].[LicenseSets]
    ADD CONSTRAINT [PK_LicenseSets] PRIMARY KEY CLUSTERED ([LicenseSetId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[LicenseKeys]...';



CREATE TABLE [dbo].[LicenseKeys] (
    [LicenseKeyId]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [LicenseSetId]     INT            NOT NULL,
    [LicenseKey]       NVARCHAR (500) NOT NULL,
    [HashedLicenseKey] VARCHAR (500)  NOT NULL,
    [CreatedOn]        DATETIME       NOT NULL
);



PRINT N'Creating PK_LicenseKeys...';



ALTER TABLE [dbo].[LicenseKeys]
    ADD CONSTRAINT [PK_LicenseKeys] PRIMARY KEY CLUSTERED ([LicenseKeyId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[Features]...';



CREATE TABLE [dbo].[Features] (
    [FeatureId] INT              IDENTITY (1, 1) NOT NULL,
    [ProductId] INT              NOT NULL,
    [Name]      NVARCHAR (150)   NOT NULL,
		[Description] NVARCHAR (500)   NULL,
    [UniquePad] UNIQUEIDENTIFIER NOT NULL
);



PRINT N'Creating PK_Features...';



ALTER TABLE [dbo].[Features]
    ADD CONSTRAINT [PK_Features] PRIMARY KEY CLUSTERED ([FeatureId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[Licenses]...';



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



PRINT N'Creating PK_Licenses_1...';



ALTER TABLE [dbo].[Licenses]
    ADD CONSTRAINT [PK_Licenses_1] PRIMARY KEY CLUSTERED ([LicenseId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[LicenseSetFeatures]...';



CREATE TABLE [dbo].[LicenseSetFeatures] (
    [LicenseSetFeatureId] INT IDENTITY (1, 1) NOT NULL,
    [LicenseSetId]        INT NOT NULL,
    [FeatureId]           INT NOT NULL
);



PRINT N'Creating PK_LicenseSetFeatures...';



ALTER TABLE [dbo].[LicenseSetFeatures]
    ADD CONSTRAINT [PK_LicenseSetFeatures] PRIMARY KEY CLUSTERED ([LicenseSetFeatureId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[KeyGeneratorTypes]...';



CREATE TABLE [dbo].[KeyGeneratorTypes] (
    [KeyGeneratorTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [Description]        NVARCHAR (50) NOT NULL
);



PRINT N'Creating PK_LicenseGeneratorTypes...';



ALTER TABLE [dbo].[KeyGeneratorTypes]
    ADD CONSTRAINT [PK_LicenseGeneratorTypes] PRIMARY KEY CLUSTERED ([KeyGeneratorTypeId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[TrialExpirationOptions]...';



CREATE TABLE [dbo].[TrialExpirationOptions] (
    [TrialExpirationOptionId] INT          IDENTITY (1, 1) NOT NULL,
    [Description]             VARCHAR (50) NOT NULL
);



PRINT N'Creating PK_TrialExpirationOptions...';



ALTER TABLE [dbo].[TrialExpirationOptions]
    ADD CONSTRAINT [PK_TrialExpirationOptions] PRIMARY KEY CLUSTERED ([TrialExpirationOptionId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[TrialSettings]...';



CREATE TABLE [dbo].[TrialSettings] (
    [TrialSettingId]          INT          IDENTITY (1, 1) NOT NULL,
    [LicenseId]               INT          NOT NULL,
    [TrialExpirationOptionId] INT          NOT NULL,
    [ExpirationData]          VARCHAR (50) NOT NULL
);



PRINT N'Creating PK_TrialSettings...';



ALTER TABLE [dbo].[TrialSettings]
    ADD CONSTRAINT [PK_TrialSettings] PRIMARY KEY CLUSTERED ([TrialSettingId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[Services]...';



CREATE TABLE [dbo].[Services] (
    [ServiceId]                    INT              IDENTITY (1, 1) NOT NULL,
    [Name]                         VARCHAR (100)    NOT NULL,
    [ClientUrl]                    VARCHAR (250)    NOT NULL,
    [ManagementUrl]                VARCHAR (250)    NOT NULL,
    [Token]                        VARCHAR (MAX)    NOT NULL,
    [InboundPublicKey]             VARCHAR (MAX)    NOT NULL,
    [InboundPrivateKey]            VARCHAR (MAX)    NOT NULL,
    [OutboundPublicKey]            VARCHAR (MAX)    NOT NULL,
    [OutboundPrivateKey]           VARCHAR (MAX)    NOT NULL,
    [ManagementInboundPublicKey]   VARCHAR (MAX)    NOT NULL,
    [ManagementInboundPrivateKey]  VARCHAR (MAX)    NOT NULL,
    [ManagementOutboundPublicKey]  VARCHAR (MAX)    NOT NULL,
    [ManagementOutboundPrivateKey] VARCHAR (MAX)    NOT NULL,
    [UniquePad]                    UNIQUEIDENTIFIER NOT NULL,
    [Initialized]                  BIT              NOT NULL,
    [Tested]                       BIT              NOT NULL,
    [LockToIp]                     BIT              NOT NULL,
    [ClientRequestToken]           VARCHAR (250)    NOT NULL,
    [ManagementRequestToken]       VARCHAR (250)    NOT NULL,
    [CreatedDate]                  DATETIME         NOT NULL
);



PRINT N'Creating PK_Services...';



ALTER TABLE [dbo].[Services]
    ADD CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED ([ServiceId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[Products]...';



CREATE TABLE [dbo].[Products] (
    [ProductId]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (150) NOT NULL,
    [Description] NVARCHAR (500) NOT NULL,
    [UniquePad]   NVARCHAR (50)  NOT NULL
);



PRINT N'Creating PK_Products...';



ALTER TABLE [dbo].[Products]
    ADD CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([ProductId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating [dbo].[DBVersions]...';



CREATE TABLE [dbo].[DBVersions] (
    [DBVersionId] INT NOT NULL
);



PRINT N'Creating PK_DBVersions...';



ALTER TABLE [dbo].[DBVersions]
    ADD CONSTRAINT [PK_DBVersions] PRIMARY KEY CLUSTERED ([DBVersionId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);



PRINT N'Creating DF_Services_Tested...';



ALTER TABLE [dbo].[Services]
    ADD CONSTRAINT [DF_Services_Tested] DEFAULT ((0)) FOR [Tested];



PRINT N'Creating FK_ProductVersions_Products...';



ALTER TABLE [dbo].[ProductVersions] WITH NOCHECK
    ADD CONSTRAINT [FK_ProductVersions_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_ProductEditions_Products...';



ALTER TABLE [dbo].[ProductEditions] WITH NOCHECK
    ADD CONSTRAINT [FK_ProductEditions_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_LicenseSets_Licenses...';



ALTER TABLE [dbo].[LicenseSets] WITH NOCHECK
    ADD CONSTRAINT [FK_LicenseSets_Licenses] FOREIGN KEY ([LicenseId]) REFERENCES [dbo].[Licenses] ([LicenseId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_LicenseKeys_LicenseKeys...';



ALTER TABLE [dbo].[LicenseKeys] WITH NOCHECK
    ADD CONSTRAINT [FK_LicenseKeys_LicenseKeys] FOREIGN KEY ([LicenseSetId]) REFERENCES [dbo].[LicenseSets] ([LicenseSetId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_Features_Products...';



ALTER TABLE [dbo].[Features] WITH NOCHECK
    ADD CONSTRAINT [FK_Features_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_Licenses_Products...';



ALTER TABLE [dbo].[Licenses] WITH NOCHECK
    ADD CONSTRAINT [FK_Licenses_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_Licenses_KeyGeneratorTypes...';



ALTER TABLE [dbo].[Licenses] WITH NOCHECK
    ADD CONSTRAINT [FK_Licenses_KeyGeneratorTypes] FOREIGN KEY ([KeyGeneratorTypeId]) REFERENCES [dbo].[KeyGeneratorTypes] ([KeyGeneratorTypeId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_Licenses_Services...';



ALTER TABLE [dbo].[Licenses] WITH NOCHECK
    ADD CONSTRAINT [FK_Licenses_Services] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Services] ([ServiceId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_LicenseSetFeatures_Features...';



ALTER TABLE [dbo].[LicenseSetFeatures] WITH NOCHECK
    ADD CONSTRAINT [FK_LicenseSetFeatures_Features] FOREIGN KEY ([FeatureId]) REFERENCES [dbo].[Features] ([FeatureId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_LicenseSetFeatures_LicenseSets...';



ALTER TABLE [dbo].[LicenseSetFeatures] WITH NOCHECK
    ADD CONSTRAINT [FK_LicenseSetFeatures_LicenseSets] FOREIGN KEY ([LicenseSetId]) REFERENCES [dbo].[LicenseSets] ([LicenseSetId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_TrialSettings_TrialExpirationOptions...';



ALTER TABLE [dbo].[TrialSettings] WITH NOCHECK
    ADD CONSTRAINT [FK_TrialSettings_TrialExpirationOptions] FOREIGN KEY ([TrialExpirationOptionId]) REFERENCES [dbo].[TrialExpirationOptions] ([TrialExpirationOptionId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Creating FK_TrialSettings_Licenses...';



ALTER TABLE [dbo].[TrialSettings] WITH NOCHECK
    ADD CONSTRAINT [FK_TrialSettings_Licenses] FOREIGN KEY ([LicenseId]) REFERENCES [dbo].[Licenses] ([LicenseId]) ON DELETE NO ACTION ON UPDATE NO ACTION;



PRINT N'Checking existing data against newly created constraints';


ALTER TABLE [dbo].[ProductVersions] WITH CHECK CHECK CONSTRAINT [FK_ProductVersions_Products];

ALTER TABLE [dbo].[ProductEditions] WITH CHECK CHECK CONSTRAINT [FK_ProductEditions_Products];

ALTER TABLE [dbo].[LicenseSets] WITH CHECK CHECK CONSTRAINT [FK_LicenseSets_Licenses];

ALTER TABLE [dbo].[LicenseKeys] WITH CHECK CHECK CONSTRAINT [FK_LicenseKeys_LicenseKeys];

ALTER TABLE [dbo].[Features] WITH CHECK CHECK CONSTRAINT [FK_Features_Products];

ALTER TABLE [dbo].[Licenses] WITH CHECK CHECK CONSTRAINT [FK_Licenses_Products];

ALTER TABLE [dbo].[Licenses] WITH CHECK CHECK CONSTRAINT [FK_Licenses_KeyGeneratorTypes];

ALTER TABLE [dbo].[Licenses] WITH CHECK CHECK CONSTRAINT [FK_Licenses_Services];

ALTER TABLE [dbo].[LicenseSetFeatures] WITH CHECK CHECK CONSTRAINT [FK_LicenseSetFeatures_Features];

ALTER TABLE [dbo].[LicenseSetFeatures] WITH CHECK CHECK CONSTRAINT [FK_LicenseSetFeatures_LicenseSets];

ALTER TABLE [dbo].[TrialSettings] WITH CHECK CHECK CONSTRAINT [FK_TrialSettings_TrialExpirationOptions];

ALTER TABLE [dbo].[TrialSettings] WITH CHECK CHECK CONSTRAINT [FK_TrialSettings_Licenses];


