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

CREATE TABLE [Payments] (
    [Id] uniqueidentifier NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    [BuyerId] nvarchar(100) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Status] int NOT NULL,
    [CreatedAtUtc] datetimeoffset NOT NULL,
    [AuthorizedAtUtc] datetimeoffset NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([Id])
);
GO

CREATE UNIQUE INDEX [IX_Payments_OrderId] ON [Payments] ([OrderId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250815033513_Initial', N'8.0.17');
GO

COMMIT;
GO

