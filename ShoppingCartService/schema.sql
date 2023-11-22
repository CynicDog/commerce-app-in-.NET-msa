drop database if exists ShoppingCart
go

CREATE DATABASE ShoppingCart
GO
 
USE [ShoppingCart]
GO

CREATE TABLE [dbo].[ShoppingCart](
    [ID] int IDENTITY(1,1) PRIMARY KEY,
    [UserId] [bigint] NOT NULL,
    CONSTRAINT ShoppingCartUnique UNIQUE([ID], [UserID])
    )
GO

CREATE INDEX ShoppingCart_UserId ON [dbo].[ShoppingCart] (UserId)
GO

CREATE TABLE [dbo].[ShoppingCartItem](
    [ID] int IDENTITY(1,1) PRIMARY KEY,
    [ShoppingCartId] [int] NOT NULL,
    [ProductCatalogId] [bigint] NOT NULL,
    [ProductName] [nvarchar](100) NOT NULL,
    [ProductDescription] [nvarchar](500) NULL,
    [Amount] [int] NOT NULL,
    [Currency] [nvarchar](5) NOT NULL
    )
GO

ALTER TABLE [dbo].[ShoppingCartItem]  WITH CHECK ADD CONSTRAINT [FK_ShoppingCart] FOREIGN KEY([ShoppingCartId]) REFERENCES [dbo].[ShoppingCart] ([Id])
GO

ALTER TABLE [dbo].[ShoppingCartItem] CHECK CONSTRAINT [FK_ShoppingCart]
GO

CREATE INDEX ShoppingCartItem_ShoppingCartId ON [dbo].[ShoppingCartItem] (ShoppingCartId)
GO

INSERT INTO [dbo].[ShoppingCart] ([UserId]) VALUES
    (1), 
    (2),
    (3);

INSERT INTO [dbo].[ShoppingCartItem] ([ShoppingCartId], [ProductCatalogId], [ProductName], [ProductDescription], [Amount], [Currency]) VALUES
    (1, 101, 'Product1', 'Description1', 10.0, 'USD'),
    (1, 102, 'Product2', 'Description2', 15.0, 'EUR'),
    (2, 201, 'Product3', 'Description3', 20.0, 'USD'),
    (2, 202, 'Product4', 'Description4', 25.0, 'EUR'),
    (3, 301, 'Product5', 'Description5', 5.0, 'KOR');

-- unused table (replaced by EventStoreDB) 
CREATE TABLE [dbo].[EventStore](
    [SequenceNumber] int IDENTITY(1,1) PRIMARY KEY,
    [OccurredAt] [datetimeoffset] NOT NULL,
    [Name] [nvarchar](100)  NOT NULL,
    [Content][nvarchar](max) NOT NULL
)
GO