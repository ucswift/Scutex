ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [Scutex], FILENAME = 'G:\Databases\Scutex\Scutex.mdf', SIZE = 2048 KB, FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

