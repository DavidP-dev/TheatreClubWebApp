module TheatreClubWebApp.Server.AuthenticationHttpHandler


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
    AddPassword = fun p ->
        task {
            return
                if checkLogin p then
                    createUserToken ()
                else
                    failwith "Nesprávné heslo"

        }
        |> Async.AwaitTask
}


let handler : HttpHandler =
    let remoting logger dbConn =
        Remoting.createApi()
        |> Remoting.withRouteBuilder PasswordService.RouteBuilder
        |> Remoting.fromValue (getService dbConn)
        |> Remoting.withErrorHandler (Remoting.errorHandler logger)
        |> Remoting.buildHttpHandler

    Require.services<ILogger<_>,IDbConnection> remoting