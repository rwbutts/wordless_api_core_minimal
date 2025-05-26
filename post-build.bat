echo POST-BUILD.BAT
exit 0
rem set TARGET_DIR=%~1
rem set TARGET_DIR_LINUX=%TARGET_DIR:\=/%

rem echo TARGET_DIR = %TARGET_DIR% 
rem echo TARGET_DIR_LINUX = %TARGET_DIR_LINUX% 

rem if exist ".\wwwroot\" rd /s /q ".\wwwroot"
rem mkdir ".\wwwroot"

echo tar -xzf "./wwwroot.tgz" -C "./wwwroot" 
tar -xzf "./wwwroot.tgz" -C "./wwwroot" 
