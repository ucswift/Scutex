ALTER TABLE [dbo].[LicenseKeys]
    ADD CONSTRAINT [FK_LicenseKeys_LicenseKeys] FOREIGN KEY ([LicenseSetId]) REFERENCES [dbo].[LicenseSets] ([LicenseSetId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

