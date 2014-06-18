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
                case "asc":
                case "by":
                case "create":
                case "delete":
                case "desc":
                case "drop":
                case "from":
                case "function":
                case "group":
                case "insert":
                case "level":
                case "oct":
                case "order":
                case "proc":
                case "procedure":
                case "schema":
                case "select":
                case "status":
                case "table":
                case "update":
                case "view":
                case "where":
                    return true;
                default:
                    return false;
            }
        }
    }
}
