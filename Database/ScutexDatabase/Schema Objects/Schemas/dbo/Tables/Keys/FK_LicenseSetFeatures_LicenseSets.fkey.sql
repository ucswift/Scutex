ALTER TABLE [dbo].[LicenseSetFeatures]
    ADD CONSTRAINT [FK_LicenseSetFeatures_LicenseSets] FOREIGN KEY ([LicenseSetId]) REFERENCES [dbo].[LicenseSets] ([LicenseSetId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

