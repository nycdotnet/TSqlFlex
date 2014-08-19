using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace TSqlFlex.Core
{
    public class FlexResult
    {
        public DataTable schema = null;  //todo: should really convert this to an array so we keep just the stuff we need.
        public List<Exception> exceptions = new List<Exception>();
        public List<Object[]> data = null;
        public Int64 recordsAffected = 0;
        public int visibleColumnCount { get {
                int count = 0;
                foreach (DataRow c in schema.Rows) {
                    if ((bool)c.ItemArray[20] == false) {
                        count += 1;
                    }
                }
                return count;
            }
        }
        
    }
}
