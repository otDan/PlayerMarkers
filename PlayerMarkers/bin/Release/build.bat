echo on
for /f "tokens=3,2,4 delims=/- " %%x in ("%date%") do set d=%%y%%x%%z
set data=%d%
Echo zipping...
for /f %%X in ("C:\Users\ostas\Desktop\PM") do "c:\Program Files\7-Zip\7z.exe" a -mx "E:\Projects\VisualStudio\Rounds\PlayerMarkers\PlayerMarkers\bin\Release\PM.zip" "%%X\*"
echo Done!