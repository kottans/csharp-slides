rem set release template
copy templates\template_release.hmtl packages\FsReveal\fsreveal\template.html /Y
exit 
pause
call build GenerateSlides

rem return back original template
copy templates\template_original.html packages\FsReveal\fsreveal\template.html /Y
md release
copy temp\gh-pages\index.html release\index.html /Y
xcopy temp\gh-pages\images\*.* release\images\*.* /Y