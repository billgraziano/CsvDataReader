using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data;
using SqlUtilities;
using System.Collections.Generic;

namespace CsvDataReaderTest
{
    [TestClass]
    public class CsvTestReaderTests
    {
        [TestMethod]
        public void SimpleOpen()
        {
            CsvTestReader reader = new CsvTestReader(@"..\..\SimpleCsv.txt");
        }

        [TestMethod]
        public void OpenAndClose()
        {
            CsvTestReader reader = new CsvTestReader(@"..\..\SimpleCsv.txt");
            Assert.AreEqual(reader.IsClosed, false);
            reader.Close();
            Assert.AreEqual(reader.IsClosed, true);
        }

        [TestMethod]
        public void TestOfGoodFile()
        {
            CsvTestReader reader = new CsvTestReader(@"..\..\SimpleCsv.txt");
            Assert.AreEqual(false, reader.Read());
        }


        [TestMethod]
        public void ReadBadRows()
        {
            CsvTestReader reader = new CsvTestReader(@"..\..\InvalidCsv.txt");
            while (reader.Read())
            {
            }
            reader.Close();
        }

        [TestMethod]
        public void ReadBadRowValues()
        {
            CsvTestReader reader = new CsvTestReader(@"..\..\InvalidCsv.txt");
            reader.Read();
            Assert.AreEqual("Test", reader.GetValue(0));

            reader.Read();
            Assert.AreEqual("One,Two,Three,Four", reader.GetValue(0));

            reader.Read();
            Assert.AreEqual(@"""Row", reader.GetValue(0));

            Assert.AreEqual(false, reader.Read());

            reader.Close();
        }



    }
}
