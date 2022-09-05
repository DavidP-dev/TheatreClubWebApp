module TheatreClubWebApp.Server.WebApp

open Giraffe
open TheatreClubWebApp.Server
let webApp : HttpHandler =
    choose [
        ClubMembersHttpHandler.handler
        PerformancesHttpHandler.handler
        ReservationsHttpHandler.handler
        AuthenticationHttpHandler.handler

        htmlFile "public/index.html"
    ]