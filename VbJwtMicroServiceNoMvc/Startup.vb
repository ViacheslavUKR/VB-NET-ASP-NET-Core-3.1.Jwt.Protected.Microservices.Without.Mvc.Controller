Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports AuTestMicroservice.Data
Imports Microsoft.AspNetCore.Authentication.JwtBearer
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.AspNetCore.Routing
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Logging
Imports Microsoft.Extensions.Options
Imports Microsoft.IdentityModel.Tokens
Imports Newtonsoft.Json

Public Class Startup

    Public ReadOnly Property Configuration As IConfiguration
    Public ReadOnly Property HostingEnvironment As IWebHostEnvironment

    Public Sub New(ByVal _configuration As IConfiguration, ByVal env As IWebHostEnvironment)
        Configuration = _configuration
        HostingEnvironment = env
    End Sub

    'This method gets called by the runtime. Use this method to add services to the container.
    Public Sub ConfigureServices(ByVal services As IServiceCollection)
        services.AddCors()

        services.AddSingleton(Of WeatherForecastService)().AddAuthorization(Sub(AuthOptions)
                                                                                AuthOptions.AddPolicy("default", Sub(Policy)
                                                                                                                     Policy.AuthenticationSchemes.Add("Bearer")
                                                                                                                     Policy.RequireAuthenticatedUser()
                                                                                                                 End Sub)
                                                                            End Sub)

        services.AddAuthentication(Sub(AuOptions)
                                       AuOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme
                                       AuOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme
                                   End Sub).
                                   AddJwtBearer(Sub(JwtOptions)
                                                    JwtOptions.RequireHttpsMetadata = False
                                                    JwtOptions.SaveToken = True
                                                    JwtOptions.TokenValidationParameters = New TokenValidationParameters With {
                                                                      .ValidateLifetime = True,
                                                                      .ValidateIssuerSigningKey = True,
                                                                      .IssuerSigningKey = New SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration("Jwt:Key"))),
                                                                      .ValidateIssuer = True,
                                                                      .ValidIssuer = Configuration("Jwt:Issuer"),
                                                                      .ValidateAudience = True,
                                                                      .ValidAudience = Configuration("Jwt:Issuer")
                                                                  }
                                                    JwtOptions.Events = New JwtBearerEvents() With {
                                                                      .OnAuthenticationFailed = Function(Context)
                                                                                                    Context.Response.StatusCode = StatusCodes.Status401Unauthorized
                                                                                                    Context.Response.ContentType = "application/json; charset=utf-8"
                                                                                                    Dim Message = If(HostingEnvironment.IsDevelopment(), Context.Exception.ToString(), "An error occurred processing your authentication.")
                                                                                                    Dim Result = JsonConvert.SerializeObject(New With {Message})
                                                                                                    Context.Response.WriteAsync(Result)
                                                                                                    Context.HttpContext.Abort()
                                                                                                    Return Task.CompletedTask
                                                                                                End Function
                                                                  }
                                                End Sub)
    End Sub

    'This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    Public Sub Configure(ByVal app As IApplicationBuilder, ByVal env As IWebHostEnvironment)
        If env.IsDevelopment() Then
            app.UseDeveloperExceptionPage()
        End If

        app.UseAuthentication()
        'app.UseAuthorization() 'don't working without MVC controller

        app.UseCors(Sub(CorsOptopns)
                        CorsOptopns.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                    End Sub)


        app.Map("/WeatherApi", Sub(EndpointsOptions)
                                   EndpointsOptions.Run((Function(Context)
                                                             Return Context.Response.WriteAsync(
                                                             JsonConvert.SerializeObject(Context.RequestServices.GetService(Of WeatherForecastService).GetForecastAsync(DateTime.Now), Formatting.Indented))
                                                         End Function))
                               End Sub)



        'alternate way to bind service to endpoint
        'app.UseEndpoints(Sub(EndpointsOptions)
        '                     EndpointsOptions.MapGet("/WeatherApi", Async Sub(Context)
        '                                                                Await Context.Response.WriteAsync(
        '                                                                JsonConvert.SerializeObject(Context.RequestServices.GetService(Of WeatherForecastService).GetForecastAsync(DateTime.Now), Formatting.Indented))
        '                                                            End Sub)
        '                 End Sub)
    End Sub
End Class

