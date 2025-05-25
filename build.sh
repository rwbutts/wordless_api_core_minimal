#!/bin/sh
EXTRA_BUILD_OPTS=""

help() {
    echo -e "\nUsage\nbuild.sh release|debug [outputDirOverride]"
    echo -e "   outputDirOverride is optional; if omitted the dotnet default outputDir is used.\n"
    echo -e "   EXTRA_BUILD_OPTS = \"${EXTRA_BUILD_OPTS}\"\n"
    exit 1
}

if [ -z "$1" ]; then
    help
fi

if [[ -n "$2" ]]; then
    OUTPUT_BUILD_OPTS="-o $2"
else
    OUTPUT_BUILD_OPTS=""
fi

if [[ "${1,,}" ==  "release" ]]; then
    echo "dotnet build -c Release WordlessAPI.csproj --no-self-contained ${OUTPUT_BUILD_OPTS} ${EXTRA_BUILD_OPTS}"
    dotnet build -c Release WordlessAPI.csproj  --no-self-contained ${OUTPUT_BUILD_OPTS}  ${EXTRA_BUILD_OPTS}
elif [[ "${1,,}" ==  "debug" ]]; then
    echo "dotnet build -c Debug WordlessAPI.csproj --no-self-contained ${OUTPUT_BUILD_OPTS} ${EXTRA_BUILD_OPTS}"
    dotnet build -c Debug WordlessAPI.csproj  --no-self-contained ${OUTPUT_BUILD_OPTS}  ${EXTRA_BUILD_OPTS}
else
    echo "Invalid build config: $1"
    help
fi


