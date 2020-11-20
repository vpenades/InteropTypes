


### Dependencies

As a rule of thumb, it is desirable to avoid third party dependencies as much as possible, but some
are unavoidable.

In particular, there's the case of these:

- [System.Drawing.Primitives](https://github.com/dotnet/runtime/tree/master/src/libraries/System.Drawing.Primitives)
- [system.Drawing.Common](https://github.com/dotnet/runtime/tree/master/src/libraries/System.Drawing.Common)


Both include types like Point, PointF, Rectangle, etc, but the latter does not seem to include the
former, Which is mentioned here:

- https://github.com/dotnet/core/issues/3244


Okey, it seems the trick is the attribute:

        [System.Runtime.CompilerServices.TypeForwardedFrom("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]

which states:

_Use the TypeForwardedToAttribute attribute to move a type from one assembly to another without disrupting callers that compiled against the old assembly.__
