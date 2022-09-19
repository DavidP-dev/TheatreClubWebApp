module TheatreClubWebApp.Server.Database


open System
open System.Data
open System.Globalization
open TheatreClubWebApp.Shared.Domain

open Dapper
open Dapper.FSharp
open Dapper.FSharp.MSSQL
open Microsoft.Data.SqlClient
open Microsoft.FSharp.Core

// Database types
type MemberDB =
    {
        Id: Guid
        Name: string
        Surname: string
        Email: string
        PreferredGenres: string
        NumberOfReservedTickets: int
    }

type PerformanceDB =
    {
        Id: Guid
        Title: string
        Theatre : string
        DateAndTime: DateTimeOffset
        NumberOfAvailableTickets: int
        NumberOfReservedTickets: int
        Cost: int
        Genres: string
    }

type ReservationDB =
        {
        ReservationID: Guid
        MemberId: Guid
        MemberName: string
        MemberSurname: string
        PerformanceId: Guid
        PerformanceTitle: string
        PerformanceDateAndTime: DateTimeOffset
        NumberOfReservedTickets: int
        IsPaid: bool
        TicketsReceived: bool
        }

// Modules for transferring data from database layer to domain layer and opposite
module Transfers =
    let dateTimeOffsetToString (dto: DateTimeOffset) : string =
        let dayOfWeekInCzech =
            match dto.DayOfWeek with
            | DayOfWeek.Monday -> "Pondělí"
            | DayOfWeek.Tuesday -> "Úterý"
            | DayOfWeek.Wednesday -> "Středa"
            | DayOfWeek.Thursday -> "Čtvrtek"
            | DayOfWeek.Friday -> "Pátek"
            | DayOfWeek.Saturday -> "Sobota"
            | DayOfWeek.Sunday -> "Neděle"
            | _ -> failwith "Takový den neexistuje"

        let dateTimeString = dto.ToString("dd.MM.yyyy HH:mm")

        let dateTimeOffsetString = dayOfWeekInCzech + ", " + dateTimeString
        dateTimeOffsetString

    let tryStringToDateTimeOffset (s: string) : DateTimeOffset =
        let convertedDateTimeExact = DateTimeOffset.TryParseExact(s, "dd.MM.yyyy HH:mm", CultureInfo("es-CZ", false), DateTimeStyles.AssumeLocal)
        match convertedDateTimeExact with
            |true, value -> value
            |false, _  -> DateTimeOffset.MinValue

    let boolToString (b: bool) : string =
        let stringFromBool =
            match b with
            | true -> "Ano"
            | false -> "Ne"
        stringFromBool
    let stringToBool (s: string) :bool =
        let boolFromString =
            match s with
            | "Ano" -> true
            | "Ne" -> false
            | _ -> failwith $"Možnost -{s}-neexistuje!!!"
        boolFromString

module MembersDb =
    let parseGenre (gn: string) : Genre =
        match gn with
        | "Alternative" -> Alternative
        | "Art" -> Art
        | "Comedy" -> Comedy
        | "Dance" -> Dance
        | "Drama" -> Drama
        | "Mainstream" -> Mainstream
        | "Musical" -> Musical
        | "Philosophy" -> Philosophy
        | _ -> failwith $"There is no genre {gn}!"

    let genreToString (gn: Genre) : string =
        match gn with
        | Alternative -> "Alternative"
        | Art -> "Art"
        | Comedy -> "Comedy"
        | Dance -> "Dance"
        | Drama -> "Drama"
        | Mainstream -> "Mainstream"
        | Musical -> "Musical"
        | Philosophy -> "Philosophy"

    let dapperGenreString (gn : Genre) : string =
        match gn with
        | Alternative -> "%Alternative%"
        | Art -> "%Art%"
        | Comedy -> "%Comedy%"
        | Dance -> "%Dance%"
        | Drama -> "%Drama%"
        | Mainstream -> "%Mainstream%"
        | Musical -> "%Musical%"
        | Philosophy -> "%Philosophy%"

    let toDomain (db: MemberDB) : ClubMember = {
       Id = db.Id
       Name = db.Name
       Surname = db.Surname
       Email = db.Email
       PreferredGenres = db.PreferredGenres.Split(",") |> Array.map parseGenre |> List.ofArray
       NumberOfReservedTickets = db.NumberOfReservedTickets |> string
       }

    let toDatabase (dm: ClubMember) : MemberDB = {
        Id = dm.Id
        Name = dm.Name
        Surname = dm.Surname
        Email = dm.Email
        PreferredGenres = dm.PreferredGenres |> List.map genreToString |> (fun x -> String.Join(",", x))
        NumberOfReservedTickets = dm.NumberOfReservedTickets |> int
        }

