using System.Collections.Generic;
using NUnit.Framework;

namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    public class FlexResultSetTests
    {
        [Test()]
        public void CreatingEmptyFlexResultSet_ResultsInEmptyCollections()
        {
            FlexResultSet fsr = new FlexResultSet();
            Assert.IsNotNull(fsr);

            Assert.IsNotNull(fsr.results);
            Assert.AreEqual(0, fsr.results.Count);
        }

        [Test()]
        public void ResultSet_WithNoReturnedSchema_ResultsInNoReturnedSchemaComment()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = SchemaScriptingTests.FakeSchemaDataTable();

            fsr.results.Add(new FlexResult());

            Assert.AreEqual("--No schema for result from query.", fsr.ScriptResultDataAsInsert(0, "#result0").ToString());
        }

        [Test()]
        public void ResultSet_WithNoReturnedData_ResultsInNoReturnedDataComment()
        {
            FlexResultSet fsr = new FlexResultSet();

            var dt = SchemaScriptingTests.FakeSchemaDataTable();

            dt.LoadDataRow(SchemaScriptingTests.FakeColumn("IntNotNull", "MyStuff", 32, "int", false, 255, 255), false);
            dt.LoadDataRow(SchemaScriptingTests.FakeColumn("IntNull", "MyStuff", 32, "int", true, 255, 255), false);

            fsr.results.Add(new FlexResult());
            fsr.results[0].schema = dt;
            fsr.results[0].data = new List<object[]>();

            Assert.AreEqual("--No rows were returned from the query.", fsr.ScriptResultDataAsInsert(0, "#result0").ToString());
        }
    }
}
