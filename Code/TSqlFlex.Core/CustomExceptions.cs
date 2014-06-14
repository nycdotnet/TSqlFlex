using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{

    [Serializable]
    public class SqlExecutionException : Exception
    {
        public SqlExecutionException() { }
        public SqlExecutionException(Exception inner) : base(inner.Message, inner) { }
        public SqlExecutionException(string message) : base(message) { }
        public SqlExecutionException(string message, Exception inner) : base(message, inner) { }
        protected SqlExecutionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    [Serializable]
    public class SqlResultProcessingException : Exception
    {
      public SqlResultProcessingException() { }
      public SqlResultProcessingException( string message ) : base( message ) { }
      public SqlResultProcessingException( string message, Exception inner ) : base( message, inner ) { }
      public SqlResultProcessingException(Exception inner) : base(inner.Message, inner) { }
      protected SqlResultProcessingException( 
	    System.Runtime.Serialization.SerializationInfo info, 
	    System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
