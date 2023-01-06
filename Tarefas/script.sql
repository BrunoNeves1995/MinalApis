CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Tarefa" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Tarefa" PRIMARY KEY AUTOINCREMENT,
    "Title" NVARCHAR NOT NULL,
    "Done" BIT NOT NULL,
    "CreatedAt" SMALLDATETIME NOT NULL DEFAULT (GETDATE())
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230105232712_InitialCreation', '7.0.1');

COMMIT;

