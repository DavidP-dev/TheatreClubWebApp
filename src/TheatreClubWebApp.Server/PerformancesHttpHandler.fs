module TheatreClubWebApp.Server.PerformancesHttpHandler

open System.Data
open Giraffe
open Giraffe.GoodRead
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open TheatreClubWebApp.Shared.API
open FSharp.Control.Tasks
open TheatreClubWebApp.Shared.Errors

open TheatreClubWebApp.Server.Business


let getService (dbConn: IDbConnection) =
    {
    GetPerformances = fun _ ->
        task {
            return getAllPerformances dbConn
        }
        |> Async.AwaitTask
    SavePerformance = fun m ->
        task {
            let! _ = addPerformance dbConn m
            return m
        }
        |> Async.AwaitTask
    DeletePerformance = fun m ->
        task {
            let! _ = removePerformance dbConn m
            return ()
        }
        |> Async.AwaitTask
}


let handler : HttpHandler =
    let remoting logger dbConn =
        Remoting.createApi()
        |> Remoting.withRouteBuilder PerformancesService.RouteBuilder
        |> Remoting.fromValue (getService dbConn)
        |> Remoting.withErrorHandler (Remoting.errorHandler logger)
        |> Remoting.buildHttpHandler

    Require.services<ILogger<_>,IDbConnection> remoting

