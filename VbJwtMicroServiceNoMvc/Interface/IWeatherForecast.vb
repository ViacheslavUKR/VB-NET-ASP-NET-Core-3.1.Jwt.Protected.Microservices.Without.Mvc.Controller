Imports System
Imports System.Threading.Tasks
Public Interface IWeatherForecast
    Function GetForecastAsync(ByVal startDate As DateTime) As WeatherForecastData()

End Interface


