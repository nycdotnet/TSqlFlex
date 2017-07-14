$packageName = "T-SQL Flex";
$url = "https://github.com/nycdotnet/TSqlFlex/releases/download/0.2.1-beta/TSqlFlex-0.2.1-beta.zip";
$installLocation = "$env:ProgramData\T-SQL Flex";

Install-ChocolateyZipPackage "$packageName" "$url" "$installLocation"

function Create-RegistryKeyIfNotExists($parentKey, $testKey) {
  if ((test-path "$parentKey\$testKey") -eq $false) {
    New-Item -Path "$parentKey" -Name "$testKey" | Out-Null
  }
  if ((test-path "$parentKey\$testKey") -eq $true) {
	return "$parentKey\$testKey";
  }
  throw "unable to create or confirm existence of '$parentKey\$testKey'";
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
	    Write-Debug "64-bit processor detected.";
        Return "HKLM:\Software\Wow6432Node";
    }
	Write-Debug "32-bit processor detected.";
	Return "HKLM:\Software";
}


$root = Get-RegistrySoftwareRootKey;
$createdKey = $root;
$addKeys = "Red Gate\SIPFramework\Plugins";
$addKeys.Split("\") | % { $createdKey = Create-RegistryKeyIfNotExists $createdKey $_ }
	
if ((Test-RegistryValue $createdKey "TSQLFlex") -eq $false) {
	New-ItemProperty -Name "TSQLFlex" -Path $createdKey -Value "$installLocation\TSqlFlex.dll" | Out-Null
} else {
	Set-ItemProperty -Name "TSQLFlex" -Path $createdKey -Value "$installLocation\TSqlFlex.dll" | Out-Null
}
