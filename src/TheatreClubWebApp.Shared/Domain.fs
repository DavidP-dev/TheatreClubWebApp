module TheatreClubWebApp.Shared.Domain


// This is database program for theatre club members management
open System

// Every performance has a specific Genre
type Genre =
    | Alternative
    | Art
    | Comedy
    | Dance
    | Drama
    | Mainstream
    | Musical
    | Philosophy

// Every member of club is one ClubMember record
type ClubMember =
    {
       Id : Guid
       Name : string
       Surname : string
       Email : string
       PreferredGenres : Genre list

    }

// Every entered theatre play is one Performance record
type Performance =
    {
        Id : Guid
        Title : string
        Theatre : string
        DateAndTime: DateTimeOffset
        Tickets : string
        Reservations : string
        Cost : int
        Genres : Genre list
    }

// Reservation of specific game by specific members
type Reservation =
    {
        ReservationID : Guid
        MemberId : Guid
        MemberName : string
        MemberSurname : String
        PerformanceId : Guid
        PerformanceTitle : string
        PerformanceDateAndTime : DateTimeOffset
        IsPaid : string
        TicketsReceived : string
    }

