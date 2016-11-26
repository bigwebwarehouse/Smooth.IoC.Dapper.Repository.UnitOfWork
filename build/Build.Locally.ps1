﻿$ErrorActionPreference = "Stop"

$packageVersion = "0.0.0-C1"
$configuration = "Release"
$msBuildExe = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
$msBuildTarget = "Build"

$CommandDirectory = split-path -parent $MyInvocation.InvocationName
set-location "$CommandDirectory"
cd ../
$rootFolder = (Get-Location).Path
Write-Host Path is $rootFolder
. $CommandDirectory\MyGet.Build.ps1
