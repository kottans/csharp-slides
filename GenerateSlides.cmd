set workingFolder=%1
if [%workingFolder%]==[] echo working folder is not specified
if [%workingFolder%]==[] exit
echo Generating slides for working folder %workingFolder%
cd %workingFolder%

rem set release template
copy ..\templates\template_release.html packages\FsReveal\fsreveal\template.html /Y

call build GenerateSlides

rem return back original template
copy ..\templates\template_original.html packages\FsReveal\fsreveal\template.html /Y
md Release
copy output\index.html release\index.html /Y
xcopy output\images\*.* release\images\*.* /Y