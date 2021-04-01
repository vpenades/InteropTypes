@echo off

set GETTIMEKEY=powershell get-date -format "{yyyyMMdd-HHmm}"
for /f %%i in ('%GETTIMEKEY%') do set TIMEKEY=%%i

set VERSIONSUFFIX=Preview-%TIMEKEY%

echo Building 1.0.0-%VERSIONSUFFIX%

dotnet build -c:Release --version-suffix %VERSIONSUFFIX% /p:Authors=vpenades ..\InteropTypes.sln

md bin

move InteropBitmaps.Core\bin\release\*.*nupkg bin
move InteropDrawing.Core\bin\release\*.*nupkg bin
move InteropTensors.Core\bin\release\*.*nupkg bin
move InteropVision.Core\bin\release\*.*nupkg bin

move InteropBitmaps.Drawing\bin\release\*.*nupkg bin

move InteropBitmaps.Backends.GDI\bin\release\*.*nupkg bin
move InteropBitmaps.Backends.ImageSharp\bin\release\*.*nupkg bin
move InteropBitmaps.Backends.SkiaSharp\bin\release\*.*nupkg bin
move InteropBitmaps.Backends.STB\bin\release\*.*nupkg bin
move InteropBitmaps.Backends.WPF\bin\release\*.*nupkg bin
move InteropBitmaps.Backends.Android\bin\release\*.*nupkg bin

move InteropDrawing.Backends.GLTF\bin\release\*.*nupkg bin
move InteropDrawing.Backends.MonoGame\bin\release\*.*nupkg bin
move InteropDrawing.Backends.SVG\bin\release\*.*nupkg bin
move InteropDrawing.Backends.WPF\bin\release\*.*nupkg bin
move InteropDrawing.Backends.Plotly\bin\release\*.*nupkg bin

pause
exit /b