using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class CSVReader
    {

        public List<string> readBuffers = new List<string>();

        public CSVReader(Stream inputStream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
        {
            using (StreamReader reader = new StreamReader(inputStream, encoding, detectEncodingFromByteOrderMarks, bufferSize))
            {
                int charsRead = 0;
                char[] buffer = new char[bufferSize];
                do
                {
                    charsRead = reader.Read(buffer, 0, bufferSize);
                    if (charsRead == bufferSize)
                    {
                        readBuffers.Add(new string(buffer));
                    }
                    else
                    {
                        new string(buffer.Take(charsRead).ToArray());
                    }
                } while (charsRead == bufferSize);

            }
        }
    }
}
