using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data;
using SqlUtilities;
using System.Collections.Generic;

namespace CsvDataReaderTest
{
    [TestClass]
    public class CsvDataReaderTests
    {
        [TestMethod]
        public void SimpleOpen()
        {
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt");
        }

        [TestMethod]
        public void OpenAndClose()
        {
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt");
            Assert.AreEqual(reader.IsClosed, false);
            reader.Close();
            Assert.AreEqual(reader.IsClosed, true);
        }

        [TestMethod]
        public void HeadersParse()
        {
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt");
            Assert.AreEqual(0, reader.GetOrdinal("Header1"));
            Assert.AreEqual(1, reader.GetOrdinal("Header2"));
            Assert.AreEqual(2, reader.GetOrdinal("Header3"));
        }

        [TestMethod]
        public void ReadAllRows()
        {
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt");
            while (reader.Read())
            {
            }
            reader.Close();
        }

        [TestMethod]
        public void GetValue()
        {
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt");
            reader.Read();
            string v1 = reader.GetValue(0).ToString();
            Assert.AreEqual("Row1A", v1);
            Assert.AreEqual("Row1B", reader.GetValue(1).ToString());
        }

        [TestMethod]
        public void FieldCount()
        {
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt");
            Assert.AreEqual(3, reader.FieldCount);
        }

        [TestMethod]
        public void EmbeddedComma()
        {
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt");
            reader.Read();
            reader.Read();
            Assert.AreEqual(reader.GetValue(0).ToString(), "Quotes");
            string v1 = reader.GetValue(2).ToString();
            string expected = "Q,A";
            Assert.AreEqual(expected, v1);
            
        }
        [TestMethod]
        public void GetName()
        {
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt");
            Assert.AreEqual("Header1", reader.GetName(0));
        }

        [TestMethod]
        public void GetOrdinal()
        {
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt");
            Assert.AreEqual(0, reader.GetOrdinal("Header1"));
        }

        [TestMethod()]
        [ExpectedException(typeof(IndexOutOfRangeException), "The column ZZZZ could not be found in the results")]
        public void GetOrdinalFailure()
        {
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt");
            int i = reader.GetOrdinal("ZZZZ");
            //Assert.Fail();
        }

        [TestMethod]
        public void AddStaticValue()
        {
            Dictionary<String, String> staticColumns = new Dictionary<String, String>();
            staticColumns.Add("Column1", "Value");
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt", staticColumns);
            
            Assert.AreEqual(3, reader.GetOrdinal("Column1"));
            while (reader.Read())
            {
                Assert.AreEqual("Value", reader.GetValue(reader.GetOrdinal("Column1")));
            }
            reader.Close();
        }

        [TestMethod]
        public void TwoStaticColumns()
        {
            Dictionary<String, String> staticColumns = new Dictionary<String, String>();
            staticColumns.Add("Column1", "Value");
            staticColumns.Add("ColumnZ", "FileName");
            CsvDataReader reader = new CsvDataReader(@"..\..\SimpleCsv.txt", staticColumns);

            Assert.AreEqual(4, reader.GetOrdinal("ColumnZ"));
            while (reader.Read())
            {
                Assert.AreEqual("FileName", reader.GetValue(reader.GetOrdinal("ColumnZ")));
            }
            reader.Close();
        }

    }
}
