IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ClubCanotaje')
BEGIN
    CREATE DATABASE ClubCanotaje;
END
GO