module PerformancesDB =
    let toDomain (db: PerformanceDB) : Performance = {
        Id = db.Id
        Title = db.Title
        Theatre = db.Theatre
        DateAndTime = db.DateAndTime |> Transfers.dateTimeOffsetToString
        NumberOfAvailableTickets = db.NumberOfAvailableTickets |> string
        NumberOfReservedTickets = db.NumberOfReservedTickets |> string
        Cost = db.Cost |> string
        Genres =  db.Genres.Split(",") |> Array.map MembersDb.parseGenre |> List.ofArray
        }

    let toDatabase (dm: Performance) : PerformanceDB = {
        Id = dm.Id
        Title = dm.Title
        Theatre = dm.Theatre
        DateAndTime = dm.DateAndTime |> Transfers.tryStringToDateTimeOffset
        NumberOfAvailableTickets = dm.NumberOfAvailableTickets |> int
        NumberOfReservedTickets = dm.NumberOfReservedTickets |> int
        Cost = dm.Cost |> int
        Genres = dm.Genres |> List.map MembersDb.genreToString |> (fun x -> String.Join(",", x))}

module ReservationDB =
    let toDomain (db: ReservationDB) : Reservation = {
        ReservationID = db.ReservationID
        MemberId = db.MemberId
        MemberName = db.MemberName
        MemberSurname = db.MemberSurname
        PerformanceId = db.PerformanceId
        PerformanceTitle = db.PerformanceTitle
        PerformanceDateAndTime = db.PerformanceDateAndTime |> Transfers.dateTimeOffsetToString
        NumberOfReservedTickets = db.NumberOfReservedTickets |> string
        IsPaid = db.IsPaid
        TicketsReceived = db.TicketsReceived
    }
    let toDatabase (dm: Reservation) : ReservationDB = {
        ReservationID = dm.ReservationID
        MemberId = dm.MemberId
        MemberName = dm.MemberName
        MemberSurname = dm.MemberSurname
        PerformanceId = dm.PerformanceId
        PerformanceTitle = dm.PerformanceTitle
        PerformanceDateAndTime = dm.PerformanceDateAndTime |> Transfers.tryStringToDateTimeOffset
        NumberOfReservedTickets = dm.NumberOfReservedTickets |> int
        IsPaid = dm.IsPaid
        TicketsReceived = dm.TicketsReceived
    }

// names of database tables
let membersTable = table'<MemberDB> "ClubMembers"

let performancesTable = table'<PerformanceDB> "Performances"

let ReservationsTable = table'<ReservationDB> "Reservations"

// Operations that can be used by other Db functions
// Returns list with member by ID, can be used by other functions
let returnClubMemberById (conn: IDbConnection) (cId: Guid)  =
    let output =
        select {
            for m in membersTable do
            where (m.Id = cId)
        }
        |> conn.SelectAsync<MemberDB>
    let v = output.Result
    v |> Seq.toList |> List.map(MembersDb.toDomain)

// Returns performance by ID, can be used by other functions
let returnPerformanceById (conn: IDbConnection) (pId: Guid)  =
    let output =
        select {
            for p in performancesTable do
            where (p.Id = pId)
        }
        |> conn.SelectAsync<PerformanceDB>
    let v = output.Result
    v |> Seq.toList |> List.map(PerformancesDB.toDomain) |> List.head

// Returns reservation by reservation ID, can be used by other DB functions
let returnReservationByIdDb (conn: IDbConnection) (rId: Guid)  =
    let output =
        select {
            for r in ReservationsTable do
            where (r.ReservationID = rId)
        }
        |> conn.SelectAsync<ReservationDB>
    let v = output.Result
    v |> Seq.toList |> List.map(ReservationDB.toDomain) |> List.head

// Returns reservations by ClubMember Id, can be used by other DB functions
let returnReservationsByClubMemberId (conn: IDbConnection) (cId: Guid)  =
    let output =
        select {
            for r in ReservationsTable do
            where (r.MemberId = cId)
        }
        |> conn.SelectAsync<ReservationDB>
    let v = output.Result
    v |> Seq.toList |> List.map(ReservationDB.toDomain)

// Returns reservations by performance Id, can be used by other DB functions
let returnReservationsByPerformanceId (conn: IDbConnection) (pId: Guid)  =
    let output =
        select {
            for r in ReservationsTable do
            where (r.PerformanceId = pId)
        }
        |> conn.SelectAsync<ReservationDB>
    let v = output.Result
    v |> Seq.toList |> List.map(ReservationDB.toDomain)

