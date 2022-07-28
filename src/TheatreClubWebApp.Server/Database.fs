module TheatreClubWebApp.Server.Database


open System
open System.Data
open System.Globalization
open TheatreClubWebApp.Shared.Domain

open Dapper.FSharp
open Dapper.FSharp.MSSQL
open Microsoft.Data.SqlClient
open Microsoft.FSharp.Core

// Connection to database
let connstring = "data source=PICHA\\sqlexpress;initial catalog=TheatreClubDBTest;integrated security=True;TrustServerCertificate=True"

let getConnection () : IDbConnection =
    new SqlConnection(connstring)

// Database types
type MemberDB =
    {
        Id : Guid
        Name : string
        Surname : string
        Email : string
        PreferredGenres : string
        MemberReservations : int
    }

type PerformanceDB =
    {
        Id : Guid
        Title : string
        DateAndTime: DateTimeOffset
        NumberOfTickets : int
        Reservations : int
        Cost : int
        Genres : string
    }

type ReservationDB =
        {
        ReservationID : Guid
        MemberId : Guid
        MemberName : string
        MemberSurname: string
        PerformanceId : Guid
        PerformanceTitle : string
        PerformanceDateAndTime : DateTimeOffset
        NumberOfTickets : int
        IsPaid : bool
        TicketsReceived : bool
        }

// Modules for transferring data from database layer to domain layer and opposite
module Transfers =
    let dateTimeOffsetToString (dto:DateTimeOffset) :string =
        let dateTimeOffsetString = dto.ToString("dd.MM.yyyy HH:mm")
        dateTimeOffsetString

    let tryStringToDateTimeOffset (s:string) :DateTimeOffset =
        let convertedDateTimeExact = DateTimeOffset.TryParseExact(s, "dd.MM.yyyy HH:mm", CultureInfo("es-CZ", false), DateTimeStyles.AssumeLocal)
        match convertedDateTimeExact with
            |true, value -> value
            |false, _  -> DateTimeOffset.MinValue

    let boolToString (b:bool) :string =
        let stringFromBool =
            match b with
            | true -> "Ano"
            | false -> "Ne"
        stringFromBool
    let stringToBool (s:string) :bool =
        let boolFromString =
            match s with
            | "Ano" -> true
            | "Ne" -> false
            | _ -> failwith $"Možnost -{s}-neexistuje!!!"
        boolFromString

module MembersDb =
    let parseGenre (gn :string) : Genre =
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

    let toDomain (db:MemberDB) : ClubMember = {
       Id = db.Id
       Name = db.Name
       Surname = db.Surname
       Email = db.Email
       PreferredGenres = db.PreferredGenres.Split(",") |> Array.map parseGenre |> List.ofArray
       MemberReservations = db.MemberReservations |> string
       }

    let toDatabase (dm:ClubMember) : MemberDB = {
        Id = dm.Id
        Name = dm.Name
        Surname = dm.Surname
        Email = dm.Email
        PreferredGenres = dm.PreferredGenres |> List.map genreToString |> (fun x -> String.Join(",", x))
        MemberReservations = dm.MemberReservations |> int
        }

module PerformancesDB =
    let toDomain (db:PerformanceDB) : Performance = {
        Id = db.Id
        Title = db.Title
        DateAndTime = db.DateAndTime |> Transfers.dateTimeOffsetToString
        NumberOfTickets = db.NumberOfTickets |> string
        Reservations = db.Reservations |> string
        Cost = db.Cost |> string
        Genres =  db.Genres.Split(",") |> Array.map MembersDb.parseGenre |> List.ofArray
        }

    let toDatabase (dm:Performance) : PerformanceDB = {
        Id = dm.Id
        Title = dm.Title
        DateAndTime = dm.DateAndTime |> Transfers.tryStringToDateTimeOffset
        NumberOfTickets = dm.NumberOfTickets |> int
        Reservations = dm.Reservations |> int
        Cost = dm.Cost |> int
        Genres = dm.Genres |> List.map MembersDb.genreToString |> (fun x -> String.Join(",", x))}

