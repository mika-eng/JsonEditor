# JsonEditor

A standalone windows application to view, edit, format, transform, and validate JSON files.
It is basically a wrapper implementing the web-based jsoneditor by Jos de Jong.

Please visit the original author of the outstanding svelte-jsoneditor:

https://github.com/josdejong/svelte-jsoneditor

### How it works:

This application is a .NET 8.0 WPF app,
implementing a webview that contains the vanilla-jsoneditor, which is a standalone javascript file.
The original jsoneditor is extended by some functionalities to open and save the JSON content as JSON files.

The release artifact of this repository is a portable 32- or 64-bit executable as single file.
No installation or dependencies required.

This project for sure is not professional code. More like a do it for the fun and learn some things,
like how to implement web-based components into .NET projects and interact with it.
