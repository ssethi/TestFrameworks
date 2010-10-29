@ECHO OFF

SET Build=true
SET Copy=true
SET SolutionDir=.\PrismLibrary
SET ScriptsDir=%~dp0\Scripts
SET BinDir=.\Bin
SET Pause=true
SET Configuration=Debug
SET IncludePhone=false
SET ForceOverwrite=false

IF "%1"=="/?" GOTO HELP

IF DEFINED DevEnvDir GOTO OPTIONS

IF NOT DEFINED VS100COMNTOOLS GOTO VSNOTFOUND

ECHO.
ECHO ------------------------------------------
ECHO Setting the build environment
ECHO ------------------------------------------
ECHO.
CALL "%VS100COMNTOOLS%\vsvars32.bat" > NUL 
IF ERRORLEVEL 1 GOTO ERROR


:OPTIONS

REM  ----------------------------------------------------
REM  If the current parameter is /q, do not pause
REM  at the end of execution.
REM  ----------------------------------------------------

IF /i "%1"=="/q" (
	SET Pause=false
	SHIFT
)

REM  ----------------------------------------------------
REM  If the current parameter is /Release or /Debug, 
REM  update the build configuration.
REM  ----------------------------------------------------

IF /i "%1"=="/Release" (
	SET Configuration=Release
	SHIFT
	GOTO ACTION
)

IF /i "%1"=="/Debug" (
	SHIFT
	GOTO ACTION
)

:ACTION

REM  ----------------------------------------------------
REM  If the current parameter is /BuildAndCopy, /BuildOnly or
REM  /CopyOnly, update the action.
REM  ----------------------------------------------------

IF /i "%1"=="/BuildOnly" (
	SET Copy=false
	SHIFT
	GOTO PHONE
)

IF /i "%1"=="/CopyOnly" (
	SET Build=false
	SHIFT
	GOTO PHONE
)

IF /i "%1"=="/BuildAndCopy" (
	SHIFT
	GOTO PHONE
)

:PHONE

REM  ----------------------------------------------------
REM  If the current parameter is /IncludePhone,
REM  include the phone binaries.
REM  ----------------------------------------------------

IF /i "%1"=="/IncludePhone" (
	SET IncludePhone=true
	SHIFT
	GOTO OVERWRITE
)

:OVERWRITE

REM  ----------------------------------------------------
REM  If the current parameter is /Force,
REM  force overwrite of read-only files.
REM  ----------------------------------------------------

IF /i "%1"=="/Force" (
	SET ForceOverwrite=true
	SHIFT
	GOTO START
)


:START

IF EXIST %BinDir% (
	
	IF %ForceOverwrite%==true GOTO BUILD

	ECHO.
	ECHO ------------------------------------------
	ECHO Bin folder already exists.
	ECHO ------------------------------------------
	ECHO.

	CHOICE /m "Confirm overwrite?"
	IF ERRORLEVEL 2 GOTO EXIT
)


:BUILD

IF NOT %Build%==true (
	ECHO.
	ECHO ------------------------------------------
	ECHO Build skipped
	ECHO ------------------------------------------
	ECHO.
	GOTO COPY
)

ECHO.
ECHO ------------------------------------------
ECHO Build initiated
ECHO ------------------------------------------
ECHO.

CALL msbuild.exe "%SolutionDir%\PrismLibrary.sln" /t:Rebuild /p:Configuration=%Configuration%
IF ERRORLEVEL 1 GOTO ERROR

IF NOT %IncludePhone%==true GOTO BUILDCOMPLETE

CALL msbuild.exe "%SolutionDir%\Phone\PrismLibrary.Phone.sln" /t:Rebuild /p:Configuration=%Configuration%
IF ERRORLEVEL 1 GOTO ERROR

:BUILDCOMPLETE

ECHO.
ECHO ------------------------------------------
ECHO Build completed
ECHO ------------------------------------------
ECHO.


:COPY

IF NOT %Copy%==true (
	ECHO.
	ECHO ------------------------------------------
	ECHO Copy skipped
	ECHO ------------------------------------------
	ECHO.

	GOTO EXIT
)

ECHO.
ECHO ----------------------------------------
ECHO Copy initiated
ECHO ----------------------------------------
ECHO.

REM Expand folder variables to full paths
FOR %%G IN (%BinDir%) DO SET BinDir=%%~fG

CALL msbuild.exe "%ScriptsDir%\CopyPrismLibrary.proj" /verbosity:normal /p:Configuration=%Configuration% /p:BinFolder="%BinDir%" /p:CopyPhone=%IncludePhone% /p:ForceOverwrite=%ForceOverwrite%
IF ERRORLEVEL 1 GOTO ERROR

ECHO.
ECHO ------------------------------------------
ECHO Copy completed
ECHO ------------------------------------------
ECHO.


:EXIT

IF %Pause%==true PAUSE
ECHO ON
@EXIT


:VSNOTFOUND

ECHO.
ECHO ------------------------------------------
ECHO 'VS100COMNTOOLS' not set
ECHO Cannot set the build environment
ECHO ------------------------------------------
ECHO.

IF %Pause%==true PAUSE
ECHO ON
@EXIT -1


:ERROR

ECHO.
ECHO ------------------------------------------
ECHO An error occured while updating the library - %ERRORLEVEL%
ECHO ------------------------------------------
ECHO.

IF %Pause%==true PAUSE
ECHO ON
@EXIT ERRORLEVEL


:HELP

ECHO.
ECHO UpdatePrismBinaries.bat:
ECHO Builds the Prism Library binaries and copies them to the bin folder.
ECHO Must be executed from the source folder
ECHO.
ECHO Usage: 
ECHO UpdatePrismBinaries.bat /?
ECHO UpdatePrismBinaries.bat [/q] [/Release^|/Debug] [/BuildAndCopy^|/BuildOnly^|/CopyOnly] [/IncludePhone] [/Force]
ECHO.
ECHO Options:
ECHO - /? : Show this help message
ECHO - /q : Do not pause when done
ECHO - /Release ^| /Debug : Choose the build configuration. Default is "/Debug"
ECHO - /BuildAndCopy ^| /BuildOnly ^| /CopyOnly : Choose the script action. Default is "/BuildAndCopy"
ECHO - /IncludePhone : Include the Phone binaries in the build and copy actions. Default is not to include them.
ECHO - /Force : Overwrites binary files if the current binary files are read only. Default is to fail if files are read only.
ECHO.

ECHO ON
@EXIT
