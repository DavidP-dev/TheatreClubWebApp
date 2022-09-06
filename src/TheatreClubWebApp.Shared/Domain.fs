module TheatreClubWebApp.Shared.Domain

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
       Id: Guid
       Name: string
       Surname: string
       Email: string
       PreferredGenres: Genre list
       NumberOfReservedTickets: string
    }

// Every entered theatre play is one Performance record
type Performance =
    {
        Id: Guid
        Title: string
        Theatre: string
        DateAndTime: string
        NumberOfAvailableTickets: string
        NumberOfReservedTickets: string
        Cost: string
        Genres: Genre list
    }

// Reservation of specific performance made by specific member
type Reservation =
    {
        ReservationID: Guid
        MemberId: Guid
        MemberName: string
        MemberSurname: String
        PerformanceId: Guid
        PerformanceTitle: string
        PerformanceDateAndTime: string
        NumberOfReservedTickets: string
        IsPaid: bool
        TicketsReceived: bool
    }

// User token used for user authentication
type UserTokenInfo =
    {
        Token: string
        ExpiresOn: DateTimeOffset
    }