// Checks existence of club member by searching for his email in database
let tryGetMemberByEmail (conn: IDbConnection) (email: string) =
    let vysl =
        select {
            for m in membersTable do
            where (m.Email = email)
        }
        |> conn.SelectAsync<MemberDB>


    let v = vysl.Result
    v
    |> Seq.tryHead
    |> Option.map MembersDb.toDomain

// Checks existence of club member by searching for his Id in database
let tryGetMemberById (conn: IDbConnection) (cMId: Guid) =
    let vysl =
        select {
            for m in membersTable do
            where (m.Id = cMId)
        }
        |> conn.SelectAsync<MemberDB>


    let v = vysl.Result
    v
    |> Seq.tryHead
    |> Option.map MembersDb.toDomain

// Checks existence of performance in database by Title and Date
let tryGetPerformanceByTitleAndDate (conn: IDbConnection) (performance: Performance) =
    let parsedPerformance = performance.DateAndTime |> Transfers.tryStringToDateTimeOffset
    let vysl =
        select {
            for p in performancesTable do
            where (p.Title = performance.Title && p.DateAndTime = parsedPerformance)}
        |> conn.SelectAsync<PerformanceDB>

    let v = vysl.Result
    v
    |> Seq.tryHead
    |> Option.map PerformancesDB.toDomain

// Checks existence of performance in database by performance Id
let tryGetPerformanceById (conn: IDbConnection) (pId: Guid) =
    let vysl =
        select {
            for p in performancesTable do
            where (p.Id = pId )}
        |> conn.SelectAsync<PerformanceDB>

    let v = vysl.Result
    v
    |> Seq.tryHead
    |> Option.map PerformancesDB.toDomain


// Checks existence of registration in database
let tryGetReservationById (conn: IDbConnection) (rId: Guid) =
    let vysl =
        select {
            for r in ReservationsTable do
            where (r.ReservationID = rId)}
        |> conn.SelectAsync<ReservationDB>

    let v = vysl.Result
    v
    |> Seq.tryHead
    |> Option.map ReservationDB.toDomain

// Operations with Club members
// adds Member to database
let insertCMToDb (conn: IDbConnection) (cM: ClubMember) =
    let dbMember = MembersDb.toDatabase cM
    insert {
        into membersTable
        value dbMember
    }
    |> conn.InsertAsync

// removes ClubMember from Database, modifies related performances and deletes related reservations
let removeCmFromDb (conn: IDbConnection) (cId: Guid) =
    task {
        let relatedReservations = returnReservationsByClubMemberId conn cId
        let relatedClubPerformanceWithTickets = relatedReservations |> List.map(fun x -> x.PerformanceId, x.NumberOfReservedTickets)

        conn.Open()
        use trans = conn.BeginTransaction()

        let deleteCMQuery = // Delete ClubMember
            delete {
                for m in membersTable do
                where (m.Id = cId )}
        let! _ = conn.DeleteAsync(deleteCMQuery, trans)


        for perf, tickets in relatedClubPerformanceWithTickets do
                let! _ = conn.ExecuteAsync("UPDATE Performances SET NumberOfReservedTickets = NumberOfReservedTickets - @amount, NumberOfAvailableTickets = NumberOfAvailableTickets + @amount WHERE Id = @performanceId",
                                           (["amount", box tickets; "performanceId", box perf] |> Map.ofList), trans)
                ()

        let deleteRelatedReservations = // Delete related reservations
            delete {
                for r in ReservationsTable do
                where (r.MemberId = cId)
            }
        let! _ = conn.DeleteAsync(deleteRelatedReservations, trans)

        trans.Commit()
        return ()
    }

// updates Member's data
let updateClubMemberDb (conn:IDbConnection) (cM:ClubMember) =
    let dbMember = MembersDb.toDatabase cM
    update {
        for m in membersTable do
        set dbMember
        where (m.Id = dbMember.Id)
    }
    |> conn.UpdateAsync

// Returns all club members in database
let returnAllClubMembersFromDb (conn:IDbConnection) =
        let output =
            select {
                for m in membersTable do
                selectAll
                orderBy m.Surname
                }
            |> conn.SelectAsync<MemberDB>

        let v = output.Result
        v
        |> Seq.toList |> List.map(MembersDb.toDomain)

