# Brimborium.AspNetCore.ClientAppFiles

for AspNetCore deliver ClientApp files for a SinglePageApplication.

The supported workflow is the programmer start the npm/ng tool to generate the files, so the error messages are available in the editor.
The dotnet webapp is running and deliver the (new) files.

Their is no npm server running, no CORS, no proxy forward thing.

So you can restart the npm watch or the debugger running your webapp independedly.

The sample-simple is an angular project. The sample-simple/README.md descipe the steps to rebuild it.

The sample-i18n is an angular project with localization. The sample-i18n/README.md descipe the steps to rebuild it.
