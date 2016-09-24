
# thanks => https://stackoverflow.com/questions/1183183/path-of-currently-executing-powershell-script
function Get-ScriptDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

# thanks => https://stackoverflow.com/questions/1153126/how-to-create-a-zip-archive-with-powershell/
function ZipFiles( $zipfilename, $sourcedir )
{
	Add-Type -Assembly System.IO.Compression.FileSystem
	$compressionLevel = ([System.IO.Compression.CompressionLevel]::Optimal)
	[System.IO.Compression.ZipFile]::CreateFromDirectory($sourcedir, $zipfilename, $compressionLevel, $false)
}


Push-Location
$scriptPath = Get-ScriptDirectory
CD $scriptPath

Remove-Item -Recurse -Force Binaries | Out-Null
New-Item -ItemType Directory -Force Binaries\All | Out-Null

Copy-Item ..\Code\TSqlFlex\bin\Release\RedGate.SIPFrameworkShared.dll .\Binaries\All
Copy-Item ..\Code\TSqlFlex\bin\Release\TSqlFlex.Core.dll .\Binaries\All
Copy-Item ..\Code\TSqlFlex\bin\Release\TSqlFlex.dll .\Binaries\All
Copy-Item ..\InstallationInstructions.md .\Binaries\All\InstallationInstructions.txt
Copy-Item ..\License.md .\Binaries\All\License.txt
Copy-Item ..\README.md .\Binaries\All\README.txt

$outputZip = Join-Path $scriptPath (-Join("Binaries\TSqlFlex.zip"));
$inputFolder = Join-Path $scriptPath "Binaries\All";

ZipFiles $outputZip $inputFolder;


$TSqlFlexDllVersionInfo = (Get-Item (Join-Path $scriptPath "Binaries\All\TSqlFlex.Core.dll")).VersionInfo;
$TSqlFlexDllVersion = -join($TSqlFlexDllVersionInfo.FileMajorPart, ".", $TSqlFlexDllVersionInfo.FileMinorPart,
	".",$TSqlFlexDllVersionInfo.FilePrivatePart);
$TSqlFlexDllVersion

#read the AssemblySemverPrereleaseTag from the assembly in a new PowerShell instance.
# then rename the zip file accordingly
Invoke-Expression (-Join('cmd /c start powershell -Command { 
	$assembly = [Reflection.Assembly]::ReflectionOnlyLoadFrom((Join-Path "', $scriptPath, '" "Binaries\All\TSqlFlex.Core.dll"));
	$atts = [reflection.customattributedata]::GetCustomAttributes($assembly);
	$ver = $atts | Where-Object {$_.AttributeType.ToString() -eq "AssemblySemverPrereleaseTag"} | % {$_.ConstructorArguments[0].ToString()}
	$ver = $ver.Replace("`"","")
	if ($ver.length -gt 0) {
		$ver = -Join("-", $ver);
	}
	$fromFileName = (Join-Path "', $scriptPath, '" "Binaries\TSqlFlex.zip");
	$toFileName =   (Join-Path "', $scriptPath, '" (-Join("Binaries\TSqlFlex-", "', $TSqlFlexDllVersion, '", $ver, ".zip")));
	Rename-Item $fromFileName $toFileName;
}'));


& "C:\Program Files (x86)\Notepad++\notepad++.exe" "..\Chocolatey\TSQLFlex.nuspec"
& "C:\Program Files (x86)\Notepad++\notepad++.exe" "..\Chocolatey\tools\chocolateyInstall.ps1"
& "C:\Program Files (x86)\Notepad++\notepad++.exe" "..\README.md"

start "https://github.com/nycdotnet/TSqlFlex/releases"

explorer ".\Binaries"

Pop-Location