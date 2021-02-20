# $ErrorActionPreference = "Stop"
Set-PSDebug -Trace 2

Write-Output "clearing monit file"
Remove-Item "$env:APP_JOB_DIR/monit"
$null > "$env:APP_JOB_DIR/monit"

Write-Output "------------------- Running Deploy Script -------------------"
Add-Type -assembly "system.io.compression.filesystem"

# Write-Output "-- envirobnment variables --"
# Get-Childitem env:*
# $archiveLocation = "$env:PKG_BASE_DIR\$env:APPLICATION_NAME\Health.zip"
# Write-Output "application-name directory"
# Get-ChildItem "$env:PKG_BASE_DIR\$env:APPLICATION_NAME"
# Write-Output "package base directory" # info
# Tree $env:PKG_BASE_DIR # info

# stop process if it's already running
# $healthprocess = Get-Process Health -ErrorAction SilentlyContinue
# if ($healthprocess -and !$healthprocess.HasExited) {
# 	Write-Output "WARN: ending Health process"
# 	$healthprocess | Stop-Process -Force
# }
# Remove-Variable healthprocess

# exit if process is already running
$healthprocess = Get-Process Health -ErrorAction SilentlyContinue
if ($healthprocess -and !$healthprocess.HasExited) {
	Write-Output "WARN: process already running. Exiting."
	return
}
Remove-Variable healthprocess

$archiveName = "$env:PKG_BASE_DIR/$env:APPLICATION_NAME/Health.zip"
Write-Output "Extracting Archive: $archiveName"
[io.compression.zipfile]::ExtractToDirectory($archiveName, "$env:PKG_BASE_DIR/$env:APPLICATION_NAME")
Write-Output "Done Extracting"

if ($env:HEALTH_NOSTART -eq 'true') {
	Write-Output 'Do not start detected - exiting script'
	return
}

Write-Output "Starting health monitor"
$Command = "$env:PKG_BASE_DIR/$env:APPLICATION_NAME/Health.exe"
if ($env:HEALTH_URL) {
	$Command += " -Url $env:HEALTH_URL"
}
if ($env:HEALTH_INTERVAL) {
	$Command += " -Interval $env:HEALTH_INTERVAL"
}
if ($env:HEALTH_GRACEPERIOD) {
	$Command += " -GracePeriod $env:HEALTH_GRACEPERIOD"
}
if ($env:HEALTH_TIMEOUT) {
	$Command += " -GracePeriod $env:HEALTH_TIMEOUT"
}
Write-Output "Running Command: $Command"
Invoke-Expression $Command

Get-Date

Write-Output "Looking for process..."

$procName = 'Health'
$sec = 10
$endTime = [System.DateTime]::Now.AddSeconds($sec)
while ($endTime -gt [System.DateTime]::Now) {
	$proc = @(Get-Process | Select-Object -Property ProcessName, Id, WS | Where-Object {$_.ProcessName -eq $procName})
	if ($proc.length -gt 0) {
		Write-Output "SUCCESS: $procName started at $(Get-Date)"
		exit
	}
	Start-Sleep -Milliseconds 200
}
Write-Output "ERROR: $procName did not start after $sec seconds."

$tl = @(tasklist)
Write-Output "Task List"
foreach ($p in $tl) { Write-Output "$p" }
