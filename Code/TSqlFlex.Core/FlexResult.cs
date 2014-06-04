using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class FlexResult
    {
        public DataTable schema = null;  //todo: should really convert this to an array so we keep just the stuff we need.
        public List<Exception> exceptions = new List<Exception>();
        public List<Object[]> data = null;

    }
}
