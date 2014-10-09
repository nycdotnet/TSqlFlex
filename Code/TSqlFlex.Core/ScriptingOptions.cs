using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSqlFlex.Core
{
    public class ScriptingOptions
    {

        public static ScriptingOptions Defaults()
        {
            ScriptingOptions so = new ScriptingOptions();
            so.ScriptColumnNames = false;
            so.ScriptIdentityFields = true;
            so.ScriptRowversionFields = ScriptRowversionStyle.ScriptForInsert;
            return so;
        }

        public bool ScriptColumnNames { get; set; }
        public static string ScriptColumnNamesDescription {
            get
            {
                return "If enabled, the column names will be included in the INSERT Statement\n" +
                    "On: INSERT INTO #Table (Field1) VALUES ('');\n" +
                    "Off: INSERT INTO #Table VALUES ('');";
            }
        }

        public bool ScriptIdentityFields { get; set; }
        public static string ScriptIdentityFieldsDescription
        {
            get
            {
                return "If enabled, IDENTITY fields will be included in the INSERT Statements.\n" +
                    "If disabled, IDENTITY fields will be excluded.\n" +
                    "This is sometimes useful to disable because you can only insert to an IDENTITY field\n" + 
                    "if IDENTITY_INSERT is on.";
            }
        }


        public enum ScriptRowversionStyle
        {
            ScriptForInsert = 1,
            ScriptForSchema = 2,
            SkipThem = 3
        }

        public ScriptRowversionStyle ScriptRowversionFields { get; set; }
        public static string ScriptRowversionFieldsDescription
        {
            get
            {
                return "In SQL Server, you can't insert explicit values to a ROWVERSION or TIMESTAMP field.\n" +
                    "'Script for INSERT' will script these fields as either BINARY(8) or VARBINARY(8) so the INSERTs will work.\n" +
                    "'Script for schema' will keeps the field as ROWVERSION or TIMESTAMP, but the scripted INSERTs will not run.\n" +
                    "'Skip them' will simply exclude TIMESTAMP or ROWVERSION fields from the output.";
            }
        }
        
    }
}
