echo POST-BUILD.BAT
set TARGET_DIR=%~1
set TARGET_DIR_LINUX=%TARGET_DIR:\=/%

rem echo TARGET_DIR = %TARGET_DIR% 
rem echo TARGET_DIR_LINUX = %TARGET_DIR_LINUX% 

if exist "%TARGET_DIR%wwwroot\" rd /s /q "%TARGET_DIR%wwwroot"
mkdir "%TARGET_DIR%wwwroot"

echo tar -xzf "./wwwroot.tgz" -C "%TARGET_DIR_LINUX%wwwroot" 
tar -xzf "./wwwroot.tgz" -C "%TARGET_DIR_LINUX%wwwroot" 
