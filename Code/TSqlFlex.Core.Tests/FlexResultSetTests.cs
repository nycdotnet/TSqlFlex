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

        [Test()]
        public void CanCreateEmptyFlexResultSet()
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

        public static DataTable FakeSchemaDataTable() {
            var dt = new DataTable("test");
            dt.LoadDataRow(new object[] { "Name", (int)0, (int)100, (short)255, (short)255, false, false, System.DBNull.Value, DBNull.Value, "Name", DBNull.Value, "MyStuff", typeof(String), false, (int)12, false, false, false, false, false, false, false, false, typeof(System.Data.SqlTypes.SqlString), "nvarchar", DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, (int)12, false }, false);
            return dt;
        }
    }
}
