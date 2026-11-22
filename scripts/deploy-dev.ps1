param(
    [Parameter(Mandatory = $true)]
    [string] $ProjectDirectory,

    [Parameter(Mandatory = $true)]
    [string] $BuiltDll,

    [Parameter(Mandatory = $true)]
    [string] $GodotExecutable,

    [Parameter(Mandatory = $true)]
    [string] $ModsDirectory
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$env:STS2_DEPLOY_IN_PROGRESS = "1"

$modName = "MoreUpgradedCardPreviews"
$manifest = Join-Path $ProjectDirectory "mod_manifest.json"
$exportPreset = Join-Path $ProjectDirectory "export_presets.cfg"
$buildDirectory = Join-Path $ProjectDirectory "build\dev"
$pack = Join-Path $buildDirectory "$modName.pck"
$gameDirectory = Split-Path -Parent $ModsDirectory
$destination = Join-Path $ModsDirectory $modName

if (-not (Test-Path -LiteralPath $BuiltDll -PathType Leaf)) { throw "Missing DLL: $BuiltDll"}
if (-not (Test-Path -LiteralPath $manifest -PathType Leaf)) { throw "Missing mod manifest: $manifest"}
if (-not (Test-Path -LiteralPath $exportPreset -PathType Leaf)) { throw "Missing Godot export preset: $exportPreset"}
if (-not (Test-Path -LiteralPath $GodotExecutable -PathType Leaf)) { throw "Godot executable does not exist: $GodotExecutable"}
if (-not (Test-Path -LiteralPath $gameDirectory -PathType Container)) { throw "Game directory does not exist: $gameDirectory. Fix ModDeployDirectory."}

New-Item -ItemType Directory -Path $buildDirectory -Force | Out-Null

if (Test-Path -LiteralPath $pack) { Remove-Item -LiteralPath $pack -Force}
Write-Host "[STS2] Exporting Godot PCK..."

$godotArguments = '--headless --path "{0}" --export-pack "Windows Desktop" "{1}"' -f $ProjectDirectory, $pack

$godotProcess = Start-Process `
    -FilePath $GodotExecutable `
    -ArgumentList $godotArguments `
    -WorkingDirectory $ProjectDirectory `
    -Wait `
    -PassThru `
    -NoNewWindow `
    -ErrorAction Stop

$godotExitCode = $godotProcess.ExitCode
if ($godotExitCode -ne 0) { throw "Godot PCK export failed (exit code: $godotExitCode)."}
if (-not (Test-Path -LiteralPath $pack -PathType Leaf)) { throw "Godot completed but did not create the PCK: $pack"}
if ((Get-Item -LiteralPath $pack).Length -eq 0) { throw "Godot created an empty PCK: $pack"}

New-Item -ItemType Directory -Path $destination -Force | Out-Null

Write-Host "[STS2] Deploying to $destination"
Copy-Item -LiteralPath $BuiltDll -Destination (Join-Path $destination "$modName.dll") -Force -ErrorAction Stop
Copy-Item -LiteralPath $manifest -Destination (Join-Path $destination "mod_manifest.json") -Force -ErrorAction Stop
Copy-Item -LiteralPath $pack -Destination (Join-Path $destination "$modName.pck") -Force -ErrorAction Stop

$pdb = [System.IO.Path]::ChangeExtension($BuiltDll, ".pdb")
if (Test-Path -LiteralPath $pdb -PathType Leaf) {
    Copy-Item -LiteralPath $pdb -Destination (Join-Path $destination "$modName.pdb") -Force -ErrorAction Stop
    Write-Host "[STS2] Mod deployed: DLL + PDB + PCK + manifest."
} else {
    Write-Warning "PDB not found: $pdb. C# breakpoints may not work."
    Write-Host "[STS2] Mod deployed: DLL + PCK + manifest."
}
