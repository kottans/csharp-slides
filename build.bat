cls

.paket\paket.bootstrapper.exe
if errorlevel 1 (
  exit /b %errorlevel%
)

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

set TARGET=
set SLIDES=

if not [%2] == [] (
	set TARGET="%1"
	set SLIDES="%2"
) else (
	if not [%1] == [] (
		set SLIDES="%1"
	) else (
		echo "please specify slides folder"
		exit /b
	)
)

"packages\FAKE\tools\Fake.exe" "build.fsx" "target=%TARGET%" "slides=%SLIDES%"