@ECHO OFF

SET ScriptsDir=%~dp0\Scripts
SET BinDir=.\Bin
SET Pause=true
SET Register=true

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
REM  If the current parameter is /u, removes the 
REM  registration for the assemblies.
REM  ----------------------------------------------------

IF /i "%1"=="/u" (
	SET Register=false
	SHIFT
)


ECHO.
ECHO ------------------------------------------
ECHO Registration initiated
ECHO ------------------------------------------
ECHO.

REM Expand folder variables to full paths
FOR %%G IN (%BinDir%) DO SET BinDir=%%~fG

CALL msbuild.exe "%ScriptsDir%\RegisterPrismLibrary.proj" /verbosity:normal /p:BinFolder="%BinDir%" /p:Register=%Register%
IF ERRORLEVEL 1 GOTO ERROR

IF /i "%Register%" == "true" (
	ECHO.
	ECHO ------------------------------------------
	ECHO Registration completed
	ECHO ------------------------------------------
	ECHO.
) ELSE (
	ECHO.
	ECHO ------------------------------------------
	ECHO Registration removed
	ECHO ------------------------------------------
	ECHO.
)


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
ECHO RegisterPrismBinaries.bat:
ECHO Registers the prism binaries folders as assembly folders.
ECHO Must be executed from the source folder
ECHO.
ECHO Usage: 
ECHO RegisterPrismBinaries.bat /?
ECHO RegisterPrismBinaries.bat [/q] [/u]
ECHO.
ECHO Options:
ECHO - /? : Show this help message
ECHO - /q : Do not pause when done
ECHO - /u : Remove the prism binaries registration
ECHO.

ECHO ON
@EXIT