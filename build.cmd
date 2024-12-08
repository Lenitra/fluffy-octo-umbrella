@echo off

:: Définir les variables de base
set "UNITY_PATH=C:\Program Files\Unity\Hub\Editor\6000.0.28f1\Editor\Unity.exe"
set "PROJECT_PATH=HexWar"
set "OUTPUT_PATH=Builds"
set "BUILD_METHOD=BuildAutomation.BuildAllPlatforms"
set "LOG_FILE=build_log.txt"
set "FILE_NAME=HexWar_%DATE:~12,2%.%DATE:~4,2%.%DATE:~7,2%"




:: Vérifier si les dossiers de sortie existent
if not exist "%OUTPUT_PATH%" mkdir "%OUTPUT_PATH%"

:: Étape 1 : Exécuter Unity pour compiler le projet
echo Building the project...
"%UNITY_PATH%" -batchmode -nographics -quit -projectPath "%PROJECT_PATH%" -executeMethod %BUILD_METHOD% -logFile "%LOG_FILE%"
if %errorlevel% neq 0 (
    echo Build failed! Check %LOG_FILE% for details.
    pause
    exit /b %errorlevel%
)

echo Build completed successfully.

:: Étape 2 : Créer des fichiers ZIP pour chaque plateforme
echo Creating ZIP archives...

:: Linux
if exist "%OUTPUT_PATH%\Linux" (
    powershell -Command "Compress-Archive -Path '%OUTPUT_PATH%\Linux\*' -DestinationPath '%OUTPUT_PATH%\Linux_%FILE_NAME%.zip'"
    echo Linux build zipped successfully.
)

:: Windows
if exist "%OUTPUT_PATH%\Windows" (
    powershell -Command "Compress-Archive -Path '%OUTPUT_PATH%\Windows\*' -DestinationPath '%OUTPUT_PATH%\Windows_%FILE_NAME%.zip'"
    echo Windows build zipped successfully.
)

:: Android
:: if exist "%OUTPUT_PATH%\Android" (
::     powershell -Command "Compress-Archive -Path '%OUTPUT_PATH%\Android\*' -DestinationPath '%OUTPUT_PATH%\Android_Build.zip'"
::     echo Android build zipped successfully.
:: )

:: Étape 3 : Fin du script
echo All builds are zipped and ready.

exit /b 0
