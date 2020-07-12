Imports System

Public Class WeatherForecastData
    Implements IWeatherForecastData

    Public Property [Date] As Date Implements IWeatherForecastData.Date

    Public Property Summary As String Implements IWeatherForecastData.Summary

    Public Property TemperatureC As Integer Implements IWeatherForecastData.TemperatureC

    Public ReadOnly Property TemperatureF As Integer Implements IWeatherForecastData.TemperatureF
        Get
            Return 32 + CInt((TemperatureC / 0.5556))
        End Get
    End Property

End Class

