@echo off

set GETTIMEKEY=powershell get-date -format "{yyyyMMdd-HHmm}"
for /f %%i in ('%GETTIMEKEY%') do set TIMEKEY=%%i

set VERSIONSUFFIX=Preview-%TIMEKEY%

echo Building 1.0.0-%VERSIONSUFFIX%

dotnet build -c:Release --version-suffix %VERSIONSUFFIX% /p:Authors=vpenades ..\InteropBitmaps.sln

pause