// Returns club members by preferred genres
let returnClubMembersByGenre (conn:IDbConnection) (genre : Genre) =
    let genreString = MembersDb.dapperGenreString genre
    let output =
         select {
             for m in membersTable do
             where (like m.PreferredGenres genreString)}
             |> conn.SelectAsync<MemberDB>

    let v = output.Result
    v
    |> Seq.toList |> List.map(MembersDb.toDomain)

// Operations with performances
// function adds performance
let addPerformanceToDb (conn:IDbConnection) (perf:Performance) =
    let dbPerformance = PerformancesDB.toDatabase perf
    insert {
        into performancesTable
        value dbPerformance
    }
    |> conn.InsertAsync

// function removes Performance, modifies related members data and deletes related reservations
let removePerformanceFromDb (conn : IDbConnection) (pId : Guid) =
    task {
        let relatedReservations = returnReservationsByPerformanceId conn pId
        let relatedMembersIdsAndReservedTickets =  relatedReservations |> List.map(fun x -> x.MemberId, x.NumberOfReservedTickets)

        conn.Open()
        use trans = conn.BeginTransaction()

        let deletePerformanceQuery =
                delete {
                    for p in performancesTable do
                    where (p.Id = pId )
                }
        let! _ = conn.DeleteAsync(deletePerformanceQuery, trans)

        for cId,reservedTickets in relatedMembersIdsAndReservedTickets do
            let! _ =
                conn.ExecuteAsync("UPDATE ClubMembers SET NumberOfReservedTickets = NumberOfReservedTickets - @amount WHERE Id = @memberId",
                                  (["amount", box reservedTickets; "memberId", box cId] |> Map.ofList), trans)
            ()

        let deleteRelatedReservationsQuery =
                    delete {
                        for r in ReservationsTable do
                        where (r.PerformanceId = pId)
                    }
        let! _ = conn.DeleteAsync(deleteRelatedReservationsQuery, trans)

        trans.Commit()
        return ()
    }

// function updates performance
let updatePerformanceDb (conn: IDbConnection) (perf: Performance) =
    let dbPerformance = PerformancesDB.toDatabase perf
    update {
        for p in performancesTable do
        set dbPerformance
        where (p.Id = dbPerformance.Id)
    }
    |> conn.UpdateAsync

// Returns all performances
let returnAllPerformancesFromDb (conn:IDbConnection) =
        let output =
            select {
                for p in performancesTable do
                selectAll
                orderBy p.DateAndTime
                }
            |> conn.SelectAsync<PerformanceDB>

        let v = output.Result
        v |> Seq.toList |> List.map(PerformancesDB.toDomain)

// Returns performances by Genres
let returnPerformancesByGenre (conn:IDbConnection) (genre : Genre) =
    let genreString = MembersDb.dapperGenreString genre
    let output =
         select {
             for p in performancesTable do
             where (like p.Genres genreString)}
             |> conn.SelectAsync<PerformanceDB>

    let v = output.Result
    v
    |> Seq.toList |> List.map(PerformancesDB.toDomain)
// Operations with reservations
// Function add Reservation
let addReservationToDb (conn: IDbConnection) (res: Reservation) =
    let dbReservation = ReservationDB.toDatabase res
    task {
        conn.Open()

        use trans = conn.BeginTransaction()

        let addToReservationTableQuery =
            insert {
                into ReservationsTable
                value dbReservation
            }

        let! _ = conn.InsertAsync(addToReservationTableQuery, trans)

        let! _ =
            conn.ExecuteAsync("UPDATE ClubMembers SET NumberOfReservedTickets = NumberOfReservedTickets + @amount WHERE Id = @memberId",
                              (["amount", box dbReservation.NumberOfReservedTickets; "memberId", box dbReservation.MemberId] |> Map.ofList), trans)

        let! _ =
            conn.ExecuteAsync("UPDATE Performances SET NumberOfReservedTickets = NumberOfReservedTickets + @amount, NumberOfAvailableTickets = NumberOfAvailableTickets - @amount WHERE Id = @performanceId",
                              (["amount", box dbReservation.NumberOfReservedTickets; "performanceId", box dbReservation.PerformanceId] |> Map.ofList), trans)

        trans.Commit()
        return ()
    }

