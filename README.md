# Pokemon Api
 Pokemon Api challenge for TrueLayer

# How to run
Open the solution file (PokemonApi.sln) in Visual Studio
Ensure that the Project "PokemonApi" is set as the startup project. 
Nuget packages should be included (and are all referenced from the default Nuget repo) but to ensure this the case you can go to  Tools > Options > NuGet Package Manager. Under Package Restore options, select "Allow NuGet to download missing packages".
Run the project - this should open a browser window with the Swagger definitions for the two endpoints, which will also allow you to test the API.
Unit tests are under the "PokemonApiTest" project - right-click on this and select "Run unit tests" to run these.