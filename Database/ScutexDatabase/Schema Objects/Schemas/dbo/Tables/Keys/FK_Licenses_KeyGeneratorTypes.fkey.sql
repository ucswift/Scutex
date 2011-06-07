ALTER TABLE [dbo].[Licenses]
    ADD CONSTRAINT [FK_Licenses_KeyGeneratorTypes] FOREIGN KEY ([KeyGeneratorTypeId]) REFERENCES [dbo].[KeyGeneratorTypes] ([KeyGeneratorTypeId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

