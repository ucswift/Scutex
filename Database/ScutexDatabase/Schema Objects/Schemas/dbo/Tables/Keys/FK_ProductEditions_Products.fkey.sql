ALTER TABLE [dbo].[ProductEditions]
    ADD CONSTRAINT [FK_ProductEditions_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

