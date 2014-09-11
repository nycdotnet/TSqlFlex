T-SQL Flex
==========

T-SQL Flex is a scripting productivity tool for SQL Server Management Studio that uses the Red Gate SIP Framework.

*This is alpha-quality software - DO NOT RUN IN PRODUCTION!!!*

T-SQL Flex can script out the returned schema and data of any T-SQL query simply and with high accuracy.  It can also export the data to the XML spreadsheet format which can be opened in Excel without having messed-up date formatting or losing leading zeros; multiple result sets are automatically placed on multiple worksheets.  Much more is planned.

![tsqlflexscripttoinserts](https://cloud.githubusercontent.com/assets/3755379/4175774/d1b0772e-35e4-11e4-975c-12df856bd9e2.gif)

**To install T-SQL Flex:**

Follow the instructions [here](https://github.com/nycdotnet/TSqlFlex/blob/master/InstallationInstructions.md).

**For Support**

Please either create issues on GitHub, or reach out to Steve on Twitter at [@nycdotnet](https://twitter.com/nycdotnet).

**Patch notes:**
  * v0.0.8-alpha (2014-09-06):
      * Added about box with version info and links for feedback, issues, and updates.
	  * Added button to open the latest scripted XML spreadsheet in Excel.
	  * Fix for some rare cross-thread UI update issues.
  * v0.0.7-alpha (2014-09-02):
      * Significant bug fixes for internationalization issues surrounding time and number formatting including tests.  Cultures where . is used as the time separator and , as a decimal point should work OK now for both Excel and SQL INSERT scripts.  Special thanks to Gianluca Sartori (@SpaghettiDBA) for assistance with troubleshooting these issues.
	  * Bug fixes for incorrect columns and commas appearing in INSERT scripts due to hidden fields.
	  * Added more line-feeds to the scripted XML Spreadsheet 2003 output.
	  * Finished refactoring to "format" functions (this is an internal change only).
  * v0.0.6-alpha (2014-08-30):
      * Significant improvements to exception handling during all phases of querying and scripting
	  * Significant changes to disk-based buffering.  Now uses .NET IsolatedStorage.
	  * Significant refactoring - moved query processing logic from UI to Core DLL.
	  * Fixed bug where synthetic columns (select 'a' as [z]) were incorrectly hidden.
	  * Fixed bug with scripting binary and other byte[] data fields to Excel.
	  * Updated SIP Framework to 1.0.1.243 (from July 2014).
  * v0.0.5-alpha (2014-08-22):
      * Export to "XML Spreadsheet 2003" functionality added - this is very early alpha for this feature.
      * Started significant refactoring effort for data scripting in T-SQL field vs general presentation.
      * Started work to use a file stream rather than a string builder for scripting the data.  Currently only used with the Excel XML export.
  * v0.0.4-alpha (2014-06-18):
      * Converted to background worker.  Added cancel button, timer, and progress bar.
	  * Additional scripted output "minification" improvements (dropping insignificant decimals for example).
	  * Other improvements to quality of scripted output such as bracketing of keywords.
  * v0.0.3-alpha (2014-06-13): Fixed data script escaping bug for single quotes.
  * v0.0.2-alpha (2014-06-11): Data scripting implemented.  Improved window.
  * v0.0.1-alpha (2014-06-01): Initial release.  Schema scripting working.

**Debugging an add-in:**
  * See the Red Gate document on this issue: http://documentation.red-gate.com/display/MA/Debugging+an+SSMS+addin


**Build checklist**
  * Compiles and all tests pass.
  * Checked-in to master branch on GitHub.
  * Updated version in AssemblyInfo.cs and Extension.cs.
  * Build in release mode and test it out.
  * Zip up the DLLs with the license, README, and installation instructions (rename all to .txt) and post to GitHub.
  * Add a screenshot via GitHub and edit the README and release FAQ.
