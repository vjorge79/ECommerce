IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Orders] (
    [Id] uniqueidentifier NOT NULL,
    [BuyerId] nvarchar(100) NOT NULL,
    [OrderDate] datetimeoffset NOT NULL,
    [ShipToAddress_Street] nvarchar(200) NOT NULL,
    [ShipToAddress_City] nvarchar(100) NOT NULL,
    [ShipToAddress_State] nvarchar(100) NOT NULL,
    [ShipToAddress_Country] nvarchar(100) NOT NULL,
    [ShipToAddress_ZipCode] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [OrderItems] (
    [Id] uniqueidentifier NOT NULL,
    [ItemOrdered_CatalogItemId] int NOT NULL,
    [ItemOrdered_ProductName] nvarchar(200) NOT NULL,
    [ItemOrdered_PictureUri] nvarchar(500) NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [Units] int NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250814223755_Initial', N'8.0.19');
GO

COMMIT;
GO

