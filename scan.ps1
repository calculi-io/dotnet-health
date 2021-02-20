Clear-Host

$VSINSTALLDIR_2017 = "\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise"
$testResultsDir = ".\TestResults"

Remove-Item $testResultsDir -Force -Recurse -ErrorAction SilentlyContinue

# Visual Studio Tools with Xunit
SonarScanner.MSBuild.exe begin /key:"health" /n:"Adam" /v:"1.0" /d:sonar.cs.vstest.reportsPaths="$testResultsDir\*.trx" /d:sonar.cs.vscoveragexml.reportsPaths="$testResultsDir\coverage*.xml"
MSBuild.exe /restore /t:Rebuild /p:Configuration=Debug
& "$VSINSTALLDIR_2017\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" /EnableCodeCoverage "Health\bin\Debug\Health.exe" /Logger:trx
$i = 0
$reportFiles = Get-ChildItem -Path TestResults *.coverage -Recurse | %{$_.FullName}
Foreach ($f in $reportFiles) {
	if ($f.Contains("\In\")) {
		$reportFiles = $reportFiles -ne $f
	}
}
Foreach ($f in $reportFiles) {
	Write-Output "Found: $f"
	$destName = "VisualStudio"
	$ext = "coveragexml"
	$cname = "coverage"
	if ($i -gt 0) {
		$destName += "_$i"
		$cname += "_$i"
	}
	Copy-Item -Path $f -destination "$testResultsDir\$destName.$ext"
	Write-Output "copying to $testResultsDir\$destName.$ext"
	& "$VSINSTALLDIR_2017\Team Tools\Dynamic Code Coverage Tools\CodeCoverage.exe" analyze /output:"$testResultsDir\$cname.xml" "$testResultsDir\$destName.$ext"
	$i = $i + 1
}

SonarScanner.MSBuild.exe end
