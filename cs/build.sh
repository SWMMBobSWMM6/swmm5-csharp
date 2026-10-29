#!/bin/bash
# Build runswmm.dll with the .NET 8 SDK's compiler directly (no NuGet restore needed).
set -e
cd "$(dirname "$0")"
FW=$(ls -d /usr/lib/dotnet/shared/Microsoft.NETCore.App/8.* | head -1)
CSC=$(ls /usr/lib/dotnet/sdk/*/Roslyn/bincore/csc.dll | head -1)
mkdir -p bin
REFS=$(for f in $FW/*.dll; do case $(basename $f) in Microsoft.VisualBasic*|mscorlib.dll|netstandard.dll|System.Private.*) ;; *) echo -n "-r:$f ";; esac; done)
dotnet $CSC -nologo -optimize+ -debug:portable -unsafe- -nowarn:CS0162,CS0164,CS0168,CS0219,CS0414,CS0649,CS1717,CS0665,CS8321,CS0108 \
  -langversion:latest -out:bin/runswmm.dll $REFS -r:$FW/netstandard.dll -r:$FW/System.Private.CoreLib.dll gen/*.cs runtime/*.cs "$@"
printf '{"runtimeOptions":{"tfm":"net8.0","framework":{"name":"Microsoft.NETCore.App","version":"%s"},"configProperties":{"System.Globalization.Invariant":true,"System.Runtime.TieredPGO":true}}}' $(basename $FW) > bin/runswmm.runtimeconfig.json
