module TheatreClubWebApp.Shared.API

open TheatreClubWebApp.Shared.Domain

type Service = {
    GetMessage : bool -> Async<string>

    GetClubMembers: unit -> Async<ClubMember list>
}
with
    static member RouteBuilder _ m = sprintf "/api/service/%s" m