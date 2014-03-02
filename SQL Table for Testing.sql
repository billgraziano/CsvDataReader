use tempdb
go
IF OBJECT_ID('dbo.CsvDataReader') IS NOT NULL
	DROP TABLE dbo.CsvDataReader;
GO
CREATE TABLE dbo.CsvDataReader
(
	Header1 VARCHAR(100),
	Header2 VARCHAR(100),
	Header3 VARCHAR(100)

);