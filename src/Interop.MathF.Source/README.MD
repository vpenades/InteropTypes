### System.MathF source code for older frameworks.

For some reason, MathF is not available in some frameworks, which makes multitargeting difficult.

The affected frameworks are:
- NetStandard 2.0 and older.
- NetFramework 4.7.2 and older.


The possibility to add support to older frameworks is discussed [here](https://github.com/dotnet/runtime/issues/20113).


Source code as been downloaded from Microsoft DotNet Runtime repository:

- [MathF.cs](https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/MathF.cs)
- [MathF.CoreCLR.cs](https://github.com/dotnet/runtime/blob/master/src/coreclr/System.Private.CoreLib/src/System/MathF.CoreCLR.cs)
- [Single.cs](https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/Single.cs)
- [Half.cs](https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/Half.cs)
- [Number.NumberToFloatingPointBits.cs](https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/Number.NumberToFloatingPointBits.cs)
- [BitConverter.cs](https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/BitConverter.cs)
 
This implementation does not replace Microsoft's own MathF.
