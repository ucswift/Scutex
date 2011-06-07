SET NUMERIC_ROUNDABORT OFF

SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON

SET DATEFORMAT YMD

SET XACT_ABORT ON

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE

-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)
BEGIN TRANSACTION

-- Add row to [dbo].[DBVersions]
INSERT INTO [dbo].[DBVersions] ([DBVersionId]) VALUES (1)

-- Add rows to [dbo].[KeyGeneratorTypes]
SET IDENTITY_INSERT [dbo].[KeyGeneratorTypes] ON
INSERT INTO [dbo].[KeyGeneratorTypes] ([KeyGeneratorTypeId], [Description]) VALUES (1, N'Static Small')
INSERT INTO [dbo].[KeyGeneratorTypes] ([KeyGeneratorTypeId], [Description]) VALUES (2, N'Static Large')
INSERT INTO [dbo].[KeyGeneratorTypes] ([KeyGeneratorTypeId], [Description]) VALUES (3, N'Dynamic Small')
INSERT INTO [dbo].[KeyGeneratorTypes] ([KeyGeneratorTypeId], [Description]) VALUES (4, N'Dynamic Large')
SET IDENTITY_INSERT [dbo].[KeyGeneratorTypes] OFF
-- Operation applied to 4 rows out of 4

-- Add rows to [dbo].[TrialExpirationOptions]
SET IDENTITY_INSERT [dbo].[TrialExpirationOptions] ON
INSERT INTO [dbo].[TrialExpirationOptions] ([TrialExpirationOptionId], [Description]) VALUES (1, 'Days')
INSERT INTO [dbo].[TrialExpirationOptions] ([TrialExpirationOptionId], [Description]) VALUES (2, 'Executions')
INSERT INTO [dbo].[TrialExpirationOptions] ([TrialExpirationOptionId], [Description]) VALUES (3, 'Date')
INSERT INTO [dbo].[TrialExpirationOptions] ([TrialExpirationOptionId], [Description]) VALUES (4, 'Runtime')
SET IDENTITY_INSERT [dbo].[TrialExpirationOptions] OFF
-- Operation applied to 4 rows out of 4
COMMIT TRANSACTION


-- Reseed identity on [dbo].[TrialExpirationOptions]
DBCC CHECKIDENT('[dbo].[TrialExpirationOptions]', RESEED, 4)


-- Reseed identity on [dbo].[KeyGeneratorTypes]
DBCC CHECKIDENT('[dbo].[KeyGeneratorTypes]', RESEED, 4)
