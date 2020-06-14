if not exist "..\..\al-code-outline\bin" md "..\..\al-code-outline\bin"
if not exist "..\..\al-code-outline\bin\netcore" md "..\..\al-code-outline\bin\netcore"
if not exist "..\..\al-code-outline\bin\netcore\darwin" md "..\..\al-code-outline\bin\netcore\darwin"
if not exist "..\..\al-code-outline\bin\netcore\win32" md "..\..\al-code-outline\bin\netcore\win32"
if not exist "..\..\al-code-outline\bin\netcore\linux" md "..\..\al-code-outline\bin\netcore\linux"
if not exist "..\..\al-code-outline\bin\netframework" md "..\..\al-code-outline\bin\netframework"

del "..\..\al-code-outline\bin\netcore\*.*" /Q
del "..\..\al-code-outline\bin\netcore\darwin\*.*" /Q
del "..\..\al-code-outline\bin\netcore\win32\*.*" /Q
del "..\..\al-code-outline\bin\netcore\linux\*.*" /Q


