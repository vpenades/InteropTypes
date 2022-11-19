@echo off

rem https://developers.google.com/protocol-buffers/docs/reference/csharp-generated

..\..\tools\protoc\bin\protoc DrawingPrimitives.proto --csharp_out=.\ --csharp_opt=file_extension=.g.cs

if NOT ["%errorlevel%"]==["0"] pause

