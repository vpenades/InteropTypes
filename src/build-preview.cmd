@echo off

:: ============================================= Define Version suffix

set GETTIMEKEY=powershell get-date -format "{yyyyMMdd-HHmm}"
for /f %%i in ('%GETTIMEKEY%') do set TIMEKEY=%%i

set VERSIONSUFFIX=Preview-%TIMEKEY%

echo Building 1.0.0-%VERSIONSUFFIX%

:: ============================================= DOTNET builder

dotnet build -restore:true -c:Release --version-suffix %VERSIONSUFFIX% ..\InteropTypes.slnx
dotnet pack -c:Release --version-suffix %VERSIONSUFFIX% ..\InteropTypes.slnx

:: ============================================= MSBUILD builder

rem set MSBUILDPROPERTIES=Configuration=Release;VersionSuffix=%VERSIONSUFFIX%
rem msbuild -p:%MSBUILDPROPERTIES% ..\InteropTypes.slnx
rem msbuild -t:pack -p:%MSBUILDPROPERTIES% ..\InteropTypes.slnx

:: ============================================= Copy output

md bin

for /r %%i in (*.*nupkg) do move %%i bin

pause
exit /b