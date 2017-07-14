T-SQL Flex
==========

T-SQL Flex is a scripting productivity tool for SQL Server Management Studio that uses the Red Gate SIP Framework.  It is intended for use on non-production servers only.

T-SQL Flex can script out the returned schema and data of any T-SQL query simply and with high accuracy.  It can also export the data to the XML spreadsheet format which can be opened in Excel without having messed-up date formatting or losing leading zeros; multiple result sets are automatically placed on multiple worksheets.  It can also export the result of queries to properly-escaped CSV files.

T-SQL Flex is compatible with SQL Server Management Studio 2008 and higher, and requires .NET 3.5 on the server running SSMS.  T-SQL Flex can be used when connecting to SQL Server 2000 as long as SSMS 2008 or higher is used; Note that the generated T-SQL scripts will only be compatible with SQL Server 2008 and higher.

![tsqlflexscripttoinserts](https://cloud.githubusercontent.com/assets/3755379/4175774/d1b0772e-35e4-11e4-975c-12df856bd9e2.gif)

**To install T-SQL Flex:**

Follow the instructions [here](https://github.com/nycdotnet/TSqlFlex/blob/master/InstallationInstructions.md).

**For Support**

Please either create issues on GitHub, or reach out to Steve on Twitter at [@nycdotnet](https://twitter.com/nycdotnet).

**Patch notes:**
  * v0.2.1-beta (2016-07-13):
      * Fixed scripting of TIME to XML Spreadsheet (no longer crashes)
      * Improved scripting of TIME to SQL scripts - can include up to 7 digits of scale.
      * Improved scripting of DATETIME2 to SQL scripts - now includes scale number if relevant.
  * v0.2.0-beta (2016-09-23):
      * Implemented new script as C# feature.
	  * Many behind-the-scenes code improvements.
	  * Added escaping for several SQL keywords.
	  * Updated to work with NUnit 3 and RedGate SIP FW 1.0.1.246.
  * v0.1.0-beta (2015-05-07):
      * Reverted to use .NET 3.5 to restore compatibility with SSMS 2008.
	  * Added CSV export feature! T-SQL Flex can now export the results of any set of queries to properly-escaped CSV files that open correctly in Excel or any text editor.
	  * Added a handful of additional T-SQL keywords.
	  * Removed "This is alpha software" warning.  Beta 1 will likely have only minimal further changes prior to the v1.0 release of T-SQL Flex.
  * v0.0.11-alpha (2015-01-23):
      * Updated to use .NET 4.5 - this version is now required to run T-SQL Flex.
	  * Improved scripter to continuously increment the "#Result" table number to prevent conflicts in a session.
      * Fixed lockup of SSMS when switching between database servers. (#33)
      * Fixed missing column header for anonymous columns in Excel export (#29)
	  * Added WEIGHT, TARGET, and NONE as T-SQL keywords.
      * Implemented improved logging.
  * Older patch notes are [available here](ArchivedPatchNotes.md).

  
**Debugging an add-in:**
  * See the Red Gate document on this issue: http://documentation.red-gate.com/display/MA/Debugging+an+SSMS+addin

  
**Build checklist**
  * Compiles and all tests pass.
  * Checked-in to master branch on GitHub.
  * Updated version in both AssemblyInfo.cs files.
  * Build in release mode, switch the registry to use release mode, and test it out.
  * Run .\Deploy\prepDeploy.ps1
  * Zip up the DLLs with the license, README, and installation instructions (rename all to .txt) and post to GitHub.
  * Add a screenshot via GitHub and edit the README and release FAQ.
  * Chocolatey
    * Quit SSMS.
    * edit .nuspec with version and patch notes.
	* edit chocolateyInstall.ps1 with new GitHub release URL.
	* Run from admin powershell (under TSqlFlex\Chocolatey):
	  * `cpack`
	  * `cinst tsqlflex -source $pwd -pre`   ( use `-force` if already installed)
	  * `cuninst tsqlflex`
	* `cpush tsqlflex.VERSION_NUMBER.nupkg`  (you can type `cpush .\t<tab>`)
	
