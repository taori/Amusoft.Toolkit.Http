$configuration = "Release"
$verbosity = "n"
$versionSuffix = "beta"

dotnet restore "$PSScriptRoot/../src/All.sln" --verbosity $verbosity
Write-Host "Restore complete" -ForegroundColor Green

dotnet build "$PSScriptRoot/../src/All.sln" --verbosity $verbosity -c $configuration --no-restore
Write-Host "Build complete" -ForegroundColor Green

dotnet test "$PSScriptRoot/../src/All.sln" --verbosity $verbosity -c $configuration --no-build 
Write-Host "Test complete" -ForegroundColor Green

dotnet pack "$PSScriptRoot/../src/Amusoft.Toolkit.Http/Amusoft.Toolkit.Http.csproj" --verbosity $verbosity -c $configuration -o ../artifacts/nupkg --no-build /p:VersionSuffix=$versionSuffix
