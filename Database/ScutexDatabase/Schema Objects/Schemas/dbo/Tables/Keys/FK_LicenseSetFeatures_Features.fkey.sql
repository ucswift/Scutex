ALTER TABLE [dbo].[LicenseSetFeatures]
    ADD CONSTRAINT [FK_LicenseSetFeatures_Features] FOREIGN KEY ([FeatureId]) REFERENCES [dbo].[Features] ([FeatureId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

