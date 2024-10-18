@echo off
setlocal enabledelayedexpansion

for %%f in (*.png) do (
    set "filename=%%~nf"
    ffmpeg -i "%%f" "!filename!.bmp"
)

endlocal
