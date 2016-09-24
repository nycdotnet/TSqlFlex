$packageName = "T-SQL Flex";
$installLocation = "$env:ProgramData\T-SQL Flex";

function Remove-ItemIfExists($itemName) {
	if ((Test-Path $itemName) -eq $true) {
	    Remove-Item $itemName;
	}
}

# thanks: http://stackoverflow.com/questions/5648931/test-if-registry-value-exists
function Test-RegistryValue($regkey, $name) {
    $exists = Get-ItemProperty -Path "$regkey" -Name "$name" -ErrorAction SilentlyContinue
    If (($exists -ne $null) -and ($exists.Length -ne 0)) {
        Return $true
    }
    Return $false
}

function Get-RegistrySoftwareRootKey() {
    if (Get-ProcessorBits -eq 64)  {
        Return "HKLM:\Software\Wow6432Node";
    }
	Return "HKLM:\Software";
}

Remove-ItemIfExists "$installLocation\RedGate.SIPFrameworkShared.dll";
Remove-ItemIfExists "$installLocation\TSqlFlex.Core.dll";
Remove-ItemIfExists "$installLocation\TSqlFlex.dll";
Remove-ItemIfExists "$installLocation\InstallationInstructions.txt";
Remove-ItemIfExists "$installLocation\License.txt";
Remove-ItemIfExists "$installLocation\README.txt";
	
$root = Get-RegistrySoftwareRootKey;
$pluginsPath = "$root\Red Gate\SIPFramework\Plugins";
if ((Test-RegistryValue $pluginsPath "TSQLFlex") -eq $true) {
	Remove-ItemProperty -Name "TSQLFlex" -Path $pluginsPath | Out-Null
}

try {
  Remove-ItemIfExists "$installLocation";
} catch {
  # Don't sweat it.
}
