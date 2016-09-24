using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    class QueryRunnerTests
    {

        [Test()]
        public void EmptyResultsProducesEmptySqlColumnList()
        {
            var sut = SQLColumnList.CreateFromSchemaTable(null);
            Assert.IsNotNull(sut);
            Assert.AreEqual(0, sut.Count);
        }

        [Test()]
        public void EmptyColumnList_NotRenderableAsCreateTable()
        {
            var sut = new FlexResult();
            sut.schema = new List<SQLColumn>();
            Assert.AreEqual(false, FieldScripting.ResultIsRenderableAsCreateTable(sut));
        }

        [Test()]
        public void NullColumnList_NotRenderableAsCreateTable()
        {
            var sut = new FlexResult();
            Assert.AreEqual(false, FieldScripting.ResultIsRenderableAsCreateTable(sut));
        }

        [Test()]
        public void PopulatedColumnList_IsRenderableAsCreateTable()
        {
            var sut = new FlexResult();
            sut.schema = new List<SQLColumn>() { new SQLColumn() { ColumnName = "test", DataType = "int" }};
            Assert.AreEqual(true, FieldScripting.ResultIsRenderableAsCreateTable(sut));
        }

    }
}
