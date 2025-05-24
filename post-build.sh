echo POST BUILD
if [ -d wwwroot ]; then
     rm -rf wwwroot
fi

mkdir wwwroot
echo tar -xzf $(ProjectDir)/wwwroot.tgz -C ./wwwroot
tar -xzf $(ProjectDir)/wwwroot.tgz -C ./wwwroot 