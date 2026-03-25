CREATE DATABASE KertKerdesDb;
GO

USE KertKerdesDb;
GO

---------------------------------------------------
-- FELHASZNALOK
---------------------------------------------------

CREATE TABLE Felhasznalok
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Felhasznalonev NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    JelszoHash NVARCHAR(500) NOT NULL,
    Szerepkor NVARCHAR(50) NOT NULL,
    RegisztracioDatuma DATETIME NOT NULL
);

---------------------------------------------------
-- TEMAKOROK
---------------------------------------------------

CREATE TABLE Temakorok
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nev NVARCHAR(150) NOT NULL,
    Leiras NVARCHAR(300) NULL
);

---------------------------------------------------
-- KERDESEK
---------------------------------------------------

CREATE TABLE Kerdesek
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Cim NVARCHAR(200) NOT NULL,
    Leiras NVARCHAR(MAX) NOT NULL,
    FelhasznaloId INT NOT NULL,
    TemakorId INT NOT NULL,
    Datum DATETIME NOT NULL,
    Jovahagyva BIT NOT NULL,
    Szavazat INT NOT NULL,

    FOREIGN KEY (FelhasznaloId) REFERENCES Felhasznalok(Id),
    FOREIGN KEY (TemakorId) REFERENCES Temakorok(Id)
);

---------------------------------------------------
-- VALASZOK
---------------------------------------------------

CREATE TABLE Valaszok
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    KerdesId INT NOT NULL,
    FelhasznaloId INT NOT NULL,
    Szoveg NVARCHAR(MAX) NOT NULL,
    Datum DATETIME NOT NULL,
    Jovahagyva BIT NOT NULL,
    Elfogadott BIT NOT NULL,
    Szavazat INT NOT NULL,

    FOREIGN KEY (KerdesId) REFERENCES Kerdesek(Id),
    FOREIGN KEY (FelhasznaloId) REFERENCES Felhasznalok(Id)
);

---------------------------------------------------
-- CIMKEK
---------------------------------------------------

CREATE TABLE Cimkek
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nev NVARCHAR(100) NOT NULL
);

---------------------------------------------------
-- KERDES-CIMKEK
---------------------------------------------------

CREATE TABLE KerdesCimkek
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    KerdesId INT NOT NULL,
    CimkeId INT NOT NULL,

    FOREIGN KEY (KerdesId) REFERENCES Kerdesek(Id),
    FOREIGN KEY (CimkeId) REFERENCES Cimkek(Id)
);

---------------------------------------------------
-- SZAVAZATOK
---------------------------------------------------

CREATE TABLE Szavazatok
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FelhasznaloId INT NOT NULL,
    KerdesId INT NULL,
    ValaszId INT NULL,
    Ertek INT NOT NULL,

    FOREIGN KEY (FelhasznaloId) REFERENCES Felhasznalok(Id),
    FOREIGN KEY (KerdesId) REFERENCES Kerdesek(Id),
    FOREIGN KEY (ValaszId) REFERENCES Valaszok(Id)
);

---------------------------------------------------
-- MINTAADATOK
---------------------------------------------------

INSERT INTO Felhasznalok
(Felhasznalonev, Email, JelszoHash, Szerepkor, RegisztracioDatuma)
VALUES
('admin', 'admin@kertkerdes.hu', '123', 'Admin', GETDATE()),
('norbert.barics', 'norbert@kertkerdes.hu', '123', 'Felhasznalo', GETDATE());

---------------------------------------------------

INSERT INTO Temakorok
(Nev, Leiras)
VALUES
('Talaj és tápanyagellátás', 'Talajkezelési kérdések'),
('Öntözés és vízgazdálkodás', 'Öntözési technikák'),
('Szobanövények', 'Lakásban tartott növények');

---------------------------------------------------

INSERT INTO Cimkek
(Nev)
VALUES
('komposzt'),
('öntözés'),
('palánta'),
('bio');

---------------------------------------------------

INSERT INTO Kerdesek
(Cim, Leiras, FelhasznaloId, TemakorId, Datum, Jovahagyva, Szavazat)
VALUES
(
'Hogyan készítsek hatékony komposztot?',
'Kezdő kertész vagyok és szeretnék komposztálni.',
2,
1,
GETDATE(),
1,
0
);

---------------------------------------------------

INSERT INTO KerdesCimkek
(KerdesId, CimkeId)
VALUES
(1,1),
(1,4);