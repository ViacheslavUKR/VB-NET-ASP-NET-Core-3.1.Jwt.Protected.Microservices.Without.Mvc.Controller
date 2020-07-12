Imports System
Public Interface IWeatherForecastData
    Property [Date] As DateTime
    Property Summary As String
    Property TemperatureC As Integer
    ReadOnly Property TemperatureF As Integer
End Interface

