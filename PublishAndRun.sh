cd webbapp/clientapp
npm ci
npm audit fix
npm run build
cd ../..
dotnet build webbapp/webbapp.csproj -c Release /p:DeployOnBuild=true /p:PublishProfile=FolderProfile1
cd Publish
dotnet webbapp.dll

