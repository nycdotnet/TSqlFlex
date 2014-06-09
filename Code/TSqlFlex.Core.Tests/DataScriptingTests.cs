using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TSqlFlex.Core;
using Microsoft.SqlServer.Types;


namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    class DataScriptingTests
    {

        [Test()]
        public void BIGINT_Data_ScriptsCorrectly()
        {
            Int64 baseData = 999999999999;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "bigint", false, 0, 0);
            Assert.AreEqual("999999999999", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "positive bigint");
            
            baseData = -999999999999;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "bigint", false, 0, 0);
            Assert.AreEqual("-999999999999", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "negative bigint");

        }

        [Test()]
        public void NUMERIC_Data_ScriptsCorrectly()
        {
            Decimal baseData = 12345.6789M;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "numeric", false, 10, 4);
            Assert.AreEqual("12345.6789", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "positive numeric");

            baseData = -12345.6789M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "numeric", false, 10, 4);
            Assert.AreEqual("-12345.6789", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "negative numeric");
        }

        [Test()]
        public void BIT_Data_ScriptsCorrectly()
        {
            bool baseData = true;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 1, "bit", false, 0, 0);
            Assert.AreEqual("1", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "true should script as 1");

            baseData = false;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 1, "bit", false, 0, 0);
            Assert.AreEqual("0", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "false should script as 0");
        }

        [Test()]
        public void SMALLINT_Data_ScriptsCorrectly()
        {
            Int16 baseData = 31000;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 16, "smallint", false, 0, 0);
            Assert.AreEqual("31000", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "positive smallint");

            baseData = -31000;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 16, "smallint", false, 0, 0);
            Assert.AreEqual("-31000", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "negative smallint");
        }

        [Test()]
        public void DECIMAL_Data_ScriptsCorrectly()
        {
            Decimal baseData = 12345.6789M;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "decimal", false, 10, 4);
            Assert.AreEqual("12345.6789", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "positive decimal");

            baseData = -12345.6789M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "decimal", false, 10, 4);
            Assert.AreEqual("-12345.6789", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "negative decimal");
        }

        [Test()]
        public void SMALLMONEY_Data_ScriptsCorrectly()
        {
            Decimal baseData = 200000.1234M;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "smallmoney", false, 10, 4);
            Assert.AreEqual("200000.1234", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "positive smallmoney");

            baseData = -200000.1234M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "smallmoney", false, 10, 4);
            Assert.AreEqual("-200000.1234", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "negative smallmoney");
        }

        [Test()]
        public void INT_Data_ScriptsCorrectly()
        {
            Int32 baseData = 2000123456;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "int", false, 0, 0);
            Assert.AreEqual("2000123456", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "positive int");

            baseData = -2000123456;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "int", false, 0, 0);
            Assert.AreEqual("-2000123456", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "negative int");
        }

        [Test()]
        public void TINYINT_Data_ScriptsCorrectly()
        {
            sbyte baseData = 125;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 8, "tinyint", false, 0, 0);
            Assert.AreEqual("125", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "positive tinyint");

            baseData = -125;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 8, "tinyint", false, 0, 0);
            Assert.AreEqual("-125", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "negative tinyint");
        }

        [Test()]
        public void MONEY_Data_ScriptsCorrectly()
        {
            Decimal baseData = 200000000.1234M;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "money", false, 10, 4);
            Assert.AreEqual("200000000.1234", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "positive money");

            baseData = -200000000.1234M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "smallmoney", false, 10, 4);
            Assert.AreEqual("-200000000.1234", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "negative money");
        }

        [Test()]
        public void FLOAT_Data_ScriptsCorrectly()
        {
            double baseData = 200000000.1234;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "float", false, 0, 0);
            Assert.AreEqual("200000000.1234", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "positive float");

            baseData = -200000000.1234;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "smallmoney", false, 0, 0);
            Assert.AreEqual("-200000000.1234", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "negative float");
        }

        [Test()]
        public void REAL_Data_ScriptsCorrectly()
        {
            Single baseData = 200.1234F;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "float", false, 0, 0);
            Assert.AreEqual("200.1234", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "positive real");

            baseData = -200.1234F;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "smallmoney", false, 0, 0);
            Assert.AreEqual("-200.1234", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "negative real");
        }

        [Test()]
        public void DATE_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(2000,10,31);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "date", false, 0, 0);
            Assert.AreEqual("'2000-10-31'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "date");
        }

        [Test()]
        public void DATETIMEOFFSET_Data_ScriptsCorrectly()
        {
            DateTimeOffset baseData = new DateTimeOffset(2000, 10, 31, 2, 33, 44, new TimeSpan(3,0,0));
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "datetimeoffset", false, 0, 0);
            Assert.AreEqual("'2000-10-31T02:33:44+03:00'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "datetimeoffset no fractional seconds");

            baseData = baseData.AddTicks(1234567);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.1234567+03:00'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "datetimeoffset fractional seconds");
        }

        [Test()]
        public void DATETIME2_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(2000, 10, 31, 2, 33, 44);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "datetime2", false, 0, 0);
            Assert.AreEqual("'2000-10-31T02:33:44'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "datetime2 no fractional seconds");

            baseData = baseData.AddTicks(1234567);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.1234567'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "datetime2 fractional seconds");
        }

        [Test()]
        public void SMALLDATETIME_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(2000, 10, 31, 2, 33, 0);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "smalldatetime", false, 0, 0);
            Assert.AreEqual("'2000-10-31T02:33:00'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "smalldatetime");
        }

        [Test()]
        public void DATETIME_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(2000, 10, 31, 2, 33, 44);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "datetime", false, 0, 0);
            Assert.AreEqual("'2000-10-31T02:33:44'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "datetime no fractional seconds");

            baseData = baseData.AddTicks(1230000);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.123'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "datetime fractional seconds");
        }

        [Test()]
        public void TIME_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(1900, 1, 1, 2, 33, 44);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "time", false, 0, 0);
            Assert.AreEqual("'02:33:44'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "time no fractional seconds");

            baseData = baseData.AddTicks(1234567);
            data = baseData;
            Assert.AreEqual("'02:33:44.1234567'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "time fractional seconds");
        }

        [Test()]
        public void CHAR_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!   "; //extra spaces to pad to 15 characters are intentional since this is how the data comes back from SQL with the fixed-width type.
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "char", false, 0, 0);
            Assert.AreEqual("'hello world!'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "char");
            
            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "char", false, 0, 0);
            Assert.AreEqual("'trailing space'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "char trims trailing space when scripted.");
        }

        [Test()]
        public void VARCHAR_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!"; 
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "varchar", false, 0, 0);
            Assert.AreEqual("'hello world!'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "varchar");

            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "varchar", false, 0, 0);
            Assert.AreEqual("'trailing space '", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "varchar does not trim trailing space when scripted.");
        }

        [Test()]
        public void TEXT_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!";
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "text", false, 0, 0);
            Assert.AreEqual("'hello world!'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "text");

            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "text", false, 0, 0);
            Assert.AreEqual("'trailing space '", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "text does not trim trailing space when scripted.");
        }

        [Test()]
        public void NCHAR_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!   "; //extra spaces to pad to 15 characters are intentional since this is how the data comes back from SQL with the fixed-width type.
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "nchar", false, 0, 0);
            Assert.AreEqual("N'hello world!'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "nchar");

            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "nchar", false, 0, 0);
            Assert.AreEqual("N'trailing space'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "nchar trims trailing space when scripted.");
        }

        [Test()]
        public void NVARCHAR_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!";
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "nvarchar", false, 0, 0);
            Assert.AreEqual("N'hello world!'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "nvarchar");

            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "nvarchar", false, 0, 0);
            Assert.AreEqual("N'trailing space '", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "nvarchar does not trim trailing space when scripted.");
        }

        [Test()]
        public void NTEXT_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!";
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "ntext", false, 0, 0);
            Assert.AreEqual("N'hello world!'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "ntext");

            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "ntext", false, 0, 0);
            Assert.AreEqual("N'trailing space '", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "ntext does not trim trailing space when scripted.");
        }

        [Test()]
        public void BINARY_Data_ScriptsCorrectly()
        {
            Int32 baseData = 123456;
            byte[] data = BitConverter.GetBytes(baseData);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);  //SQL Server represents binary as Big Endian
            }

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "binary", false, 0, 0);
            Assert.AreEqual("0x000000000000000000000000000000000001E240", FlexResultSet.valueAsTSQLLiteral((object)data, fieldInfo), "binary");
        }

        [Test()]
        public void VARBINARY_Data_ScriptsCorrectly()
        {
            Int32 baseData = 123456;
            byte[] data = BitConverter.GetBytes(baseData);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);  //SQL Server represents binary as Big Endian
            }

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "varbinary", false, 0, 0);
            Assert.AreEqual("0x0001E240", FlexResultSet.valueAsTSQLLiteral((object)data, fieldInfo), "varbinary");
        }

        [Test()]
        public void IMAGE_Data_ScriptsCorrectly()
        {
            Int32 baseData = 123456;
            byte[] data = BitConverter.GetBytes(baseData);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);  //SQL Server represents binary as Big Endian
            }

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "image", false, 0, 0);
            Assert.AreEqual("0x0001E240", FlexResultSet.valueAsTSQLLiteral((object)data, fieldInfo), "image");
        }


        [Test()]
        public void TIMESTAMP_Data_ScriptsCorrectly()
        {
            Int32 baseData = 123456;
            byte[] data = BitConverter.GetBytes(baseData);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);  //SQL Server represents binary as Big Endian
            }

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 8, "timestamp", false, 0, 0);
            Assert.AreEqual("0x000000000001E240", FlexResultSet.valueAsTSQLLiteral((object)data, fieldInfo), "timestamp");
        }

        [Test()]
        public void HIERARCHYID_Data_ScriptsCorrectly()
        {
            SqlHierarchyId baseData = SqlHierarchyId.Parse("/");
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "hierarchyid", false, 0, 0);
            Assert.AreEqual("0x", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "hierarchyid");

            baseData = SqlHierarchyId.Parse("/1/");
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "hierarchyid", false, 0, 0);
            Assert.AreEqual("0x58", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "hierarchyid");
        }

        [Test()]
        public void UNIQUEIDENTIFIER_Data_ScriptsCorrectly()
        {
            Guid baseData = Guid.Parse("9631B0CA-D86F-4CA2-BFA5-93A9980D050A");
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "uniqueidentifier", false, 0, 0);
            Assert.AreEqual("'9631B0CA-D86F-4CA2-BFA5-93A9980D050A'", FlexResultSet.valueAsTSQLLiteral(data, fieldInfo), "uniqueidentifier");
        }
    }
}
