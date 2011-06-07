ALTER TABLE [dbo].[Services]
    ADD CONSTRAINT [DF_Services_Tested] DEFAULT ((0)) FOR [Tested];

