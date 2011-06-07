ALTER TABLE [dbo].[LicenseSets]
    ADD CONSTRAINT [FK_LicenseSets_Licenses] FOREIGN KEY ([LicenseId]) REFERENCES [dbo].[Licenses] ([LicenseId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

