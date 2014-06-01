T-SQL Flex
==========

This is the initial alpha release of T-SQL Flex - a scripting productivity tool for SQL Server Management Studio that uses the Red Gate SIP Framework.

T-SQL Flex should be able to script out the returned schema of any T-SQL query simply and with high accuracy.  Much more is planned.

*This is alpha-quality software - DO NOT RUN IN PRODUCTION!!!*

![tsqlflex-0 0 1-alpha](https://cloud.githubusercontent.com/assets/3755379/3141685/44ecfa22-e992-11e3-8f0b-fe49879dae80.png)


**To try T-SQL Flex:**
  * Download the latest release from the [GitHub releases page](https://github.com/nycdotnet/TSqlFlex/releases).
  * Install the [Red Gate SIP framework](http://documentation.red-gate.com/display/MA/Redistributing+the+framework).
  * Create a new registry string value (REG_SZ) in the appropriate location to point to the extracted TSQLFlex.dll:

|Architecture|Registry Value Location|
|----|-----|
|32-bit|HKLM\SOFTWARE\Red Gate\SIPFramework\Plugins|
|64-bit|HKLM\SOFTWARE\Wow6432Node\Red Gate\SIPFramework\Plugins|
*Example: name = "TSQLFlex", value = "C:\ExtractedFiles\TSqlFlex.dll"*
  * Launch SQL Server Management Studio and click the T-SQL Flex button.

![tsqlflex-0 0 1-alpha](https://cloud.githubusercontent.com/assets/3755379/3141685/44ecfa22-e992-11e3-8f0b-fe49879dae80.png)

**To uninstall T-SQL Flex:**
  * Simply delete the registry key and the extracted files and restart SSMS.

Please create issues on GitHub or reach out to Steve on Twitter at [@nycdotnet](https://twitter.com/nycdotnet).
