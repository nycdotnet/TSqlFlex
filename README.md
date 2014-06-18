T-SQL Flex
==========

T-SQL Flex is a scripting productivity tool for SQL Server Management Studio that uses the Red Gate SIP Framework.

*This is alpha-quality software - DO NOT RUN IN PRODUCTION!!!*

T-SQL Flex should be able to script out the returned schema and data of any T-SQL query simply and with high accuracy.  Much more is planned.



![tsqlflex-0 0 4-alpha](https://cloud.githubusercontent.com/assets/3755379/3309662/46c2a2e4-f6a2-11e3-90ba-3026e9560181.png)


**To install T-SQL Flex:**
  * Download the latest release from the [GitHub releases page](https://github.com/nycdotnet/TSqlFlex/releases).
  * Install the [Red Gate SIP framework](http://documentation.red-gate.com/display/MA/Redistributing+the+framework).
  * Create a new registry string value (REG_SZ) in the appropriate location to point to the extracted TSQLFlex.dll:

|Architecture|Registry Value Location|
|----|-----|
|32-bit Windows|HKLM\SOFTWARE\Red Gate\SIPFramework\Plugins|
|64-bit Windows|HKLM\SOFTWARE\Wow6432Node\Red Gate\SIPFramework\Plugins|
*Example: name = "TSQLFlex", value = "C:\ExtractedFiles\TSqlFlex.dll"*
  * Launch SQL Server Management Studio and click the T-SQL Flex button.
  * Fix the window positioning (will be better in next release).
  * Type one or more queries in the top panel and click the Run'n'Rollback button.  T-SQL Flex will run your query in the scope of an ADO.NET Transaction that is rolled-back when the batch completes.  The schema returned from those queries will be scripted in the lower panel.

**To uninstall T-SQL Flex:**
  * Simply delete the registry key and the extracted files and restart SSMS.

Please create issues on GitHub or reach out to Steve on Twitter at [@nycdotnet](https://twitter.com/nycdotnet).

**Patch notes:**
  * v0.0.4-alpha (2014-06-18):
      * Converted to background worker.  Added cancel button, timer, and progress bar.
	  * Additional scripted output "minification" improvements (dropping insignificant decimals for example).
	  * Other improvements to quality of scripted output such as bracketing of keywords.
  * v0.0.3-alpha (2014-06-13): Fixed data script escaping bug for single quotes.
  * v0.0.2-alpha (2014-06-11): Data scripting implemented.  Improved window.
  * v0.0.1-alpha (2014-06-01): Initial release.  Schema scripting working.
