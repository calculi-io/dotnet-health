Write-Output "Looking for process..."

$procName = 'notepad'
$sec = 10
$endTime = [System.DateTime]::Now.AddSeconds($sec)
while ($endTime -gt [System.DateTime]::Now) {
	$proc = @(Get-Process | Select-Object -Property ProcessName, Id, WS | Where-Object {$_.ProcessName -eq $procName})
	if ($proc.length -gt 0) {
		Write-Output "SUCCESS: $procName started at $(Get-Date)"
		exit
	}
	Start-Sleep -Milliseconds 2000
}
Write-Output "ERROR: $procName did not start after $sec seconds."