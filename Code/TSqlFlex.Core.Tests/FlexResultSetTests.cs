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
    public class FlexResultSetTests
    {
        //todo: Support this query:
        /*
          --should have every data type that SQL Server 2014 supports, including aliases.
          SELECT CAST(1 AS BIGINT) [aBigInt], CAST(1.0 AS NUMERIC(2,1)) AS [aNumeric], CAST(1 AS BIT) AS [aBit],
          CAST(1 AS SMALLINT) AS [aSmallInt], CAST(1.0 AS DECIMAL(2,1)) AS [aDecimal], CAST(1.0 AS SMALLMONEY) as [aSmallMoney],
          CAST(1 AS INT) as [anInt], CAST(1 as TINYINT) as [aTinyInt], CAST(1 as MONEY) as [aMoney], 
          CAST(1 as FLOAT(53)) as [aFloat], CAST(1 as REAL),
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

            Assert.IsNotNull(fsr.schemaTables);
            Assert.AreEqual(0, fsr.schemaTables.Count);

            Assert.IsNotNull(fsr.exceptions);
            Assert.AreEqual(0, fsr.exceptions.Count);
        }

        [Test()]
        public void BIGINT_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("BIGINTNotNull", "MyStuff", 8, "bigint", false,0,0), false);
            dt.LoadDataRow(FakeColumn("BIGINTNull", "MyStuff", 8, "bigint", true, 0, 0), false);

            fsr.schemaTables.Add(dt);

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

            fsr.schemaTables.Add(dt);

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

            fsr.schemaTables.Add(dt);

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

            fsr.schemaTables.Add(dt);

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
            dt.LoadDataRow(FakeColumn("decimal2_1Null", "MyStuff", 17, "decimal", true, 4, 3), false);

            fsr.schemaTables.Add(dt);

            var expected = @"CREATE TABLE MyTable(
    decimal2_1NotNull decimal(2,1) NOT NULL,
    decimal2_1Null decimal(4,3) NULL
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

            fsr.schemaTables.Add(dt);

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

            fsr.schemaTables.Add(dt);

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

            fsr.schemaTables.Add(dt);

            var expected = @"CREATE TABLE MyTable(
    TinyIntNotNull tinyint NOT NULL,
    TinyIntNull tinyint NULL
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

            fsr.schemaTables.Add(dt);

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
        public void VARCHAR_ScriptsCorrectly()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = FakeSchemaDataTable();

            dt.LoadDataRow(FakeColumn("Name100NotNull", "MyStuff", 100, "varchar", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("NameMaxNotNull", "MyStuff", Int32.MaxValue, "varchar", false, 0, 0), false);
            dt.LoadDataRow(FakeColumn("Name100Null", "MyStuff", 100, "varchar", true, 0, 0), false);
            dt.LoadDataRow(FakeColumn("NameMaxNull", "MyStuff", Int32.MaxValue, "varchar", true, 0, 0), false);

            fsr.schemaTables.Add(dt);

            var expected = @"CREATE TABLE MyTable(
    Name100NotNull varchar(100) NOT NULL,
    NameMaxNotNull varchar(MAX) NOT NULL,
    Name100Null varchar(100) NULL,
    NameMaxNull varchar(MAX) NULL
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
