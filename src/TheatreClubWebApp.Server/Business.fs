module TheatreClubWebApp.Server.Business

open System
open System.Data
open TheatreClubWebApp.Shared.Domain
open Database

// Checks club member existence and adds club member to database
let register (conn:IDbConnection) (cM:ClubMember) =
    let maybeMember = tryGetMemberByEmail conn cM.Email
    match maybeMember with
    | Some ex -> failwith $"Uzivatel s timto emailem, ID = {ex.Id} uz existuje"
    | None ->
        insertCMToDb conn cM

// Checks club member existence and removes club member from database
let unregister (conn:IDbConnection) (cMId:Guid) =
    let maybeMember = tryGetMemberById conn cMId
    match maybeMember with
        | Some _ -> removeCmFromDb conn cMId
        | None -> failwith $"Uživatel s Id: {cMId} v databázi neexistuje."

// Checks club member for existence and updates all his data
let updateClubMember (conn:IDbConnection) (cM : ClubMember) =
    let maybeMember = tryGetMemberById conn cM.Id
    match maybeMember with
        | Some _ -> updateClubMemberDb conn cM
        | None -> failwith $"Uživatel s Id: {cM.Id} v databází neexistuje."

// Checks performance existence and inserts performance to database
let addPerformance (conn:IDbConnection) (perf:Performance) =
    let maybePerformance = tryGetPerformanceByTitleAndDate conn perf
    match maybePerformance with
    | Some ex -> failwith $"Divadlení představení {ex.Title} již v databázi existuje."
    | None -> addPerformanceToDb conn perf

// Checks performance existence and and removes performance from database
let removePerformance (conn:IDbConnection) (pId:Guid) =
    let maybePerformance = tryGetPerformanceById conn pId
    match maybePerformance with
    | Some _ -> removePerformanceFromDb conn pId
    | None -> failwith $"Divadlení představení s Id:{pId} není v databázi."

// Checks performance for existence and updates all performance data
let updatePerformance (conn:IDbConnection) (perf : Performance) =
    let maybePerformance = tryGetPerformanceById conn perf.Id
    match maybePerformance with
        | Some _ -> updatePerformanceDb conn perf
        | None -> failwith $"Uživatel s Id: {perf.Id} v databází neexistuje."

// Checks reservation existence and adds reservation to database
let addReservation (conn:IDbConnection) (res:Reservation) =
    let maybeReservation = tryGetReservationById conn res.ReservationID
    match maybeReservation with
    | Some _ -> failwith $"Rezervace ID {res.ReservationID} je už databázi."
    | None -> addReservationToDb conn res

// Checks reservation existence and removes reservation from database
let removeReservation (conn : IDbConnection) (rId : Guid) =
    let maybeReservation = tryGetReservationById conn rId
    match maybeReservation with
    | Some _ -> removeReservationFromDb conn rId
    | None _ -> failwith $"Rezervace ID {rId} v databázi neexistuje."

// Returns all Club members from database
let getAllClubMembers (conn:IDbConnection) =
    returnAllClubMembersFromDb conn

// Returns all performances
let getAllPerformances (conn:IDbConnection) =
    let performancesList = returnAllPerformancesFromDb conn
    performancesList

// Returns all reservations
let getAllReservations (conn:IDbConnection) =
    let reservationsList = returnAllReservationsFromDb conn
    reservationsList

// Returns all undelivered reservations
let getAllUndeliveredReservations (conn:IDbConnection) =
    let undeliveredReservationsList = returnAllUndeliveredReservations conn
    printfn "Zde je aktuální seznam nedoručených vstupenek: %A" undeliveredReservationsList

// Returns all unpaid reservations
let getAllUnpaidReservations (conn:IDbConnection) =
    let unPaidReservationsList = returnAllUnpaidReservations conn
    printfn "Zde je aktuální seznam nezaplacených objednávek %A" unPaidReservationsList

// Return member by ID
let getClubMemberById (conn:IDbConnection) (cId:Guid) =
    let memberById = returnClubMemberById conn cId |> List.head
    memberById

// Return performance by ID
let getPerformanceById (con:IDbConnection) (cId:Guid) =
    let performanceById =  returnPerformanceById con cId |> List.head
    performanceById

// Return club members by genre
let getClubMembersByGenre (conn:IDbConnection) (genre:Genre) =
    let membersByPreferenceList = returnClubMembersByGenre conn genre
    printfn "Zde je seznam členů se zadanou preferencí: %A" membersByPreferenceList

// Return performance by genre
let getPerformancesByGenre (conn:IDbConnection) (genre:Genre) =
    let performanceByGenreList = returnPerformancesByGenre conn genre
    printfn "Zde je seznam představení dle zadaného žánru: %A" performanceByGenreList