// Function remove Reservation, modifies related performances and club members
let removeReservationFromDb (conn: IDbConnection) (res: Reservation) =
    let dbReservation = ReservationDB.toDatabase res
    task {
        conn.Open()
        use trans = conn.BeginTransaction()

        let removeFromReservationTableQuery =
            delete {
                for r in ReservationsTable do
                where (r.ReservationID = dbReservation.ReservationID )
            }
        let! _ = conn.DeleteAsync(removeFromReservationTableQuery, trans)

        let! _ =
            conn.ExecuteAsync("UPDATE ClubMembers SET NumberOfReservedTickets = NumberOfReservedTickets - @amount WHERE Id = @memberId",
                              (["amount", box dbReservation.NumberOfReservedTickets; "memberId", box dbReservation.MemberId] |> Map.ofList), trans)

        let! _ =
            conn.ExecuteAsync("UPDATE Performances SET NumberOfReservedTickets = NumberOfReservedTickets - @amount, NumberOfAvailableTickets = NumberOfAvailableTickets + @amount WHERE Id = @performanceId",
                              (["amount", box dbReservation.NumberOfReservedTickets; "performanceId", box dbReservation.PerformanceId] |> Map.ofList), trans)

        trans.Commit()
        return ()
    }

// Function update Reservation, related ClubMember and Performance
let updateReservationDb (conn:IDbConnection) (res:Reservation) =
    let dbReservation = ReservationDB.toDatabase res
    task {

        let oldState = (returnReservationByIdDb conn res.ReservationID) |> ReservationDB.toDatabase
        let isNewMore = dbReservation.NumberOfReservedTickets > oldState.NumberOfReservedTickets
        let isSame = dbReservation.NumberOfReservedTickets = oldState.NumberOfReservedTickets

        conn.Open()
        use trans = conn.BeginTransaction()

        let updateReservationQuery =
            update {
                for r in ReservationsTable do
                set dbReservation
                where (r.ReservationID = dbReservation.ReservationID)
            }
        let! _ = conn.UpdateAsync(updateReservationQuery, trans)


        if isNewMore then

                let! _ =
                    conn.ExecuteAsync("UPDATE ClubMembers SET NumberOfReservedTickets = NumberOfReservedTickets + @amount WHERE Id = @memberId",
                                      (["amount", box (dbReservation.NumberOfReservedTickets - oldState.NumberOfReservedTickets); "memberId", box dbReservation.MemberId] |> Map.ofList), trans)

                let! _ =
                    conn.ExecuteAsync("UPDATE Performances SET NumberOfReservedTickets = NumberOfReservedTickets + @amount, NumberOfAvailableTickets = NumberOfAvailableTickets - @amount WHERE Id = @performanceId",
                                      (["amount", box (dbReservation.NumberOfReservedTickets - oldState.NumberOfReservedTickets); "performanceId", box dbReservation.PerformanceId] |> Map.ofList), trans)
                ()

        elif isSame then
                ()

        else

                let! _ = conn.ExecuteAsync("UPDATE ClubMembers SET NumberOfReservedTickets = NumberOfReservedTickets - @amount WHERE Id = @memberId",
                                  (["amount", box (oldState.NumberOfReservedTickets - dbReservation.NumberOfReservedTickets); "memberId", box dbReservation.MemberId] |> Map.ofList), trans)

                let! _ = conn.ExecuteAsync("UPDATE Performances SET NumberOfReservedTickets = NumberOfReservedTickets - @amount, NumberOfAvailableTickets = NumberOfAvailableTickets + @amount WHERE Id = @performanceId",
                                  (["amount", box (oldState.NumberOfReservedTickets - dbReservation.NumberOfReservedTickets); "performanceId", box dbReservation.PerformanceId] |> Map.ofList), trans)
                ()


        trans.Commit()
        return ()
    }

// Returns all reservations
let returnAllReservationsFromDb (conn:IDbConnection) =
        let output =
            select {
                for r in ReservationsTable do
                selectAll
                orderBy r.PerformanceTitle
                }
            |> conn.SelectAsync<ReservationDB>

        let v = output.Result
        v
        |> Seq.toList |> List.map(ReservationDB.toDomain)

// Returns all undelivered reservations
let returnAllUndeliveredReservations (conn:IDbConnection) =
    let output =
        select {
            for r in ReservationsTable do
            where (r.TicketsReceived = false)
            }
        |> conn.SelectAsync<ReservationDB>

    let v = output.Result
    v
    |> Seq.toList |> List.map(ReservationDB.toDomain)

// Returns all unpaid reservation
let returnAllUnpaidReservations (conn:IDbConnection) =
    let output =
        select {
            for r in ReservationsTable do
            where (r.IsPaid = false)}
        |> conn.SelectAsync<ReservationDB>

    let v = output.Result
    v
    |> Seq.toList |> List.map(ReservationDB.toDomain)


