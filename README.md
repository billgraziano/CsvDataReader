CsvDataReader=============A simple C# IDataReader implementation to read CSV files.  This was built to improve CSV performance in PowerShell.  The goal is to enable code like the following.	[System.Reflection.Assembly]::LoadFrom("CsvDataReader.dll")	$reader = New-Object SqlUtilities.CsvDataReader("SimpleCsv.txt")			$bulkCopy = new-object ("Data.SqlClient.SqlBulkCopy") $ConnectionString	$bulkCopy.DestinationTableName = "CsvDataReader"			$bulkCopy.WriteToServer($reader);