module TheatreClubWebApp.Client.Pages.EditReservation

open System
open Elmish
open Feliz
open Feliz.UseElmish
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Router
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Shared.Domain

// Adding reservation
type Model = {
    Res : Reservation option
    ClubMembers : ClubMember list
    Performances : Performance list
    SelectedCm : Guid option
    SelectedPerf : Guid option
    EnteredNumberOfTickets : string option
    SelectedIsPaid : bool option
    SelectedTicketsReceived : bool option
    IsValid : bool
}

type Msg =
   | LoadReservation of Guid
   | ReservationLoaded of Reservation
   | ClubMemberSelected of Guid
   | PerformanceSelected of Guid
   | FormChanged of Reservation
   | FormSubmitted
   | FormSaved

let init (rId : Guid) =
    {
        Res = None
        ClubMembers = List.empty
        Performances = List.empty
        SelectedCm = None
        SelectedPerf = None
        EnteredNumberOfTickets = None
        SelectedIsPaid = None
        SelectedTicketsReceived = None
        IsValid = false
    }, Cmd.ofMsg (LoadReservation rId)

let private validate (res : Reservation) =
    String.IsNullOrWhiteSpace(res.MemberName) |> not
    && String.IsNullOrWhiteSpace(res.MemberSurname) |> not
    && String.IsNullOrWhiteSpace(res.PerformanceTitle) |> not
    && String.IsNullOrWhiteSpace(res.PerformanceDateAndTime) |> not
    && String.IsNullOrWhiteSpace(res.NumberOfTickets) |> not

let update msg (state: Model) =

    match msg with
    | LoadReservation rId -> state, Cmd.OfAsync.perform serviceR.GetReservation rId (fun x -> ReservationLoaded x)
    | ReservationLoaded r -> { state with Res = Some r; IsValid = validate r }, Cmd.none
    | ClubMemberSelected cId -> { state with SelectedCm = Some cId }, Cmd.none
    | PerformanceSelected pId -> { state with SelectedPerf = Some pId }, Cmd.none
    | FormChanged r -> {
                            state with
                                Res = Some r
                                IsValid = validate r
                            }, Cmd.none
    | FormSubmitted ->
        let nextCmd =
            match state.Res with
            | Some r ->
                let selectedMember =
                    state.SelectedCm
                    |> Option.bind (fun m -> state.ClubMembers |> List.tryFind ( fun x -> x.Id = m))
                let reservationWithMember =
                    selectedMember
                    |> Option.map (fun m -> {r with
                                              MemberId = m.Id
                                              MemberName = m.Name
                                              MemberSurname = m.Surname})
                    |> Option.defaultValue r
                let selectedPerformance =
                    state.SelectedPerf
                    |> Option.bind (fun p -> state.Performances |> List.tryFind ( fun x -> x.Id = p))
                let reservationWithPerformance =
                    selectedPerformance
                    |> Option.map (fun p -> {reservationWithMember with
                                              PerformanceId = p.Id
                                              PerformanceTitle = p.Title
                                              PerformanceDateAndTime = p.DateAndTime})
                    |> Option.defaultValue reservationWithMember
                Cmd.OfAsync.perform serviceR.UpdateReservation reservationWithPerformance (fun _ -> FormSaved)
            | None -> Cmd.none
        state, nextCmd
    | FormSaved -> state, Page.Reservations |> Cmd.navigatePage





let stringDateTimeToDayTimeOffSet (s:string) =
    match s |> DateTimeOffset.TryParse with
    | true, value -> value
    | false, _ -> DateTimeOffset.MinValue

let private alertRow =
    Daisy.alert [
        alert.info
        prop.className "justify-center"
        prop.text "Změň požadované údaje."
    ]

