param([switch]$Force)

$toolsPath = Join-Path . -ChildPath "tools"

If ((Test-Path $toolsPath) -eq $true -and $Force -eq $true) {
    Remove-Item $toolsPath -Force -Recurse
}
If ((Test-Path $toolsPath) -eq $false) {
    New-Item $toolsPath -Type directory
}

# NUGET
$nugetPath = Join-Path $toolsPath -ChildPath "nuget"
$nugetPathExists = Test-Path $nugetPath

If ($nugetPathExists -eq $false) {
    New-Item $nugetPath -Type directory
    $nuget = Join-Path $nugetPath -ChildPath "nuget.exe"
    Invoke-WebRequest "https://www.nuget.org/nuget.exe" -OutFile $nuget
    &$nuget update -self
}

$nuget = Join-Path $nugetPath -ChildPath "nuget.exe"

# PSAKE
$psakePath = Join-Path $toolsPath -ChildPath "psake"
$psakePathExists = Test-Path $psakePath
If ($psakePathExists -eq $false) {
    &$nuget install psake -Version 4.6.0 -OutputDirectory $toolsPath -ExcludeVersion
}

#NUNIT
$nunitPath = Join-Path $toolsPath -ChildPath "nunit"
$nunitPathExists = Test-Path $nunitPath
If ($nunitPathExists -eq $false) {
    &$nuget install nunit.console -Version 3.5.0 -OutputDirectory $nunitPath -ExcludeVersion
}