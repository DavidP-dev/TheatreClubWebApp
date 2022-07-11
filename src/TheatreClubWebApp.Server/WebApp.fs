module TheatreClubWebApp.Server.WebApp
open Giraffe
let webApp : HttpHandler =
    choose [
        ClubMembersHttpHandlers.handler

        htmlFile "public/index.html"
    ]