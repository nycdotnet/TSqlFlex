using System;
using System.Data.SqlClient;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace TSqlFlex.Core
{
    public class SqlRunParameters : IDisposable
    {
        public const string TO_INSERT_STATEMENTS = "To INSERT Statements";
        public const string TO_XML_SPREADSHEET = "To XML Spreadsheet (Excel)";

        public SqlConnectionStringBuilder connStringBuilder;
        public string sqlToRun;
        public string outputType;
        public StringBuilder exceptionsText = new StringBuilder();
        public StringBuilder resultsText = new StringBuilder();
        private string fileName;
        private StreamWriter outputStream;
        public int exceptionCount = 0;
        public bool worksheetIsValid = false;

        public string outputFilename()
        {
            return this.fileName;
        }
        

        public SqlRunParameters(SqlConnectionStringBuilder csb, string sqlToRun, string outputType, string outputFileName = "")
        {
            this.connStringBuilder = csb;
            this.sqlToRun = sqlToRun;
            this.outputType = outputType;
            //need a non-roaming store.

            var isolatedStore = IsolatedStorageFile.GetUserStoreForDomain();

            this.fileName = "TSQLFlex_" + DateTime.UtcNow.ToString("yyyyMMddhhmmss.fffffff") + "_temp.txt";

            var isoStream = new IsolatedStorageFileStream(this.fileName, FileMode.OpenOrCreate, FileAccess.Write, isolatedStore);
            this.outputStream = new StreamWriter(isoStream, Encoding.UTF8);

        }

        public void flushAndCloseOutputStreamIfNeeded()
        {
            if (outputStream != null && outputStream.BaseStream != null)
            {
                outputStream.Flush();
                outputStream.Close();
            }
        }

        public virtual void Dispose()
        {
            flushAndCloseOutputStreamIfNeeded();
            outputStream.Dispose();
        }

        public void WriteToStream(string dataToWrite)
        {
            outputStream.Write(dataToWrite);
        }

        public void saveOutputStreamTo(string saveAsFileName)
        {
            flushAndCloseOutputStreamIfNeeded();

            var isolatedStore = IsolatedStorageFile.GetUserStoreForDomain();
            var isoStream = new IsolatedStorageFileStream(this.fileName, FileMode.Open, FileAccess.Read, isolatedStore);

            var outputStreamWriter = new FileStream(saveAsFileName, FileMode.OpenOrCreate, FileAccess.Write);
            
            CopyStreamToEnd(isoStream, outputStreamWriter);
            
            outputStreamWriter.Flush();
            outputStreamWriter.Close();
            isoStream.Close();

        }

        //thanks! http://stackoverflow.com/questions/12970957/when-shall-i-do-a-explicity-flush-while-copying-streams-in-c
        public static void CopyStreamToEnd(Stream source, Stream destination)
        {
            byte[] buffer = new byte[32768];
            int bytesReadCount;
            while ((bytesReadCount = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                destination.Write(buffer, 0, bytesReadCount);
            }
        }
        
    }
}
