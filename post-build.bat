echo POST BUILD BATCH FILE
set TARGET_DIR=%~1
rem echo TargetDir = %TARGET_DIR%

if exist "%TARGET_DIR%wwwroot\" rd /s /q "%TARGET_DIR%wwwroot"

mkdir "%TARGET_DIR%wwwroot"
echo tar -xzf "./wwwroot.tgz" -C "%TARGET_DIR%wwwroot" 
tar -xzf "./wwwroot.tgz" -C "%TARGET_DIR%wwwroot" 
