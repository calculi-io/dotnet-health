@PowerShell -NonInteractive -NoProfile -Command "& {.\isolation_tests.ps1; exit $LastExitCode }"
@echo From Cmd.exe: integration_tests.ps1 exited with exit code %errorlevel%
