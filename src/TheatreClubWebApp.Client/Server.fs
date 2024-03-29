﻿module TheatreClubWebApp.Client.Server

open Fable.SimpleJson
open Fable.Remoting.Client
open TheatreClubWebApp.Shared.Errors
open TheatreClubWebApp.Shared.API

let private exnToError (e:exn) : ServerError =
    match e with
    | :? ProxyRequestException as ex ->
        try
            let serverError = Json.parseAs<{| error: ServerError |}>(ex.Response.ResponseBody)
            serverError.error
        with _ -> ServerError.Exception(e.Message)
    | _ -> ServerError.Exception(e.Message)

type ServerResult<'a> = Result<'a,ServerError>

module Cmd =
    open Elmish

    module OfAsync =
        let eitherAsResult fn resultMsg =
            Cmd.OfAsync.either fn () (Result.Ok >> resultMsg) (exnToError >> Result.Error >> resultMsg)

let service =
    Remoting.createApi()
    |> Remoting.withRouteBuilder ClubMembersService.RouteBuilder
    |> Remoting.buildProxy<ClubMembersService>

let serviceP =
    Remoting.createApi()
    |> Remoting.withRouteBuilder PerformancesService.RouteBuilder
    |> Remoting.buildProxy<PerformancesService>

let serviceR =
    Remoting.createApi()
    |> Remoting.withRouteBuilder ReservationsService.RouteBuilder
    |> Remoting.buildProxy<ReservationsService>

let servicePass =
    Remoting.createApi()
    |> Remoting.withRouteBuilder PasswordService.RouteBuilder
    |> Remoting.buildProxy<PasswordService>