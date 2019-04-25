dotnet pack .\src\Cautious.Enigma\
rm -r .\samples\ConsoleApp\obj
dotnet restore .\samples\ConsoleApp\
dotnet publish .\samples\ConsoleApp
