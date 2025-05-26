echo POST-BUILD.BAT
rem set TARGET_DIR=%~1
rem set TARGET_DIR_LINUX=%TARGET_DIR:\=/%

rem echo TARGET_DIR = %TARGET_DIR% 
rem echo TARGET_DIR_LINUX = %TARGET_DIR_LINUX% 

if exist ".\wwwroot\" rd /s /q ".\wwwroot"
mkdir ".\wwwroot"

echo tar -xzf "./wwwroot.tgz" -C "./wwwroot" 
tar -xzf "./wwwroot.tgz" -C "./wwwroot" 
