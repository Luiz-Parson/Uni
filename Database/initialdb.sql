CREATE DATABASE ConnectorUniDb
GO

USE ConnectorUniDb
GO

CREATE TABLE SystemUser (
	Id INT NOT NULL IDENTITY,
	Username VARCHAR(50) NOT NULL,
	Password VARCHAR(300) NOT NULL,
	IsAdmin BIT NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	CreatedOn DATETIME DEFAULT GETDATE(),
	UpdatedBy VARCHAR(50) NOT NULL,
	UpdatedOn DATETIME DEFAULT GETDATE(),
	CONSTRAINT PK_SystemUser PRIMARY KEY(Id),
	CONSTRAINT UC_SystemUser UNIQUE(Username)
)
GO

CREATE TABLE Product (
	Id INT NOT NULL IDENTITY,
	Description VARCHAR(50) NOT NULL,
	SKU VARCHAR(50) NOT NULL,
	EPC VARCHAR(50) NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	CreatedOn DATETIME DEFAULT GETDATE(),
	UpdatedBy VARCHAR(50) NOT NULL,
	UpdatedOn DATETIME DEFAULT GETDATE(),
    	DeletedBy VARCHAR(50) NULL,
    	DeletedOn DATETIME NULL,
	CONSTRAINT PK_Product PRIMARY KEY(Id)
)
GO

CREATE TABLE AccessControl (
	Id INT NOT NULL IDENTITY,
	Direction CHAR(3) NOT NULL,
	ProductId INT NOT NULL,
	AccessedOn DATETIME NOT NULL,
	Status VARCHAR(50) NULL,
	CONSTRAINT PK_AccessControl PRIMARY KEY(Id),
	CONSTRAINT FK_ProductAccessControl FOREIGN KEY (ProductId) REFERENCES Product(Id)
)
GO

CREATE TABLE GeneralControl (
	Id INT NOT NULL IDENTITY,
	ProductId INT NOT NULL,
	AccessedOn DATETIME NOT NULL,
	CONSTRAINT PK_GeneralControl PRIMARY KEY(Id),
	CONSTRAINT FK_ProductGeneralControl FOREIGN KEY (ProductId) REFERENCES Product(Id)
)
GO

CREATE TABLE ExclusionControl (
	Id INT NOT NULL IDENTITY,
	ProductId INT NOT NULL,
	Category VARCHAR(50) NULL,
	ExcludedOn DATETIME NOT NULL,
	CONSTRAINT PK_ExclusionControl PRIMARY KEY(Id),
	CONSTRAINT FK_ProductExclusionControl FOREIGN KEY (ProductId) REFERENCES Product(Id)
)
GO

INSERT INTO SystemUser (Username, Password, IsAdmin, CreatedBy, UpdatedBy) VALUES ('admin', 'pIbvhgmpVHahDBTYUgQvew==', 1, 'admin', 'admin') /*senha=admin*/
GO
INSERT INTO SystemUser (Username, Password, IsAdmin, CreatedBy, UpdatedBy) VALUES ('operador', 'pF0lvNsg3C9rMw7O37k4NLKkSvWizD3hWQ64WYJ1Osw=', 0, 'admin', 'admin') /*senha=operador*/
GO