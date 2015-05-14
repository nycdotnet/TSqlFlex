using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class TSqlRules
    {
        //todo: fill in complete list from documentation
        public static bool IsReservedWord(string word)
        {
            switch (word.ToLower())
            {
                case "action":
                case "alter":
                case "as":
                case "asc":
                case "auto":
                case "by":
                case "clustered":
                case "changes":
                case "column":
                case "constraint":
                case "create":
                case "dec":
                case "default":
                case "delete":
                case "desc":
                case "drop":
                case "elements":
                case "exec":
                case "execute":
                case "from":
                case "function":
                case "group":
                case "insert":
                case "index":
                case "key":
                case "level":
                case "nonclustered":
                case "none":
                case "oct":
                case "order":
                case "primary":
                case "proc":
                case "procedure":
                case "raw":
                case "root":
                case "rule":
                case "schema":
                case "select":
                case "shift":
                case "statistics":
                case "status":
                case "sysname":
                case "table":
                case "target":
                case "trigger":
                case "update":
                case "user":
                case "version":
                case "view":
                case "weight":
                case "where":
                case "xml":
                    return true;
                default:
                    return false;
            }
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
