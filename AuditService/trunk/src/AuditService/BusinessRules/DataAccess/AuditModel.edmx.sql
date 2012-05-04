
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/15/2011 15:02:05
-- Generated from EDMX file: D:\Dev\Audit Service\trunk\src\AuditService\BusinessRules\DataAccess\AuditModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Audit];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_EntityEntityAttribute]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Attribute] DROP CONSTRAINT [FK_EntityEntityAttribute];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganisationEntity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Entity] DROP CONSTRAINT [FK_OrganisationEntity];
GO
IF OBJECT_ID(N'[dbo].[FK_EntityTag_Entity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntityTag] DROP CONSTRAINT [FK_EntityTag_Entity];
GO
IF OBJECT_ID(N'[dbo].[FK_EntityTag_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntityTag] DROP CONSTRAINT [FK_EntityTag_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_AuditLevelAudit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Entity_Audit] DROP CONSTRAINT [FK_AuditLevelAudit];
GO
IF OBJECT_ID(N'[dbo].[FK_Audit_inherits_Entity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Entity_Audit] DROP CONSTRAINT [FK_Audit_inherits_Entity];
GO
IF OBJECT_ID(N'[dbo].[FK_BinaryAttribute_inherits_Attribute]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Attribute_BinaryAttribute] DROP CONSTRAINT [FK_BinaryAttribute_inherits_Attribute];
GO
IF OBJECT_ID(N'[dbo].[FK_FileAttribute_inherits_BinaryAttribute]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Attribute_FileAttribute] DROP CONSTRAINT [FK_FileAttribute_inherits_BinaryAttribute];
GO
IF OBJECT_ID(N'[dbo].[FK_StringAttribute_inherits_Attribute]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Attribute_StringAttribute] DROP CONSTRAINT [FK_StringAttribute_inherits_Attribute];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Attribute]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Attribute];
GO
IF OBJECT_ID(N'[dbo].[Entity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Entity];
GO
IF OBJECT_ID(N'[dbo].[Organisation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Organisation];
GO
IF OBJECT_ID(N'[dbo].[Tag]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tag];
GO
IF OBJECT_ID(N'[dbo].[AuditLevel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AuditLevel];
GO
IF OBJECT_ID(N'[dbo].[Entity_Audit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Entity_Audit];
GO
IF OBJECT_ID(N'[dbo].[Attribute_BinaryAttribute]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Attribute_BinaryAttribute];
GO
IF OBJECT_ID(N'[dbo].[Attribute_FileAttribute]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Attribute_FileAttribute];
GO
IF OBJECT_ID(N'[dbo].[Attribute_StringAttribute]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Attribute_StringAttribute];
GO
IF OBJECT_ID(N'[dbo].[EntityTag]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntityTag];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Attribute'
CREATE TABLE [dbo].[Attribute] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] datetime  NULL,
    [DateDeleted] datetime  NULL,
    [Entity_Id] int  NOT NULL
);
GO

-- Creating table 'Entity'
CREATE TABLE [dbo].[Entity] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateDeleted] datetime  NULL,
    [Organisation_Id] int  NOT NULL
);
GO

-- Creating table 'Organisation'
CREATE TABLE [dbo].[Organisation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrganisationName] nvarchar(max)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] datetime  NULL,
    [DateDeleted] datetime  NULL
);
GO

-- Creating table 'Tag'
CREATE TABLE [dbo].[Tag] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TagName] nvarchar(max)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] datetime  NULL,
    [DateDeleted] datetime  NULL
);
GO

-- Creating table 'AuditLevel'
CREATE TABLE [dbo].[AuditLevel] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Level] int  NOT NULL
);
GO

-- Creating table 'Entity_Audit'
CREATE TABLE [dbo].[Entity_Audit] (
    [Application] nvarchar(max)  NULL,
    [Id] int  NOT NULL,
    [AuditLevel_Id] int  NOT NULL
);
GO

-- Creating table 'Attribute_BinaryAttribute'
CREATE TABLE [dbo].[Attribute_BinaryAttribute] (
    [Value] varbinary(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Attribute_FileAttribute'
CREATE TABLE [dbo].[Attribute_FileAttribute] (
    [Extension] nvarchar(max)  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Attribute_StringAttribute'
CREATE TABLE [dbo].[Attribute_StringAttribute] (
    [Value] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'EntityTag'
CREATE TABLE [dbo].[EntityTag] (
    [Entity_Id] int  NOT NULL,
    [Tag_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Attribute'
ALTER TABLE [dbo].[Attribute]
ADD CONSTRAINT [PK_Attribute]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Entity'
ALTER TABLE [dbo].[Entity]
ADD CONSTRAINT [PK_Entity]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Organisation'
ALTER TABLE [dbo].[Organisation]
ADD CONSTRAINT [PK_Organisation]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tag'
ALTER TABLE [dbo].[Tag]
ADD CONSTRAINT [PK_Tag]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AuditLevel'
ALTER TABLE [dbo].[AuditLevel]
ADD CONSTRAINT [PK_AuditLevel]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Entity_Audit'
ALTER TABLE [dbo].[Entity_Audit]
ADD CONSTRAINT [PK_Entity_Audit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Attribute_BinaryAttribute'
ALTER TABLE [dbo].[Attribute_BinaryAttribute]
ADD CONSTRAINT [PK_Attribute_BinaryAttribute]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Attribute_FileAttribute'
ALTER TABLE [dbo].[Attribute_FileAttribute]
ADD CONSTRAINT [PK_Attribute_FileAttribute]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Attribute_StringAttribute'
ALTER TABLE [dbo].[Attribute_StringAttribute]
ADD CONSTRAINT [PK_Attribute_StringAttribute]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Entity_Id], [Tag_Id] in table 'EntityTag'
ALTER TABLE [dbo].[EntityTag]
ADD CONSTRAINT [PK_EntityTag]
    PRIMARY KEY NONCLUSTERED ([Entity_Id], [Tag_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Entity_Id] in table 'Attribute'
ALTER TABLE [dbo].[Attribute]
ADD CONSTRAINT [FK_EntityEntityAttribute]
    FOREIGN KEY ([Entity_Id])
    REFERENCES [dbo].[Entity]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EntityEntityAttribute'
CREATE INDEX [IX_FK_EntityEntityAttribute]
ON [dbo].[Attribute]
    ([Entity_Id]);
GO

-- Creating foreign key on [Organisation_Id] in table 'Entity'
ALTER TABLE [dbo].[Entity]
ADD CONSTRAINT [FK_OrganisationEntity]
    FOREIGN KEY ([Organisation_Id])
    REFERENCES [dbo].[Organisation]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganisationEntity'
CREATE INDEX [IX_FK_OrganisationEntity]
ON [dbo].[Entity]
    ([Organisation_Id]);
GO

-- Creating foreign key on [Entity_Id] in table 'EntityTag'
ALTER TABLE [dbo].[EntityTag]
ADD CONSTRAINT [FK_EntityTag_Entity]
    FOREIGN KEY ([Entity_Id])
    REFERENCES [dbo].[Entity]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tag_Id] in table 'EntityTag'
ALTER TABLE [dbo].[EntityTag]
ADD CONSTRAINT [FK_EntityTag_Tag]
    FOREIGN KEY ([Tag_Id])
    REFERENCES [dbo].[Tag]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EntityTag_Tag'
CREATE INDEX [IX_FK_EntityTag_Tag]
ON [dbo].[EntityTag]
    ([Tag_Id]);
GO

-- Creating foreign key on [AuditLevel_Id] in table 'Entity_Audit'
ALTER TABLE [dbo].[Entity_Audit]
ADD CONSTRAINT [FK_AuditLevelAudit]
    FOREIGN KEY ([AuditLevel_Id])
    REFERENCES [dbo].[AuditLevel]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AuditLevelAudit'
CREATE INDEX [IX_FK_AuditLevelAudit]
ON [dbo].[Entity_Audit]
    ([AuditLevel_Id]);
GO

-- Creating foreign key on [Id] in table 'Entity_Audit'
ALTER TABLE [dbo].[Entity_Audit]
ADD CONSTRAINT [FK_Audit_inherits_Entity]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Entity]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Attribute_BinaryAttribute'
ALTER TABLE [dbo].[Attribute_BinaryAttribute]
ADD CONSTRAINT [FK_BinaryAttribute_inherits_Attribute]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Attribute]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Attribute_FileAttribute'
ALTER TABLE [dbo].[Attribute_FileAttribute]
ADD CONSTRAINT [FK_FileAttribute_inherits_BinaryAttribute]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Attribute_BinaryAttribute]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Attribute_StringAttribute'
ALTER TABLE [dbo].[Attribute_StringAttribute]
ADD CONSTRAINT [FK_StringAttribute_inherits_Attribute]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Attribute]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------