let private selectRow (mem:ClubMember list) (perf:Performance list) (state:Model) dispatch =

    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.dropdown [
                prop.children [
                    Daisy.button.button [
                        button.primary
                        match state.SelectedCm with
                        | Some m ->
                            state.ClubMembers
                            |> List.tryFind (fun (c:ClubMember) -> c.Id = m)
                            |> Option.map (fun m -> prop.text ("Objednávající: " + m.Surname + " " + m.Name))
                            |> Option.defaultValue (prop.text "Vyber objednávajícího")
                        | None -> prop.text "Vyber objednávajícího"
                    ]
                    Daisy.dropdownContent [
                        prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                        prop.tabIndex 0
                        prop.children [
                            for m in mem ->
                                Html.li [
                                    Html.a [
                                        prop.text (m.Surname + " " + m.Name)
                                        prop.onClick (fun ev ->
                                            ev.preventDefault()
                                            ClubMemberSelected m.Id |> dispatch)
                                    ]
                                ]
                        ]
                    ]
                ]
            ]
            Daisy.dropdown [
                prop.children [
                    Daisy.button.button [
                        button.primary
                        match state.SelectedPerf with
                        | Some m ->
                            state.Performances
                            |> List.tryFind (fun (p:Performance) -> p.Id = m)
                            |> Option.map (fun p -> prop.text "Představení: " + p.Title + " " + p.DateAndTime))
                            |> Option.defaultValue (prop.text "Vyber divadelní představení")
                        | None -> prop.text "Vyber divadelní představení"
                   ]
                    Daisy.dropdownContent [
                        prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                        prop.tabIndex 0
                        prop.children [
                            for p in perf ->
                                Html.li [
                                    Html.a [
                                        prop.text (p.Title + " " + p.DateAndTime)
                                        prop.onClick (fun ev ->
                                            ev.preventDefault()
                                            PerformanceSelected p.Id |> dispatch)
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]


let private inputRow state dispatch =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.formControl [
                Daisy.label [
                    prop.for' "NumberOFTickets"
                    prop.children [
                        Daisy.labelText "Zadej počet vstupenek:"
                    ]
                ]
                Daisy.input [
                    input.bordered
                    prop.placeholder "Počet vstupenek"
                    prop.name "NumberOfTickets"
                    prop.defaultValue state.Res.NumberOfTickets
                    prop.onChange (fun v ->
                        v. |> NumberOfTicketsSelected |> dispatch
                    )
                ]
            ]
        ]
    ]

let private selectRow3 state dispatch =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [

            Daisy.dropdown [
                prop.children [

                    Daisy.button.button [
                        button.primary
                        match state.SelectedIsPaid with
                            | Some(false) -> prop.text "Vstupenky nejsou zaplaceny"
                            | Some(true) -> prop.text "Vstupenky jsou zaplaceny"
                            | None -> prop.text "JSOU VSTUPENKY ZAPLACENY?"
                    ]
                    Daisy.dropdownContent [
                        prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                        prop.tabIndex 0
                        prop.children [
                            Html.li [
                                Html.a [
                                    prop.text "Vstupenky JSOU ZAPLACENY."
                                    prop.onClick (fun ev ->
                                            ev.preventDefault()
                                            true |> IsPaidSelected |> dispatch)
                                ]
                            ]
                            Html.li [
                                Html.a [
                                    prop.text "Vstupenky NEJSOU ZAPLACENY."
                                    prop.onClick (fun ev ->
                                            ev.preventDefault()
                                            false |> IsPaidSelected |> dispatch)
                                ]
                            ]
                        ]
                    ]
                ]
            ]
            Daisy.dropdown [
                prop.children [
                    Daisy.button.button [
                        button.primary
                        match state.SelectedTicketsReceived with
                            | Some(false) -> prop.text "Vstupenky nejsou doručeny"
                            | Some(true) -> prop.text "Vstupenky jsou doručeny"
                            | None -> prop.text "JSOU VSTUPENKY DORUČENY?"
                    ]
                    Daisy.dropdownContent [
                        prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                        prop.tabIndex 0
                        prop.children [
                            Html.li [
                                Html.a [
                                    prop.text "Vstupenky JSOU DORUČENY."
                                    prop.onClick (fun ev ->
                                            ev.preventDefault()
                                            true |> TicketReceivedSelected |> dispatch)
                                ]
                            ]
                            Html.li [
                                Html.a [
                                    prop.text "Vstupenky NEJSOU DORUČENY."
                                    prop.onClick (fun ev ->
                                            ev.preventDefault()
                                            false |> TicketReceivedSelected |> dispatch)
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]

[<ReactComponent>]

let EditReservationView (rId : Guid) =

// Load ClubMember list for dropdown menu
//    let members, setMembers = React.useState(List.empty)
//
//    let loadMembers () = async {
//        let! members = service.GetClubMembers()
//        setMembers members
//    }
//    React.useEffectOnce(loadMembers >> Async.StartImmediate)
//
//// Load Performances list for dropdown menu
//    let performances, setPerformances = React.useState(List.Empty)
//
//    let loadPerformances () = async {
//        let! performances = serviceP.GetPerformances()
//        setPerformances performances
//    }
//    React.useEffectOnce(loadPerformances >> Async.StartImmediate)

// Saving reservation
    let state, dispatch = React.useElmish(init rId, update, [||])

// Page layout

    Html.form [
        prop.onSubmit (fun e ->
                e.preventDefault()
                let reservationId = Guid.NewGuid()
                let msg = "form sent" + reservationId.ToString()
                Fable.Core.JS.console.log(sprintf "%A" state)
                FormSubmitted |> dispatch)

        prop.children [
            Html.div [
                prop.className "flex flex-col items-center gap-4 mx-14"
                prop.children [

                    alertRow
                    selectRow state.ClubMembers state.Performances state dispatch
                    inputRow state dispatch
                    selectRow3 state dispatch


                    Html.div [

                        Daisy.button.button [
                            button.outline
                            button.primary
                            button.lg
                            prop.text "Ulož změny"
                        ]
                    ]
                ]
            ]
        ]
    ]