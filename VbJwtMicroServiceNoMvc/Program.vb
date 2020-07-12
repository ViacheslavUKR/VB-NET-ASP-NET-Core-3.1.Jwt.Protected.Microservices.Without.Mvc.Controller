Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.AspNetCore
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Logging

Module Program
    Public Sub Main(ByVal args As String())
        CreateWebHostBuilder(args).Build().Run()
    End Sub

    Function CreateWebHostBuilder(ByVal args As String()) As IHostBuilder
        Return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(Sub(webBuilder)
                                                                            webBuilder.UseStartup(Of Startup)()
                                                                            webBuilder.UseUrls("http://localhost:6000/")
                                                                        End Sub)
    End Function
End Module
