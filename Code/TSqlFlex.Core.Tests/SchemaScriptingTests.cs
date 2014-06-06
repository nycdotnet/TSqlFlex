using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TSqlFlex.Core;

namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    public class SchemaScriptingTests
    {
        /*
          --This query has every column data type that SQL Server 2014 supports, including aliases.
          SELECT CAST(1 AS BIGINT) [aBigInt], CAST(1.0 AS NUMERIC(2,1)) AS [aNumeric], CAST(1 AS BIT) AS [aBit],
          CAST(1 AS SMALLINT) AS [aSmallInt], CAST(1.0 AS DECIMAL(2,1)) AS [aDecimal], CAST(1.0 AS SMALLMONEY) as [aSmallMoney],
          CAST(1 AS INT) as [anInt], CAST(1 as TINYINT) as [aTinyInt], CAST(1 as MONEY) as [aMoney], 
          CAST(1 as FLOAT(53)) as [aFloat], CAST(1 as FLOAT(24)) as [aFloat], CAST(1 as REAL),
          CAST('2000-01-01T00:00:00' as DATE) as [aDate], CAST('2000-01-01T00:00:00+01:00' as DATETIMEOFFSET) as [aDateTimeOffset],
          CAST('2000-01-01T00:00:00' as DATETIME2) as [aDateTime2], CAST('2000-01-01T00:00:00' as SMALLDATETIME) as [aSmallDateTime],
          CAST('2000-01-01T00:00:00' as DATETIME) as [aDateTime], CAST('2000-01-01T00:00:00' as TIME) as [aTime],
          CAST('ABC' as CHAR(3)) as [aCHAR], CAST('ABC' as VARCHAR(3)) as [aVarChar], CAST('ABC' as TEXT) as [aText],
          CAST(N'ABC' as NCHAR(3)) as [anNCHAR], CAST(N'ABC' as NVARCHAR(3)) as [anNVarCHAR], CAST(N'ABC' as NTEXT) as [anNText],
          CAST(1 as BINARY) as [aBinary], CAST(1 as VARBINARY) as [aVarBinary], CAST('ABC' as IMAGE) as [anImage],
          CAST(0 AS TIMESTAMP) as [aTimestamp], CAST('/' as HIERARCHYID) as [aHierarchyID], NewID() as [aUniqueIdentifier],
          CAST(1 as sql_variant), cast('<a />' as XML),
           (geography::STGeomFromText('LINESTRING(-122.360 47.656, -122.343 47.656 )', 4326)) as [aGeography],
           (geometry::STGeomFromText('LINESTRING (100 100, 20 180, 180 180)', 0)) as [aGeometry]
         * */

        [Test()]
        public void CreatingEmptyFlexResultSet_ResultsInEmptyCollections()
        {
            FlexResultSet fsr = new FlexResultSet();
            Assert.IsNotNull(fsr);
            
            Assert.IsNotNull(fsr.results);
            Assert.AreEqual(0, fsr.results.Count);

       }

        [Test()]
        public void BIGINT_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("BIGINTNotNull", "MyStuff", 8, "bigint", false,0,0), false);
            dt.LoadDataRow(FakeColumn("BIGINTNull", "MyStuff", 8, "bigint", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    BIGINTNotNull bigint NOT NULL,
    BIGINTNull bigint NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void NUMERIC_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("Numeric2_1NotNull", "MyStuff", 17, "numeric", false, 2, 1), false);
            dt.LoadDataRow(FakeColumn("Numeric2_1Null", "MyStuff", 17, "numeric", true, 4, 3), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;


            var expected = @"CREATE TABLE MyTable(
    Numeric2_1NotNull numeric(2,1) NOT NULL,
    Numeric2_1Null numeric(4,3) NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void BIT_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("BitNotNull", "MyStuff", 1, "bit", false, 255, 255), false);
            dt.LoadDataRow(FakeColumn("BitNull", "MyStuff", 1, "bit", true, 255, 255), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;


            var expected = @"CREATE TABLE MyTable(
    BitNotNull bit NOT NULL,
    BitNull bit NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void SMALLINT_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("SmallIntNotNull", "MyStuff", 16, "smallint", false, 255, 255), false);
            dt.LoadDataRow(FakeColumn("SmallIntNull", "MyStuff", 16, "smallint", true, 255, 255), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;


            var expected = @"CREATE TABLE MyTable(
    SmallIntNotNull smallint NOT NULL,
    SmallIntNull smallint NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }


        [Test()]
        public void DECIMAL_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("decimal2_1NotNull", "MyStuff", 17, "decimal", false, 2, 1), false);
            dt.LoadDataRow(FakeColumn("decimal2_1Null", "MyStuff", 17, "decimal", true, 2, 1), false);
            dt.LoadDataRow(FakeColumn("decimal4_3NotNull", "MyStuff", 17, "decimal", false, 4, 3), false);
            dt.LoadDataRow(FakeColumn("decimal4_3Null", "MyStuff", 17, "decimal", true, 4, 3), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;


            var expected = @"CREATE TABLE MyTable(
    decimal2_1NotNull decimal(2,1) NOT NULL,
    decimal2_1Null decimal(2,1) NULL,
    decimal4_3NotNull decimal(4,3) NOT NULL,
    decimal4_3Null decimal(4,3) NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }


        [Test()]
        public void SMALLMONEY_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("SmallMoneyNotNull", "MyStuff", 4, "smallmoney", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("SmallMoneyNull", "MyStuff", 4, "smallmoney", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;


            var expected = @"CREATE TABLE MyTable(
    SmallMoneyNotNull smallmoney NOT NULL,
    SmallMoneyNull smallmoney NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }


        [Test()]
        public void INT_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("IntNotNull", "MyStuff", 32, "int", false, 255, 255), false);
            dt.LoadDataRow(FakeColumn("IntNull", "MyStuff", 32, "int", true, 255, 255), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    IntNotNull int NOT NULL,
    IntNull int NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void TINYINT_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("TinyIntNotNull", "MyStuff", 8, "tinyint", false, 255, 255), false);
            dt.LoadDataRow(FakeColumn("TinyIntNull", "MyStuff", 8, "tinyint", true, 255, 255), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    TinyIntNotNull tinyint NOT NULL,
    TinyIntNull tinyint NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void MONEY_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("MoneyNotNull", "MyStuff", 8, "money", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("MoneyNull", "MyStuff", 8, "money", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    MoneyNotNull money NOT NULL,
    MoneyNull money NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }


         [Test()]
        public void FLOAT_AND_REAL_ScriptCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("Float53NotNull", "MyStuff", 64, "float", false, 2, 1), false);
            dt.LoadDataRow(FakeColumn("Float53Null", "MyStuff", 64, "float", true, 2, 1), false);
            dt.LoadDataRow(FakeColumn("Float40NotNull", "MyStuff", 64, "float", false, 2, 1), false);
            dt.LoadDataRow(FakeColumn("Float40Null", "MyStuff", 64, "float", true, 2, 1), false);
            dt.LoadDataRow(FakeColumn("Float24NotNull", "MyStuff", 32, "real", false, 4, 3), false);
            dt.LoadDataRow(FakeColumn("Float24Null", "MyStuff", 32, "real", true, 4, 3), false);
            dt.LoadDataRow(FakeColumn("Float17NotNull", "MyStuff", 32, "real", false, 4, 3), false);
            dt.LoadDataRow(FakeColumn("Float17Null", "MyStuff", 32, "real", true, 4, 3), false);
            
            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    Float53NotNull float(53) NOT NULL,
    Float53Null float(53) NULL,
    Float40NotNull float(53) NOT NULL,
    Float40Null float(53) NULL,
    Float24NotNull float(24) NOT NULL,
    Float24Null float(24) NULL,
    Float17NotNull float(24) NOT NULL,
    Float17Null float(24) NULL
);
";

            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }


         [Test()]
         public void DATE_ScriptsCorrectly()
         {
             FlexResultSet fsr = new FlexResultSet();

             var dt = FakeSchemaDataTable();

             dt.LoadDataRow(FakeColumn("DateNotNull", "MyStuff", 8, "date", false, 0, 0), false);
             dt.LoadDataRow(FakeColumn("DateNull", "MyStuff", 8, "date", true, 0, 0), false);

             fsr.results.Add(new FlexResult());
             fsr.results[0].schema = dt;

             var expected = @"CREATE TABLE MyTable(
    DateNotNull date NOT NULL,
    DateNull date NULL
);
";
             Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
         }

         [Test()]
         public void DATETIMEOFFSET_ScriptsCorrectly()
         {
             FlexResultSet fsr = new FlexResultSet();

             var dt = FakeSchemaDataTable();

             dt.LoadDataRow(FakeColumn("DateTimeOffset_7NotNull", "MyStuff", 80, "datetimeoffset", false, 0, 7), false);
             dt.LoadDataRow(FakeColumn("DateTimeOffset_7Null", "MyStuff", 80, "datetimeoffset", true, 0, 7), false);
             dt.LoadDataRow(FakeColumn("DateTimeOffset_2NotNull", "MyStuff", 80, "datetimeoffset", false, 0, 2), false);
             dt.LoadDataRow(FakeColumn("DateTimeOffset_2Null", "MyStuff", 80, "datetimeoffset", true, 0, 2), false);
             dt.LoadDataRow(FakeColumn("DateTimeOffset_4NotNull", "MyStuff", 80, "datetimeoffset", false, 0, 4), false);
             dt.LoadDataRow(FakeColumn("DateTimeOffset_4Null", "MyStuff", 80, "datetimeoffset", true, 0, 4), false);

             fsr.results.Add(new FlexResult());
             fsr.results[0].schema = dt;

             var expected = @"CREATE TABLE MyTable(
    DateTimeOffset_7NotNull datetimeoffset NOT NULL,
    DateTimeOffset_7Null datetimeoffset NULL,
    DateTimeOffset_2NotNull datetimeoffset(2) NOT NULL,
    DateTimeOffset_2Null datetimeoffset(2) NULL,
    DateTimeOffset_4NotNull datetimeoffset(4) NOT NULL,
    DateTimeOffset_4Null datetimeoffset(4) NULL
);
";
             Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
         }

         [Test()]
         public void DATETIME2_ScriptsCorrectly()
         {
             FlexResultSet fsr = new FlexResultSet();

             var dt = FakeSchemaDataTable();

             dt.LoadDataRow(FakeColumn("DateTime2NotNull", "MyStuff", 8, "datetime2", false, 0, 0), false);
             dt.LoadDataRow(FakeColumn("DateTime2Null", "MyStuff", 8, "datetime2", true, 0, 0), false);

             fsr.results.Add(new FlexResult());
             fsr.results[0].schema = dt;

             var expected = @"CREATE TABLE MyTable(
    DateTime2NotNull datetime2 NOT NULL,
    DateTime2Null datetime2 NULL
);
";
             Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
         }

         [Test()]
         public void SMALLDATETIME_ScriptsCorrectly()
         {
             FlexResultSet fsr = new FlexResultSet();

             var dt = FakeSchemaDataTable();

             dt.LoadDataRow(FakeColumn("SmallDateTimeNotNull", "MyStuff", 8, "smalldatetime", false, 0, 0), false);
             dt.LoadDataRow(FakeColumn("SmallDateTimeNull", "MyStuff", 8, "smalldatetime", true, 0, 0), false);

             fsr.results.Add(new FlexResult());
             fsr.results[0].schema = dt;

             var expected = @"CREATE TABLE MyTable(
    SmallDateTimeNotNull smalldatetime NOT NULL,
    SmallDateTimeNull smalldatetime NULL
);
";
             Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
         }

         [Test()]
         public void DATETIME_ScriptsCorrectly()
         {
             FlexResultSet fsr = new FlexResultSet();

             var dt = FakeSchemaDataTable();

             dt.LoadDataRow(FakeColumn("DateTimeNotNull", "MyStuff", 8, "datetime", false, 0, 0), false);
             dt.LoadDataRow(FakeColumn("DateTimeNull", "MyStuff", 8, "datetime", true, 0, 0), false);

             fsr.results.Add(new FlexResult());
             fsr.results[0].schema = dt;

             var expected = @"CREATE TABLE MyTable(
    DateTimeNotNull datetime NOT NULL,
    DateTimeNull datetime NULL
);
";
             Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
         }

         [Test()]
         public void TIME_ScriptsCorrectly()
         {
             FlexResultSet fsr = new FlexResultSet();

             var dt = FakeSchemaDataTable();

             dt.LoadDataRow(FakeColumn("TimeNotNull", "MyStuff", 8, "time", false, 0, 7), false);
             dt.LoadDataRow(FakeColumn("TimeNull", "MyStuff", 8, "time", true, 0, 7), false);
             dt.LoadDataRow(FakeColumn("Time2NotNull", "MyStuff", 8, "time", false, 0, 2), false);
             dt.LoadDataRow(FakeColumn("Time2Null", "MyStuff", 8, "time", true, 0, 2), false);
             dt.LoadDataRow(FakeColumn("Time4NotNull", "MyStuff", 8, "time", false, 0, 4), false);
             dt.LoadDataRow(FakeColumn("Time4Null", "MyStuff", 8, "time", true, 0, 4), false);

             fsr.results.Add(new FlexResult());
             fsr.results[0].schema = dt;

             var expected = @"CREATE TABLE MyTable(
    TimeNotNull time NOT NULL,
    TimeNull time NULL,
    Time2NotNull time(2) NOT NULL,
    Time2Null time(2) NULL,
    Time4NotNull time(4) NOT NULL,
    Time4Null time(4) NULL
);
";
             Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
         }

         [Test()]
         public void CHAR_ScriptsCorrectly()
         {
             FlexResultSet fsr = new FlexResultSet();

             var dt = FakeSchemaDataTable();

             dt.LoadDataRow(FakeColumn("Char100NotNull", "MyStuff", 100, "char", false, 0, 0), false);
             dt.LoadDataRow(FakeColumn("Char100Null", "MyStuff", 100, "char", true, 0, 0), false);

             fsr.results.Add(new FlexResult());
             fsr.results[0].schema = dt;

             var expected = @"CREATE TABLE MyTable(
    Char100NotNull char(100) NOT NULL,
    Char100Null char(100) NULL
);
";
             Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
         }


         [Test()]
         public void VARCHAR_ScriptsCorrectly()
         {
             FlexResultSet fsr = new FlexResultSet();

             var dt = FakeSchemaDataTable();

             dt.LoadDataRow(FakeColumn("Name100NotNull", "MyStuff", 100, "varchar", false, 0, 0), false);
             dt.LoadDataRow(FakeColumn("NameMaxNotNull", "MyStuff", Int32.MaxValue, "varchar", false, 0, 0), false);
             dt.LoadDataRow(FakeColumn("Name100Null", "MyStuff", 100, "varchar", true, 0, 0), false);
             dt.LoadDataRow(FakeColumn("NameMaxNull", "MyStuff", Int32.MaxValue, "varchar", true, 0, 0), false);

             fsr.results.Add(new FlexResult());
             fsr.results[0].schema = dt;

             var expected = @"CREATE TABLE MyTable(
    Name100NotNull varchar(100) NOT NULL,
    NameMaxNotNull varchar(MAX) NOT NULL,
    Name100Null varchar(100) NULL,
    NameMaxNull varchar(MAX) NULL
);
";
             Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
         }

         [Test()]
         public void TEXT_ScriptsCorrectly()
         {
             FlexResultSet fsr = new FlexResultSet();

             var dt = FakeSchemaDataTable();

             dt.LoadDataRow(FakeColumn("TextNotNull", "MyStuff", 0, "text", false, 0, 0), false);
             dt.LoadDataRow(FakeColumn("TextNull", "MyStuff", 0, "text", true, 0, 0), false);

             fsr.results.Add(new FlexResult());
             fsr.results[0].schema = dt;

             var expected = @"CREATE TABLE MyTable(
    TextNotNull text NOT NULL,
    TextNull text NULL
);
";
             Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
         }

         [Test()]
         public void NCHAR_ScriptsCorrectly()
         {
             FlexResultSet fsr = new FlexResultSet();

             var dt = FakeSchemaDataTable();

             dt.LoadDataRow(FakeColumn("NChar100NotNull", "MyStuff", 100, "nchar", false, 0, 0), false);
             dt.LoadDataRow(FakeColumn("NChar100Null", "MyStuff", 100, "nchar", true, 0, 0), false);

             fsr.results.Add(new FlexResult());
             fsr.results[0].schema = dt;

             var expected = @"CREATE TABLE MyTable(
    NChar100NotNull nchar(100) NOT NULL,
    NChar100Null nchar(100) NULL
);
";
             Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
         }


        [Test()]
        public void NVARCHAR_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("Name100NotNull", "MyStuff", 100, "nvarchar", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("NameMaxNotNull", "MyStuff", Int32.MaxValue, "nvarchar", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("Name100Null", "MyStuff", 100, "nvarchar", true, 0, 0), false);
            dt.LoadDataRow(FakeColumn("NameMaxNull", "MyStuff", Int32.MaxValue, "nvarchar", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    Name100NotNull nvarchar(100) NOT NULL,
    NameMaxNotNull nvarchar(MAX) NOT NULL,
    Name100Null nvarchar(100) NULL,
    NameMaxNull nvarchar(MAX) NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void NTEXT_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("NTextNotNull", "MyStuff", 0, "ntext", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("NTextNull", "MyStuff", 0, "ntext", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    NTextNotNull ntext NOT NULL,
    NTextNull ntext NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void BINARY_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("Binary100NotNull", "MyStuff", 100, "binary", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("Binary100Null", "MyStuff", 100, "binary", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    Binary100NotNull binary(100) NOT NULL,
    Binary100Null binary(100) NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void VARBINARY_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("VarBin100NotNull", "MyStuff", 100, "varbinary", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("VarBinMaxNotNull", "MyStuff", Int32.MaxValue, "varbinary", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("VarBin100Null", "MyStuff", 100, "varbinary", true, 0, 0), false);
            dt.LoadDataRow(FakeColumn("VarBinMaxNull", "MyStuff", Int32.MaxValue, "varbinary", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    VarBin100NotNull varbinary(100) NOT NULL,
    VarBinMaxNotNull varbinary(MAX) NOT NULL,
    VarBin100Null varbinary(100) NULL,
    VarBinMaxNull varbinary(MAX) NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }


        [Test()]
        public void IMAGE_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("ImageNotNull", "MyStuff", 0, "image", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("ImageNull", "MyStuff", 0, "image", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    ImageNotNull image NOT NULL,
    ImageNull image NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }


        [Test()]
        public void TIMESTAMP_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("TimeStampNotNull", "MyStuff", 8, "timestamp", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("TimeStampNull", "MyStuff", 8, "timestamp", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    TimeStampNotNull timestamp NOT NULL,
    TimeStampNull timestamp NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void HIERARCHYID_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("HierarchyIdNotNull", "MyStuff", 8, "dbname.sys.hierarchyid", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("HierarchyIdNull", "MyStuff", 8, "dbname.sys.hierarchyid", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    HierarchyIdNotNull hierarchyid NOT NULL,
    HierarchyIdNull hierarchyid NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void UNIQUEIDENTIFIER_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("UniqueIdentifierNotNull", "MyStuff", 128, "uniqueidentifier", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("UniqueIdentifierNull", "MyStuff", 128, "uniqueidentifier", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    UniqueIdentifierNotNull uniqueidentifier NOT NULL,
    UniqueIdentifierNull uniqueidentifier NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void SQL_VARIANT_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("Sql_VariantNotNull", "MyStuff", 128, "sql_variant", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("Sql_VariantNull", "MyStuff", 128, "sql_variant", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    Sql_VariantNotNull sql_variant NOT NULL,
    Sql_VariantNull sql_variant NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void XML_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("XMLNotNull", "MyStuff", 0, "xml", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("XMLNull", "MyStuff", 0, "xml", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    XMLNotNull xml NOT NULL,
    XMLNull xml NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void GEOGRAPHY_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("GeographyNotNull", "MyStuff", 0, "dbname.sys.geography", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("GeographyNull", "MyStuff", 0, "dbname.sys.geography", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    GeographyNotNull geography NOT NULL,
    GeographyNull geography NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }

        [Test()]
        public void GEOMETRY_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("GeometryNotNull", "MyStuff", 0, "dbname.sys.geometry", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("GeometryNull", "MyStuff", 0, "dbname.sys.geometry", true, 0, 0), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;

            var expected = @"CREATE TABLE MyTable(
    GeometryNotNull geometry NOT NULL,
    GeometryNull geometry NULL
);
";
            Assert.AreEqual(expected, fsr.ScriptResultAsCreateTable(0, "MyTable"));
        }


        public static DataTable FakeSchemaDataTable() {
            var dt = new DataTable("test");
            dt.Columns.Add(new DataColumn("ColumnName",typeof(object)));
            dt.Columns.Add(new DataColumn("ColumnOrdinal", typeof(object)));
            dt.Columns.Add(new DataColumn("ColumnSize", typeof(object)));
            dt.Columns.Add(new DataColumn("NumericPrecision", typeof(object)));
            dt.Columns.Add(new DataColumn("NumericScale", typeof(object)));
            dt.Columns.Add(new DataColumn("IsUnique", typeof(object)));
            dt.Columns.Add(new DataColumn("IsKey", typeof(object)));
            dt.Columns.Add(new DataColumn("BaseServerName", typeof(object)));
            dt.Columns.Add(new DataColumn("BaseCatalogName", typeof(object)));
            dt.Columns.Add(new DataColumn("BaseColumnName", typeof(object)));
            dt.Columns.Add(new DataColumn("BaseSchemaName", typeof(object)));
            dt.Columns.Add(new DataColumn("BaseTableName", typeof(object)));
            dt.Columns.Add(new DataColumn("DataType", typeof(object)));
            dt.Columns.Add(new DataColumn("AllowDBNull", typeof(object)));
            dt.Columns.Add(new DataColumn("ProviderType", typeof(object)));
            dt.Columns.Add(new DataColumn("IsAliased", typeof(object)));
            dt.Columns.Add(new DataColumn("IsExpression", typeof(object)));
            dt.Columns.Add(new DataColumn("IsIdentity", typeof(object)));
            dt.Columns.Add(new DataColumn("IsAutoIncrement", typeof(object)));
            dt.Columns.Add(new DataColumn("IsRowVersion", typeof(object)));
            dt.Columns.Add(new DataColumn("IsHidden", typeof(object)));
            dt.Columns.Add(new DataColumn("IsLong", typeof(object)));
            dt.Columns.Add(new DataColumn("IsReadOnly", typeof(object)));
            dt.Columns.Add(new DataColumn("ProviderSpecificDataType", typeof(object)));
            dt.Columns.Add(new DataColumn("DataTypeName", typeof(object)));
            dt.Columns.Add(new DataColumn("XmlSchemaCollectionDatabase", typeof(object)));
            dt.Columns.Add(new DataColumn("XmlSchemaCollectionOwningSchema", typeof(object)));
            dt.Columns.Add(new DataColumn("XmlSchemaCollectionName", typeof(object)));
            dt.Columns.Add(new DataColumn("UdtAssemblyQualifiedName", typeof(object)));
            dt.Columns.Add(new DataColumn("NonVersionedProviderType", typeof(object)));
            dt.Columns.Add(new DataColumn("IsColumnSet", typeof(object)));
            return dt;
        }

        public static object[] FakeColumn(string ColumnName, string TableName, int ColumnSize, string DataType, bool AllowNulls, short NumericPrecision, short NumericScale)
        {
            return new object[] { ColumnName, (int)0, (int)ColumnSize, NumericPrecision, NumericScale, false, false, System.DBNull.Value, DBNull.Value, ColumnName, DBNull.Value, TableName, typeof(String), AllowNulls, (int)12, false, false, false, false, false, false, false, false, typeof(System.Data.SqlTypes.SqlString), DataType, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, (int)12, false };
        }
    }
}
