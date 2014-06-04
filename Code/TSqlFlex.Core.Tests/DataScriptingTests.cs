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
    }
}
