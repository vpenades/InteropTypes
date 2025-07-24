
dotnet publish TestComponentsUnderAOT.App.csproj -c Release -o bin/Publish

cd bin/Publish

TestComponentsUnderAOT.App.exe

pause