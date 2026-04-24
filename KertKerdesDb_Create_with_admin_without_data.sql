USE master;
GO

IF DB_ID('KertKerdesDb') IS NOT NULL
BEGIN
    ALTER DATABASE KertKerdesDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE KertKerdesDb;
END
GO

CREATE DATABASE KertKerdesDb;
GO

USE KertKerdesDb;
GO

CREATE TABLE Felhasznalok (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Felhasznalonev NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    JelszoHash NVARCHAR(500) NOT NULL,
    Szerepkor NVARCHAR(50) NOT NULL DEFAULT 'Felhasznalo',
    RegisztracioDatuma DATETIME NOT NULL DEFAULT GETDATE(),
    ModeratorJovahagyva BIT NOT NULL DEFAULT 0,
    ModeratorElutasitva BIT NOT NULL DEFAULT 0
);

CREATE TABLE Temakorok (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nev NVARCHAR(150) NOT NULL,
    Leiras NVARCHAR(300) NULL
);

SET IDENTITY_INSERT Temakorok ON;

INSERT INTO Temakorok (Id, Nev, Leiras) VALUES
(1, N'Talaj és tápanyagellátás', N'Talaj típusa, pH, trágyázás'),
(2, N'Öntözés és vízgazdálkodás', N'Öntözési kérdések'),
(3, N'Növényvédelem és betegségek', N'Kártevők és betegségek'),
(4, N'Kerti növények', N'Zöldségek és gyümölcsök'),
(5, N'Szobanövények', N'Lakásban tartott növények');

SET IDENTITY_INSERT Temakorok OFF;

CREATE TABLE Cimkek (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nev NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Kerdesek (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Cim NVARCHAR(200) NOT NULL,
    Leiras NVARCHAR(MAX) NOT NULL,
    FelhasznaloId INT NOT NULL,
    TemakorId INT NOT NULL,
    Datum DATETIME NOT NULL DEFAULT GETDATE(),
    Jovahagyva BIT NOT NULL DEFAULT 0,
    Szavazat INT NOT NULL DEFAULT 0,
    FOREIGN KEY (FelhasznaloId) REFERENCES Felhasznalok(Id),
    FOREIGN KEY (TemakorId) REFERENCES Temakorok(Id)
);

CREATE TABLE Valaszok (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    KerdesId INT NOT NULL,
    FelhasznaloId INT NOT NULL,
    Szoveg NVARCHAR(MAX) NOT NULL,
    Datum DATETIME NOT NULL DEFAULT GETDATE(),
    Jovahagyva BIT NOT NULL DEFAULT 0,
    Elfogadott BIT NOT NULL DEFAULT 0,
    Szavazat INT NOT NULL DEFAULT 0,
    FOREIGN KEY (KerdesId) REFERENCES Kerdesek(Id),
    FOREIGN KEY (FelhasznaloId) REFERENCES Felhasznalok(Id)
);

CREATE TABLE KerdesCimkek (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    KerdesId INT NOT NULL,
    CimkeId INT NOT NULL,
    FOREIGN KEY (KerdesId) REFERENCES Kerdesek(Id),
    FOREIGN KEY (CimkeId) REFERENCES Cimkek(Id)
);

CREATE TABLE Szavazatok (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FelhasznaloId INT NOT NULL,
    KerdesId INT NULL,
    ValaszId INT NULL,
    Ertek INT NOT NULL CHECK (Ertek IN (-1,1)),
    FOREIGN KEY (FelhasznaloId) REFERENCES Felhasznalok(Id),
    FOREIGN KEY (KerdesId) REFERENCES Kerdesek(Id),
    FOREIGN KEY (ValaszId) REFERENCES Valaszok(Id)
);

CREATE UNIQUE INDEX IX_Szavazatok_FelhasznaloId_KerdesId
ON Szavazatok(FelhasznaloId, KerdesId)
WHERE KerdesId IS NOT NULL;

CREATE UNIQUE INDEX IX_Szavazatok_FelhasznaloId_ValaszId
ON Szavazatok(FelhasznaloId, ValaszId)
WHERE ValaszId IS NOT NULL;

INSERT INTO Felhasznalok
(
    Felhasznalonev,
    Email,
    JelszoHash,
    Szerepkor,
    RegisztracioDatuma,
    ModeratorJovahagyva,
    ModeratorElutasitva
)
VALUES
(
    N'admin',
    N'admin@kertkerdes.hu',
    N'240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
    N'Admin',
    GETDATE(),
    1,
    0
);

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'kertkerdes')
BEGIN
    CREATE USER kertkerdes FOR LOGIN kertkerdes;
END
GO

ALTER ROLE db_owner ADD MEMBER kertkerdes;
GO