#!/bin/bash

dotnet pack ./src/Cautious.Enigma/
rm -rf ./samples/ConsoleApp/obj
dotnet restore ./samples/ConsoleApp/
dotnet publish ./samples/ConsoleApp
