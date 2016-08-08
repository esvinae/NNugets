
Download `http://www-01.ibm.com/support/docview.wss?uid=swg27024064` and install.

Go to `c:\Program Files\IBM\WebSphere MQ\bin\`. 

Copy into `lib` next : 

- Only managed `.dll`s with no `WCF`, `SOAP`, `WSDL` in names.
- No configurations, readme, texts, executables, scripts or other files. 
- Policy only for version 8.0+.

From `c:\Program Files\IBM\WebSphere MQ\bin\symbols\dll\` copy into `lib` only symbols for copied assemblies.

Run `build.cmd`