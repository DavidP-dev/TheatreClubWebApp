module TheatreClubWebApp.Client.Pages.AddReservation

open System
open Elmish
open Feliz
open Feliz.UseElmish
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Router
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Shared.Domain

// Adding reservation
type ModelR = {
    Res : Reservation
    IsValid : bool
}

type MsgR =
   | FormChanged of Reservation
   | FormSubmitted
   | FormSaved

let init () =
    {
        Res = {
            ReservationID = Guid.NewGuid()
            MemberId = Guid.NewGuid()
            MemberName = ""
            MemberSurname = ""
            PerformanceId = Guid.NewGuid()
            PerformanceTitle = ""
            PerformanceDateAndTime = ""
            NumberOfTickets = ""
            IsPaid = ""
            TicketsReceived = ""
        }
        IsValid = false
    }, Cmd.none

let private validate (r:Reservation) = true

let update msgR (state: ModelR) =
    match msgR with
    | FormChanged r -> { state with Res = r; IsValid = validate r }, Cmd.none
    | FormSubmitted ->
        let nextCmd =
            if state.IsValid then Cmd.OfAsync.perform serviceR.SaveReservation state.Res (fun _ -> FormSaved)
            else Cmd.none
        state, nextCmd
    | FormSaved -> state, Page.Reservations |> Cmd.navigatePage

let stringDateTimeToDayTimeOffSet (s:string) =
    match s |> DateTimeOffset.TryParse with
    | true, value -> value
    | false, _ -> DateTimeOffset.MinValue

// Update Reservation label in ClubMember Type - to do

// Update Reservations label in Performance Type - to do


let private alertRow =
    Daisy.alert [
        alert.info
        prop.className "justify-center"
        prop.text "Pro přidání rezervace vyplň níže vyobrazený formulář."
    ]

let private selectRow (mem:ClubMember list) (perf:Performance list)=

    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.dropdown [
                Daisy.button.button [
                    button.primary
                    prop.text "Vyber objednávajícího"
                ]
                Daisy.dropdownContent [
                    prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                    prop.tabIndex 0
                    prop.children [
                        for m in mem ->
                            Html.li [Html.a [prop.text (m.Surname + " " + m.Name)]]

                    ]
                ]
            ]
            Daisy.dropdown [
                Daisy.button.button [
                    button.primary
                    prop.text "Vyber divadelní představení"
                ]
                Daisy.dropdownContent [
                    prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                    prop.tabIndex 0
                    prop.children [
                        for p in perf ->
                            Html.li [Html.a [prop.text (p.Title + " " + p.DateAndTime)]]
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
                        { state.Res with NumberOfTickets = v } |> FormChanged |> dispatch
                    )
                ]
            ]
        ]
    ]

let private selectRow3 =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.dropdown [
                Daisy.button.button [
                    button.primary
                    prop.text "Jsou vstupenky zaplaceny?"
                ]
                Daisy.dropdownContent [
                    prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                    prop.tabIndex 0
                    prop.children [
                        Html.li [Html.a [prop.text "Vstupenky JSOU ZAPLACENY."]]
                        Html.li [Html.a [prop.text "Vstupenky NEJSOU ZAPLACENY."]]
                    ]
                ]
            ]
            Daisy.dropdown [
                Daisy.button.button [
                    button.primary
                    prop.text "Jsou vstupenky doručeny?"
                ]
                Daisy.dropdownContent [
                    prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                    prop.tabIndex 0
                    prop.children [
                        Html.li [Html.a [prop.text "Vstupenky JSOU DORUČENY."]]
                        Html.li [Html.a [prop.text "Vstupenky NEJSOU DORUČENY."]]
                    ]
                ]
            ]
        ]
    ]

[<ReactComponent>]

let AddReservationView () =

// Load ClubMember list for dropdown menu
    let members, setMembers = React.useState(List.empty)

    let loadMembers () = async {
        let! members = service.GetClubMembers()
        setMembers members
    }
    React.useEffectOnce(loadMembers >> Async.StartImmediate)

// Load Performances list for dropdown menu
    let performances, setPerformances = React.useState(List.Empty)

    let loadPerformances () = async {
        let! performances = serviceP.GetPerformances()
        setPerformances performances
    }
    React.useEffectOnce(loadPerformances >> Async.StartImmediate)

// Saving reservation
    let state, dispatch = React.useElmish(init, update, [||])

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
                    selectRow members performances
                    inputRow state dispatch
                    selectRow3


                    Html.div [

                        Daisy.button.button [
                            button.outline
                            button.primary
                            button.lg
                            prop.text "Přidej rezervaci"
                        ]
                    ]
                ]
            ]
        ]
    ]