$archiveName = ".\Health.zip"

# cannot user msbuild /restore because only have write permissions to current directory and below
msbuild.exe /restore
msbuild.exe /t:Rebuild /p:Configuration=Release

Remove-Item $archiveName -ErrorAction Ignore

Write-Output "Creating Archive: $archiveName"

$Command=" & 'C:\Program Files\7-Zip\7z.exe' a $archiveName .\Health\bin\Release\*"
Write-Output "Running: $Command"
$Command | Invoke-Expression

if(-Not(Test-Path $archiveName)){
    Write-Output "WARNING: $archiveName was not created"
}
