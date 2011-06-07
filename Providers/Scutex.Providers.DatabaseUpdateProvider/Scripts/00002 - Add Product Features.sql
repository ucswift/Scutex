-- Version: 05002EE5-B5E5-49F0-907D-24D86D69514A
CREATE TABLE dbo.ProductFeatures
	(
	ProductFeatureId int NOT NULL,
	ProductId int NOT NULL,
	Name nvarchar(150) NULL,
	Description nvarchar(500) NULL,
	UniquePad nvarchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ProductFeatures ADD CONSTRAINT
	PK_ProductFeatures PRIMARY KEY CLUSTERED 
	(
	ProductFeatureId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
