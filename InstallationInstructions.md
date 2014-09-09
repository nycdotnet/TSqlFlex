T-SQL Flex Installation Instructions
====================================

  * Download the latest release from the [GitHub releases page](https://github.com/nycdotnet/TSqlFlex/releases).
  * Extract the contents of the ZIP file somewhere, for example `C:\ProgramData\T-SQL Flex\`
  * Right-click on each of the extracted DLLs, choose properties, and click `Unblock` on the bottom of the general tab.  This will allow SSMS to load the DLL.
  * Install the [Red Gate SIP framework](http://documentation.red-gate.com/display/MA/Redistributing+the+framework).
  * Figure out if you have 32-bit or 64-bit Windows; it affects the next step.
  * Open Regedit and create or navigate to the below registry key:
    * If 32-bit Windows: `HKLM\SOFTWARE\Red Gate\SIPFramework\Plugins`
    * If 64-bit Windows: `HKLM\SOFTWARE\Wow6432Node\Red Gate\SIPFramework\Plugins`
  * Create a new registry string value (REG_SZ) there to point to the extracted TSqlFlex.dll.
    * For example:
      * value name: `TSqlFlex`
      * value data: `C:\ProgramData\T-SQL Flex\TSqlFlex.dll`

**Using T-SQL Flex:**
  * Launch SQL Server Management Studio and click the T-SQL Flex button.
  * Type one or more queries in the top panel and click the Run'n'Rollback button.
    * T-SQL Flex will run your query in the scope of an ADO.NET Transaction that is rolled-back when the batch completes.  The schema returned from those queries will be scripted in the lower panel.
	* You can also have your query scripted into an Excel sheet by selecting that in the dropdown.

**To uninstall T-SQL Flex:**
  * Simply delete the registry key and the extracted files and restart SSMS.

Please create issues on GitHub or reach out to Steve on Twitter at [@nycdotnet](https://twitter.com/nycdotnet).


