CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

CREATE TABLE "Shop" (
    "ShopId" INTEGER NOT NULL CONSTRAINT "PK_Shop" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NULL
);

CREATE TABLE "Users" (
    "UserId" TEXT NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY,
    "Name" TEXT NULL,
    "Birthday" TEXT NOT NULL
);

CREATE TABLE "ShoppingSession" (
    "ShoppingSessionId" INTEGER NOT NULL CONSTRAINT "PK_ShoppingSession" PRIMARY KEY AUTOINCREMENT,
    "Date" TEXT NOT NULL,
    "UserId" INTEGER NOT NULL,
    "UserId1" TEXT NULL,
    CONSTRAINT "FK_ShoppingSession_Users_UserId1" FOREIGN KEY ("UserId1") REFERENCES "Users" ("UserId") ON DELETE RESTRICT
);

CREATE TABLE "Product" (
    "ProductId" INTEGER NOT NULL CONSTRAINT "PK_Product" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NULL,
    "Discount" INTEGER NOT NULL,
    "FullPrice" TEXT NOT NULL,
    "ShopId" INTEGER NOT NULL,
    "ShoppingSessionId" INTEGER NULL,
    CONSTRAINT "FK_Product_Shop_ShopId" FOREIGN KEY ("ShopId") REFERENCES "Shop" ("ShopId") ON DELETE CASCADE,
    CONSTRAINT "FK_Product_ShoppingSession_ShoppingSessionId" FOREIGN KEY ("ShoppingSessionId") REFERENCES "ShoppingSession" ("ShoppingSessionId") ON DELETE RESTRICT
);

CREATE INDEX "IX_Product_ShopId" ON "Product" ("ShopId");

CREATE INDEX "IX_Product_ShoppingSessionId" ON "Product" ("ShoppingSessionId");

CREATE INDEX "IX_ShoppingSession_UserId1" ON "ShoppingSession" ("UserId1");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20181201110237_Initial', '2.1.4-rtm-31024');

