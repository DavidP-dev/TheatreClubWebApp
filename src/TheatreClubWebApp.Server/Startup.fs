﻿module TheatreClubWebApp.Server.Startup

open System.Data
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Data.SqlClient
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.Extensions.Logging

type Startup(cfg:IConfiguration, env:IWebHostEnvironment) =
    member _.ConfigureServices (services:IServiceCollection) =
        let connstring = cfg.GetConnectionString("theatreDb")
        services.AddTransient<IDbConnection>(fun _ -> new SqlConnection(connstring)) |> ignore

        services
            .AddApplicationInsightsTelemetry(cfg.["APPINSIGHTS_INSTRUMENTATIONKEY"])
            .AddGiraffe() |> ignore
    member _.Configure(app:IApplicationBuilder) =
        app
            .UseStaticFiles()
            .UseGiraffe WebApp.webApp