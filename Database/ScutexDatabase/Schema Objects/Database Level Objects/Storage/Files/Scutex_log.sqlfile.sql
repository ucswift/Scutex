ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [Scutex_log], FILENAME = 'G:\Databases\Scutex\Scutex_log.ldf', SIZE = 1024 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

