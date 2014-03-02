using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace SqlUtilities
{
    public class CsvDataReader : IDataReader
    {
        // The DataReader should always be open when returned to the user.
        private bool _isClosed = false;


        private StreamReader _stream;
        private string[] _headers;
        private string[] _currentRow;

        // This should match strings and strings that
        // have quotes around them and include embedded commas
        private Regex _CsvRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))", RegexOptions.Compiled);

        public CsvDataReader(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            _stream = new StreamReader(fileName);

            _headers = _stream.ReadLine().Split(',');
        }

        public bool Read()
        {
            if (_stream == null) return false;
            if (_stream.EndOfStream) return false;

            _currentRow = _CsvRegex.Split(_stream.ReadLine());

            // Unfortunately my Regex keeps the quotes around strings.
            // Those have to go.
            // I'm sure there's a better way but this works.
            for (int i = 0; i < _currentRow.Length; i++)
            {
                _currentRow[i] = _currentRow[i].Trim('"');
            }

            // 
            return true;
        }

        public Object GetValue(int i)
        {
            return _currentRow[i];
        }

        public String GetName(int i)
        {
            return _headers[i];
        }

        public int FieldCount
        {
            // Return the count of the number of columns, which in
            // this case is the size of the column metadata
            // array.
            get { return _headers.Length; }
        }

        ///*
        // * Because the user should not be able to directly create a 
        // * DataReader object, the constructors are
        // * marked as internal.
        // */
        //internal TemplateDataReader(SampleDb.SampleDbResultSet resultset)
        //{
        //    m_resultset = resultset;
        //}

        //internal TemplateDataReader(SampleDb.SampleDbResultSet resultset, TemplateConnection connection)
        //{
        //    m_resultset = resultset;
        //    m_connection = connection;
        //}

        /****
         * METHODS / PROPERTIES FROM IDataReader.
         ****/
        public int Depth
        {
            /*
             * Always return a value of zero if nesting is not supported.
             */
            get { return 0; }
        }

        public bool IsClosed
        {
            /*
             * Keep track of the reader state - some methods should be
             * disallowed if the reader is closed.
             */
            get { return _isClosed; }
        }

        public int RecordsAffected
        {
            /*
             * RecordsAffected is only applicable to batch statements
             * that include inserts/updates/deletes. The sample always
             * returns -1.
             */
            get { return -1; }
        }

        public void Close()
        { 
            _isClosed = true;
        }

        public bool NextResult()
        {
            // The sample only returns a single resultset. However,
            // DbDataAdapter expects NextResult to return a value.
            return false;
        }

        

        public DataTable GetSchemaTable()
        {
            //$
            throw new NotSupportedException();
        }

        /****
         * METHODS / PROPERTIES FROM IDataRecord.
         ****/




        public String GetDataTypeName(int i)
        {
            /*
             * Usually this would return the name of the type
             * as used on the back end, for example 'smallint' or 'varchar'.
             * The sample returns the simple name of the .NET Framework type.
             */
            return "X";
        }

        public Type GetFieldType(int i)
        {
            // Return the actual Type class for the data type.
            return typeof(int);
        }



        public int GetValues(object[] values)
        {
            int i = 0, j = 0;
            //for (; i < values.Length && j < m_resultset.metaData.Length; i++, j++)
            //{
            //    values[i] = m_resultset.data[m_nPos, j];
            //}

            return i;
        }

        public int GetOrdinal(string name)
        {
            // Look for the ordinal of the column with the same name and return it.
            //for (int i = 0; i < m_resultset.metaData.Length; i++)
            //{
            //    if (0 == _cultureAwareCompare(name, m_resultset.metaData[i].name))
            //    {
            //        return i;
            //    }
            //}

            int result = -1;
            for (int i = 0; i < _headers.Length; i++)
                if (_headers[i] == name)
                {
                    result = i;
                    break;
                }
            return result;

            // Throw an exception if the ordinal cannot be found.
            throw new IndexOutOfRangeException("Could not find specified column in results");
        }

        public object this[int i]
        {
            get { return "X"; }
        }

        public object this[String name]
        {
            // Look up the ordinal and return 
            // the value at that position.
            get { return this[GetOrdinal(name)]; }
        }

       

        public Int32 GetInt32(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }






        /*
         * Implementation specific methods.
         */
        //private int _cultureAwareCompare(string strA, string strB)
        //{
        //    return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);
        //}

        void IDisposable.Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    this.Close();
                }
                catch (Exception e)
                {
                    throw new SystemException("An exception of type " + e.GetType() +
                                              " was encountered while closing the TemplateDataReader.");
                }
            }
        }

        #region Not Used

        public bool GetBoolean(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }
        public byte GetByte(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            // The sample does not support this method.
            throw new NotSupportedException("GetBytes not supported.");
        }

        public char GetChar(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            // The sample does not support this method.
            throw new NotSupportedException("GetChars not supported.");
        }

        public Guid GetGuid(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }

        public Int16 GetInt16(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotSupportedException();
        }

        public Int64 GetInt64(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }

        public float GetFloat(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }

        public double GetDouble(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }

        public String GetString(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }

        public Decimal GetDecimal(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
             */
            throw new NotSupportedException();
        }

        public DateTime GetDateTime(int i)
        {
            /*
             * Force the cast to return the type. InvalidCastException
             * should be thrown if the data is not already of the correct type.
            */
            throw new NotSupportedException();
        }

        public IDataReader GetData(int i)
        {
            /*
             * The sample code does not support this method. Normally,
             * this would be used to expose nested tables and
             * other hierarchical data.
             */
            throw new NotSupportedException("GetData not supported.");
        }


        #endregion


    }
}
