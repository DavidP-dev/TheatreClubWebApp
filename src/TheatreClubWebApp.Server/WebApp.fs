module TheatreClubWebApp.Server.WebApp

open Giraffe
let webApp : HttpHandler =
    choose [
        ClubMembersHttpHandler.handler
        PerformancesHttpHandler.handler
        ReservationsHttpHandler.handler

        htmlFile "public/index.html"
    ]