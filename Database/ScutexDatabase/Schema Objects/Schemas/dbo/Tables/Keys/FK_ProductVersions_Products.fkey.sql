ALTER TABLE [dbo].[ProductVersions]
    ADD CONSTRAINT [FK_ProductVersions_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

