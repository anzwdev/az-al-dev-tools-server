# AZ AL Dev Tools Server

Language server for AL Code Outline (AZ AL Dev Tools) extension for Visual Studio Code. 

This project uses Json based language server protocol to exchange messages between visual studio code extension and language server process running in the background. Protocol implementation is based on Microsoft PowerShellEditorServices language server source code (https://github.com/PowerShell/PowerShellEditorServices).

At this moment these 2 message types are supported:
- parsing source code of AL file and returning document symbols
- loading symbols from APP package file


