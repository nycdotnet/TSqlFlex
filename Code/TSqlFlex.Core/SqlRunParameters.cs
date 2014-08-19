using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace TSqlFlex.Core
{
    public class SqlRunParameters
    {
        public const string TO_INSERT_STATEMENTS = "To INSERT Statements";
        public const string TO_XML_SPREADSHEET = "To XML Spreadsheet (Excel)";

        public SqlConnectionStringBuilder connStringBuilder;
        public string sqlToRun;
        public string outputType;
        public string tempOutputFileName;
        public StreamWriter tempOutputStream; //todo: need to handle disposal of this better in the sad case.
        public StringBuilder scriptedResult = null;

        public SqlRunParameters(SqlConnectionStringBuilder csb, string sqlToRun, string outputType, string outputFileName = "")
        {
            this.connStringBuilder = csb;
            this.sqlToRun = sqlToRun;
            this.outputType = outputType;
            this.tempOutputFileName = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "TSqlFlex" + DateTime.Now.ToString("_yyyyMMddTHHmmss.fffffff") + ".xml");
            this.tempOutputStream = new StreamWriter(File.Open(tempOutputFileName, FileMode.Create), Encoding.UTF8);
        }
        
    }
}
