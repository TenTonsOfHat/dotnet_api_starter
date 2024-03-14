@echo off
set "var=%cd%"
cd /d %~dp0
SET /P "variable=enter the name of your new api: "
echo about to create api project %variable%, press enter to continue or ctrl+c to exit
pause 
REM create the sln directory
mkdir %variable%
cd %variable%

REM create sln
dotnet new sln
REM create project
dotnet new webapi --no-https --name %variable%  --use-controllers --no-https --framework net8.0
REM add project to sln
dotnet sln add %variable%/%variable%.csproj 

cd %variable%
REM add default packages
dotnet add package Serilog
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Enrichers.Environment
dotnet add package Serilog.Enrichers.Thread
dotnet add package Serilog.Sinks.Console

cd /d %var%