
Download `http://www.oracle.com/technetwork/middleware/coherence/downloads/index.html` and install

Copy `c:\Program Files (x86)\Oracle\Coherence for .NET\bin\` to `lib`

Copy `c:\Program Files (x86)\Oracle\Coherence for .NET\config\` to `content`

Run `build.cmd`


How can I find source code ?  I want to debug with logical stack trace. 
---

There is no source. The dlls are of Oracle. I just put these onto Nuget. Try some decompiler with debug support. Or decompile into C# and compile again. Try to install dlls from Oracle installer directly. Validate your xml client config. I believe these dlls are only for Windows, not yet ported to Mono or .NET Core. These dlls are with pdb. So some debug should work. Validate client and server version are compatible.