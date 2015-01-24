$name = 'T-SQL Flex'
$url  = 'https://github.com/nycdotnet/TSqlFlex/releases/download/0.0.10-alpha/TSqlFlex-v0.0.10-alpha.zip'
$flexInstallPath = "$env:ProgramData\T-SQL Flex"


try {
  Install-ChocolateyZipPackage $name $url $flexInstallPath
  Write-ChocolateySuccess $name
} catch {
  Write-ChocolateyFailure $name
}

