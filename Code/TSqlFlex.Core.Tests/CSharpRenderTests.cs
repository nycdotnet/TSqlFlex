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

        [TestCase("aaaa", 'a')]
        [TestCase("AAAA", 'A')]
        [TestCase("zzzz", 'z')]
        [TestCase("ZZZZ", 'Z')]
        [TestCase("____", '_')]
        [TestCase("@_", '@')]
        public void FieldNameStartsWithAZazUnderscoreAt_WhenProcessed_FirstCharIsSame(
            string fieldName, char expectedFirstChar)
        {
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName(fieldName)[0], expectedFirstChar);
        }

        [TestCase("@", "@")]
        [TestCase("@@", "@_")]
        [TestCase("@@@", "@__")]
        [TestCase("@a@", "@a_")]
        public void AtSignAfterFirstCharacter_WhenProcessed_IsReplaced(
            string fieldName, string expectedResult)
        {
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName(fieldName), expectedResult);
        }


        [TestCase("-test", '_')]
        [TestCase(" test", '_')]
        [TestCase("!test", '_')]
        public void FieldNameStartsWithDashSpaceExclaim_WhenProcessed_FirstCharIsUnderscore(
            string fieldName, char expectedFirstChar)
        {
            Assert.AreEqual(CSharpRenderer.FieldNameToCSharpPropertyName(fieldName)[0], expectedFirstChar);
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
