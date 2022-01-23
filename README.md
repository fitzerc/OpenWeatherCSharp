# OpenWeatherCSharp
REST API that takes a latitude and longitude, queries the Open Weather API, and returns a very concise summary.

An Open Weather API key is required to use OpenWeatherCSharp. You can register at https://openweathermap.org to get a key.

To run OpenWeatherCSharp
1. Open solution in Visual Studio (can be ran and built from other IDE's, but that process will not be covered here)
2. Update the apiKey in appsettings.json with your Open Weather API key
3. Ensure OpenWeatherCSharp is set as startup project and start debugging

Example request: http://localhost:7175/api/weather?lat=33.44&lon=-96.731667