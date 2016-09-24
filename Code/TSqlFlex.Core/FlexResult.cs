using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace TSqlFlex.Core
{
    public class FlexResult
    {
        public List<SQLColumn> schema = null;
        public List<Exception> exceptions = new List<Exception>();
        public List<Object[]> data = null;
        public Int64 recordsAffected = 0;
        public int visibleColumnCount {
            get {
                return schema.Where(col => !col.IsHidden).Count();
            }
        }
    }
}
