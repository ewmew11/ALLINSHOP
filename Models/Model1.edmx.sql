
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/31/2021 15:37:01
-- Generated from EDMX file: C:\work_102\ALLINSHOP\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [AllInShop];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Products_Heroes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Products_Heroes];
GO
IF OBJECT_ID(N'[dbo].[FK_Products_Types]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Products_Types];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Admintrators]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Admintrators];
GO
IF OBJECT_ID(N'[dbo].[Heroes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Heroes];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Types]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Types];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Admintrators'
CREATE TABLE [dbo].[Admintrators] (
    [Admin_ID] uniqueidentifier  NOT NULL,
    [Admin_Name] varchar(50)  NOT NULL,
    [Admin_Pa] varchar(50)  NOT NULL
);
GO

-- Creating table 'Heroes'
CREATE TABLE [dbo].[Heroes] (
    [Hero_ID] uniqueidentifier  NOT NULL,
    [Hero_Name] varchar(50)  NULL,
    [Hero_Pic] varchar(max)  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [PO_ID] uniqueidentifier  NOT NULL,
    [PO_Name] varchar(50)  NOT NULL,
    [PO_Price] decimal(19,4)  NOT NULL,
    [PO_Pic] nvarchar(max)  NOT NULL,
    [Type_ID] uniqueidentifier  NOT NULL,
    [Hero_ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Types'
CREATE TABLE [dbo].[Types] (
    [Type_ID] uniqueidentifier  NOT NULL,
    [Type_Name] varchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Admin_ID] in table 'Admintrators'
ALTER TABLE [dbo].[Admintrators]
ADD CONSTRAINT [PK_Admintrators]
    PRIMARY KEY CLUSTERED ([Admin_ID] ASC);
GO

-- Creating primary key on [Hero_ID] in table 'Heroes'
ALTER TABLE [dbo].[Heroes]
ADD CONSTRAINT [PK_Heroes]
    PRIMARY KEY CLUSTERED ([Hero_ID] ASC);
GO

-- Creating primary key on [PO_ID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([PO_ID] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [Type_ID] in table 'Types'
ALTER TABLE [dbo].[Types]
ADD CONSTRAINT [PK_Types]
    PRIMARY KEY CLUSTERED ([Type_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Hero_ID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Products_Heroes]
    FOREIGN KEY ([Hero_ID])
    REFERENCES [dbo].[Heroes]
        ([Hero_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Products_Heroes'
CREATE INDEX [IX_FK_Products_Heroes]
ON [dbo].[Products]
    ([Hero_ID]);
GO

-- Creating foreign key on [Type_ID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Products_Types]
    FOREIGN KEY ([Type_ID])
    REFERENCES [dbo].[Types]
        ([Type_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Products_Types'
CREATE INDEX [IX_FK_Products_Types]
ON [dbo].[Products]
    ([Type_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------