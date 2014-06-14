using System;
using System.Collections.Generic;
using System.Data;

namespace TSqlFlex.Core
{
    public class FlexResult
    {
        public DataTable schema = null;  //todo: should really convert this to an array so we keep just the stuff we need.
        public List<Exception> exceptions = new List<Exception>();
        public List<Object[]> data = null;
        public Int64 recordsAffected = 0;
    }
}
