module TheatreClubWebApp.Server.ClubMembersHttpHandler

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
    GetClubMembers = fun _ ->
        task {
            return getAllClubMembers dbConn
        }
        |> Async.AwaitTask

}


let handler : HttpHandler =
    let remoting logger dbConn =
        Remoting.createApi()
        |> Remoting.withRouteBuilder ClubMembersService.RouteBuilder
        |> Remoting.fromValue (getService dbConn)
        |> Remoting.withErrorHandler (Remoting.errorHandler logger)
        |> Remoting.buildHttpHandler

    Require.services<ILogger<_>,IDbConnection> remoting

