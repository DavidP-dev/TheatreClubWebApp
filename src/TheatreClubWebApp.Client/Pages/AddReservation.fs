module TheatreClubWebApp.Client.Pages.AddReservation

open System
open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server

let private alertRow =
    Daisy.alert [
        alert.info
        prop.text "Pro přidání rezervace vyber z níže uvedených možností:"
    ]

let private selectRow =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.select [
                select.bordered
                prop.className "w-full max-w-xs"
                prop.children [
                    Html.option "Vyber rezervující osobu"
                    Html.option "Dvořáčková Petra"
                    Html.option "Ferjentsik Karel"
                    Html.option "Pícha David"
                ]
            ]
            Daisy.select [
                select.bordered
                prop.className "w-full max-w-xs"
                prop.children [
                    Html.option "Vyber divadlo"
                    Html.option "A Studio Rubín"
                    Html.option "Divadlo pod Palmovkou"
                    Html.option "Jatka 78"
                ]
            ]
        ]
    ]

let private selectRow2 =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.select [
                select.bordered
                prop.className "w-full max-w-xs"
                prop.children [
                    Html.option "Vyber představení"
                    Html.option "Federer a Nadal"
                    Html.option "Hubte skauty"
                    Html.option "Pérák"
                ]
            ]
            Daisy.select [
                select.bordered
                prop.className "w-full max-w-xs"
                prop.children [
                    Html.option "Vyber datum a čas představení"
                    Html.option "10 10 2022 18:00"
                    Html.option "11 12 2022 18:00 "

                ]
            ]
        ]
    ]



let private inputInfo =
    Daisy.alert [
        alert.info
        prop.text "Zadej počet vstupenek:"
        ]

let private selectInfo2 =
    Daisy.alert [
        alert.info
        prop.text "Vyber z níže uvedených možností"
        ]

let private inputRow =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.formControl [
                Daisy.label [Daisy.labelText "Počet vstupenek:"]
                Daisy.input [input.bordered; prop.placeholder "Počet vstupenek"]
            ]
        ]
    ]

let private selectRow3 =
    Html.div [
        prop.className "flex flex-row gap-4"
        prop.children [
            Daisy.select [
                select.bordered
                prop.className "w-full max-w-xs"
                prop.children [
                    Html.option "Jsou vstupenky zaplaceny?"
                    Html.option "Vstupenky JSOU ZAPLACENY."
                    Html.option "Vstupenky NEJSOU ZAPLACENY."
                ]
            ]
            Daisy.select [
                select.bordered
                prop.className "w-full max-w-xs"
                prop.children [
                    Html.option "Byly vstupenky doručeny?"
                    Html.option "Vstupenky BYLY DORUČENY."
                    Html.option "11 12 2022 18:00 "

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
            selectRow2
            inputInfo
            inputRow
            selectInfo2
            selectRow3


            Html.div [

                Daisy.button.button [
                    button.outline
                    button.primary
                    button.lg
                    prop.text "Přidej představení"
                ]
            ]
        ]
    ]
