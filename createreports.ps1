<# $username = 'test'
$password = 'Int3g2019!'

$commands = @'
	$w = whoami
	Write-Output "using $w"
'@

$securePassword = ConvertTo-SecureString $password -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential $username, $securePassword
Start-Process -NoNewWindow -FilePath Powershell -Credential $credential -ArgumentList "-Command", $commands
 #>


$VSINSTALLDIR_2017 = "\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise"

$proc = Start-Process "$VSINSTALLDIR_2017\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" -ArgumentList "/EnableCodeCoverage Health\bin\Debug\Health.exe /Logger:trx /Diag:./diag.log" -Wait

# $proc = Start-Process "$VSINSTALLDIR_2017\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" -ArgumentList "/EnableCodeCoverage Health\bin\Debug\Health.exe /Logger:trx /Diag:./diag.log" -PassThru -NoNewWindow
# $proc.WaitForExit()
# "Exit code: {0}", $proc.ExitCode
