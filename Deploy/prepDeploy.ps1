
# thanks => http://stackoverflow.com/questions/1183183/path-of-currently-executing-powershell-script
function Get-ScriptDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

Push-Location
$scriptPath = Get-ScriptDirectory
CD $scriptPath

Remove-Item -Recurse -Force Binaries | Out-Null
New-Item -ItemType Directory -Force Binaries | Out-Null

Copy-Item ..\Code\TSqlFlex\bin\Release\RedGate.SIPFrameworkShared.dll .\Binaries
Copy-Item ..\Code\TSqlFlex\bin\Release\TSqlFlex.Core.dll .\Binaries
Copy-Item ..\Code\TSqlFlex\bin\Release\TSqlFlex.dll .\Binaries
Copy-Item ..\InstallationInstructions.md .\Binaries\InstallationInstructions.txt
Copy-Item ..\License.md .\Binaries\License.txt
Copy-Item ..\README.md .\Binaries\README.txt

& "C:\Program Files (x86)\Notepad++\notepad++.exe" "..\Chocolatey\TSQLFlex.nuspec"
& "C:\Program Files (x86)\Notepad++\notepad++.exe" "..\Chocolatey\tools\chocolateyInstall.ps1"
& "C:\Program Files (x86)\Notepad++\notepad++.exe" "..\README.md"

start "https://github.com/nycdotnet/TSqlFlex/releases"

explorer ".\Binaries"

Pop-Location