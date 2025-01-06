# Brimborium.AspNetCore.ClientAppFiles

for AspNetCore deliver ClientApp files for a SinglePageApplication.

The supported workflow is the programmer start the npm/ng tool to generate the files, so the error messages are available in the editor.
The dotnet webapp is running and deliver the (new) files.

Their is no npm server running, no CORS, no proxy forward thing.

Windows authentication works since you are not using a proxy.

So you can restart the npm watch or the debugger running your webapp independently.

a) sample-simple
 
    https://github.com/FlorianGrimm/Brimborium.AspNetCore.ClientAppFiles/tree/main/sample-simple

    The sample-simple is an angular project. The sample-simple/README.md describes the steps to rebuild it.

b) sample-i18n
 
    https://github.com/FlorianGrimm/Brimborium.AspNetCore.ClientAppFiles/tree/main/sample-i18n
 a 
    The sample-i18n is an angular project with localization. The sample-i18n/README.md describes the steps to rebuild it.

see also https://github.com/FlorianGrimm/Brimborium.AspNetCore.ClientAppFiles/blob/main/src/Brimborium.AspNetCore.ClientAppFiles/README.md

