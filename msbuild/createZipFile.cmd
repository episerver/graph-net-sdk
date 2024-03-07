@echo off
powershell .\msbuild\createZipFile.ps1 -findVersion %1%
EXIT /B %errorlevel%