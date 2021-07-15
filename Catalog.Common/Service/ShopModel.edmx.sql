
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/15/2021 08:55:25
-- Generated from EDMX file: C:\Users\Alvin\source\repos\Catalog\Catalog.Common\Service\ShopModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Catalog];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Product_Subcategory_SubcategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Product_Subcategory_SubcategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_SpecialOfferProduct_Product_ProductID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SpecialOfferProducts] DROP CONSTRAINT [FK_SpecialOfferProduct_Product_ProductID];
GO
IF OBJECT_ID(N'[dbo].[FK_Subcategory_Category_CategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subcategories] DROP CONSTRAINT [FK_Subcategory_Category_CategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_SpecialOfferProduct_SpecialOffer_SpecialOfferID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SpecialOfferProducts] DROP CONSTRAINT [FK_SpecialOfferProduct_SpecialOffer_SpecialOfferID];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_Photo_ProductID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Photos] DROP CONSTRAINT [FK_Product_Photo_ProductID];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_ProductInventory_ID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inventories] DROP CONSTRAINT [FK_Product_ProductInventory_ID];
GO
IF OBJECT_ID(N'[dbo].[FK_ShoppingCartShoppingCartItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ShoppingCartItems] DROP CONSTRAINT [FK_ShoppingCartShoppingCartItem];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSpecialOfferProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SpecialOfferProducts] DROP CONSTRAINT [FK_UserSpecialOfferProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_ShoppingCartItem_Product_ID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ShoppingCartItems] DROP CONSTRAINT [FK_ShoppingCartItem_Product_ID];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Photos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Photos];
GO
IF OBJECT_ID(N'[dbo].[Subcategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subcategories];
GO
IF OBJECT_ID(N'[dbo].[TransactionHistories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionHistories];
GO
IF OBJECT_ID(N'[dbo].[ShoppingCartItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShoppingCartItems];
GO
IF OBJECT_ID(N'[dbo].[SpecialOffers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpecialOffers];
GO
IF OBJECT_ID(N'[dbo].[SpecialOfferProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpecialOfferProducts];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Inventories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Inventories];
GO
IF OBJECT_ID(N'[dbo].[ShoppingCarts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShoppingCarts];
GO
IF OBJECT_ID(N'[dbo].[Settings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Settings];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [LocationID] smallint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [CostRate] decimal(10,4)  NOT NULL,
    [Availability] decimal(8,2)  NOT NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductID] int  NOT NULL,
    [SubcategoryID] int  NULL,
    [ArticleNumber] nvarchar(max)  NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Brand] nvarchar(max)  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Price] decimal(19,2)  NOT NULL,
    [Description] nvarchar(400)  NOT NULL,
    [rowguid] uniqueidentifier  NOT NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [CategoryID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [rowguid] uniqueidentifier  NOT NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- Creating table 'Photos'
CREATE TABLE [dbo].[Photos] (
    [ID] int  NOT NULL,
    [ProductID] int  NOT NULL,
    [FileName] nvarchar(50)  NULL,
    [ThumbNailPhoto] varbinary(max)  NULL,
    [LargePhoto] varbinary(max)  NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- Creating table 'Subcategories'
CREATE TABLE [dbo].[Subcategories] (
    [SubcategoryID] int IDENTITY(1,1) NOT NULL,
    [CategoryID] int  NOT NULL,
    [ChildSubcategoryID] int  NULL,
    [Name] nvarchar(50)  NOT NULL,
    [rowguid] uniqueidentifier  NOT NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- Creating table 'TransactionHistories'
CREATE TABLE [dbo].[TransactionHistories] (
    [TransactionID] int IDENTITY(1,1) NOT NULL,
    [TransactionDate] datetime  NOT NULL,
    [Quantity] int  NOT NULL,
    [TotalCost] decimal(19,2)  NOT NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- Creating table 'ShoppingCartItems'
CREATE TABLE [dbo].[ShoppingCartItems] (
    [ID] int  NOT NULL,
    [ProductID] int  NOT NULL,
    [ShoppingCartID] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [UnitPrice] decimal(19,2)  NOT NULL,
    [rowguid] uniqueidentifier  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- Creating table 'SpecialOffers'
CREATE TABLE [dbo].[SpecialOffers] (
    [SpecialOfferID] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(255)  NOT NULL,
    [DiscountPct] decimal(19,2)  NOT NULL,
    [Type] nvarchar(50)  NOT NULL,
    [Category] nvarchar(50)  NOT NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [MinQty] int  NOT NULL,
    [MaxQty] int  NULL,
    [rowguid] uniqueidentifier  NOT NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- Creating table 'SpecialOfferProducts'
CREATE TABLE [dbo].[SpecialOfferProducts] (
    [SpecialOfferID] int  NOT NULL,
    [ID] int  NOT NULL,
    [UserID] int  NOT NULL,
    [rowguid] uniqueidentifier  NOT NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [EmailAddress] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Inventories'
CREATE TABLE [dbo].[Inventories] (
    [ID] int  NOT NULL,
    [ProductID] int  NOT NULL,
    [CategoryID] int  NOT NULL,
    [SubcategoryID] int  NULL,
    [StockLevel1] nvarchar(max)  NOT NULL,
    [StockLevel2] nvarchar(max)  NOT NULL,
    [Pack] nvarchar(max)  NOT NULL,
    [rowguid] uniqueidentifier  NOT NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- Creating table 'ShoppingCarts'
CREATE TABLE [dbo].[ShoppingCarts] (
    [ShoppingCartID] int IDENTITY(1,1) NOT NULL,
    [Status] tinyint  NOT NULL,
    [TotalQuantity] int  NOT NULL,
    [TotalPrice] decimal(19,2)  NOT NULL,
    [ModifiedDate] datetime  NOT NULL,
    [DateCreated] datetime  NOT NULL
);
GO

-- Creating table 'Settings'
CREATE TABLE [dbo].[Settings] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AskConfirmation] bit  NOT NULL,
    [LoadImage] bit  NOT NULL,
    [UpdateInterval] int  NOT NULL,
    [LeftPanelWidth] smallint  NOT NULL,
    [ModifiedDate] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [LocationID] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([LocationID] ASC);
GO

-- Creating primary key on [ID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [CategoryID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([CategoryID] ASC);
GO

-- Creating primary key on [ID] in table 'Photos'
ALTER TABLE [dbo].[Photos]
ADD CONSTRAINT [PK_Photos]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [SubcategoryID] in table 'Subcategories'
ALTER TABLE [dbo].[Subcategories]
ADD CONSTRAINT [PK_Subcategories]
    PRIMARY KEY CLUSTERED ([SubcategoryID] ASC);
GO

-- Creating primary key on [TransactionID] in table 'TransactionHistories'
ALTER TABLE [dbo].[TransactionHistories]
ADD CONSTRAINT [PK_TransactionHistories]
    PRIMARY KEY CLUSTERED ([TransactionID] ASC);
GO

-- Creating primary key on [ID] in table 'ShoppingCartItems'
ALTER TABLE [dbo].[ShoppingCartItems]
ADD CONSTRAINT [PK_ShoppingCartItems]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [SpecialOfferID] in table 'SpecialOffers'
ALTER TABLE [dbo].[SpecialOffers]
ADD CONSTRAINT [PK_SpecialOffers]
    PRIMARY KEY CLUSTERED ([SpecialOfferID] ASC);
GO

-- Creating primary key on [SpecialOfferID], [ID] in table 'SpecialOfferProducts'
ALTER TABLE [dbo].[SpecialOfferProducts]
ADD CONSTRAINT [PK_SpecialOfferProducts]
    PRIMARY KEY CLUSTERED ([SpecialOfferID], [ID] ASC);
GO

-- Creating primary key on [UserID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [ID] in table 'Inventories'
ALTER TABLE [dbo].[Inventories]
ADD CONSTRAINT [PK_Inventories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ShoppingCartID] in table 'ShoppingCarts'
ALTER TABLE [dbo].[ShoppingCarts]
ADD CONSTRAINT [PK_ShoppingCarts]
    PRIMARY KEY CLUSTERED ([ShoppingCartID] ASC);
GO

-- Creating primary key on [ID] in table 'Settings'
ALTER TABLE [dbo].[Settings]
ADD CONSTRAINT [PK_Settings]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SubcategoryID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Product_Subcategory_SubcategoryID]
    FOREIGN KEY ([SubcategoryID])
    REFERENCES [dbo].[Subcategories]
        ([SubcategoryID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_Subcategory_SubcategoryID'
CREATE INDEX [IX_FK_Product_Subcategory_SubcategoryID]
ON [dbo].[Products]
    ([SubcategoryID]);
GO

-- Creating foreign key on [ID] in table 'SpecialOfferProducts'
ALTER TABLE [dbo].[SpecialOfferProducts]
ADD CONSTRAINT [FK_SpecialOfferProduct_Product_ProductID]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[Products]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SpecialOfferProduct_Product_ProductID'
CREATE INDEX [IX_FK_SpecialOfferProduct_Product_ProductID]
ON [dbo].[SpecialOfferProducts]
    ([ID]);
GO

-- Creating foreign key on [CategoryID] in table 'Subcategories'
ALTER TABLE [dbo].[Subcategories]
ADD CONSTRAINT [FK_Subcategory_Category_CategoryID]
    FOREIGN KEY ([CategoryID])
    REFERENCES [dbo].[Categories]
        ([CategoryID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Subcategory_Category_CategoryID'
CREATE INDEX [IX_FK_Subcategory_Category_CategoryID]
ON [dbo].[Subcategories]
    ([CategoryID]);
GO

-- Creating foreign key on [SpecialOfferID] in table 'SpecialOfferProducts'
ALTER TABLE [dbo].[SpecialOfferProducts]
ADD CONSTRAINT [FK_SpecialOfferProduct_SpecialOffer_SpecialOfferID]
    FOREIGN KEY ([SpecialOfferID])
    REFERENCES [dbo].[SpecialOffers]
        ([SpecialOfferID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ID] in table 'Photos'
ALTER TABLE [dbo].[Photos]
ADD CONSTRAINT [FK_Product_Photo_ProductID]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[Products]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ID] in table 'Inventories'
ALTER TABLE [dbo].[Inventories]
ADD CONSTRAINT [FK_Product_ProductInventory_ID]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[Products]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ShoppingCartID] in table 'ShoppingCartItems'
ALTER TABLE [dbo].[ShoppingCartItems]
ADD CONSTRAINT [FK_ShoppingCartShoppingCartItem]
    FOREIGN KEY ([ShoppingCartID])
    REFERENCES [dbo].[ShoppingCarts]
        ([ShoppingCartID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ShoppingCartShoppingCartItem'
CREATE INDEX [IX_FK_ShoppingCartShoppingCartItem]
ON [dbo].[ShoppingCartItems]
    ([ShoppingCartID]);
GO

-- Creating foreign key on [UserID] in table 'SpecialOfferProducts'
ALTER TABLE [dbo].[SpecialOfferProducts]
ADD CONSTRAINT [FK_UserSpecialOfferProduct]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSpecialOfferProduct'
CREATE INDEX [IX_FK_UserSpecialOfferProduct]
ON [dbo].[SpecialOfferProducts]
    ([UserID]);
GO

-- Creating foreign key on [ID] in table 'ShoppingCartItems'
ALTER TABLE [dbo].[ShoppingCartItems]
ADD CONSTRAINT [FK_ShoppingCartItem_Product_ID]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[Products]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------