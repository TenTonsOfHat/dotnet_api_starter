@echo off
@REM: docker build builds the docker container 
@REM: --file: Name of the Dockerfile (Default is 'PATH/Dockerfile')
@REM: --tag: Name and optionally a tag in the 'name:tag' format
@REM: . : the . at the end tells docker that we should use the current path as our build context

set "var=%cd%"
cd /d %~dp0
cd ..
docker build --file .\dotnet_api_starter\Dockerfile --tag dotnet_api_starter .
cd /d %var%