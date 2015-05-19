echo off
for /f "delims=\" %%a in ("%cd%") do set currentDir=%%~nxa

cd ..
call GenerateSlides.cmd "%currentDir%"