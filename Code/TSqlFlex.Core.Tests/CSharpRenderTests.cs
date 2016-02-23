using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core.Tests
{
    [TestFixture()]
    class CSharpRenderTests
    {
        [Test()]
        public void two_simple_columns_returns_valid_class()
        {
            var resultSet = new FlexResultSet();
            var result = new FlexResult();
            result.schema = new List<SQLColumn>() {
                new SQLColumn() { ColumnName = "testa", DataType = "int" },
                new SQLColumn() { ColumnName = "testb", DataType = "varchar" }
            };
            result.data = new List<object[]>();
            resultSet.results.Add(result);

            var srp = new SqlRunParameters(new SqlConnectionStringBuilder(), SqlRunParameters.TO_CSHARP, "");
            CSharpRenderer.renderAsCSharp(resultSet, srp);
            var expected = @"public class Result0
{
    public int testa { get; set; }
    public string testb { get; set; }
}
";
            Assert.AreEqual(srp.resultsText.ToString(), expected);
        }

        [Test()]
        public void handle_duplicate_column_names()
        {
            var resultSet = new FlexResultSet();
            var result = new FlexResult();
            result.schema = new List<SQLColumn>() {
                new SQLColumn() { ColumnName = "testa", DataType = "int" },
                new SQLColumn() { ColumnName = "testa", DataType = "varchar" }
            };
            result.data = new List<object[]>();
            resultSet.results.Add(result);

            var srp = new SqlRunParameters(new SqlConnectionStringBuilder(), SqlRunParameters.TO_CSHARP, "");
            CSharpRenderer.renderAsCSharp(resultSet, srp);
            var expected = @"public class Result0
{
    public int testa { get; set; }
    public string testa_2 { get; set; }
}
";
            Assert.AreEqual(srp.resultsText.ToString(), expected);
            
        }

        [Test()]
        public void FieldNameStartsWithAZazUnderscoreAt_WhenProcessed_FirstCharIsSame()
        {
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("aaaa")[0], 'a');
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("AAAA")[0], 'A');
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("zzzz")[0], 'z');
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("ZZZZ")[0], 'Z');
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("____")[0], '_');
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("@_")[0], '@');
        }

        [Test()]
        public void AtSignAfterFirstCharacter_WhenProcessed_IsReplaced()
        {
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("@"), "@");
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("@@"), "@_");
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("@@@"), "@__");
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("@a@"), "@a_");
        }

        [Test()]
        public void FieldNameStartsWithDashSpaceExclaim_WhenProcessed_FirstCharIsUnderscore()
        {
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("-test")[0], '_');
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName(" test")[0], '_');
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName("!test")[0], '_');
        }

        [Test()]
        public void EscapedNamesLeadingToACollisionAreHandledCorrectly()
        {
            var resultSet = new FlexResultSet();
            var result = new FlexResult();
            result.schema = new List<SQLColumn>() {
                new SQLColumn() { ColumnName = "test_1", DataType = "varchar" },
                new SQLColumn() { ColumnName = "test?1", DataType = "varchar" }
            };
            result.data = new List<object[]>();
            resultSet.results.Add(result);

            var srp = new SqlRunParameters(new SqlConnectionStringBuilder(), SqlRunParameters.TO_CSHARP, "");
            CSharpRenderer.renderAsCSharp(resultSet, srp);
            var expected = @"public class Result0
{
    public string test_1 { get; set; }
    public string test_1_2 { get; set; }
}
";
            Assert.AreEqual(srp.resultsText.ToString(), expected);
        }

        [Test()]
        public void AnonymousColumnsAreHandledCorrectly()
        {
            var resultSet = new FlexResultSet();
            var result = new FlexResult();
            result.schema = new List<SQLColumn>() {
                new SQLColumn() { ColumnName = "", DataType = "varchar" },
                new SQLColumn() { ColumnName = "", DataType = "int" }
            };
            result.data = new List<object[]>();
            resultSet.results.Add(result);

            var srp = new SqlRunParameters(new SqlConnectionStringBuilder(), SqlRunParameters.TO_CSHARP, "");
            CSharpRenderer.renderAsCSharp(resultSet, srp);
            var expected = @"public class Result0
{
    public string anonymousProperty { get; set; }
    public int anonymousProperty_2 { get; set; }
}
";
            Assert.AreEqual(srp.resultsText.ToString(), expected);
        }

    }


}
