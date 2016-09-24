using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class TSqlRules
    {
        private static HashSet<string> reservedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
            "action",
            "alter",
            "as",
            "asc",
            "auto",
            "by",
            "close",
            "clustered",
            "changes",
            "column",
            "constraint",
            "create",
            "dec",
            "default",
            "delete",
            "desc",
            "drop",
            "elements",
            "exec",
            "execute",
            "from",
            "function",
            "group",
            "insert",
            "index",
            "key",
            "level",
            "nonclustered",
            "none",
            "oct",
            "open",
            "order",
            "primary",
            "proc",
            "procedure",
            "raw",
            "root",
            "rule",
            "schema",
            "select",
            "shift",
            "statistics",
            "status",
            "sysname",
            "symmetric",
            "table",
            "target",
            "trigger",
            "unique",
            "update",
            "user",
            "version",
            "view",
            "weight",
            "with",
            "where",
            "xml",
        };

        public static bool IsReservedWord(string word)
        {
            return reservedWords.Contains(word);
        }

        public static bool ContainsWhitespace(string fieldName)
        {
            return (fieldName.Contains(' ')
                || fieldName.Contains('\t')
                || fieldName.Contains('\n')
                || fieldName.Contains('\r'));
        }

        public static bool ContainsSquareBracket(string fieldName)
        {
            return (fieldName.Contains('[') || fieldName.Contains(']'));
        }
    }
}
