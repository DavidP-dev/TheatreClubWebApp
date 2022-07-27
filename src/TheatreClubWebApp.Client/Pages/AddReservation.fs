module TheatreClubWebApp.Client.Pages.AddReservation

open System
open Elmish
open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Shared.Domain

type Model = {
    Res : Reservation
    IsValid : bool
}

type Msg =
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
            Theatre = ""
            PerformanceTitle = ""
            PerformanceDateAndTime = DateTimeOffset.MinValue
            NumberOfTickets = ""
            IsPaid = ""
            TicketsReceived = ""
        }
        IsValid = false
    }, Cmd.none


let private alertRow =
    Daisy.alert [
        alert.info
        prop.className "justify-center"
        prop.text "Pro přidání rezervace vyplň níže vyobrazený formulář."
    ]

let private selectRow =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.dropdown [
                Daisy.button.button [
                    button.primary
                    prop.text "Vyber obědnávajícího"
                ]
                Daisy.dropdownContent [
                    prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                    prop.tabIndex 0
                    prop.children [
                        Html.li [Html.a [prop.text "Dvořáčková Petra"]]
                        Html.li [Html.a [prop.text "Ferjentsik Karel"]]
                        Html.li [Html.a [prop.text "Konášová Linda"]]
                        Html.li [Html.a [prop.text "Pícha David"]]
                    ]
                ]
            ]
            Daisy.dropdown [
                Daisy.button.button [
                    button.primary
                    prop.text "Vyber divadlo"
                ]
                Daisy.dropdownContent [
                    prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                    prop.tabIndex 0
                    prop.children [
                        Html.li [Html.a [prop.text "Divadlo s nejdelším názvem v Praze"]]
                        Html.li [Html.a [prop.text "Nejkrásnější divadlo"]]
                        Html.li [Html.a [prop.text ""]]
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
                        Html.li [Html.a [prop.text "Divadlo s nejdelším názvem v Praze"]]
                        Html.li [Html.a [prop.text "Nejkrásnější divadlo"]]
                        Html.li [Html.a [prop.text ""]]
                    ]
                ]
            ]
            Daisy.dropdown [
                Daisy.button.button [
                    button.primary
                    prop.text "Vyber čas představení"
                ]
                Daisy.dropdownContent [
                    prop.className "p-2 shadow menu bg-base-100 rounded-box w-52"
                    prop.tabIndex 0
                    prop.children [
                        Html.li [Html.a [prop.text "Divadlo s nejdelším názvem v Praze"]]
                        Html.li [Html.a [prop.text "Nejkrásnější divadlo"]]
                        Html.li [Html.a [prop.text ""]]
                    ]
                ]
            ]
        ]
    ]


let private inputRow =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.formControl [
                Daisy.label [Daisy.labelText "Zadej počet vstupenek:"]
                Daisy.input [input.bordered; prop.placeholder "Počet vstupenek"]
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
    Html.div [
        prop.className "flex flex-col items-center gap-4 mx-14"
        prop.children [

            alertRow
            selectRow
            inputRow
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
