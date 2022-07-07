module TheatreClubWebApp.Server.WebApp

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



let getServiceFromCtx (httpContext: HttpContext) =
    let dbConn = httpContext.GetService<IDbConnection>()
    {
    GetMessage = fun success ->
        task {
            if success then return "Hi from Server!"
            else return ServerError.failwith (ServerError.Exception "OMG, something terrible happened")
        }
        |> Async.AwaitTask
    GetClubMembers = fun _ ->
        task {
            return getAllClubMembers dbConn
        }
        |> Async.AwaitTask

}

let webApp : HttpHandler =
    let remoting logger =
        Remoting.createApi()
        |> Remoting.withRouteBuilder Service.RouteBuilder
        |> Remoting.fromContext getServiceFromCtx
        |> Remoting.withErrorHandler (Remoting.errorHandler logger)
        |> Remoting.buildHttpHandler
    choose [
        Require.services<ILogger<_>> remoting
        htmlFile "public/index.html"
    ]