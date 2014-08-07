using System;
using System.Data.SqlTypes;
using NUnit.Framework;
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
            Assert.AreEqual("999999999999", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "positive bigint");
            
            baseData = -999999999999;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "bigint", false, 0, 0);
            Assert.AreEqual("-999999999999", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "negative bigint");

        }

        [Test()]
        public void NUMERIC_Data_ScriptsCorrectly()
        {
            Decimal baseData = 12345.6789M;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "numeric", false, 10, 4);
            Assert.AreEqual("12345.6789", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "positive numeric");

            baseData = -12345.6789M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "numeric", false, 10, 4);
            Assert.AreEqual("-12345.6789", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "negative numeric");

            baseData = 12345.0000M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "numeric", false, 10, 4);
            Assert.AreEqual("12345", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed");

            baseData = 12345.1000M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "numeric", false, 10, 4);
            Assert.AreEqual("12345.1", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed (partially significant decimal)");
        }

        [Test()]
        public void BIT_Data_ScriptsCorrectly()
        {
            bool baseData = true;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 1, "bit", false, 0, 0);
            Assert.AreEqual("1", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "true should script as 1");

            baseData = false;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 1, "bit", false, 0, 0);
            Assert.AreEqual("0", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "false should script as 0");
        }

        [Test()]
        public void SMALLINT_Data_ScriptsCorrectly()
        {
            Int16 baseData = 31000;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 16, "smallint", false, 0, 0);
            Assert.AreEqual("31000", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "positive smallint");

            baseData = -31000;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 16, "smallint", false, 0, 0);
            Assert.AreEqual("-31000", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "negative smallint");
        }

        [Test()]
        public void DECIMAL_Data_ScriptsCorrectly()
        {
            Decimal baseData = 12345.6789M;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "decimal", false, 10, 4);
            Assert.AreEqual("12345.6789", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "positive decimal");

            baseData = -12345.6789M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "decimal", false, 10, 4);
            Assert.AreEqual("-12345.6789", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "negative decimal");

            baseData = 12345.0000M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "decimal", false, 10, 4);
            Assert.AreEqual("12345", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed");

            baseData = 12345.1000M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "decimal", false, 10, 4);
            Assert.AreEqual("12345.1", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed (partially significant decimal)");
        }

        [Test()]
        public void SMALLMONEY_Data_ScriptsCorrectly()
        {
            Decimal baseData = 200000.1234M;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "smallmoney", false, 10, 4);
            Assert.AreEqual("200000.1234", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "positive smallmoney");

            baseData = -200000.1234M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "smallmoney", false, 10, 4);
            Assert.AreEqual("-200000.1234", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "negative smallmoney");

            baseData = 12345.0000M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "smallmoney", false, 10, 4);
            Assert.AreEqual("12345", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed");

            baseData = 12345.1000M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "smallmoney", false, 10, 4);
            Assert.AreEqual("12345.1", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed (partially significant decimal)");
        }

        [Test()]
        public void INT_Data_ScriptsCorrectly()
        {
            Int32 baseData = 2000123456;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "int", false, 0, 0);
            Assert.AreEqual("2000123456", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "positive int");

            baseData = -2000123456;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "int", false, 0, 0);
            Assert.AreEqual("-2000123456", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "negative int");
        }

        [Test()]
        public void TINYINT_Data_ScriptsCorrectly()
        {
            sbyte baseData = 125;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 8, "tinyint", false, 0, 0);
            Assert.AreEqual("125", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "positive tinyint");

            baseData = -125;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 8, "tinyint", false, 0, 0);
            Assert.AreEqual("-125", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "negative tinyint");
        }

        [Test()]
        public void MONEY_Data_ScriptsCorrectly()
        {
            Decimal baseData = 200000000.1234M;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "money", false, 10, 4);
            Assert.AreEqual("200000000.1234", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "positive money");

            baseData = -200000000.1234M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "money", false, 10, 4);
            Assert.AreEqual("-200000000.1234", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "negative money");

            baseData = 12345.0000M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "money", false, 10, 4);
            Assert.AreEqual("12345", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed");

            baseData = 12345.1000M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "money", false, 10, 4);
            Assert.AreEqual("12345.1", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed (partially significant decimal)");
        }

        [Test()]
        public void FLOAT_Data_ScriptsCorrectly()
        {
            double baseData = 200000000.1234;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "float", false, 0, 0);
            Assert.AreEqual("200000000.1234", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "positive float");

            baseData = -200000000.1234;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "float", false, 0, 0);
            Assert.AreEqual("-200000000.1234", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "negative float");

            baseData = 12345.0000;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "float", false, 10, 4);
            Assert.AreEqual("12345", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed");

            baseData = 12345.2000;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "float", false, 10, 4);
            Assert.AreEqual("12345.2", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed (partially significant decimal)");
        }

        [Test()]
        public void REAL_Data_ScriptsCorrectly()
        {
            Single baseData = 200.1234F;
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "real", false, 0, 0);
            Assert.AreEqual("200.1234", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "positive real");

            baseData = -200.1234F;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "real", false, 0, 0);
            Assert.AreEqual("-200.1234", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "negative real");

            baseData = 12345.0000F;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "real", false, 10, 4);
            Assert.AreEqual("12345", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed");

            baseData = 12345.2000F;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "float", false, 10, 4);
            Assert.AreEqual("12345.2", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed (partially significant decimal)");
        }

        [Test()]
        public void DATE_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(2000,10,31);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "date", false, 0, 0);
            Assert.AreEqual("'2000-10-31'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "date");
        }

        [Test()]
        public void DATETIMEOFFSET_Data_ScriptsCorrectly()
        {
            DateTimeOffset baseData = new DateTimeOffset(2000, 10, 31, 2, 33, 44, new TimeSpan(3,0,0));
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "datetimeoffset", false, 0, 0);
            Assert.AreEqual("'2000-10-31T02:33:44+03:00'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "datetimeoffset no fractional seconds");

            baseData = baseData.AddTicks(1234567);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.1234567+03:00'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "datetimeoffset fractional seconds");
        }

        [Test()]
        public void DATETIME2_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(2000, 10, 31, 2, 33, 44);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "datetime2", false, 0, 0);
            Assert.AreEqual("'2000-10-31T02:33:44'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "datetime2 no fractional seconds");

            baseData = baseData.AddTicks(1234567);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.1234567'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "datetime2 fractional seconds");

            baseData = baseData = new DateTime(2000, 10, 31, 2, 33, 44).AddMilliseconds(100);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.1'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "partial fractional seconds");

            baseData = new DateTime(2000, 10, 31, 0, 0, 0);
            data = baseData;
            Assert.AreEqual("'2000-10-31'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "midnight omits time altogether");
        }

        [Test()]
        public void SMALLDATETIME_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(2000, 10, 31, 2, 33, 0);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "smalldatetime", false, 0, 0);
            Assert.AreEqual("'2000-10-31T02:33:00'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "smalldatetime");
            Assert.AreEqual("2000-10-31T02:33:00", FieldScripting.formatSmallDateTime(data, false), "smalldatetime");

            baseData = new DateTime(2000, 10, 31, 0, 0, 0);
            data = baseData;
            Assert.AreEqual("'2000-10-31'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "midnight omits time altogether");
            Assert.AreEqual("2000-10-31", FieldScripting.formatSmallDateTime(data, false), "midnight omits time altogether");
        }

        [Test()]
        public void DATETIME_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(2000, 10, 31, 2, 33, 44);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "datetime", false, 0, 0);
            Assert.AreEqual("'2000-10-31T02:33:44'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "datetime no fractional seconds");
            Assert.AreEqual("2000-10-31T02:33:44", FieldScripting.formatDateTime(data, false), "datetime no fractional seconds");

            baseData = baseData.AddTicks(1230000);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.123'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "datetime fractional seconds");
            Assert.AreEqual("2000-10-31T02:33:44.123", FieldScripting.formatDateTime(data, false), "datetime fractional seconds");

            baseData = baseData = new DateTime(2000, 10, 31, 2, 33, 44).AddMilliseconds(100);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.1'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "partial fractional seconds");
            Assert.AreEqual("2000-10-31T02:33:44.1", FieldScripting.formatDateTime(data, false), "partial fractional seconds");

            baseData = new DateTime(2000, 10, 31, 0, 0, 0);
            data = baseData;
            Assert.AreEqual("'2000-10-31'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "midnight omits time altogether");
            Assert.AreEqual("2000-10-31", FieldScripting.formatDateTime(data, false), "midnight omits time altogether");
        }

        [Test()]
        public void TIME_LowResolution_Data_ScriptsCorrectly()
        {
            TimeSpan baseData = new TimeSpan(0, 2, 33, 44);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "time", false, 0, 0);
            Assert.AreEqual("'02:33:44'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "time no fractional seconds");

            baseData = new TimeSpan(0, 2, 33, 44, 100);
            data = baseData;
            Assert.AreEqual("'02:33:44.1'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "time partial fractional seconds");

            baseData = new TimeSpan(0, 2, 33, 44, 123);
            data = baseData;
            Assert.AreEqual("'02:33:44.123'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "time fractional seconds");
        }

        [Test()]
        public void TIME_HighResolution_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(1900,1,1,2,33,44);
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 32, "time", false, 0, 0);
            Assert.AreEqual("'02:33:44'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "time no fractional seconds");

            baseData = new DateTime(1900, 1, 1, 2, 33, 44).AddMilliseconds(100);
            data = baseData;
            Assert.AreEqual("'02:33:44.1'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "partial fractional seconds");

            baseData = new DateTime(1900, 1, 1, 2, 33, 44).AddTicks(1234567);
            data = baseData;
            Assert.AreEqual("'02:33:44.1234567'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "time fractional seconds");
        }

        [Test()]
        public void CHAR_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!   "; //extra spaces to pad to 15 characters are intentional since this is how the data comes back from SQL with the fixed-width type.
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "char", false, 0, 0);
            Assert.AreEqual("'hello world!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "char");
            
            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "char", false, 0, 0);
            Assert.AreEqual("'trailing space'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "char trims trailing space when scripted.");

            baseData = "That's fun!    ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "char", false, 0, 0);
            Assert.AreEqual("'That''s fun!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "char escapes quotes in strings.");
        }

        [Test()]
        public void VARCHAR_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!"; 
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "varchar", false, 0, 0);
            Assert.AreEqual("'hello world!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "varchar");

            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "varchar", false, 0, 0);
            Assert.AreEqual("'trailing space '", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "varchar does not trim trailing space when scripted.");

            baseData = "That's fun!";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "varchar", false, 0, 0);
            Assert.AreEqual("'That''s fun!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "varchar escapes quotes in strings.");
        }

        [Test()]
        public void TEXT_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!";
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "text", false, 0, 0);
            Assert.AreEqual("'hello world!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "text");

            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "text", false, 0, 0);
            Assert.AreEqual("'trailing space '", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "text does not trim trailing space when scripted.");

            baseData = "That's fun!";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "text", false, 0, 0);
            Assert.AreEqual("'That''s fun!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "text escapes quotes in strings.");
        }

        [Test()]
        public void NCHAR_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!   "; //extra spaces to pad to 15 characters are intentional since this is how the data comes back from SQL with the fixed-width type.
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "nchar", false, 0, 0);
            Assert.AreEqual("N'hello world!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "nchar");

            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "nchar", false, 0, 0);
            Assert.AreEqual("N'trailing space'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "nchar trims trailing space when scripted.");

            baseData = "That's fun!    ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "nchar", false, 0, 0);
            Assert.AreEqual("N'That''s fun!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "nchar escapes quotes in strings.");
        }

        [Test()]
        public void NVARCHAR_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!";
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "nvarchar", false, 0, 0);
            Assert.AreEqual("N'hello world!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "nvarchar");

            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "nvarchar", false, 0, 0);
            Assert.AreEqual("N'trailing space '", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "nvarchar does not trim trailing space when scripted.");

            baseData = "That's fun!";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "nvarchar", false, 0, 0);
            Assert.AreEqual("N'That''s fun!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "nvarchar escapes quotes in strings.");
        }

        [Test()]
        public void NTEXT_Data_ScriptsCorrectly()
        {
            string baseData = "hello world!";
            object data = baseData;
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "ntext", false, 0, 0);
            Assert.AreEqual("N'hello world!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "ntext");

            baseData = "trailing space ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "ntext", false, 0, 0);
            Assert.AreEqual("N'trailing space '", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "ntext does not trim trailing space when scripted.");

            baseData = "That's fun!";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 15, "ntext", false, 0, 0);
            Assert.AreEqual("N'That''s fun!'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "nvarchar escapes quotes in strings.");
        }

        [Test()]
        public void BINARY_Data_ScriptsCorrectly()
        {
            Int32 baseData = 123456;
            byte[] data = BitConverter.GetBytes(baseData);
            EnsureByteArrayIsBigEndianLikeSQLServer(data);

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "binary", false, 0, 0);
            Assert.AreEqual("0x000000000000000000000000000000000001E240", FieldScripting.valueAsTSQLLiteral((object)data, fieldInfo), "binary");
        }

        [Test()]
        public void VARBINARY_Data_ScriptsCorrectly()
        {
            Int32 baseData = 123456;
            byte[] data = BitConverter.GetBytes(baseData);
            EnsureByteArrayIsBigEndianLikeSQLServer(data);

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "varbinary", false, 0, 0);
            Assert.AreEqual("0x0001E240", FieldScripting.valueAsTSQLLiteral((object)data, fieldInfo), "varbinary");
        }

        [Test()]
        public void IMAGE_Data_ScriptsCorrectly()
        {
            Int32 baseData = 123456;
            byte[] data = BitConverter.GetBytes(baseData);
            EnsureByteArrayIsBigEndianLikeSQLServer(data);

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "image", false, 0, 0);
            Assert.AreEqual("0x0001E240", FieldScripting.valueAsTSQLLiteral((object)data, fieldInfo), "image");
        }


        [Test()]
        public void TIMESTAMP_Data_ScriptsCorrectly()
        {
            Int32 baseData = 123456;
            byte[] data = BitConverter.GetBytes(baseData);
            EnsureByteArrayIsBigEndianLikeSQLServer(data);

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 8, "timestamp", false, 0, 0);
            Assert.AreEqual("0x000000000001E240", FieldScripting.valueAsTSQLLiteral((object)data, fieldInfo), "timestamp");
        }

        [Test()]
        public void HIERARCHYID_Data_ScriptsCorrectly()
        {
            SqlHierarchyId baseData = SqlHierarchyId.Parse("/");
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "sys.junk.hierarchyid", false, 0, 0);
            Assert.AreEqual("0x", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "hierarchyid");

            baseData = SqlHierarchyId.Parse("/1/");
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "sys.junk.hierarchyid", false, 0, 0);
            Assert.AreEqual("0x58", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "hierarchyid");
        }

        [Test()]
        public void UNIQUEIDENTIFIER_Data_ScriptsCorrectly()
        {
            Guid baseData = Guid.Parse("9631B0CA-D86F-4CA2-BFA5-93A9980D050A");
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "uniqueidentifier", false, 0, 0);
            Assert.AreEqual("'9631B0CA-D86F-4CA2-BFA5-93A9980D050A'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "uniqueidentifier");
            Assert.AreEqual("9631B0CA-D86F-4CA2-BFA5-93A9980D050A", FieldScripting.formatGuid(data, false), "uniqueidentifier");
        }

        [Test()]
        public void SQLVARIANT_String_Data_ScriptsCorrectly()
        {
            string baseData = "sql variant test";
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 0, 0);
            Assert.AreEqual("N'sql variant test'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant");

            baseData = "That's fun!    ";
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 0, 0);
            Assert.AreEqual("N'That''s fun!    '", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant escapes string fields");
        }

        [Test()]
        public void SQLVARIANT_Guid_Data_ScriptsCorrectly()
        {
            Guid baseData = Guid.Parse("9631B0CA-D86F-4CA2-BFA5-93A9980D050A");
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 0, 0);
            Assert.AreEqual("'9631B0CA-D86F-4CA2-BFA5-93A9980D050A'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant");
        }

        [Test()]
        public void SQLVARIANT_Int_Data_ScriptsCorrectly()
        {
            int baseData = 1234567;
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 0, 0);
            Assert.AreEqual("1234567", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant");
        }

        [Test()]
        public void SQLVARIANT_Decimal_Data_ScriptsCorrectly()
        {
            Decimal baseData = 1234567.89M;
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 0, 0);
            Assert.AreEqual("1234567.89", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant");

            baseData = 12345.0000M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 10, 4);
            Assert.AreEqual("12345", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed");

            baseData = 12345.1000M;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 10, 4);
            Assert.AreEqual("12345.1", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed (partially significant decimal)");
        }

        [Test()]
        public void SQLVARIANT_Float_Data_ScriptsCorrectly()
        {
            Double baseData = 1234567.89;
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 0, 0);
            Assert.AreEqual("1234567.89", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant");

            baseData = 12345.0000;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 10, 4);
            Assert.AreEqual("12345", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed");

            baseData = 12345.2000;
            data = baseData;
            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 10, 4);
            Assert.AreEqual("12345.2", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "insignificant digits are trimmed (partially significant decimal)");
        }

        [Test()]
        public void SQLVARIANT_Bit_Data_ScriptsCorrectly()
        {
            bool baseData = false;
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 0, 0);
            Assert.AreEqual("0", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant");
        }

        [Test()]
        public void SQLVARIANT_DateTimeOffset_Data_ScriptsCorrectly()
        {
            DateTimeOffset baseData = new DateTimeOffset(new DateTime(2014, 5, 30), new TimeSpan(1, 0, 0));
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 0, 0);
            Assert.AreEqual("'2014-05-30T00:00:00+01:00'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant");
        }
        
        [Test()]
        public void SQLVARIANT_DateTime_Data_ScriptsCorrectly()
        {
            DateTime baseData = new DateTime(2014, 5, 30);
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sql_variant", false, 0, 0);
            Assert.AreEqual("'2014-05-30'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant");

            baseData = new DateTime(2014, 5, 30, 1, 2, 3);
            data = baseData;
            Assert.AreEqual("'2014-05-30T01:02:03'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant");

            baseData = baseData = new DateTime(2000, 10, 31, 2, 33, 44).AddMilliseconds(100);
            data = baseData;
            Assert.AreEqual("'2000-10-31T02:33:44.1'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "sql_variant partial fractional seconds");
        }

        [Test()]
        public void SQLVARIANT_Binary_Data_ScriptsCorrectly()
        {
            Int32 baseData = 123456;
            byte[] data = BitConverter.GetBytes(baseData);
            EnsureByteArrayIsBigEndianLikeSQLServer(data);

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 20, "sql_variant", false, 0, 0);
            Assert.AreEqual("0x0001E240", FieldScripting.valueAsTSQLLiteral((object)data, fieldInfo), "sql_variant");
        }

        [Test()]
        public void XML_Data_ScriptsCorrectly()
        {
            string baseData = "<r><n1 myattrib=\"testattrib\">Some Data</n1></r>";
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "xml", false, 0, 0);
            Assert.AreEqual("N'<r><n1 myattrib=\"testattrib\">Some Data</n1></r>'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "xml");

            baseData = "<r><n1 myattrib=\"testattrib\">Some Data</n1><n2>Property escaping... that's important.</n2></r>";
            data = baseData;

            fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "xml", false, 0, 0);
            Assert.AreEqual("N'<r><n1 myattrib=\"testattrib\">Some Data</n1><n2>Property escaping... that''s important.</n2></r>'", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "xml escapes quotes in strings");
        }

        [Test()]
        public void GEOGRAPHY_Data_ScriptsCorrectly()
        {
            SqlGeography baseData = SqlGeography.STGeomFromText(new SqlChars("LINESTRING(-122.360 47.656, -122.343 47.656 )"), 4326);
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sys.junk.geography", false, 0, 0);
            Assert.AreEqual("geography::STGeomFromText('LINESTRING (-122.36 47.656, -122.343 47.656)',4326)", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "geography");
            Assert.AreEqual("LINESTRING (-122.36 47.656, -122.343 47.656) 4326", FieldScripting.formatGeography(data, false), "geography");
        }

        [Test()]
        public void GEOMETRY_Data_ScriptsCorrectly()
        {
            SqlGeometry baseData = SqlGeometry.STGeomFromText(new SqlChars("LINESTRING (100 100, 20 180, 180 180)"), 0);
            object data = baseData;

            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "sys.junk.geometry", false, 0, 0);
            Assert.AreEqual("geometry::STGeomFromText('LINESTRING (100 100, 20 180, 180 180)',0)", FieldScripting.valueAsTSQLLiteral(data, fieldInfo), "geometry");
            Assert.AreEqual("LINESTRING (100 100, 20 180, 180 180) 0", FieldScripting.formatGeometry(data, false), "geometry");
        }

        [Test()]
        public void NULL_Data_ScriptsAsNull()
        {
            var fieldInfo = SchemaScriptingTests.FakeColumn("test", "test", 64, "int", false, 0, 0);
            Assert.AreEqual("NULL", FieldScripting.valueAsTSQLLiteral((object)System.DBNull.Value, fieldInfo), "int");
        }

        [Test()]
        public void ReservedWord_IsScriptedWithBrackets()
        {

            string fieldName = "Drop";
            var fieldInfo = SchemaScriptingTests.FakeColumn(fieldName, "test", 32, "int", false, 0, 0);

            Assert.AreEqual(true, TSqlRules.IsReservedWord(fieldName));
            Assert.AreEqual("[Drop]", FieldScripting.FieldNameOrDefault(fieldInfo,0));

        }

        [Test()]
        public void NonReservedWord_IsScriptedWithoutBrackets()
        {

            string fieldName = "TestColumnName";
            var fieldInfo = SchemaScriptingTests.FakeColumn(fieldName, "test", 32, "int", false, 0, 0);

            Assert.AreEqual(false, TSqlRules.IsReservedWord(fieldName));
            Assert.AreEqual("TestColumnName", FieldScripting.FieldNameOrDefault(fieldInfo, 0));

        }

        [Test()]
        public void EmptyColumnName_IsScriptedAnonymously()
        {

            string fieldName = "";
            var fieldInfo = SchemaScriptingTests.FakeColumn(fieldName, "test", 32, "int", false, 0, 0);

            Assert.AreEqual(false, TSqlRules.IsReservedWord(fieldName));
            Assert.AreEqual("anonymousColumn1", FieldScripting.FieldNameOrDefault(fieldInfo, 0));

        }

        private static void EnsureByteArrayIsBigEndianLikeSQLServer(byte[] data)
        {
            //SQL Server represents binary as Big Endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }
        }

    }
}
