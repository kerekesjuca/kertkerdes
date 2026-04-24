USE KertKerdesDb;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name='kertkerdes')
BEGIN
    CREATE USER kertkerdes FOR LOGIN kertkerdes;
END
GO

ALTER ROLE db_owner ADD MEMBER kertkerdes;
GO