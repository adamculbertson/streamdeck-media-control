param (
    [Parameter(Mandatory = $true)]
    [string]$UUID, # UUID of the plugin, in reverse-dns format: com.example.plugin
    [Parameter(Mandatory = $true)]
    [string]$ProjectPath, # Path to the C# Project files
    [Parameter(Mandatory = $true)]
    [string]$ProjectName, # Name of the project (such as "MyAwesomeApp") to determine the .exe name
    [Parameter(Mandatory = $true)]
    [string]$OutPath, # Location to store all of the output files, including the .streamDeckPlugin file
    [string]$Platform = "win-x64", # Platform type to compile for, typically just 'win-x64'
    [string]$Arch = "x64", # Architecture to use, typically just 'x64'
    [switch]$Force # Remove the build directory without prompting
)

# Get path to .csproj file
$csprojFile = Get-ChildItem -Path $ProjectPath -Filter "$ProjectName.csproj" | Select-Object -First 1

if (-not $csprojFile) {
    Write-Error "Could not find a .csproj file in $ProjectPath"
    exit 1
}

# Load and parse the .csproj XML
[xml]$csprojXml = Get-Content $csprojFile.FullName
$targetFramework = $csprojXml.Project.PropertyGroup.TargetFramework

if (-not $targetFramework) {
    Write-Error "Could not determine <TargetFramework> from $($csprojFile.FullName)"
    exit 1
}

Write-Host "Detected target framework: $targetFramework"
$NetVersion = $targetFramework

# Path to the .sdPlugin directory
$sdDir = (Join-Path $OutPath "$UUID.sdPlugin")

if (Test-Path "$sdDir") {
    if(!$Force) {
        $prompt = Read-Host "The build directory '$sdDir' already exists. Delete it to continue? (y/n)"

        if ($prompt -match '^[Yy]$') {
            Remove-Item -Path "$sdDir" -Recurse -Force
            Write-Host "Directory deleted."
        } else {
            Write-Host "Aborted by user."
            exit 1
        }
    }
    else {
        Remove-Item -Path "$sdDir" -Recurse -Force
        Write-Host "Directory deleted due to -Force."
    }
}

# Create the directories to the .sdPlugin directory
$null = New-Item -ItemType Directory -Path "$sdDir" -Force

Set-Location "$ProjectPath"

#  Check for project directories and copy those
if (Test-Path "images") {
    Copy-Item -Path "images" -Destination "$sdDir" -Recurse
}
#  Check for project directories and copy those
if (Test-Path "icons") {
    Copy-Item -Path "icons" -Destination "$sdDir" -Recurse
}
if (Test-Path "PropertyInspector") {
    Copy-Item -Path "PropertyInspector" -Destination "$sdDir" -Recurse    
}

# Check for manifest.json, which is a required file
if (!(Test-Path "manifest.json")) {
    Write-Error "No manifest.json was found. It is a required file."
    exit 1
}

Copy-Item -Path "manifest.json" -Destination "$sdDir"

# Compile to a self-contained .exe file
dotnet publish -c Release -r "$Platform" --self-contained true -p:PublishSingleFile=true
if (!$?)
{
    Write-Error "Failed to compile project"
    exit 1
}

# Get the path to the built executable
$ExePath = (Join-Path "bin\Release\$NetVersion\win-x64\publish" "$ProjectName.exe")
if (!(Test-Path "$ExePath")) {
    Write-Error "Fatal: Project compiled, but cannot locate $ExePath"
    exit 2
}

# Create the $sdDir/bin/$Arch path, typically $sdDir/bin/x64
$BinPath = (Join-Path (Join-Path "$sdDir" "bin") "$Arch")
$null = New-Item -ItemType Directory -Path "$BinPath" -Force

# Copy the executable to the bin path
Copy-Item -Path "$ExePath" "$BinPath"

$ArchivePath = (Join-Path "$sdDir" "$UUID.streamDeckPlugin")
Compress-Archive -Path "$sdDir" -DestinationPath "$ArchivePath"