Write-Output "starting web request for integration test"
Get-Date
Invoke-WebRequest -Uri http://127.0.0.1/ -Method 'GET' -TimeoutSec 60 -UseBasicParsing

if (-Not($?)) {
	Write-Output "Web Request Failed $(Get-Date)"
	throw "Web Request Failed"
} else {
	Write-Output "Web Request Success $(Get-Date)"
}
