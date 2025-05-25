#!/bin/sh
echo POST-BUILD.SH

TARGET_PATH_WIN="$1"
TARGET_PATH_LINUX="${TARGET_PATH_WIN//\\//}"

echo "TARGET_PATH_WIN = $TARGET_PATH_WIN"
echo "TARGET_PATH_LINUX = $TARGET_PATH_LINUX"

if [ -d wwwroot ]; then
     rm -rf wwwroot
fi

mkdir "${TARGET_PATH_LINUX}wwwroot"

echo "tar -xzf ./wwwroot.tgz -C ${TARGET_PATH_LINUX}wwwroot"
tar -xzf ./wwwroot.tgz -C "${TARGET_PATH_LINUX}wwwroot" 

sleep 10
