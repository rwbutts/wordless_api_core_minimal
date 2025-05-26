#!/bin/sh
echo POST-BUILD.SH
exit 0
#TARGET_PATH_WIN="$1"
#TARGET_PATH_LINUX="${TARGET_PATH_WIN//\\//}"

#echo "TARGET_PATH_WIN = $TARGET_PATH_WIN"
#echo "TARGET_PATH_LINUX = $TARGET_PATH_LINUX"

#if [ -d ./wwwroot ]; then
#     rm -rf ./wwwroot
#fi

#mkdir "./wwwroot"

echo "tar -xzf ./wwwroot.tgz -C ./wwwroot"
tar -xzf ./wwwroot.tgz -C ./wwwroot"

sleep 10
