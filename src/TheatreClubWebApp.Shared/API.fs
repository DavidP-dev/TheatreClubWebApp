module TheatreClubWebApp.Shared.API

open System
open TheatreClubWebApp.Shared.Domain

type ClubMembersService = {
    GetClubMembers : unit -> Async<ClubMember list>
    GetClubMember : Guid -> Async<ClubMember>
    UpdateClubMember : ClubMember -> Async<ClubMember>
    SaveClubMember : ClubMember -> Async<ClubMember>
    DeleteClubMember : Guid -> Async<unit>

}
with
    static member RouteBuilder _ m = sprintf "/api/clubmembers/%s" m


type PerformancesService = {
    GetPerformances : unit -> Async<Performance list>
    GetPerformance : Guid -> Async<Performance>
    UpdatePerformance : Performance -> Async<Performance>
    SavePerformance : Performance -> Async<Performance>
    DeletePerformance : Guid -> Async<unit>
}
with
    static member RouteBuilder _ m = sprintf "/api/performances/%s" m

type ReservationsService = {
    GetReservations : unit -> Async<Reservation list>
    GetReservation : Guid -> Async<Reservation>
    UpdateReservation : Reservation -> Async<Reservation>
    SaveReservation : Reservation -> Async<Reservation>
    DeleteReservation : Guid -> Async<unit>
}
with
    static member RouteBuilder _ m = sprintf "/api/reservations/%s" m


type PasswordService = {
    AddPassword: string -> Async<UserTokenInfo>
}
with
    static member RouteBuilder _ p = sprintf "/api/password/%s" p
