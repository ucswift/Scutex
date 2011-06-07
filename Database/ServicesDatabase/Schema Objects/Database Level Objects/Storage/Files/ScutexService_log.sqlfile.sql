ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [ScutexService_log], FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\ScutexService_log.ldf', SIZE = 1024 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

