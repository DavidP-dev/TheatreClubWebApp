module TheatreClubWebApp.Shared.API

open TheatreClubWebApp.Shared.Domain

type ClubMembersService = {
    GetClubMembers : unit -> Async<ClubMember list>
}
with
    static member RouteBuilder _ m = sprintf "/api/clubmembers/%s" m


type PerformancesService = {
    GetPerformances : unit -> Async<Performance list>
}
with
    static member RouteBuilder _ m = sprintf "/api/performances/%s" m

type ReservationsService = {
    GetReservations : unit -> Async<Reservation list>
}
with
    static member RouteBuilder _ m = sprintf "/api/reservations/%s" m

