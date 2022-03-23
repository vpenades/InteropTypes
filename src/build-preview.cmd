@echo off

:: ============================================= Define Version suffix

set GETTIMEKEY=powershell get-date -format "{yyyyMMdd-HHmm}"
for /f %%i in ('%GETTIMEKEY%') do set TIMEKEY=%%i

set VERSIONSUFFIX=Preview-%TIMEKEY%

echo Building 1.0.0-%VERSIONSUFFIX%

:: ============================================= DOTNET builder

dotnet build -restore:true -c:Release --version-suffix %VERSIONSUFFIX% /p:NoWarn=1591 /p:Authors=vpenades ..\InteropTypes.sln
dotnet pack -c:Release --version-suffix %VERSIONSUFFIX% /p:NoWarn=1591 /p:Authors=vpenades ..\InteropTypes.sln

:: ============================================= MSBUILD builder

rem set MSBUILDPROPERTIES=Configuration=Release;VersionSuffix=%VERSIONSUFFIX%;NoWarn=1591
rem msbuild -p:%MSBUILDPROPERTIES% ..\InteropTypes.sln
rem msbuild -t:pack -p:%MSBUILDPROPERTIES% ..\InteropTypes.sln

:: ============================================= Copy output

md bin

move InteropTypes.Graphics.Bitmaps.PixelFormats\bin\release\*.*nupkg bin
move InteropTypes.Graphics.Bitmaps.Core\bin\release\*.*nupkg bin

move InteropTypes.Graphics.Drawing.Core\bin\release\*.*nupkg bin
move InteropTypes.Graphics.Drawing.Toolkit\bin\release\*.*nupkg bin

move InteropTypes.Tensors\bin\release\*.*nupkg bin

move InteropTypes.Vision.Core\bin\release\*.*nupkg bin

move InteropTypes.Codecs.STB\bin\release\*.*nupkg bin
move InteropTypes.Codecs.MJPEG\bin\release\*.*nupkg bin

move InteropTypes.Graphics.Backends.GDI\bin\release\*.*nupkg bin
move InteropTypes.Graphics.Backends.WPF\bin\release\*.*nupkg bin
move InteropTypes.Graphics.Backends.SVG\bin\release\*.*nupkg bin
move InteropTypes.Graphics.Backends.Plotly\bin\release\*.*nupkg bin
move InteropTypes.Graphics.Backends.GLTF\bin\release\*.*nupkg bin
move InteropTypes.Graphics.Backends.MonoGame\bin\release\*.*nupkg bin
move InteropTypes.Graphics.Backends.ImageSharp\bin\release\*.*nupkg bin
move InteropTypes.Graphics.Backends.SkiaSharp\bin\release\*.*nupkg bin
move InteropTypes.Graphics.Backends.Android\bin\release\*.*nupkg bin

move InteropTypes.Vision.Backends.OnnxRuntime\bin\release\*.*nupkg bin

pause
exit /b