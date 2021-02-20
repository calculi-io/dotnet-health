$healthprocess = Get-Process Health -ErrorAction SilentlyContinue
if ($healthprocess -and !$healthprocess.HasExited) {
	$healthprocess | Stop-Process -Force
}
