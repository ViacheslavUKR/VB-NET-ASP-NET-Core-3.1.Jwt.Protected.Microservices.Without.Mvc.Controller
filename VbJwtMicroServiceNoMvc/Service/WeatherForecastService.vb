Imports Microsoft.AspNetCore.Authorization
Imports System
Imports System.Linq
Imports System.Threading.Tasks

'<Authorize(AuthenticationSchemes:="Bearer", Policy:="default")> - not working without MVC controller
Public Class WeatherForecastService
    Implements IWeatherForecast

    Private Shared ReadOnly Summaries As String() = {"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"}

    Public Function GetForecastAsync(startDate As Date) As WeatherForecastData() Implements IWeatherForecast.GetForecastAsync
        Dim rng = New Random()

        Dim Arr = Enumerable.Range(1, 5).Select(Function(index) New WeatherForecastData With {
            .Date = startDate.AddDays(index),
            .TemperatureC = rng.Next(-20, 55),
            .Summary = Summaries(rng.Next(Summaries.Length))
        }).ToArray()

        Return Arr
    End Function

End Class

