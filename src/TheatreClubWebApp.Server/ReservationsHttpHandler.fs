module TheatreClubWebApp.Server.ReservationsHttpHandler

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
    GetReservations = fun _ ->
        task {
            return getAllReservations dbConn
        }
        |> Async.AwaitTask
    SaveReservation = fun r ->
    task {
        let! _ = addReservation dbConn r
        return r
    }
    |> Async.AwaitTask

}


let handler : HttpHandler =
    let remoting logger dbConn =
        Remoting.createApi()
        |> Remoting.withRouteBuilder ReservationsService.RouteBuilder
        |> Remoting.fromValue (getService dbConn)
        |> Remoting.withErrorHandler (Remoting.errorHandler logger)
        |> Remoting.buildHttpHandler

    Require.services<ILogger<_>,IDbConnection> remoting
