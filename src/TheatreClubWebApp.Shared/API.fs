module TheatreClubWebApp.Shared.API

open TheatreClubWebApp.Shared.Domain

type Service = {
    GetMessage : bool -> Async<string>

    GetClubMembers : unit -> Async<ClubMember list>
}
with
    static member RouteBuilder _ m = sprintf "/api/service/%s" m


type ServiceP = {
    GetMessage : bool -> Async<string>

    GetPerformances : unit -> Async<Performance list>
}
with
    static member RouteBuilder _ m = sprintf "/api/service/%s" m

type ServiceR = {
    GetMessage : bool -> Async<string>

    GetReservations : unit -> Async<Reservation list>
}
with
    static member RouteBuilder _ m = sprintf "/api/service/%s" m

