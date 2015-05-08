T-SQL Flex Installation Instructions
====================================

## Best way
T-SQL Flex is now [available](https://chocolatey.org/packages/tsqlflex) via Chocolatey!  This is the best and easiest way to install T-SQL Flex and the Red Gate SIP Framework (which is configured as a dependent package and automatically downloaded and installed too).

First, get Chocolatey from https://chocolatey.org/ .  Once Chocolatey is installed, close all copies of SSMS and open an *administrative* PowerShell prompt (right-click, Run as Administrator).  From that *administrative* PowerShell prompt, run one of the following commands:

```PowerShell
#install the latest version of T-SQL Flex
choco install tsqlflex -pre 

#install a specific version of T-SQL Flex  (in this case, v 0.0.11)
choco install tsqlflex -pre -version 0.0.11

#upgrade T-SQL Flex (only if you installed it via Chocolatey)
choco upgrade tsqlflex -pre

#uninstall T-SQL Flex
choco uninstall tsqlflex
```

Here's the code for the T-SQL Flex Chocolatey packages in case you want to know what you're running: https://github.com/nycdotnet/TSqlFlex/tree/master/Chocolatey/tools


## Manual Install Instructions
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