module ReservationDB =
    let toDomain (db:ReservationDB) : Reservation = {
        ReservationID = db.ReservationID
        MemberId = db.MemberId
        MemberName = db.MemberName
        MemberSurname = db.MemberSurname
        PerformanceId = db.PerformanceId
        PerformanceTitle = db.PerformanceTitle
        PerformanceDateAndTime = db.PerformanceDateAndTime |> Transfers.dateTimeOffsetToString
        NumberOfTickets = db.NumberOfTickets |> string
        IsPaid = db.IsPaid |> Transfers.boolToString
        TicketsReceived = db.TicketsReceived |> Transfers.boolToString
    }
    let toDatabase (dm:Reservation) : ReservationDB = {
        ReservationID = dm.ReservationID
        MemberId = dm.MemberId
        MemberName = dm.MemberName
        MemberSurname = dm.MemberSurname
        PerformanceId = dm.PerformanceId
        PerformanceTitle = dm.PerformanceTitle
        PerformanceDateAndTime = dm.PerformanceDateAndTime |> Transfers.tryStringToDateTimeOffset
        NumberOfTickets = dm.NumberOfTickets |> int
        IsPaid = dm.IsPaid |> Transfers.stringToBool
        TicketsReceived = dm.IsPaid |> Transfers.stringToBool
    }

// names of database tables

let membersTable = table'<MemberDB> "ClubMembers"

let performancesTable = table'<PerformanceDB> "Performances"

let ReservationsTable = table'<ReservationDB> "Reservations"




// Checks existence of club member by searching for his email in database
let tryGetMemberByEmail (conn:IDbConnection) (email:string) =
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

// Checks existence of performance in database
let tryGetPerformanceByTitleAndDate (conn:IDbConnection) (performance:Performance) =
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

// Checks existence of registration in database
let tryGetReservationByIds (conn:IDbConnection) (res:Reservation) =
    let vysl =
        select {
            for r in ReservationsTable do
            where (r.PerformanceId = res.PerformanceId && r.MemberId = res.MemberId)}
        |> conn.SelectAsync<ReservationDB>

    let v = vysl.Result
    v
    |> Seq.tryHead
    |> Option.map ReservationDB.toDomain

// adds Member to database
let insertCMToDb (conn:IDbConnection) (cM:ClubMember) =
    let dbMember = MembersDb.toDatabase cM
    insert {
        into membersTable
        value dbMember
    }
    |> conn.InsertAsync


// removes Member from Database
let removeCmFromDb (conn:IDbConnection) (cM:ClubMember) =
    delete {
        for m in membersTable do
        where (m.Email = cM.Email )}
    |> conn.DeleteAsync

// function add Performance
let addPerformanceToDb (conn:IDbConnection) (perf:Performance) =
    let dbPerformance = PerformancesDB.toDatabase perf
    insert {
        into performancesTable
        value dbPerformance
    }
    |> conn.InsertAsync

// function remove Performance
let removePerformanceFromDb (conn:IDbConnection) (perf:Performance) =
    delete {
        for p in performancesTable do
        where (p.Id = perf.Id )}
    |> conn.DeleteAsync

// function add Reservation
let addReservationToDb (conn:IDbConnection) (res:Reservation) =
    let dbReservation = ReservationDB.toDatabase res
    insert {
        into ReservationsTable
        value dbReservation
    }
    |> conn.InsertAsync

// function remove Reservation
let removeReservationFromDb (conn:IDbConnection) (res:Reservation) =
    delete {
        for r in ReservationsTable do
        where (r.ReservationID = res.ReservationID )}
    |> conn.DeleteAsync

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

// Returns all performances
let returnAllPerformancesFromDb (conn:IDbConnection) =
        let output =
            select {
                for p in performancesTable do
                selectAll
                orderBy p.Title
                }
            |> conn.SelectAsync<PerformanceDB>

        let v = output.Result
        v |> Seq.toList |> List.map(PerformancesDB.toDomain)

// Returns all reservations
let returnAllReservationsFromDb (conn:IDbConnection) =
        let output =
            select {
                for r in ReservationsTable do
                selectAll
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

// Returns list with member by ID
let returnClubMemberById (conn:IDbConnection) (cId:Guid)  =
    let output =
        select {
            for m in membersTable do
            where (m.Id = cId)
        }
        |> conn.SelectAsync<MemberDB>
    let v = output.Result
    v |> Seq.toList |> List.map(MembersDb.toDomain)

// Returns performance by ID
let returnPerformanceById (conn:IDbConnection) (pId:Guid)  =
    let output =
        select {
            for p in membersTable do
            where (p.Id = pId)
        }
        |> conn.SelectAsync<PerformanceDB>
    let v = output.Result
    v |> Seq.toList |> List.map(PerformancesDB.toDomain)

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

