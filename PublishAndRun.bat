if not exist "publish" mkdir publish
cd webbapp\clientapp\
call npm ci
call npm audit fix
call npm run build
cd ..\..\
dotnet build webbapp\webbapp.csproj -c Release /p:DeployOnBuild=true /p:PublishProfile=FolderProfile1
cd publish
dotnet webbapp.dll
pause